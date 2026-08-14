# Single-image persistent inference worker design

Date: 2026-08-14

## Goal

Connect the existing WinForms application to the existing PatchCore/DINOv2 demo so a user can select one local image, run production-shaped inference, see the marked result, and browse a durable record. The Python process and the active product model remain loaded between inspections so a warmed-up inspection targets completion within 15 seconds on the current CPU-only workstation.

## Confirmed environment

- WinForms project: `E:\winForm\180Decetion`, .NET Framework 4.8.
- Python project: `E:\winForm\patchcore`.
- Python interpreter: `C:\Users\wlena\anaconda3\envs\pytorch\python.exe`.
- PyTorch environment: PyTorch 2.12 CPU, OpenCV 4.13, NumPy 2.2.6, no NVIDIA GPU.
- Existing single-image CLI: `industrial_anomaly\inspect_image.py`.
- Existing product: `phone`, with PatchCore model and DINOv2 defect banks under `industrial_anomaly\products\phone\models`.
- Images are approximately 5472 x 3648 and use tiled PatchCore inference.
- The current environment has an OpenMP runtime collision. The child worker will set `KMP_DUPLICATE_LIB_OK=TRUE` before importing OpenCV, NumPy, PyTorch, or project modules. This is intentionally scoped to the worker process.

## Scope

### Included

- Manual selection of one local image in the existing detection page.
- A persistent Python worker using JSON Lines over stdin/stdout.
- One loaded product model at a time.
- Product model paths and anomaly threshold taken from product configuration.
- PASS/NG based on `AnomalyThreshold`, shown as an uncalibrated threshold.
- Marked image, anomaly map, ROI, canonical JSON result, and optional original image retention.
- Durable records that the records page can filter and open.
- Worker lifecycle, timeout, restart, protocol, and actionable error handling.
- Automated protocol/normalization tests plus a real-model smoke test.

### Excluded

- Camera discovery, preview, acquisition, and triggering.
- Model training or defect-bank construction.
- Statistical calibration of the anomaly threshold.
- Parallel inspections, multi-worker scheduling, HTTP services, or remote inference.
- Redesign of inactive legacy pages (`TabMain`, `TabTemplate`, `TabAnalysis`).

## Architecture

The existing `Index` form remains the UI coordinator. A new `InferenceWorkerService` owns a single Python child process. The child runs a new `winforms_worker.py` entry point and keeps one `InspectionRuntime` loaded. Requests and responses are newline-delimited JSON.

```text
TabDetection
    -> Index
        -> InferenceWorkerService
            -> pytorch/python.exe -u winforms_worker.py
                -> InspectionRuntime
                    -> existing PatchCoreDINOv2Pipeline and DefectExemplarBank
                -> runtime/results/<date>/<inspection-id>/
        <- canonical DetectionResult
    <- marked image and decision

TabRecords -> recursively reads canonical result.json files
```

The algorithm implementation remains unchanged. The new runtime composes the existing pipeline and banks, moving only the orchestration currently embedded in `inspect_image.py` into a reusable, long-lived object. The existing CLI will continue to work by calling the same runtime.

## Python components

### `InspectionRuntime`

`InspectionRuntime` has a clear two-stage lifecycle:

1. `load_product(config)` loads PatchCore, CLS bank, and patch-center bank once.
2. `inspect(image_path, output_dir, record_metadata)` runs the existing ROI extraction, embeddings, score fusion, decision, and artifact generation.

Only the active product is retained. Loading a different product disposes references to the previous runtime and runs garbage collection before loading the next product. A repeated load with the same normalized configuration is a no-op.

The runtime accepts:

- product key;
- PatchCore model directory;
- defect-bank directory;
- anomaly threshold;
- device, fixed to `cpu` by default;
- the existing bbox/ROI parameters with their current defaults.

It returns a canonical dictionary and atomically writes the same dictionary to `result.json` using a temporary file followed by replacement.

### `winforms_worker.py`

The worker:

- sets `KMP_DUPLICATE_LIB_OK=TRUE` before native-library imports;
- saves the original stdout stream for protocol responses;
- redirects ordinary Python and algorithm stdout to stderr so logs cannot corrupt JSONL;
- emits one compact JSON object per line and flushes every response;
- processes one command at a time;
- exits cleanly on EOF or `shutdown`.

Supported commands:

```json
{"id":"1","command":"load_product","product":"phone","patchcore_model_dir":"...","bank_dir":"...","anomaly_threshold":0.5}
{"id":"2","command":"inspect","record_id":"...","record_time":"...","image_path":"...","output_dir":"...","save_original":true,"save_marked":true}
{"id":"3","command":"ping"}
{"id":"4","command":"shutdown"}
```

Every response echoes `id`, contains `ok`, and has either `result` or an error object with `code`, `message`, and a stderr-safe traceback. Startup emits an unsolicited `worker_ready` event; successful model loading emits product metadata.

## WinForms components

### `InferenceWorkerService`

The service replaces the per-inspection process launch in the current `InferenceService`. It is responsible for:

- resolving configuration and validating files before launch;
- launching the configured interpreter directly with `-u`, not through `conda run`;
- setting the OpenMP environment variable on `ProcessStartInfo` as a second guard;
- asynchronously draining both stdout and stderr;
- parsing JSONL responses and matching them to requests;
- serializing requests so only one model load or inspection is active;
- applying startup, model-load, and inspection timeouts;
- detecting process exit or malformed protocol;
- graceful shutdown with forced termination only after a short deadline;
- restart after settings changes or a worker failure.

The first model load happens in the background after the form becomes visible. Initial model loading is not included in the 15-second inspection target. Detection is enabled only after a product is ready.

### Product configuration

The existing fields become operational:

- `PatchCoreModelDirectory` selects the model directory.
- `DefectBankDirectory` selects the bank directory.
- `AnomalyThreshold` is passed to the runtime.
- `SimilarityThreshold` remains persisted and displayed but does not alter PASS/NG in this phase; the existing Python fusion continues to report similarity and margin.

If model paths are blank, they resolve under:

```text
<InferenceWorkingDirectory>\industrial_anomaly\products\<lowercase-product>\models\patchcore
<InferenceWorkingDirectory>\industrial_anomaly\products\<lowercase-product>\models\defect_bank
```

The display name remains unchanged in records; only the Python lookup key is lowercased.

### System configuration

The existing settings are retained with these meanings:

- `PythonExecutable`: direct path to the environment's `python.exe`.
- `InferenceScript`: path to `winforms_worker.py`.
- `InferenceWorkingDirectory`: `E:\winForm\patchcore` for the current workstation.
- `InferenceResultDirectory`: root for durable inspection folders.
- `InferenceTimeoutSeconds`: per-image timeout; default 120 seconds.
- `SaveOriginalImage` and `SaveMarkedImage`: artifact retention policy.

`InferenceArgumentsTemplate` remains in configuration for backward compatibility but is unused in worker mode. A new `InferenceStartupTimeoutSeconds` defaults to 300 seconds because CPU model loading can be slow.

## UI state and data flow

The detection page exposes these mutually exclusive states:

1. Worker starting.
2. Product model loading.
3. Ready, with text such as `模型就绪 · 阈值 0.5000（待校准）`.
4. Inspecting.
5. Result shown.
6. Worker unavailable, with a retry action or a settings instruction.

The flow for one image is:

1. User selects an existing image.
2. UI verifies that the worker is ready and disables duplicate submission.
3. WinForms creates a unique record ID and output directory.
4. The worker validates and loads the image, performs inference, writes artifacts, writes canonical JSON, and returns the canonical result.
5. WinForms displays `full_marked.jpg` when retained and available, otherwise the source image.
6. The records page refreshes and finds the new canonical record recursively.
7. UI re-enables detection regardless of success or failure.

Changing the selected product triggers an asynchronous model reload. Detection remains disabled until that product reports ready.

## Record format and storage

Each inspection owns one directory:

```text
runtime/results/YYYYMMDD/YYYYMMDD_HHmmss_fff_<short-id>/
├── original.<ext>       # only when SaveOriginalImage=true
├── full_marked.jpg      # only when SaveMarkedImage=true
├── anomaly_map.png
├── roi.png
└── result.json
```

`result.json` contains at least:

```json
{
  "schema_version": 1,
  "record_id": "...",
  "record_time": "2026-08-14T14:30:25.123+08:00",
  "product": "Phone",
  "status": "NG",
  "is_ng": true,
  "defect_class": "shao1",
  "anomaly_score": 0.72,
  "anomaly_threshold": 0.5,
  "threshold_calibration": "uncalibrated",
  "similarity": 0.91,
  "margin": 0.18,
  "elapsed_ms": 8300,
  "source_image_path": "...",
  "image_path": "...",
  "marked_image_path": "...",
  "heatmap_path": "...",
  "roi_path": "..."
}
```

Paths are absolute in worker responses and persisted records. The records page reads only files with `schema_version: 1`; legacy top-level JSON remains readable through the existing fallback parser.

## Error handling

- Missing interpreter, worker, model, bank, or image is rejected before expensive work and shown with the exact missing path.
- A Python exception returns a structured error while the worker remains alive when safe.
- A model-load exception marks the product unavailable until retry or configuration change.
- An inspection timeout kills and restarts the worker because canceling native inference in place is unsafe.
- Unexpected process exit fails the pending request, records recent stderr, and exposes a restart action.
- Malformed stdout is treated as a protocol error, logged, and never interpreted as a PASS result.
- Missing or non-finite scores fail the inspection rather than defaulting to zero or PASS.
- Output is written to a unique directory; a failed inspection may leave diagnostics but is not listed as a completed record unless a valid canonical `result.json` exists.
- Settings and product changes during inference are deferred until the active request finishes.

## Performance policy

- Initial worker and model warm-up may exceed 15 seconds and is reported separately.
- The warmed-up target is 15 seconds from submitting an image to receiving the canonical result.
- The implementation records model-load and per-inspection wall times.
- No unsupported CPU parallelism tuning is included initially. The measured real-model smoke test determines whether further profiling is required.

## Testing and acceptance

Automated coverage will include:

- Python unit tests for command validation, result normalization, threshold decisions, artifact paths, and structured errors with the model layer mocked.
- Worker protocol tests for ready, load, inspect, malformed request, ping, and shutdown behavior.
- A standalone .NET Framework 4.8 console test harness under `tests\InferenceWorkerHarness`, with no NuGet dependency, plus a deterministic fake Python worker. It covers JSONL parsing, request correlation, timeout, process exit, and path resolution.
- A complete WinForms Debug build with zero errors.
- A real-model smoke test using one existing `data\phone\test` image and the `pytorch` environment.

Acceptance criteria:

1. Application startup remains responsive while the worker warms up.
2. After the `Phone` model is ready, selecting one valid image and clicking detect produces a PASS/NG result using threshold 0.5.
3. The marked image is displayed and a canonical record is visible in the records page.
4. A second inspection reuses the same Python PID and loaded model.
5. Missing configuration and worker crashes produce actionable errors without crashing WinForms.
6. OpenMP startup succeeds without changing the parent process environment.
7. The warmed-up real-image time is measured and reported against the 15-second target.

## Delivery sequence

1. Add tests for the Python runtime and JSONL worker protocol.
2. Extract reusable inspection orchestration and add the persistent worker.
3. Add C# protocol models and worker process service with a fake-worker test path.
4. Integrate product loading and single-image detection into the current UI.
5. Standardize result persistence and update record discovery.
6. Configure the current interpreter, worker, working directory, and product defaults.
7. Run automated tests, full build, worker smoke tests, and a warmed-up real-model benchmark.
