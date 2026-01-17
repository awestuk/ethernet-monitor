namespace EthernetMonitor.Models;

/// <summary>
/// Represents information about an Ethernet adapter.
/// </summary>
public record EthernetAdapterInfo(
    string Id,
    string Name,
    string Description,
    long SpeedBps,
    bool IsOperational)
{
    /// <summary>
    /// Gets the formatted speed string (e.g., "2.5 Gbps", "1 Gbps", "100 Mbps").
    /// </summary>
    public string FormattedSpeed => SpeedBps switch
    {
        >= 2_000_000_000 => $"{SpeedBps / 1_000_000_000.0:0.#} Gbps",
        >= 1_000_000_000 => $"{SpeedBps / 1_000_000_000.0:0.#} Gbps",
        >= 1_000_000 => $"{SpeedBps / 1_000_000} Mbps",
        >= 1_000 => $"{SpeedBps / 1_000} Kbps",
        _ => $"{SpeedBps} bps"
    };

    /// <summary>
    /// Gets the speed in Gbps for comparison purposes.
    /// </summary>
    public double SpeedGbps => SpeedBps / 1_000_000_000.0;
}
