using Microsoft.Maui.Graphics;

namespace Water_reminder.Controls;

public class HydrationProgressView : GraphicsView
{
    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
        nameof(Progress),
        typeof(double),
        typeof(HydrationProgressView),
        0d,
        propertyChanged: static (bindable, oldValue, newValue) =>
            ((HydrationProgressView)bindable).AnimateTo((double)oldValue, (double)newValue));

    private readonly HydrationProgressDrawable _drawable = new();
    private readonly IDispatcherTimer _timer;
    private double _displayProgress;
    private double _targetProgress;
    private DateTime _lastFrame = DateTime.UtcNow;

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public HydrationProgressView()
    {
        Drawable = _drawable;
        InputTransparent = true;

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(16);
        _timer.Tick += (_, _) => Tick();
        Loaded += (_, _) => _timer.Start();
        Unloaded += (_, _) => _timer.Stop();
    }

    private void AnimateTo(double from, double to)
    {
        _displayProgress = Math.Clamp(from, 0, 1);
        _targetProgress = Math.Clamp(to, 0, 1);
        _drawable.Progress = _displayProgress;
        Invalidate();

        if (!_timer.IsRunning)
        {
            _lastFrame = DateTime.UtcNow;
            _timer.Start();
        }
    }

    private void Tick()
    {
        var now = DateTime.UtcNow;
        var delta = Math.Max(0.001, (now - _lastFrame).TotalSeconds);
        _lastFrame = now;

        var distance = _targetProgress - _displayProgress;
        if (Math.Abs(distance) > 0.001)
        {
            _displayProgress += distance * Math.Min(1, delta * 5.8);
        }
        else
        {
            _displayProgress = _targetProgress;
        }

        _drawable.Progress = Math.Clamp(_displayProgress, 0, 1);
        _drawable.Time += delta;
        Invalidate();
    }
}

internal class HydrationProgressDrawable : IDrawable
{
    private readonly Bubble[] _bubbles =
    [
        new(0.28f, 0.12f, 3.4f, 0.28f, 0.00f),
        new(0.60f, 0.20f, 2.2f, 0.34f, 0.28f),
        new(0.42f, 0.36f, 2.8f, 0.25f, 0.52f),
        new(0.72f, 0.48f, 4.0f, 0.38f, 0.72f),
        new(0.32f, 0.62f, 3.0f, 0.31f, 0.95f),
        new(0.56f, 0.76f, 2.0f, 0.23f, 1.15f),
        new(0.70f, 0.86f, 4.4f, 0.36f, 1.42f),
    ];

    public double Progress { get; set; }
    public double Time { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var padding = 1.5f;
        var cup = new RectF(
            dirtyRect.X + padding,
            dirtyRect.Y + padding,
            dirtyRect.Width - padding * 2,
            dirtyRect.Height - padding * 2);
        var radius = cup.Width / 2f;

        canvas.SaveState();
        canvas.FillColor = Color.FromArgb("#24315F");
        canvas.FillRoundedRectangle(cup, radius);

        canvas.ClipPath(CreateRoundedPath(cup, radius));

        var progress = (float)Math.Clamp(Progress, 0, 1);
        if (progress > 0.001f)
        {
            var waterHeight = Math.Max(cup.Width * 0.22f, cup.Height * progress);
            waterHeight = Math.Min(cup.Height, waterHeight);
            var water = new RectF(cup.X, cup.Bottom - waterHeight, cup.Width, waterHeight);

            var waterPaint = new LinearGradientPaint
            {
                StartPoint = new Point(0, 1),
                EndPoint = new Point(0, 0),
                GradientStops =
                [
                    new PaintGradientStop(0, Color.FromArgb("#1B2A55")),
                    new PaintGradientStop(1, Color.FromArgb("#2B579E"))
                ]
            };

            canvas.SetFillPaint(waterPaint, water);
            canvas.FillRoundedRectangle(water, radius);

            foreach (var bubble in _bubbles)
            {
                var cycle = ((Time * bubble.Speed + bubble.Offset) % 1.0);
                var inset = Math.Max(bubble.Size + 2, 6);
                var usableHeight = Math.Max(1, water.Height - inset * 2);
                var usableWidth = Math.Max(1, water.Width - inset * 2);
                var y = water.Bottom - inset - (float)cycle * usableHeight;
                var x = water.X + inset + usableWidth * bubble.X;

                if (y < water.Y + bubble.Size || y > water.Bottom - bubble.Size)
                {
                    continue;
                }

                var fade = (float)Math.Sin(cycle * Math.PI);
                canvas.FillColor = Color.FromRgba(190, 238, 255, 0.18f + fade * 0.34f);
                canvas.FillCircle(x, y, bubble.Size);
            }
        }

        canvas.RestoreState();

        canvas.StrokeColor = Color.FromRgba(148, 180, 220, 0.32f);
        canvas.StrokeSize = 1.2f;
        canvas.DrawRoundedRectangle(cup, radius);
    }

    private static PathF CreateRoundedPath(RectF rect, float radius)
    {
        var path = new PathF();
        path.MoveTo(rect.Left + radius, rect.Top);
        path.LineTo(rect.Right - radius, rect.Top);
        path.QuadTo(rect.Right, rect.Top, rect.Right, rect.Top + radius);
        path.LineTo(rect.Right, rect.Bottom - radius);
        path.QuadTo(rect.Right, rect.Bottom, rect.Right - radius, rect.Bottom);
        path.LineTo(rect.Left + radius, rect.Bottom);
        path.QuadTo(rect.Left, rect.Bottom, rect.Left, rect.Bottom - radius);
        path.LineTo(rect.Left, rect.Top + radius);
        path.QuadTo(rect.Left, rect.Top, rect.Left + radius, rect.Top);
        path.Close();
        return path;
    }

    private readonly record struct Bubble(float X, float Y, float Size, float Speed, float Offset);
}
