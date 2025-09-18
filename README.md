# Ultimaker.ApiClient 🖨️

**Not official library for Ultimaker APIs 😅**

Provides full API client ⚙️ functionality to interact with Ultimaker devices.

Feel free to contribute ✍️ or report issues 🐛. Let's make it better together!

---

## Features ✨

* Access Ultimaker printers 🖨️, materials 🧱, and other API endpoints 🌐
* Authentication 🔑 support
* All datetime returns in UTC 🕒
* Fully async ⚡ API calls

---

## Installation 📥

### NuGet
```bash
dotnet add package Ultimaker.ApiClient --version 1.0.0
```

### PackageReference
```xml
<PackageReference Include="Ultimaker.ApiClient" Version="1.0.0" />
```

---

## Usage 🛠️

### Basic Example
```csharp
var client = new UltimakerClient("http://YOUR_HOST_HERE");

// get current print job 🖨️
var printJob = await client.PrintJob.Get();
```

### Advanced Example with Authentication 🔐
```csharp
var client = new UltimakerClient("http://YOUR_HOST_HERE", "YOUR_USERNAME", "YOUR_PASSWORD");

// get gcode 📄
var gcode = await client.PrintJob.GetGCode();
```

---

## Configuration ⚙️

* **Authentication 🔑**: Provide username/password when creating the client

---

## License 📜

MIT License. See [LICENSE](https://github.com/HAOYI99/Ultimaker.ApiClient/blob/main/LICENSE) for details.

---

## Repository 🏛️

[GitHub Repository](https://github.com/HAOYI99/Ultimaker.ApiClient)****