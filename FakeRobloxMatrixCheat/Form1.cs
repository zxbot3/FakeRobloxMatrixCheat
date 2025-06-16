using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FakeRobloxMatrixCheat
{
    public partial class Form1 : Form
    {
        private Keys _toggleKey = Keys.None;
        private bool _bindingMode = false;
        private const int WM_HOTKEY = 0x0312;
        private int _hotkeyId = 1;

        // Declare controls at class level
        private CheckBox cbESPBox;
        private ComboBox cbESPType;
        private CheckBox cbESPFilled;
        private CheckBox cbESPDistance;
        private CheckBox cbESPName;
        private CheckBox cbESPHealth;
        private CheckBox cbESPSnapLine;

        // New controls for health check
        private CheckBox cbHealthCheck;
        private Label lblHealthCondition;
        private Label lblHealthValue;
        private Button btnHealthMinus;
        private Button btnHealthPlus;
        private int healthMinValue = 0;

        // Declare Aimbot controls at class level
        private CheckBox cbAimbot;
        private TextBox tbAimbotKey;
        private Label lblSortType;
        private ComboBox cbSortType;
        private Label lblSelectPart;
        private ComboBox cbSelectPart;
        private Label lblSmooth;
        private TrackBar tbSmooth;
        private Label lblSmoothVal;
        private CheckBox cbToggleAimbot;
        private CheckBox cbPredict;
        private Label lblPredictMath;
        private ComboBox cbPredictMath;
        private Label lblPredictX;
        private TrackBar tbPredictX;
        private Label lblPredictY;
        private TrackBar tbPredictY;
        private CheckBox cbSticky;
        private CheckBox cbSilentAim;

        // Declare Misc controls at class level
        private CheckBox cbNoclip;
        private TextBox tbNoclipKey;
        private Label lblNoclipSpeed;
        private TrackBar tbNoclipSpeed;
        private CheckBox cbFlight;
        private TextBox tbFlightKey;
        private Label lblFlightSpeed;
        private TrackBar tbFlightSpeed;
        private Label lblFlightMode;
        private ComboBox cbFlightMode;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true; // Allow form to receive key events before controls
            InitializeVisualToggles();
            InitializeAimbotControls();
            InitializeMiscControls();

            // Wire up button2's Click event
            button2.Click += button2_Click;
            button3.Click += button3_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            _bindingMode = true;
            MessageBox.Show("Press the key you want to bind to toggle the MTX GUI.", "Key Bind", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_bindingMode)
            {
                if (_toggleKey != Keys.None)
                {
                    // Unregister previous hotkey
                    UnregisterHotKey(this.Handle, _hotkeyId);
                }

                _toggleKey = keyData;
                _bindingMode = false;

                // Register new hotkey (no modifiers)
                RegisterHotKey(this.Handle, _hotkeyId, 0, (uint)_toggleKey);

                MessageBox.Show($"Key '{_toggleKey}' bound. Press OK to confirm.", "MTX KeyBind", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true; // Consume the key
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                this.Visible = !this.Visible;
                if (this.Visible)
                {
                    this.Activate();
                }
            }
            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_toggleKey != Keys.None)
            {
                UnregisterHotKey(this.Handle, _hotkeyId);
            }
            base.OnFormClosing(e);
        }

        private void InitializeVisualToggles()
        {
            int yOffset = 130 - 50;
            int rightMargin = 50;

            // Move health check controls further down and to the right
            int healthYOffset = 80; // Down
            int healthXOffset = 220; // Right

            // Health Check controls (moved down and right)
            cbHealthCheck = new CheckBox()
            {
                Text = "Health Check",
                Location = new Point(20 + healthXOffset, 10 + yOffset + healthYOffset),
                AutoSize = true,
                Visible = false
            };

            lblHealthCondition = new Label()
            {
                Text = "Health Condition (Min):",
                Location = new Point(20 + healthXOffset, 35 + yOffset + healthYOffset),
                AutoSize = true,
                ForeColor = Color.White,
                Visible = false
            };

            lblHealthValue = new Label()
            {
                Text = healthMinValue.ToString(),
                Location = new Point(170 + healthXOffset, 35 + yOffset + healthYOffset),
                AutoSize = true,
                ForeColor = Color.White,
                Visible = false
            };

            btnHealthMinus = new Button()
            {
                Text = "-",
                Location = new Point(200 + healthXOffset, 30 + yOffset + healthYOffset),
                Size = new Size(25, 25),
                Visible = false
            };
            btnHealthMinus.Click += BtnHealthMinus_Click;

            btnHealthPlus = new Button()
            {
                Text = "+",
                Location = new Point(230 + healthXOffset, 30 + yOffset + healthYOffset),
                Size = new Size(25, 25),
                Visible = false
            };
            btnHealthPlus.Click += BtnHealthPlus_Click;

            // Existing module controls, moved further up
            int moduleYOffset = yOffset + 60;
            cbESPBox = new CheckBox() { Text = "ESP Box", Location = new Point(20, 20 + moduleYOffset), AutoSize = true, Visible = false };
            cbESPType = new ComboBox() { Location = new Point(120, 18 + moduleYOffset), Width = 80, Visible = false };
            cbESPType.Items.Add("2D");
            cbESPType.SelectedIndex = 0;

            cbESPFilled = new CheckBox() { Text = "ESP Filled", Location = new Point(20, 60 + moduleYOffset), AutoSize = true, Visible = false };
            cbESPDistance = new CheckBox() { Text = "ESP Distance", Location = new Point(20, 100 + moduleYOffset), AutoSize = true, Visible = false };
            cbESPName = new CheckBox() { Text = "ESP Name", Location = new Point(20, 140 + moduleYOffset), AutoSize = true, Visible = false };
            cbESPHealth = new CheckBox() { Text = "ESP Health", Location = new Point(20, 180 + moduleYOffset), AutoSize = true, Visible = false };
            cbESPSnapLine = new CheckBox() { Text = "ESP SnapLine", Location = new Point(20, 220 + moduleYOffset), AutoSize = true, Visible = false };

            this.Controls.AddRange(new Control[] {
                cbHealthCheck, lblHealthCondition, lblHealthValue, btnHealthMinus, btnHealthPlus,
                cbESPBox, cbESPType, cbESPFilled, cbESPDistance, cbESPName, cbESPHealth, cbESPSnapLine
            });
        }

        private void BtnHealthMinus_Click(object sender, EventArgs e)
        {
            if (healthMinValue > 0)
            {
                healthMinValue--;
                lblHealthValue.Text = healthMinValue.ToString();
            }
        }

        private void BtnHealthPlus_Click(object sender, EventArgs e)
        {
            healthMinValue++;
            lblHealthValue.Text = healthMinValue.ToString();
        }

        // Call this from button1's Click event
        private void button1_Click(object sender, EventArgs e)
        {
            // Hide aimbot and misc controls when showing visual toggles
            ShowAimbotControls(false);
            ShowMiscControls(false);
            ShowVisualToggles(true);
        }

        // Call this from any other button's Click event
        private void OtherButton_Click(object sender, EventArgs e)
        {
            ShowVisualToggles(false);
        }

        private void ShowVisualToggles(bool show)
        {
            Color foreColor = show ? Color.White : SystemColors.ControlText;

            cbHealthCheck.Visible = show;
            lblHealthCondition.Visible = show;
            lblHealthValue.Visible = show;
            btnHealthMinus.Visible = show;
            btnHealthPlus.Visible = show;

            lblHealthCondition.ForeColor = foreColor;
            lblHealthValue.ForeColor = foreColor;

            cbESPBox.Visible = show;
            cbESPType.Visible = show;
            cbESPFilled.Visible = show;
            cbESPDistance.Visible = show;
            cbESPName.Visible = show;
            cbESPHealth.Visible = show;
            cbESPSnapLine.Visible = show;

            cbESPBox.ForeColor = foreColor;
            cbESPFilled.ForeColor = foreColor;
            cbESPDistance.ForeColor = foreColor;
            cbESPName.ForeColor = foreColor;
            cbESPHealth.ForeColor = foreColor;
            cbESPSnapLine.ForeColor = foreColor;
        }

        private void InitializeAimbotControls()
        {
            int rightX = 500; // Right-side X position for aimbot options
            int downY = 150;  // Down offset for all aimbot controls
            int upAdjust = 120; // Move left-side controls up by 120

            // Right-side controls (stay in place)
            cbAimbot = new CheckBox { Text = "Aimbot", Location = new Point(rightX, downY + 0), Visible = false };
            tbAimbotKey = new TextBox { Text = "N/A", ReadOnly = true, Location = new Point(rightX + 100, downY + 0), Width = 60, Visible = false };
            lblSortType = new Label { Text = "Sort Type", Location = new Point(rightX, downY + 40), Visible = false };
            cbSortType = new ComboBox { Location = new Point(rightX + 100, downY + 40), Width = 150, Visible = false };
            cbSortType.Items.AddRange(new string[] { "Near Crosshair", "Closest", "Random" });
            cbSortType.SelectedIndex = 0;
            lblSelectPart = new Label { Text = "Select Part", Location = new Point(rightX, downY + 80), Visible = false };
            cbSelectPart = new ComboBox { Location = new Point(rightX + 100, downY + 80), Width = 150, Visible = false };
            cbSelectPart.Items.AddRange(new string[] { "Head", "Body", "Legs" });
            cbSelectPart.SelectedIndex = 0;
            cbSilentAim = new CheckBox { Text = "SilentAim", Location = new Point(rightX, downY + 0), Visible = false };

            // Left-side controls (move up by 120)
            lblSmooth = new Label { Text = "Smooth", Location = new Point(20, downY + 120 - upAdjust), Visible = false };
            tbSmooth = new TrackBar { Minimum = 0, Maximum = 1000, Value = 100, TickStyle = TickStyle.None, Width = 200, Location = new Point(120, downY + 120 - upAdjust), Visible = false };
            lblSmoothVal = new Label { Text = "0.100", Location = new Point(330, downY + 120 - upAdjust), Visible = false };
            tbSmooth.Scroll += (s, e) => lblSmoothVal.Text = (tbSmooth.Value / 1000.0).ToString("0.000");

            cbToggleAimbot = new CheckBox { Text = "Toggle Aimbot", Location = new Point(20, downY + 160 - upAdjust), Visible = false };
            cbPredict = new CheckBox { Text = "Predict", Location = new Point(20, downY + 200 - upAdjust), Visible = false };

            lblPredictMath = new Label { Text = "Predict Math Aimbot", Location = new Point(20, downY + 240 - upAdjust), Visible = false };
            cbPredictMath = new ComboBox { Location = new Point(180, downY + 240 - upAdjust), Width = 100, Visible = false };
            cbPredictMath.Items.AddRange(new string[] { "Divide", "Multiply" });
            cbPredictMath.SelectedIndex = 0;

            lblPredictX = new Label { Text = "Predict X", Location = new Point(20, downY + 280 - upAdjust), Visible = false };
            tbPredictX = new TrackBar { Minimum = 0, Maximum = 1000, Value = 0, TickStyle = TickStyle.None, Width = 200, Location = new Point(120, downY + 280 - upAdjust), Visible = false };

            lblPredictY = new Label { Text = "Predict Y", Location = new Point(20, downY + 320 - upAdjust), Visible = false };
            tbPredictY = new TrackBar { Minimum = 0, Maximum = 1000, Value = 0, TickStyle = TickStyle.None, Width = 200, Location = new Point(120, downY + 320 - upAdjust), Visible = false };

            cbSticky = new CheckBox { Text = "Sticky", Location = new Point(20, downY + 360 - upAdjust), Visible = false };

            this.Controls.AddRange(new Control[] {
                cbAimbot, tbAimbotKey,
                lblSortType, cbSortType,
                lblSelectPart, cbSelectPart,
                lblSmooth, tbSmooth, lblSmoothVal,
                cbToggleAimbot,
                cbPredict,
                lblPredictMath, cbPredictMath,
                lblPredictX, tbPredictX,
                lblPredictY, tbPredictY,
                cbSticky, cbSilentAim
            });
        }

        private void ShowAimbotControls(bool show)
        {
            cbAimbot.Visible = show;
            tbAimbotKey.Visible = show;
            lblSortType.Visible = show;
            cbSortType.Visible = show;
            lblSelectPart.Visible = show;
            cbSelectPart.Visible = show;
            lblSmooth.Visible = show;
            tbSmooth.Visible = show;
            lblSmoothVal.Visible = show;
            cbToggleAimbot.Visible = show;
            cbPredict.Visible = show;
            lblPredictMath.Visible = show;
            cbPredictMath.Visible = show;
            lblPredictX.Visible = show;
            tbPredictX.Visible = show;
            lblPredictY.Visible = show;
            tbPredictY.Visible = show;
            cbSticky.Visible = show;
            cbSilentAim.Visible = show;
        }

        private void InitializeMiscControls()
        {
            int miscYOffset = 120; // Move all misc controls down by 100 pixels

            cbNoclip = new CheckBox { Text = "Noclip", Location = new Point(20, 20 + miscYOffset), Visible = false };
            tbNoclipKey = new TextBox { Text = "Keybind", Location = new Point(120, 20 + miscYOffset), Width = 70, Visible = false };

            lblNoclipSpeed = new Label { Text = "Noclip Speed", Location = new Point(20, 60 + miscYOffset), Visible = false };
            tbNoclipSpeed = new TrackBar { Minimum = 1, Maximum = 100, Value = 10, TickStyle = TickStyle.None, Width = 200, Location = new Point(120, 60 + miscYOffset), Visible = false };

            cbFlight = new CheckBox { Text = "Flight", Location = new Point(20, 100 + miscYOffset), Visible = false };
            tbFlightKey = new TextBox { Text = "Keybind", Location = new Point(120, 100 + miscYOffset), Width = 70, Visible = false };

            lblFlightSpeed = new Label { Text = "Flight Speed", Location = new Point(20, 140 + miscYOffset), Visible = false };
            tbFlightSpeed = new TrackBar { Minimum = 1, Maximum = 100, Value = 10, TickStyle = TickStyle.None, Width = 200, Location = new Point(120, 140 + miscYOffset), Visible = false };

            lblFlightMode = new Label { Text = "Mode", Location = new Point(20, 180 + miscYOffset), Visible = false };
            cbFlightMode = new ComboBox { Location = new Point(120, 180 + miscYOffset), Width = 150, Visible = false };
            cbFlightMode.Items.AddRange(new string[] { "Camera", "Humanoidrootpartmethod", "MovementExploit" });
            cbFlightMode.SelectedIndex = 0;

            this.Controls.AddRange(new Control[] {
                cbNoclip, tbNoclipKey,
                lblNoclipSpeed, tbNoclipSpeed,
                cbFlight, tbFlightKey,
                lblFlightSpeed, tbFlightSpeed,
                lblFlightMode, cbFlightMode
            });
        }

        private void ShowMiscControls(bool show)
        {
            cbNoclip.Visible = show;
            tbNoclipKey.Visible = show;
            lblNoclipSpeed.Visible = show;
            tbNoclipSpeed.Visible = show;
            cbFlight.Visible = show;
            tbFlightKey.Visible = show;
            lblFlightSpeed.Visible = show;
            tbFlightSpeed.Visible = show;
            lblFlightMode.Visible = show;
            cbFlightMode.Visible = show;
        }

        // Call this from button2's Click event
        private void button2_Click(object sender, EventArgs e)
        {
            // Hide visual and misc controls when showing aimbot controls
            ShowVisualToggles(false);
            ShowMiscControls(false);
            ShowAimbotControls(true);
        }

        // Call this from button3's Click event
        private void button3_Click(object sender, EventArgs e)
        {
            ShowVisualToggles(false);
            ShowAimbotControls(false);
            ShowMiscControls(true);
        }
    }
}
