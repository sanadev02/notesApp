namespace Notes.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }
    private async void LearnMore_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.About about)
        {
            await Launcher.Default.OpenAsync(about.MoreInfoUrl);
        }

    }

    private void BatterySwitch_Toggled(object sender, ToggledEventArgs e) =>
    WatchBattery();

    private bool _isBatteryWatched;

    private void WatchBattery()
    {

        if (!_isBatteryWatched)
        {
            Battery.Default.BatteryInfoChanged += Battery_BatteryInfoChanged;
        }
        else
        {
            Battery.Default.BatteryInfoChanged -= Battery_BatteryInfoChanged;
        }

        _isBatteryWatched = !_isBatteryWatched;
    }

    private void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
    {
        BatteryStateLabel.Text = e.State switch
        {
            BatteryState.Charging => "Battery is currently charging",
            BatteryState.Discharging => "Charger is not connected and the battery is discharging",
            BatteryState.Full => "Battery is full",
            BatteryState.NotCharging => "The battery isn't charging.",
            BatteryState.NotPresent => "Battery is not available.",
            BatteryState.Unknown => "Battery is unknown",
            _ => "Battery is unknown"
        };

        BatteryLevelLabel.Text = $"Battery is {e.ChargeLevel * 100}% charged.";
    }
}