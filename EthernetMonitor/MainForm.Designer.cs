namespace EthernetMonitor;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.lblSpeed = new Label();
        this.lblAdapterName = new Label();
        this.btnReset = new Button();
        this.refreshTimer = new System.Windows.Forms.Timer(this.components);
        this.contextMenu = new ContextMenuStrip(this.components);
        this.SuspendLayout();

        // lblSpeed
        this.lblSpeed.Dock = DockStyle.Top;
        this.lblSpeed.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        this.lblSpeed.ForeColor = Color.Green;
        this.lblSpeed.Location = new Point(0, 0);
        this.lblSpeed.Name = "lblSpeed";
        this.lblSpeed.Size = new Size(200, 45);
        this.lblSpeed.TabIndex = 0;
        this.lblSpeed.Text = "-- Gbps";
        this.lblSpeed.TextAlign = ContentAlignment.MiddleCenter;

        // lblAdapterName
        this.lblAdapterName.Dock = DockStyle.Top;
        this.lblAdapterName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        this.lblAdapterName.ForeColor = Color.Gray;
        this.lblAdapterName.Location = new Point(0, 45);
        this.lblAdapterName.Name = "lblAdapterName";
        this.lblAdapterName.Size = new Size(200, 20);
        this.lblAdapterName.TabIndex = 1;
        this.lblAdapterName.Text = "No adapter found";
        this.lblAdapterName.TextAlign = ContentAlignment.MiddleCenter;

        // btnReset
        this.btnReset.Location = new Point(40, 75);
        this.btnReset.Name = "btnReset";
        this.btnReset.Size = new Size(120, 30);
        this.btnReset.TabIndex = 2;
        this.btnReset.Text = "Reset Adapter";
        this.btnReset.UseVisualStyleBackColor = true;
        this.btnReset.Click += new EventHandler(this.btnReset_Click);

        // refreshTimer
        this.refreshTimer.Interval = 2000;
        this.refreshTimer.Tick += new EventHandler(this.refreshTimer_Tick);

        // contextMenu
        this.contextMenu.Name = "contextMenu";
        this.contextMenu.Size = new Size(181, 26);

        // MainForm
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(200, 115);
        this.Controls.Add(this.btnReset);
        this.Controls.Add(this.lblAdapterName);
        this.Controls.Add(this.lblSpeed);
        this.ContextMenuStrip = this.contextMenu;
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "MainForm";
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.Manual;
        this.Text = "Ethernet Monitor";
        this.TopMost = true;
        this.Load += new EventHandler(this.MainForm_Load);
        this.ResumeLayout(false);
    }

    private Label lblSpeed;
    private Label lblAdapterName;
    private Button btnReset;
    private System.Windows.Forms.Timer refreshTimer;
    private ContextMenuStrip contextMenu;
}
