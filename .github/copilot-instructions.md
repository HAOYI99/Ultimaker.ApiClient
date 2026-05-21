# Copilot instructions: Ultimaker.ApiClient

## Build, test, pack

Run from repo root:

```bash
dotnet restore
dotnet build Ultimaker.ApiClient.sln
dotnet test Ultimaker.ApiClient.sln
dotnet pack Ultimaker.ApiClient.Core/Ultimaker.ApiClient.Core.csproj -c Release
```

Single test:

```bash
dotnet test Ultimaker.ApiClient.Tests/Ultimaker.ApiClient.Tests.csproj --filter "FullyQualifiedName~Ultimaker.ApiClient.Tests.Services.PrintJobServiceTest.GetJob_NotFinish"
```

No dedicated lint cmd in repo.

## Architecture map

- 2 projects: `Ultimaker.ApiClient.Core` (NuGet lib), `Ultimaker.ApiClient.Tests` (xUnit + mocked HTTP).
- `UltimakerClient` (`Ultimaker.ApiClient.Core/UltimakerClient.cs`) = main facade. Owns `HttpClient`. Exposes services: `Auth`, `Material`, `Printer`, `PrintJob`, `System`, `History`, `AirManager`.
- Services in `Ultimaker.ApiClient.Core/Services/`. Shared base: `ServiceBase` (HTTP + JSON flow).
- Routes centralized in `Ultimaker.ApiClient.Core/Constants/UltimakerPaths.cs`. Use constants/builders, not inline route strings.
- DTO split: `Dto/Request`, `Dto/Response`. Enum mapping in `Enums/`.
- API return type: `UltimakerApiResponse<T>` (data, success, status, message, raw response).

## Code rules

- JSON in `ServiceBase` must keep:
  - `DateTimeZoneHandling.Utc`
  - `StringEnumConverter`
- `ServiceBase.GetAsync<T>` behavior: 404 -> non-throwing, `Data == default`; other non-success -> `EnsureSuccessStatusCode()`.
- Privileged/mutating calls must run `EnsureHasCredential()` and throw `MissingCredentialException` if no creds.
  - Examples: `PrintJob.Start/Pause/Resume/Stop`, `PrintJob.GetGCode/GetContainer`, `System.SetName`.
- `PrintJobService` uses min 1-minute timeout for long GCode fetch. Reuse same pattern for slow/large endpoints.
- `UltimakerClient.UpdateCred(...)` rebuilds `HttpClient` + all services. Do not cache service refs across cred updates.
- Tests use `RichardSzalay.MockHttp` + concrete `BaseAddress`. Assert DTO deserialize + status behavior, not status only.
- Version source: `Ultimaker.ApiClient.Core/Ultimaker.ApiClient.nuspec`. Keep README install snippet in sync.

## LLM text style

- LLM text must be short, direct, technical.
- Keep substance high. Kill filler, repetition, hedging.
- Prefer concrete file path, symbol name, command over vague text.
