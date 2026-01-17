using System.Diagnostics;

namespace EthernetMonitor.Services;

/// <summary>
/// Service for resetting Ethernet adapters.
/// </summary>
public class AdapterResetService
{
    /// <summary>
    /// Resets an adapter by disabling and re-enabling it.
    /// Requires administrator privileges (will prompt for UAC).
    /// </summary>
    /// <param name="adapterName">The name of the adapter to reset.</param>
    /// <returns>True if the reset was initiated successfully, false if user cancelled UAC.</returns>
    public async Task<bool> ResetAdapterAsync(string adapterName)
    {
        try
        {
            var escapedName = adapterName.Replace("'", "''");
            var command = $"Disable-NetAdapter -Name '{escapedName}' -Confirm:$false; " +
                          $"Start-Sleep 2; " +
                          $"Enable-NetAdapter -Name '{escapedName}' -Confirm:$false";

            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-Command \"{command}\"",
                Verb = "RunAs",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                return false;
            }

            await process.WaitForExitAsync();
            return process.ExitCode == 0;
        }
        catch (System.ComponentModel.Win32Exception ex) when (ex.NativeErrorCode == 1223)
        {
            // User cancelled UAC prompt
            return false;
        }
    }
}
