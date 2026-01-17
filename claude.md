# Ethernet Speed Monitor

A small WinForms utility that displays current Ethernet link speed in an always-on-top window with a reset button to cycle the adapter.

## Purpose

My ethernet adapter, the Killer E3100G 2.5 Gigabit, is occasionally re-negotiating at lower speeds (10/100/1000 Mbps instead of 2.5 Gbps). This tool monitors the link speed and provides a quick way to reset the adapter to force renegotiation.

## Tech Stack

- .NET 10 WinForms (net10.0-windows)
- `System.Net.NetworkInformation.NetworkInterface` for speed monitoring
- PowerShell `Disable-NetAdapter`/`Enable-NetAdapter` for adapter reset (elevated via UAC)

## Project Structure

```
EthernetMonitor/
├── EthernetMonitor.csproj
├── Program.cs
├── MainForm.cs / MainForm.Designer.cs
├── app.manifest
├── Models/
│   └── EthernetAdapterInfo.cs
└── Services/
    ├── NetworkMonitorService.cs
    └── AdapterResetService.cs
```

## Key Implementation Details

- **Virtual adapter filtering**: `NetworkMonitorService.IsVirtualAdapter()` filters out VirtualBox, VMware, Hyper-V, Tailscale, VPN tunnels, etc. to ensure the physical Ethernet adapter is selected by default.
- **Color coding**: Green (2.5+ Gbps), Yellow (1 Gbps), Red (100 Mbps), Dark Red (10 Mbps)
- **Refresh interval**: 2 seconds
- **Window**: Always-on-top, positioned top-right, `FixedToolWindow` border style
- **Right-click context menu**: Allows manual adapter selection if multiple exist

## Build & Run

```bash
cd EthernetMonitor
dotnet build
dotnet run
```

## Target Adapter

The primary use case is monitoring the **Killer E3100G 2.5 Gigabit Ethernet Controller** which appears as "Ethernet" in Windows.
