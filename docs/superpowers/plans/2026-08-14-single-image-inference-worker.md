# Persistent Single-Image Inference Worker Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make the existing WinForms detection page run one selected image through the existing PatchCore/DINOv2 models using a persistent Python worker, then display and persist a canonical PASS/NG result.

**Architecture:** A single `InferenceWorkerService` owns a long-lived `pytorch` Python process and exchanges newline-delimited JSON over redirected stdin/stdout. The Python worker loads one product runtime at a time, reuses the existing algorithm modules without changing their mathematical behavior, and writes one canonical result directory per inspection. WinForms remains responsive during startup, model loading, and inspection.

**Tech Stack:** .NET Framework 4.8 WinForms, `System.Diagnostics.Process`, `System.Web.Script.Serialization`, Python 3 in the `pytorch` Conda environment, PyTorch 2.12 CPU, OpenCV, NumPy, built-in Python `unittest`.

## Global Constraints

- Camera discovery, preview, acquisition, and triggering are out of scope.
- The existing PatchCore/DINOv2 algorithm and learned artifacts must retain their mathematical behavior.
- The worker must set `KMP_DUPLICATE_LIB_OK=TRUE` before importing OpenCV, NumPy, PyTorch, or algorithm modules.
- Use `C:\Users\wlena\anaconda3\envs\pytorch\python.exe` directly; do not invoke `conda run` from WinForms.
- Only one product model and one inspection may be active at a time.
- `AnomalyThreshold` controls PASS/NG and must be displayed as uncalibrated; default is `0.5`.
- Initial model warm-up is excluded from the 15-second goal; warmed-up single-image inference is measured against 15 seconds.
- The worker protocol is JSON Lines on stdout; all non-protocol diagnostics go to stderr.
- Python integration files under `E:\winForm\patchcore` are outside a Git repository. Before editing, record SHA-256 hashes; after editing, record new hashes in the implementation handoff. Never initialize or rewrite that directory's repository state.
- Preserve the two pre-existing untracked files in the WinForms repository.

---

## File map

### Python project: `E:\winForm\patchcore`

- Modify `industrial_anomaly/inspect_image.py`: set the child-process OpenMP variable before native imports and route the CLI through the reusable runtime.
- Create `industrial_anomaly/inspection_runtime.py`: product loading, one-image inference, artifact generation, canonical schema, and atomic JSON persistence.
- Create `industrial_anomaly/winforms_worker.py`: JSONL transport and process loop.
- Create `tests/test_inspection_runtime.py`: runtime behavior with injected fake pipeline/banks.
- Create `tests/test_winforms_worker.py`: subprocess protocol tests using a fake runtime.

### WinForms project: `E:\winForm\180Decetion`

- Modify `model/DetectionResult.cs`: canonical record fields.
- Create `model/InferenceWorkerModels.cs`: options, product options, worker state, envelope, and structured error types.
- Create `service/ResultPathService.cs`: product-path and output-directory resolution.
- Create `service/InferenceWorkerService.cs`: persistent process lifecycle and request correlation.
- Modify `service/ProductConfigService.cs`: retrieve one configured product by name and resolve defaults.
- Modify `page/Index.cs`: asynchronous worker lifecycle and inspection orchestration.
- Modify `page/TabDetection.cs`: worker/model states and uncalibrated-threshold status.
- Modify `page/TabRecords.cs`: recursive canonical record discovery and legacy fallback.
- Create `service/DetectionRecordReader.cs`: canonical and legacy record parsing independent of UI controls.
- Modify `page/TabSystemSettings.cs`: worker-oriented labels/defaults and startup timeout.
- Modify `App.config`: current interpreter, worker, working directory, and startup timeout.
- Modify `180Detection.csproj`: include new C# files.
- Create `tests/InferenceWorkerHarness/InferenceWorkerHarness.csproj`: no-NuGet .NET Framework test executable.
- Create `tests/InferenceWorkerHarness/Program.cs`: assertions for path and process behavior.
- Create `tests/InferenceWorkerHarness/fake_worker.py`: deterministic JSONL child process.
- Modify `180Detection.slnx`: include the harness project.
- Modify `README.md`: local setup, lifecycle, records, and verification commands.

---

### Task 1: Reusable Python inspection runtime

**Files:**
- Create: `E:\winForm\patchcore\industrial_anomaly\inspection_runtime.py`
- Modify: `E:\winForm\patchcore\industrial_anomaly\inspect_image.py`
- Create: `E:\winForm\patchcore\tests\test_inspection_runtime.py`

**Interfaces:**
- Consumes: existing `PatchCoreDINOv2Pipeline`, `DefectExemplarBank`, and post-processing helpers.
- Produces: `ProductRuntimeConfig`, `InspectionRuntime.load_product(config)`, and `InspectionRuntime.inspect(image_path, output_dir, record_id, record_time, save_original, save_marked) -> dict` for Task 2.

- [ ] **Step 1: Record hashes and write the failing runtime tests**

Run before editing:

```powershell
Get-FileHash E:\winForm\patchcore\industrial_anomaly\inspect_image.py -Algorithm SHA256
```

Create tests that inject fake pipeline and bank factories, avoiding real model load:

```python
class InspectionRuntimeTests(unittest.TestCase):
    def test_threshold_below_score_is_ng(self):
        runtime = make_runtime(score=0.72, similarity=0.91, predicted="shao1")
        runtime.load_product(make_config(threshold=0.5))
        result = runtime.inspect(TEST_IMAGE, self.output_dir, metadata())
        self.assertTrue(result["is_ng"])
        self.assertEqual("NG", result["status"])
        self.assertEqual("shao1", result["defect_class"])
        self.assertEqual("uncalibrated", result["threshold_calibration"])

    def test_score_below_threshold_is_pass(self):
        runtime = make_runtime(score=0.49, similarity=0.88, predicted="shao2")
        runtime.load_product(make_config(threshold=0.5))
        result = runtime.inspect(TEST_IMAGE, self.output_dir, metadata())
        self.assertFalse(result["is_ng"])
        self.assertEqual("PASS", result["status"])
        self.assertEqual("Normal", result["defect_class"])

    def test_non_finite_score_fails_instead_of_passing(self):
        runtime = make_runtime(score=float("nan"), similarity=0.0, predicted="shao1")
        runtime.load_product(make_config(threshold=0.5))
        with self.assertRaisesRegex(ValueError, "finite anomaly score"):
            runtime.inspect(TEST_IMAGE, self.output_dir, metadata())

    def test_repeated_identical_load_reuses_runtime(self):
        pipeline_factory = FakePipelineFactory()
        runtime = make_runtime(pipeline_factory=pipeline_factory)
        config = make_config(threshold=0.5)
        runtime.load_product(config)
        runtime.load_product(config)
        self.assertEqual(1, pipeline_factory.load_count)
```

- [ ] **Step 2: Run tests and verify the missing module failure**

Run:

```powershell
$env:KMP_DUPLICATE_LIB_OK='TRUE'
& C:\Users\wlena\anaconda3\envs\pytorch\python.exe -m unittest E:\winForm\patchcore\tests\test_inspection_runtime.py -v
```

Expected: FAIL with `ModuleNotFoundError` for `industrial_anomaly.inspection_runtime`.

- [ ] **Step 3: Implement the minimal reusable runtime**

Create these public types and signatures:

```python
@dataclass(frozen=True)
class ProductRuntimeConfig:
    display_name: str
    product_key: str
    patchcore_model_dir: Path
    bank_dir: Path
    anomaly_threshold: float
    device: str = "cpu"
    bbox_relative_threshold: float = 0.80
    roi_margin: float = 0.50
    center_fraction: float = 0.50

```

Implement class `InspectionRuntime` with constructor
`__init__(self, pipeline_factory=None, bank_loader=None)`, method
`load_product(self, config: ProductRuntimeConfig) -> dict`, and method
`inspect(self, image_path: str | Path, output_dir: str | Path, record_id: str,
record_time: str, save_original: bool, save_marked: bool) -> dict`.

Implementation requirements:

- Normalize and validate model/bank/image paths before loading.
- Copy the current extraction, dual-bank embedding, 50/50 fusion, overlay, ROI, and anomaly-map steps from `inspect_image.py` without changing their numeric behavior.
- Use `math.isfinite` on anomaly score, similarity, and margin where applicable.
- Decide `is_ng = anomaly_score >= anomaly_threshold`.
- Emit `Normal` for PASS and the predicted known defect for NG.
- Copy the original with `shutil.copy2` only when requested.
- Persist `full_marked.jpg` only when `save_marked` is true; otherwise return an empty `marked_image_path` while still retaining ROI and anomaly-map diagnostics.
- Write `result.json.tmp`, flush and close it, then replace `result.json` with `os.replace`.
- Return absolute artifact paths and `schema_version = 1`.
- Measure only the `inspect` call with `time.perf_counter` and write `elapsed_ms`.
- Skip runtime reconstruction when the normalized frozen config equals the current config.

At the first executable lines of `inspect_image.py`, before `cv2` and NumPy imports, add:

```python
import os
os.environ.setdefault("KMP_DUPLICATE_LIB_OK", "TRUE")
```

Refactor its `main()` to construct `ProductRuntimeConfig`, call `load_product`, and call `inspect`; retain all existing CLI arguments and console summary.

- [ ] **Step 4: Run focused tests and the CLI help check**

Run:

```powershell
& C:\Users\wlena\anaconda3\envs\pytorch\python.exe -m unittest E:\winForm\patchcore\tests\test_inspection_runtime.py -v
& C:\Users\wlena\anaconda3\envs\pytorch\python.exe E:\winForm\patchcore\industrial_anomaly\inspect_image.py --help
```

Expected: all runtime tests PASS; CLI help exits 0 without an OpenMP error and without requiring the parent shell variable.

- [ ] **Step 5: Record the Python checkpoint**

Run:

```powershell
Get-FileHash E:\winForm\patchcore\industrial_anomaly\inspect_image.py,E:\winForm\patchcore\industrial_anomaly\inspection_runtime.py,E:\winForm\patchcore\tests\test_inspection_runtime.py -Algorithm SHA256
```

Record the hashes in the implementation notes. There is no Git commit for this task because `E:\winForm\patchcore` is not a repository.

---

### Task 2: JSONL Python worker

**Files:**
- Create: `E:\winForm\patchcore\industrial_anomaly\winforms_worker.py`
- Create: `E:\winForm\patchcore\tests\test_winforms_worker.py`

**Interfaces:**
- Consumes: `ProductRuntimeConfig` and `InspectionRuntime` from Task 1.
- Produces: commands `ping`, `load_product`, `inspect`, and `shutdown`; responses `{id, ok, result}` or `{id, ok:false, error}` for Task 4.

- [ ] **Step 1: Write failing subprocess protocol tests**

Use a `--fake-runtime` worker option so protocol tests never load models:

```python
class WorkerProtocolTests(unittest.TestCase):
    def test_ready_ping_load_inspect_shutdown(self):
        worker = start_worker("--fake-runtime")
        self.assertEqual("worker_ready", read_json(worker)["event"])
        send_json(worker, {"id": "p1", "command": "ping"})
        self.assertEqual("pong", read_json(worker)["result"]["status"])
        send_json(worker, valid_load_request("l1"))
        self.assertTrue(read_json(worker)["ok"])
        send_json(worker, valid_inspect_request("i1"))
        response = read_json(worker)
        self.assertEqual("i1", response["id"])
        self.assertEqual(1, response["result"]["schema_version"])
        send_json(worker, {"id": "s1", "command": "shutdown"})
        self.assertEqual("shutting_down", read_json(worker)["result"]["status"])
        self.assertEqual(0, worker.wait(timeout=5))

    def test_malformed_json_returns_structured_error(self):
        worker = start_worker("--fake-runtime")
        read_json(worker)
        worker.stdin.write("not-json\n")
        worker.stdin.flush()
        response = read_json(worker)
        self.assertFalse(response["ok"])
        self.assertEqual("invalid_json", response["error"]["code"])
```

- [ ] **Step 2: Run the worker tests and verify failure**

Run:

```powershell
& C:\Users\wlena\anaconda3\envs\pytorch\python.exe -m unittest E:\winForm\patchcore\tests\test_winforms_worker.py -v
```

Expected: FAIL because `winforms_worker.py` does not exist.

- [ ] **Step 3: Implement the worker loop**

The file must start in this order:

```python
import os
os.environ.setdefault("KMP_DUPLICATE_LIB_OK", "TRUE")

import json
import sys
import traceback

PROTOCOL_STDOUT = sys.stdout
sys.stdout = sys.stderr
```

Implement:

```python
def emit(payload: dict) -> None:
    PROTOCOL_STDOUT.write(json.dumps(payload, ensure_ascii=False, separators=(",", ":")) + "\n")
    PROTOCOL_STDOUT.flush()

def error_response(request_id, code, exc):
    return {
        "id": request_id,
        "ok": False,
        "error": {
            "code": code,
            "message": str(exc),
            "traceback": traceback.format_exc(),
        },
    }
```

Validate that every decoded request is an object, has a string `id`, and has a supported string `command`. Map validation errors to `invalid_request`, JSON decode errors to `invalid_json`, missing resources to `resource_missing`, and runtime exceptions to `inference_failed`. Keep processing after request-level errors. Exit on EOF and after responding to `shutdown`.

- [ ] **Step 4: Run the worker tests**

Run:

```powershell
& C:\Users\wlena\anaconda3\envs\pytorch\python.exe -m unittest E:\winForm\patchcore\tests\test_winforms_worker.py -v
```

Expected: all protocol tests PASS and stderr may contain diagnostics while every stdout line remains valid JSON.

- [ ] **Step 5: Record the Python worker checkpoint**

Run:

```powershell
Get-FileHash E:\winForm\patchcore\industrial_anomaly\winforms_worker.py,E:\winForm\patchcore\tests\test_winforms_worker.py -Algorithm SHA256
```

Record the hashes in the implementation notes; no Git commit is possible in the external Python directory.

---

### Task 3: Canonical C# models and result-path resolution

**Files:**
- Modify: `model/DetectionResult.cs`
- Create: `model/InferenceWorkerModels.cs`
- Create: `service/ResultPathService.cs`
- Create: `tests/InferenceWorkerHarness/InferenceWorkerHarness.csproj`
- Create: `tests/InferenceWorkerHarness/Program.cs`
- Modify: `180Detection.csproj`
- Modify: `180Detection.slnx`

**Interfaces:**
- Consumes: canonical schema from Task 1.
- Produces: `InferenceWorkerOptions`, `InferenceProductOptions`, `WorkerState`, `WorkerEnvelope`, `DetectionResult`, and `ResultPathService.CreateInspectionPaths` for Tasks 4-6.

- [ ] **Step 1: Create the no-NuGet harness and failing path tests**

The harness is an `Exe` targeting .NET Framework 4.8 and references `..\..\180Detection.csproj`. Add assertions:

```csharp
private static void TestResultPaths()
{
    DateTimeOffset now = new DateTimeOffset(2026, 8, 14, 14, 30, 25, 123,
        TimeSpan.FromHours(8));
    InspectionPaths paths = ResultPathService.CreateInspectionPaths(
        @"C:\results", now, "abcdef123456");
    Equal(@"C:\results\20260814\20260814_143025_123_abcdef12", paths.Directory);
    Equal(Path.Combine(paths.Directory, "result.json"), paths.ResultJsonPath);
}

private static void TestDefaultProductPaths()
{
    InferenceProductOptions product = ResultPathService.ResolveProduct(
        @"E:\winForm\patchcore", new ProductConfig {
            Name = "Phone", AnomalyThreshold = 0.5, Enabled = true
        });
    Equal("phone", product.ProductKey);
    Equal(@"E:\winForm\patchcore\industrial_anomaly\products\phone\models\patchcore",
        product.PatchCoreModelDirectory);
}
```

- [ ] **Step 2: Run the harness build and verify missing-type failures**

Run:

```powershell
dotnet build tests\InferenceWorkerHarness\InferenceWorkerHarness.csproj --configuration Debug --nologo
```

Expected: FAIL because `InspectionPaths`, `InferenceProductOptions`, and `ResultPathService` do not exist.

- [ ] **Step 3: Implement canonical models and paths**

Add these essential signatures:

```csharp
public enum WorkerState { Stopped, Starting, LoadingProduct, Ready, Inspecting, Faulted }

public sealed class InferenceWorkerOptions
{
    public string PythonExecutable { get; set; }
    public string WorkerScriptPath { get; set; }
    public string WorkingDirectory { get; set; }
    public string ResultDirectory { get; set; }
    public int StartupTimeoutMilliseconds { get; set; }
    public int InspectionTimeoutMilliseconds { get; set; }
}

public sealed class InferenceProductOptions
{
    public string DisplayName { get; set; }
    public string ProductKey { get; set; }
    public string PatchCoreModelDirectory { get; set; }
    public string DefectBankDirectory { get; set; }
    public double AnomalyThreshold { get; set; }
}

public sealed class InspectionPaths
{
    public string Directory { get; set; }
    public string ResultJsonPath { get; set; }
}
```

Extend `DetectionResult` with `SchemaVersion`, `RecordId`, `RecordTime`, `Product`, `Status`, `AnomalyThreshold`, `ThresholdCalibration`, `SourceImagePath`, and `RoiPath`. Keep existing properties so UI callers remain source-compatible.

`ResultPathService` must sanitize only the generated ID, use invariant date formatting, resolve configured relative paths against `AppDomain.CurrentDomain.BaseDirectory`, lowercase the product lookup key, and validate threshold finiteness and non-negativity.

- [ ] **Step 4: Include files and run path tests**

Add the new production files to `180Detection.csproj` and the harness project to `180Detection.slnx`.

Run:

```powershell
dotnet build tests\InferenceWorkerHarness\InferenceWorkerHarness.csproj --configuration Debug --nologo
& tests\InferenceWorkerHarness\bin\Debug\InferenceWorkerHarness.exe --paths-only
```

Expected: build succeeds; harness exits 0 and prints `PATH TESTS PASSED`.

- [ ] **Step 5: Commit the model/path checkpoint**

```powershell
git add 180Detection.csproj 180Detection.slnx model service tests\InferenceWorkerHarness
git commit -m "feat: add inference worker contracts and result paths"
```

---

### Task 4: Persistent C# worker process service

**Files:**
- Create: `service/InferenceWorkerService.cs`
- Create: `tests/InferenceWorkerHarness/fake_worker.py`
- Modify: `tests/InferenceWorkerHarness/Program.cs`
- Modify: `tests/InferenceWorkerHarness/InferenceWorkerHarness.csproj`
- Modify: `180Detection.csproj`

**Interfaces:**
- Consumes: Task 3 options/models and Task 2 JSONL protocol.
- Produces: `StartAsync`, `LoadProductAsync`, `InspectAsync`, `RestartAsync`, `ShutdownAsync`, state events, and `ProcessId` for Task 5 and acceptance tests.

- [ ] **Step 1: Write fake-worker lifecycle tests**

The fake worker immediately emits `worker_ready`; `load_product` succeeds; `inspect` returns a canonical result; image path `__timeout__` does not respond; image path `__crash__` exits with code 23.

Add harness assertions:

```csharp
private static async Task TestWorkerLifecycleAsync()
{
    using (InferenceWorkerService service = CreateFakeService(inspectionTimeoutMs: 1000))
    {
        await service.StartAsync();
        await service.LoadProductAsync(FakeProduct());
        int pid = service.ProcessId;
        DetectionResult result = await service.InspectAsync(FakeImage(), true, true);
        Equal("PASS", result.Status);
        Equal(pid, service.ProcessId);
        Equal(WorkerState.Ready, service.State);
        await service.ShutdownAsync();
        Equal(WorkerState.Stopped, service.State);
    }
}

private static async Task TestTimeoutFaultsWorkerAsync()
{
    using (InferenceWorkerService service = CreateFakeService(inspectionTimeoutMs: 100))
    {
        await service.StartAsync();
        await service.LoadProductAsync(FakeProduct());
        await ThrowsAsync<TimeoutException>(() =>
            service.InspectAsync("__timeout__", true, true));
        Equal(WorkerState.Faulted, service.State);
    }
}
```

- [ ] **Step 2: Run harness and verify service-type failure**

Run:

```powershell
dotnet build tests\InferenceWorkerHarness\InferenceWorkerHarness.csproj --configuration Debug --nologo
```

Expected: FAIL because `InferenceWorkerService` does not exist.

- [ ] **Step 3: Implement process startup and JSONL correlation**

Implement these public members:

```csharp
public sealed class InferenceWorkerService : IDisposable
{
    public event EventHandler<WorkerStateChangedEventArgs> StateChanged;
    public WorkerState State { get; }
    public int ProcessId { get; }
    public string LastError { get; }
    public Task StartAsync();
    public Task LoadProductAsync(InferenceProductOptions product);
    public Task<DetectionResult> InspectAsync(string imagePath, bool saveOriginal, bool saveMarked);
    public Task RestartAsync(InferenceProductOptions product);
    public Task ShutdownAsync();
}
```

Process requirements:

- `FileName = options.PythonExecutable`.
- `Arguments = -u "<worker-script>"` using a dedicated Windows argument-quoting helper.
- Redirect stdin, stdout, and stderr; set `UseShellExecute=false` and `CreateNoWindow=true`.
- Set `KMP_DUPLICATE_LIB_OK=TRUE` and `PYTHONUNBUFFERED=1` in `EnvironmentVariables`.
- Read stdout/stderr asynchronously before waiting for readiness.
- Use `JavaScriptSerializer` and a `ConcurrentDictionary<string, TaskCompletionSource<WorkerEnvelope>>`.
- Use a `SemaphoreSlim(1,1)` across load and inspect calls.
- Generate request IDs with `Guid.NewGuid().ToString("N")`.
- On timeout, process exit, malformed protocol, or write failure: fail pending requests, kill the process if still running, and enter `Faulted`.
- Keep the last 50 stderr lines in a bounded queue for actionable errors.
- Never translate a missing/invalid result into PASS.

- [ ] **Step 4: Run the full fake-worker harness**

Run:

```powershell
$env:TEST_PYTHON_EXE='C:\Users\wlena\anaconda3\envs\pytorch\python.exe'
dotnet build tests\InferenceWorkerHarness\InferenceWorkerHarness.csproj --configuration Debug --nologo
& tests\InferenceWorkerHarness\bin\Debug\InferenceWorkerHarness.exe
```

Expected: path, lifecycle, same-PID, timeout, and crash tests pass; process list shows no orphaned fake worker after exit.

- [ ] **Step 5: Commit the service checkpoint**

```powershell
git add 180Detection.csproj service\InferenceWorkerService.cs tests\InferenceWorkerHarness
git commit -m "feat: manage persistent Python inference worker"
```

---

### Task 5: Product/configuration and detection UI integration

**Files:**
- Modify: `service/ProductConfigService.cs`
- Modify: `page/Index.cs`
- Modify: `page/TabDetection.cs`
- Modify: `page/TabSystemSettings.cs`
- Modify: `App.config`

**Interfaces:**
- Consumes: `InferenceWorkerService` from Task 4 and product/path mapping from Task 3.
- Produces: an asynchronously warmed, product-aware detection workflow and actionable UI states.

- [ ] **Step 1: Add failing product lookup tests to the harness**

Add an injectable/config-path constructor to the test expectation and assert case-insensitive lookup plus cloned return values:

```csharp
private static void TestProductLookup()
{
    ProductConfigService service = ProductConfigFixture.Create();
    ProductConfig phone = service.GetByName("phone");
    Equal("Phone", phone.Name);
    phone.Name = "Mutated";
    Equal("Phone", service.GetByName("PHONE").Name);
}
```

- [ ] **Step 2: Run the harness and verify lookup failure**

Run:

```powershell
dotnet build tests\InferenceWorkerHarness\InferenceWorkerHarness.csproj --configuration Debug --nologo
```

Expected: FAIL because `ProductConfigService.GetByName` and the injectable path constructor do not exist.

- [ ] **Step 3: Implement configuration consumption**

Add `ProductConfigService(string configPath)` for tests while retaining the parameterless constructor. Add:

```csharp
public ProductConfig GetByName(string name)
```

It returns a new `ProductConfig`, compares names case-insensitively, and throws an actionable error when disabled or missing.

Set `App.config` defaults:

```xml
<add key="PythonExecutable" value="C:\Users\wlena\anaconda3\envs\pytorch\python.exe" />
<add key="InferenceScript" value="E:\winForm\patchcore\industrial_anomaly\winforms_worker.py" />
<add key="InferenceWorkingDirectory" value="E:\winForm\patchcore" />
<add key="InferenceStartupTimeoutSeconds" value="300" />
```

Keep `InferenceTimeoutSeconds=120` and `AnomalyThreshold=0.5`. Change settings copy from “推理脚本” to “常驻 Worker 脚本”, display startup timeout, and explain that the arguments template is legacy/unused.

- [ ] **Step 4: Replace the detection orchestration**

In `Index`:

- Construct options from configuration and create `InferenceWorkerService`.
- Start it from an async `OnShown` override so form construction never blocks.
- Load the selected product and update both top-bar and page state.
- On product change, await `LoadProductAsync`; disable detection while loading.
- On detect, call `InspectAsync(selectedImagePath, SaveOriginalImage, SaveMarkedImage)` and display the returned result.
- On settings save, shut down the old worker, create a new service, and warm the selected product.
- On form close, call shutdown with a short synchronous fallback and dispose.

In `TabDetection`, add explicit methods:

```csharp
public void SetWorkerState(WorkerState state, string detail);
public void SetProductReady(string product, double threshold);
```

Ready text must be `模型就绪 · 阈值 0.5000（待校准）`. Detect stays disabled unless image exists, worker is ready, and no inspection/load is active.

- [ ] **Step 5: Run harness and application build**

Run:

```powershell
$env:TEST_PYTHON_EXE='C:\Users\wlena\anaconda3\envs\pytorch\python.exe'
& tests\InferenceWorkerHarness\bin\Debug\InferenceWorkerHarness.exe
dotnet build 180Detection.csproj --configuration Debug --no-restore --nologo
```

Expected: harness passes; application builds with zero warnings and zero errors.

- [ ] **Step 6: Commit the UI/config checkpoint**

```powershell
git add App.config page\Index.cs page\TabDetection.cs page\TabSystemSettings.cs service\ProductConfigService.cs
git commit -m "feat: connect detection UI to persistent inference"
```

---

### Task 6: Canonical record discovery and display

**Files:**
- Modify: `page/TabRecords.cs`
- Create: `service/DetectionRecordReader.cs`
- Modify: `180Detection.csproj`
- Modify: `tests/InferenceWorkerHarness/Program.cs`

**Interfaces:**
- Consumes: Task 1 canonical `result.json` and extended `DetectionResult`.
- Produces: recursive record listing with legacy top-level JSON compatibility.

- [ ] **Step 1: Write failing canonical-record fixture tests**

Extract record loading from UI construction into testable static methods and add fixtures for:

```csharp
private static void TestCanonicalRecordDiscovery()
{
    using (TemporaryResultTree tree = TemporaryResultTree.Create())
    {
        tree.WriteCanonical("20260814/a/result.json", status: "NG", product: "Phone");
        tree.WriteCanonical("20260814/b/result.json", status: "PASS", product: "Phone");
        tree.WriteInvalid("20260814/c/result.json");
        IList<DetectionRecord> records = DetectionRecordReader.ReadAll(tree.Root);
        Equal(2, records.Count);
        Equal("NG", records[0].Status);
    }
}
```

- [ ] **Step 2: Run harness and verify missing reader failure**

Run:

```powershell
dotnet build tests\InferenceWorkerHarness\InferenceWorkerHarness.csproj --configuration Debug --nologo
```

Expected: FAIL because `DetectionRecordReader` and `DetectionRecord` do not exist.

- [ ] **Step 3: Implement recursive canonical discovery**

Create public sealed `DetectionRecord` and public `DetectionRecordReader` types in the focused service file. The reader must:

- enumerate `result.json` recursively;
- require `schema_version == 1` for canonical parsing;
- validate record ID, timestamp, status, finite numeric scores, and absolute/record-relative artifact paths;
- skip `.tmp` and invalid partial files while returning diagnostics counts;
- continue reading legacy top-level `*_result.json` through the existing flexible parser;
- sort newest first;
- never default malformed/missing status to PASS.

Update grid/open-image behavior to prefer marked image, then retained original, then source image. Preserve keyword, status, and time filters.

- [ ] **Step 4: Run record tests and full build**

Run:

```powershell
dotnet build tests\InferenceWorkerHarness\InferenceWorkerHarness.csproj --configuration Debug --nologo
& tests\InferenceWorkerHarness\bin\Debug\InferenceWorkerHarness.exe --records-only
dotnet build 180Detection.csproj --configuration Debug --no-restore --nologo
```

Expected: fixture tests pass; application build reports zero errors.

- [ ] **Step 5: Commit record support**

```powershell
git add page\TabRecords.cs service\DetectionRecordReader.cs tests\InferenceWorkerHarness 180Detection.csproj
git commit -m "feat: read canonical inference records recursively"
```

---

### Task 7: Real-model smoke test, performance measurement, and operator documentation

**Files:**
- Modify: `tests/InferenceWorkerHarness/Program.cs`
- Modify: `README.md`
- Modify: `docs/superpowers/specs/2026-08-14-single-image-inference-worker-design.md` only if verified behavior differs from the design.

**Interfaces:**
- Consumes: the complete worker, C# service, current `phone` model, and an existing test image.
- Produces: verified end-to-end evidence, warmed timing, setup instructions, and final hashes for external Python changes.

- [ ] **Step 1: Add a real-worker smoke mode to the harness**

Add command-line mode:

```text
InferenceWorkerHarness.exe --real-smoke <python.exe> <worker.py> <patchcore-root> <image>
```

It must start one service instance, load `phone`, inspect the same image twice into different unique output directories, print worker PID/load/first/second timings, assert both results have canonical schema and existing artifacts, assert PID equality across both runs, and exit nonzero on any validation failure.

- [ ] **Step 2: Run all automated Python and C# tests**

Run:

```powershell
& C:\Users\wlena\anaconda3\envs\pytorch\python.exe -m unittest discover -s E:\winForm\patchcore\tests -p 'test_*.py' -v
$env:TEST_PYTHON_EXE='C:\Users\wlena\anaconda3\envs\pytorch\python.exe'
dotnet build 180Detection.slnx --configuration Debug --no-restore --nologo
& tests\InferenceWorkerHarness\bin\Debug\InferenceWorkerHarness.exe
```

Expected: all Python tests pass; solution builds; all fake-worker/path/record tests pass.

- [ ] **Step 3: Run the real model twice in one worker**

Resolve a concrete test image first:

```powershell
$image = Get-ChildItem E:\winForm\patchcore\data\phone\test -File | Where-Object Extension -Match '^\.(bmp|jpg|jpeg|png)$' | Select-Object -First 1
& tests\InferenceWorkerHarness\bin\Debug\InferenceWorkerHarness.exe --real-smoke `
  C:\Users\wlena\anaconda3\envs\pytorch\python.exe `
  E:\winForm\patchcore\industrial_anomaly\winforms_worker.py `
  E:\winForm\patchcore `
  $image.FullName
```

Expected: two valid results, same nonzero Python PID, second run reuses the loaded model. Record the second-run wall time and state explicitly whether it is at or below 15 seconds; do not claim the target if the measurement exceeds it.

- [ ] **Step 4: Perform the UI smoke test**

Run `bin\Debug\180Detection.exe`, then verify this exact checklist:

1. Window becomes interactive while the model status shows startup/loading.
2. `Phone` reaches `模型就绪 · 阈值 0.5000（待校准）`.
3. Selecting the same test image enables detection.
4. Detection shows PASS/NG, score, similarity, elapsed time, and marked image.
5. Records page lists the new result and opens its marked image.
6. Closing the application leaves no `winforms_worker.py` Python process.

- [ ] **Step 5: Write operator documentation**

Replace the one-line README with:

- prerequisites and exact tested versions;
- configured Python/worker/model paths;
- explanation of initial warm-up versus warmed inference;
- single-image operating steps;
- result directory schema;
- uncalibrated threshold warning;
- recovery for worker/model/OpenMP errors;
- test/build/smoke commands;
- explicit statement that camera support is not part of this release.

- [ ] **Step 6: Run final verification and capture external hashes**

Run:

```powershell
& C:\Users\wlena\anaconda3\envs\pytorch\python.exe -m unittest discover -s E:\winForm\patchcore\tests -p 'test_*.py' -v
$env:TEST_PYTHON_EXE='C:\Users\wlena\anaconda3\envs\pytorch\python.exe'
dotnet build 180Detection.slnx --configuration Debug --no-restore --nologo
& tests\InferenceWorkerHarness\bin\Debug\InferenceWorkerHarness.exe
git diff --check
git status --short
Get-FileHash E:\winForm\patchcore\industrial_anomaly\inspect_image.py,E:\winForm\patchcore\industrial_anomaly\inspection_runtime.py,E:\winForm\patchcore\industrial_anomaly\winforms_worker.py,E:\winForm\patchcore\tests\test_inspection_runtime.py,E:\winForm\patchcore\tests\test_winforms_worker.py -Algorithm SHA256
```

Expected: all tests/builds pass, `git diff --check` is silent, only intentional WinForms changes plus the two preserved pre-existing untracked files are shown, and external Python hashes are captured in the handoff.

- [ ] **Step 7: Commit documentation and final harness changes**

```powershell
git add README.md tests\InferenceWorkerHarness docs\superpowers
git commit -m "docs: document persistent single-image inference"
```
