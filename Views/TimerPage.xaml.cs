using System.Diagnostics;
using System.Threading;

namespace Notes.Views;

public partial class TimerPage
{
    private readonly ProgressArc _progressArc;
    private DateTime _startTime;
    private int _duration = 60;
    private double _progress;
    private CancellationTokenSource _cancellationTokenSource = new();
    private bool _isRunning = false;
    private TimeSpan _elapsedTime = TimeSpan.Zero;

    public TimerPage()
    {
        InitializeComponent();
        _progressArc = new ProgressArc();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ProgressButton.Text = "\uf144"; // Play icon - workaround because setting it in xaml broke the build for some reason
        //ProgressView.Drawable = _progressArc;['
    }

    // Handle button click events

    private void StartButton_OnClicked(object sender, EventArgs e)
    {
        if (!_isRunning)
        {
            _startTime = DateTime.Now - _elapsedTime;
            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
            UpdateArc();
        }
        else if (_isRunning && !_cancellationTokenSource.IsCancellationRequested)
        {
            _elapsedTime = DateTime.Now - _startTime;
            _cancellationTokenSource.Cancel();
        }
        else
        {
            _startTime = DateTime.Now - _elapsedTime;
            _cancellationTokenSource = new CancellationTokenSource();
            UpdateArc();
        }
    }




    //// Stop the timer 
    //private void StopButton_OnClicked(object sender, EventArgs e)
    //{
    //    _cancellationTokenSource.Cancel();
    //    _isRunning = false; 
    //}


    // Cancel the update loop
    private void ResetButton_OnClicked(object sender, EventArgs e)
    {
        _cancellationTokenSource.Cancel();
        _elapsedTime = TimeSpan.Zero;
        _isRunning = false;
        ResetView();
    }

    private async void UpdateArc()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested && _isRunning)
        {
            TimeSpan elapsedTime = DateTime.Now - _startTime;
            int secondsRemaining = (int)(_duration - elapsedTime.TotalSeconds);

            ProgressButton.Text = $"{secondsRemaining}";

            _progress = Math.Ceiling(elapsedTime.TotalSeconds);
            _progress %= _duration;
            _progressArc.Progress = _progress / _duration;

            if (secondsRemaining == 0)
            {
                _cancellationTokenSource.Cancel();
                _isRunning = false;
                Vibrate(); // Call the vibrate method when the timer reaches zero
                break;
            }

            await Task.Delay(500);
        }

        if (!_isRunning)
        {
            ResetView();
        }
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
        if (int.TryParse(DurationEntry.Text, out int minutes))
        {
            // Ensure minutes are within valid ranges
            minutes = Math.Max(0, minutes);

            _duration = minutes * 60;
        }
    }



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

    private void Vibrate()
    {
        double secondsToVibrate = 0.2;
        TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

        Vibration.Default.Vibrate(vibrationLength);
        Console.WriteLine("vibrating");
    }

}