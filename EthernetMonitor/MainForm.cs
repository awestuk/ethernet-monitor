using EthernetMonitor.Models;
using EthernetMonitor.Services;

namespace EthernetMonitor;

public partial class MainForm : Form
{
    private readonly NetworkMonitorService _networkMonitor;
    private readonly AdapterResetService _adapterReset;
    private string? _selectedAdapterName;
    private long _lastSpeedBps;

    public MainForm()
    {
        InitializeComponent();
        _networkMonitor = new NetworkMonitorService();
        _adapterReset = new AdapterResetService();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        PositionWindow();
        BuildAdapterContextMenu();
        UpdateDisplay();
        refreshTimer.Start();
    }

    private void PositionWindow()
    {
        var screen = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1920, 1080);
        this.Location = new Point(screen.Right - this.Width - 10, screen.Top + 10);
    }

    private void BuildAdapterContextMenu()
    {
        contextMenu.Items.Clear();
        var adapters = _networkMonitor.GetEthernetAdapters().ToList();

        if (adapters.Count > 1)
        {
            foreach (var adapter in adapters)
            {
                var item = new ToolStripMenuItem(adapter.Name)
                {
                    Checked = _selectedAdapterName == adapter.Name ||
                              (_selectedAdapterName == null && adapter == adapters.FirstOrDefault(a => a.IsOperational)),
                    Tag = adapter.Name
                };
                item.Click += (s, e) =>
                {
                    _selectedAdapterName = (string?)((ToolStripMenuItem)s!).Tag;
                    BuildAdapterContextMenu();
                    UpdateDisplay();
                };
                contextMenu.Items.Add(item);
            }
        }
    }

    private void UpdateDisplay()
    {
        EthernetAdapterInfo? adapter;

        if (_selectedAdapterName != null)
        {
            adapter = _networkMonitor.GetAdapterByName(_selectedAdapterName);
        }
        else
        {
            adapter = _networkMonitor.GetPrimaryAdapter();
            _selectedAdapterName = adapter?.Name;
        }

        if (adapter == null)
        {
            lblSpeed.Text = "No Adapter";
            lblSpeed.ForeColor = Color.Gray;
            lblAdapterName.Text = "Connect Ethernet cable";
            btnReset.Enabled = false;
            return;
        }

        if (!adapter.IsOperational)
        {
            lblSpeed.Text = "Disconnected";
            lblSpeed.ForeColor = Color.Gray;
            lblAdapterName.Text = adapter.Description;
            btnReset.Enabled = true;
            return;
        }

        lblSpeed.Text = adapter.FormattedSpeed;
        lblAdapterName.Text = adapter.Description;
        btnReset.Enabled = true;

        // Color coding based on speed
        lblSpeed.ForeColor = adapter.SpeedGbps switch
        {
            >= 2.0 => Color.FromArgb(0, 150, 0),    // Green for 2.5 Gbps+
            >= 1.0 => Color.FromArgb(200, 150, 0),  // Yellow/Orange for 1 Gbps
            >= 0.1 => Color.FromArgb(200, 50, 0),   // Red for 100 Mbps
            _ => Color.FromArgb(139, 0, 0)          // Dark Red for 10 Mbps or less
        };

        // Flash effect when speed changes
        if (_lastSpeedBps != 0 && _lastSpeedBps != adapter.SpeedBps)
        {
            FlashSpeedChange();
        }
        _lastSpeedBps = adapter.SpeedBps;
    }

    private async void FlashSpeedChange()
    {
        var originalColor = this.BackColor;
        for (int i = 0; i < 3; i++)
        {
            this.BackColor = Color.LightYellow;
            await Task.Delay(100);
            this.BackColor = originalColor;
            await Task.Delay(100);
        }
    }

    private void refreshTimer_Tick(object sender, EventArgs e)
    {
        UpdateDisplay();
    }

    private async void btnReset_Click(object sender, EventArgs e)
    {
        if (_selectedAdapterName == null)
        {
            return;
        }

        btnReset.Enabled = false;
        btnReset.Text = "Resetting...";

        try
        {
            var success = await _adapterReset.ResetAdapterAsync(_selectedAdapterName);
            if (!success)
            {
                MessageBox.Show(
                    "Adapter reset was cancelled or failed.",
                    "Reset Adapter",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Failed to reset adapter: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnReset.Text = "Reset Adapter";
            btnReset.Enabled = true;
        }
    }
}
