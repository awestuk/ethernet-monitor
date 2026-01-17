# Ethernet Speed Monitor

A lightweight Windows utility that displays your Ethernet link speed in an always-on-top window and lets you reset the adapter with one click.

## Why?

Some Ethernet adapters occasionally negotiate at lower speeds than expected. For example, a 2.5 Gbps adapter might connect at 100 Mbps due to cable issues, driver quirks, or switch problems. This tool lets you:

1. **Monitor** - See your current link speed at a glance
2. **Detect** - Color-coded display makes speed drops obvious
3. **Fix** - Reset the adapter to force renegotiation

## Features

- **Always-on-top** compact window in the corner of your screen
- **Color-coded speed display**:
  - 🟢 Green: 2.5+ Gbps
  - 🟡 Yellow: 1 Gbps
  - 🔴 Red: 100 Mbps
  - 🔴 Dark Red: 10 Mbps or less
- **One-click reset** - Disables and re-enables the adapter (prompts for admin)
- **Auto-refresh** every 2 seconds
- **Smart adapter detection** - Filters out virtual adapters (VirtualBox, VMware, Tailscale, etc.)
- **Multi-adapter support** - Right-click to select a different adapter

## Screenshot

```
┌─────────────────────────┐
│ Ethernet Monitor      - │
├─────────────────────────┤
│       2.5 Gbps          │  ← Color-coded
│  Killer E3100G 2.5 GbE  │  ← Adapter name
│    [ Reset Adapter ]    │  ← Triggers reset
└─────────────────────────┘
```

## Requirements

- Windows 10/11
- .NET 10 Runtime (or SDK to build)
- An Ethernet adapter

## Build & Run

```bash
cd EthernetMonitor
dotnet build
dotnet run
```

Or build a standalone executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## How It Works

- Uses `System.Net.NetworkInformation.NetworkInterface` to read adapter speed
- Reset function runs PowerShell `Disable-NetAdapter` / `Enable-NetAdapter` with UAC elevation
- Virtual adapters are filtered by checking for keywords like "virtualbox", "vmware", "tailscale", etc.

## License

MIT
