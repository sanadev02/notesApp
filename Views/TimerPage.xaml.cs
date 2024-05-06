using System.Diagnostics;
using System.Threading;

namespace Notes.Views;

public partial class TimerPage
{
    //private TimeOnly time = new();
    //private bool isRunning;

    //private DateTime _startTime;
    //private CancellationTokenSource _cancellationTokenSource;
    //private double _duration;

    //   public TimerPage()
    //{
    //	InitializeComponent();
    //}

    //private async void OnStartStop(object sender, EventArgs e)
    //{
    //	isRunning = !isRunning;
    //	startStopButton.Source = isRunning ? "pause" : "play";

    //	while (isRunning)
    //	{
    //		time = time.Add(TimeSpan.FromSeconds(1));
    //		SetTime();
    //		await Task.Delay(TimeSpan.FromSeconds(1));
    //	}
    //}

    //private void SetTime()
    //{
    //	timeLabel.Text = $"{time.Minute}:{time.Second:000}";
    //}
    private readonly ProgressArc _progressArc;
    private DateTime _startTime;
    private int _duration = 60;
    private double _progress;
    private CancellationTokenSource _cancellationTokenSource = new();

    public TimerPage()
    {
        InitializeComponent();
        _progressArc = new ProgressArc();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ProgressButton.Text = "\uf144"; // Play icon - workaround because setting it in xaml broke the build for some reason
        //ProgressView.Drawable = _progressArc;
    }

    // Handle button click events
    private void StartButton_OnClicked(object sender, EventArgs e)
    {
        _startTime = DateTime.Now;
        _cancellationTokenSource = new CancellationTokenSource();
        UpdateArc();
    }

    // Cancel the update loop
    private void ResetButton_OnClicked(object sender, EventArgs e)
    {
        _cancellationTokenSource.Cancel();
        UpdateArc();
    }

    private async void UpdateArc()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            TimeSpan elapsedTime = DateTime.Now - _startTime;
            int secondsRemaining = (int)(_duration - elapsedTime.TotalSeconds);

            ProgressButton.Text = $"{secondsRemaining}";

            _progress = Math.Ceiling(elapsedTime.TotalSeconds);
            _progress %= _duration;
            _progressArc.Progress = _progress / _duration;
            //ProgressView.Invalidate();

            if (secondsRemaining == 0)
            {
                _cancellationTokenSource.Cancel();
                return;
            }

            await Task.Delay(500);
        }

        ResetView();
    }

    private void ResetView()
    {
        _progress = 0;
        _progressArc.Progress = 100;
        //ProgressView.Invalidate();
        ProgressButton.Text = "\uf144";
    }

    private void DurationEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string[] parts = DurationEntry.Text.Split(':');

        if (parts.Length == 2 && int.TryParse(parts[0], out int minutes) && int.TryParse(parts[1], out int seconds))
        {
            // Ensure minutes and seconds are within valid ranges
            minutes = Math.Max(0, minutes);
            seconds = Math.Max(0, seconds);
            seconds = Math.Min(59, seconds);

            _duration = minutes * 60 + seconds;
        }
    }

    //TimePicker timePicker = new TimePicker
    //{
    //    Time = new TimeSpan(4, 15, 26), // Time set to "04:15:26"
    //    //Debug.WriteLine("times up!"),
    //};

    public class ProgressArc : IDrawable
    {
        public double Progress { get; set; } = 100;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Angle of the arc in degrees
            var endAngle = 90 - (int)Math.Round(Progress * 360, MidpointRounding.AwayFromZero);

            canvas.StrokeColor = Color.FromRgba("6599ff");
            canvas.StrokeSize = 4;
            Debug.WriteLine($"The rect width is {dirtyRect.Width} and height is {dirtyRect.Height}");
            canvas.DrawArc(5, 5, (dirtyRect.Width - 10), (dirtyRect.Height - 10), 90, endAngle, false, false);
        }

    }
}