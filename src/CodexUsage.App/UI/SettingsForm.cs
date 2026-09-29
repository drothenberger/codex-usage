using CodexUsage.App.Settings;

namespace CodexUsage.App.UI;

public sealed class SettingsForm : Form
{
    private const int LogicalDpi = 96;
    private const int LogicalWidth = 430;
    private const int LogicalHeight = 322;

    private readonly Label _heading;
    private readonly Label _caption;
    private readonly Label _refreshLabel;
    private readonly Label _themeLabel;
    private readonly ComboBox _refreshInterval = new();
    private readonly ComboBox _notificationThreshold = new();
    private readonly ComboBox _theme = new();
    private readonly CheckBox _notifications = new();
    private readonly CheckBox _launchAtStartup = new();
    private readonly Button _saveButton = new();
    private readonly Button _cancelButton = new();
    private readonly ThemePalette _palette;

    public SettingsForm(AppSettings settings, bool launchAtStartup)
    {
        _palette = ThemePalette.Resolve(settings.Theme);
        Text = "Codex Usage settings";
        AutoScaleDimensions = new SizeF(LogicalDpi, LogicalDpi);
        AutoScaleMode = AutoScaleMode.Dpi;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9f);
        BackColor = _palette.Background;
        ForeColor = _palette.Text;

        _heading = CreateLabel("Codex Usage", bold: true, size: 14f);
        _caption = CreateLabel("A focused Windows tray meter for your Codex plan.", bold: false, size: 8.75f);
        _caption.ForeColor = _palette.SecondaryText;
        _refreshLabel = CreateLabel("Refresh every", bold: false, size: 9f);
        _themeLabel = CreateLabel("Theme", bold: false, size: 9f);

        Controls.Add(_heading);
        Controls.Add(_caption);
        Controls.Add(_refreshLabel);
        Controls.Add(_refreshInterval);
        _refreshInterval.DropDownStyle = ComboBoxStyle.DropDownList;
        _refreshInterval.Items.AddRange(["1 minute", "2 minutes", "5 minutes", "15 minutes", "30 minutes"]);
        _refreshInterval.SelectedIndex = Array.IndexOf(new[] { 1, 2, 5, 15, 30 }, settings.RefreshIntervalMinutes);
        StyleComboBox(_refreshInterval);

        Controls.Add(_themeLabel);
        Controls.Add(_theme);
        _theme.DropDownStyle = ComboBoxStyle.DropDownList;
        _theme.Items.AddRange(["System", "Dark", "Light"]);
        _theme.SelectedIndex = (int)settings.Theme;
        StyleComboBox(_theme);

        _notifications.Text = "Notify me when available quota drops to";
        _notifications.Checked = settings.NotificationsEnabled;
        StyleCheckBox(_notifications);
        Controls.Add(_notifications);

        _notificationThreshold.DropDownStyle = ComboBoxStyle.DropDownList;
        _notificationThreshold.Items.AddRange(["30%", "20%", "10%", "0%"]);
        _notificationThreshold.SelectedIndex = Array.IndexOf(
            new[] { 30, 20, 10, 0 },
            settings.NotificationThresholdPercent);
        if (_notificationThreshold.SelectedIndex < 0)
        {
            _notificationThreshold.SelectedIndex = 1;
        }
        StyleComboBox(_notificationThreshold);
        Controls.Add(_notificationThreshold);

        _launchAtStartup.Text = "Start Codex Usage when I sign in to Windows";
        _launchAtStartup.Checked = launchAtStartup;
        StyleCheckBox(_launchAtStartup);
        Controls.Add(_launchAtStartup);

        _saveButton.Text = "Save";
        _saveButton.DialogResult = DialogResult.OK;
        StyleButton(_saveButton, primary: true);
        Controls.Add(_saveButton);

        _cancelButton.Text = "Cancel";
        _cancelButton.DialogResult = DialogResult.Cancel;
        StyleButton(_cancelButton, primary: false);
        Controls.Add(_cancelButton);

        AcceptButton = _saveButton;
        CancelButton = _cancelButton;
        LayoutContent();
    }

    public AppSettings SelectedSettings
    {
        get
        {
            var refreshValues = new[] { 1, 2, 5, 15, 30 };
            var thresholdValues = new[] { 30, 20, 10, 0 };
            return new AppSettings
            {
                RefreshIntervalMinutes = refreshValues[Math.Max(0, _refreshInterval.SelectedIndex)],
                NotificationsEnabled = _notifications.Checked,
                NotificationThresholdPercent = thresholdValues[Math.Max(0, _notificationThreshold.SelectedIndex)],
                Theme = (ThemeMode)Math.Max(0, _theme.SelectedIndex),
            };
        }
    }

    public bool LaunchAtStartup => _launchAtStartup.Checked;

    protected override void OnLoad(EventArgs eventArgs)
    {
        base.OnLoad(eventArgs);
        LayoutContent();
    }

    protected override void OnDpiChanged(DpiChangedEventArgs eventArgs)
    {
        base.OnDpiChanged(eventArgs);
        LayoutContent();
        Invalidate(invalidateChildren: true);
    }

    private void LayoutContent()
    {
        // Bounds are in 96-DPI logical units and converted for the window's current DPI,
        // because fonts scale with DPI even when manually placed controls do not.
        SetLogicalBounds(_heading, 18, 18, 390, 30);
        SetLogicalBounds(_caption, 18, 48, 390, 24);
        SetLogicalBounds(_refreshLabel, 18, 91, 180, 24);
        SetLogicalBounds(_refreshInterval, 204, 88, 204, 28);
        SetLogicalBounds(_themeLabel, 18, 133, 180, 24);
        SetLogicalBounds(_theme, 204, 130, 204, 28);
        SetLogicalBounds(_notifications, 18, 178, 290, 26);
        SetLogicalBounds(_notificationThreshold, 315, 176, 92, 28);
        SetLogicalBounds(_launchAtStartup, 18, 216, 390, 26);
        SetLogicalBounds(_cancelButton, 224, 272, 88, 32);
        SetLogicalBounds(_saveButton, 320, 272, 88, 32);
        ClientSize = new Size(
            LogicalToDeviceUnits(LogicalWidth),
            LogicalToDeviceUnits(LogicalHeight));
    }

    private void SetLogicalBounds(Control control, int x, int y, int width, int height)
    {
        control.SetBounds(
            LogicalToDeviceUnits(x),
            LogicalToDeviceUnits(y),
            LogicalToDeviceUnits(width),
            LogicalToDeviceUnits(height));
    }

    private Label CreateLabel(string text, bool bold, float size)
    {
        return new Label
        {
            Text = text,
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = _palette.Text,
            BackColor = Color.Transparent,
        };
    }

    private void StyleComboBox(ComboBox comboBox)
    {
        comboBox.BackColor = _palette.Card;
        comboBox.ForeColor = _palette.Text;
        comboBox.FlatStyle = FlatStyle.Flat;
    }

    private void StyleCheckBox(CheckBox checkBox)
    {
        checkBox.ForeColor = _palette.Text;
        checkBox.BackColor = Color.Transparent;
    }

    private void StyleButton(Button button, bool primary)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.Cursor = Cursors.Hand;
        button.BackColor = primary ? _palette.Accent : _palette.Card;
        button.ForeColor = primary ? Color.White : _palette.Text;
        button.FlatAppearance.BorderColor = primary ? _palette.Accent : _palette.Border;
    }
}
