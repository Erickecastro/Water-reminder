using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

namespace Water_reminder.Controls;

public class AnimatedCupView : GraphicsView
{
    private readonly AnimatedCupDrawable _drawable = new();
    private readonly IDispatcherTimer _timer;
    private DateTime _lastFrame = DateTime.UtcNow;
    private double _happyTime;

    public AnimatedCupView()
    {
        Drawable = _drawable;
        InputTransparent = true;

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(16);
        _timer.Tick += (_, _) => Tick();
        Loaded += (_, _) => _timer.Start();
        Unloaded += (_, _) => _timer.Stop();
    }

    public async Task CelebrateAsync()
    {
        _happyTime = 1.65;
        await Task.WhenAll(
            AnimatePropertyAsync("cup-scale-up", 1, 1.09, 110, value => Scale = value, Easing.CubicOut),
            AnimatePropertyAsync("cup-rise-up", 0, -5, 110, value => TranslationY = value, Easing.CubicOut));
        await Task.WhenAll(
            AnimatePropertyAsync("cup-scale-down", 1.09, 1, 260, value => Scale = value, Easing.SpringOut),
            AnimatePropertyAsync("cup-rise-down", -5, 0, 260, value => TranslationY = value, Easing.SpringOut));
    }

    private Task AnimatePropertyAsync(string name, double from, double to, uint duration, Action<double> setter, Easing easing)
    {
        var completion = new TaskCompletionSource();
        this.AbortAnimation(name);
        var animation = new Animation(setter, from, to, easing);
        animation.Commit(this, name, 16, duration, finished: (_, _) => completion.SetResult());
        return completion.Task;
    }

    private void Tick()
    {
        var now = DateTime.UtcNow;
        var delta = Math.Max(0.001, (now - _lastFrame).TotalSeconds);
        _lastFrame = now;

        _happyTime = Math.Max(0, _happyTime - delta);
        _drawable.Time += delta;
        _drawable.HappyAmount = _happyTime > 0 ? 1 : 0;
        Invalidate();
    }
}

internal class AnimatedCupDrawable : IDrawable
{
    public double Time { get; set; }
    public double HappyAmount { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var bob = (float)Math.Sin(Time * 2.1) * 2.2f;
        var w = dirtyRect.Width;
        var h = dirtyRect.Height;
        var cx = dirtyRect.Center.X;

        canvas.SaveState();
        canvas.Translate(0, bob);

        var cupTop = h * 0.19f;
        var cupBottom = h * 0.82f;
        var cupWidth = w * 0.66f;
        var topWidth = cupWidth;
        var bottomWidth = cupWidth * 0.74f;
        var leftTop = cx - topWidth / 2;
        var rightTop = cx + topWidth / 2;
        var leftBottom = cx - bottomWidth / 2;
        var rightBottom = cx + bottomWidth / 2;

        var cup = new PathF();
        cup.MoveTo(leftTop, cupTop);
        cup.LineTo(rightTop, cupTop);
        cup.LineTo(rightBottom, cupBottom);
        cup.QuadTo(cx, cupBottom + h * 0.035f, leftBottom, cupBottom);
        cup.Close();

        canvas.FillColor = Color.FromArgb("#E8F6FF");
        canvas.FillPath(cup);
        canvas.StrokeColor = Color.FromArgb("#061122");
        canvas.StrokeSize = 4;
        canvas.DrawPath(cup);

        var rim = new RectF(leftTop - 3, cupTop - h * 0.055f, topWidth + 6, h * 0.075f);
        canvas.FillColor = Color.FromArgb("#F7FFFA");
        canvas.FillRoundedRectangle(rim, 6);
        canvas.StrokeColor = Color.FromArgb("#061122");
        canvas.StrokeSize = 4;
        canvas.DrawRoundedRectangle(rim, 6);

        var water = new PathF();
        var waterTop = cupTop + h * 0.23f + (float)Math.Sin(Time * 1.45) * 1.2f;
        water.MoveTo(leftTop + 9, waterTop);
        water.CurveTo(cx - 15, waterTop - 5, cx + 15, waterTop + 5, rightTop - 9, waterTop);
        water.LineTo(rightBottom - 7, cupBottom - 8);
        water.QuadTo(cx, cupBottom + 3, leftBottom + 7, cupBottom - 8);
        water.Close();
        canvas.FillColor = Color.FromArgb("#8FC9FF");
        canvas.FillPath(water);

        canvas.StrokeColor = Color.FromRgba(255, 255, 255, 0.42f);
        canvas.StrokeSize = 2.2f;
        canvas.DrawLine(leftTop + 13, cupTop + h * 0.14f, leftBottom + 10, cupBottom - h * 0.13f);
        canvas.DrawLine(rightTop - 12, cupTop + h * 0.15f, rightBottom - 9, cupBottom - h * 0.16f);

        canvas.StrokeColor = Color.FromArgb("#061122");
        canvas.StrokeSize = 4;
        canvas.DrawLine(leftTop - 2, cupTop, rightTop + 2, cupTop);

        canvas.FillColor = Color.FromArgb("#E83AAF");
        canvas.FillCircle(cx - w * 0.20f, h * 0.57f, 7.2f);
        canvas.FillCircle(cx + w * 0.20f, h * 0.57f, 7.2f);

        DrawEye(canvas, cx - w * 0.13f, h * 0.49f, HappyAmount);
        DrawEye(canvas, cx + w * 0.13f, h * 0.49f, HappyAmount);
        DrawMouth(canvas, cx, h * 0.61f, w, HappyAmount);

        for (var i = 0; i < 4; i++)
        {
            var phase = (Time * 0.55 + i * 0.27) % 1.0;
            var x = cx - w * 0.18f + i * w * 0.12f;
            var y = cupBottom - h * 0.12f - (float)phase * h * 0.34f;
            var opacity = (float)Math.Sin(phase * Math.PI) * 0.36f;
            canvas.FillColor = Color.FromRgba(235, 250, 255, opacity);
            canvas.FillCircle(x, y, 2.2f + i % 2);
        }

        canvas.RestoreState();
    }

    private static void DrawEye(ICanvas canvas, float x, float y, double happy)
    {
        canvas.StrokeColor = Color.FromArgb("#061122");
        canvas.StrokeSize = 3.4f;

        if (happy > 0)
        {
            var path = new PathF();
            path.MoveTo(x - 6, y + 2);
            path.QuadTo(x, y - 5, x + 6, y + 2);
            canvas.DrawPath(path);
            return;
        }

        canvas.FillColor = Color.FromArgb("#061122");
        canvas.FillCircle(x, y, 4.2f);
        canvas.FillColor = Colors.White;
        canvas.FillCircle(x + 1.5f, y - 1.5f, 1.2f);
    }

    private static void DrawMouth(ICanvas canvas, float cx, float y, float w, double happy)
    {
        canvas.StrokeColor = Color.FromArgb("#061122");
        canvas.StrokeSize = 3.4f;

        if (happy > 0)
        {
            var rect = new RectF(cx - w * 0.10f, y - 2, w * 0.20f, w * 0.10f);
            canvas.FillColor = Colors.White;
            canvas.FillRoundedRectangle(rect, 7);
            canvas.DrawRoundedRectangle(rect, 7);
            canvas.DrawLine(rect.Left + rect.Width / 2, rect.Top + 1, rect.Left + rect.Width / 2, rect.Bottom - 1);
            return;
        }

        var smile = new PathF();
        smile.MoveTo(cx - w * 0.09f, y);
        smile.QuadTo(cx, y + w * 0.10f, cx + w * 0.09f, y);
        canvas.DrawPath(smile);
    }
}
