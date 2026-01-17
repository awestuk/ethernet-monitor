using System.Net.NetworkInformation;
using EthernetMonitor.Models;

namespace EthernetMonitor.Services;

/// <summary>
/// Service for monitoring Ethernet network adapters.
/// </summary>
public class NetworkMonitorService
{
    /// <summary>
    /// Gets all Ethernet adapters on the system.
    /// </summary>
    public IEnumerable<EthernetAdapterInfo> GetEthernetAdapters()
    {
        return NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            .Select(n => new EthernetAdapterInfo(
                Id: n.Id,
                Name: n.Name,
                Description: n.Description,
                SpeedBps: n.Speed,
                IsOperational: n.OperationalStatus == OperationalStatus.Up
            ));
    }

    /// <summary>
    /// Gets the primary Ethernet adapter (first operational physical adapter).
    /// Filters out known virtual adapters.
    /// </summary>
    public EthernetAdapterInfo? GetPrimaryAdapter()
    {
        return GetEthernetAdapters()
            .Where(a => a.IsOperational && !IsVirtualAdapter(a))
            .OrderByDescending(a => a.SpeedBps)
            .FirstOrDefault();
    }

    /// <summary>
    /// Checks if an adapter is a virtual/software adapter.
    /// </summary>
    private static bool IsVirtualAdapter(EthernetAdapterInfo adapter)
    {
        var virtualKeywords = new[]
        {
            "virtualbox", "vmware", "hyper-v", "virtual",
            "tailscale", "tunnel", "vpn", "tap-", "tun-"
        };

        var descLower = adapter.Description.ToLowerInvariant();
        var nameLower = adapter.Name.ToLowerInvariant();

        return virtualKeywords.Any(keyword =>
            descLower.Contains(keyword) || nameLower.Contains(keyword));
    }

    /// <summary>
    /// Gets an adapter by its name.
    /// </summary>
    public EthernetAdapterInfo? GetAdapterByName(string name)
    {
        return GetEthernetAdapters()
            .FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
