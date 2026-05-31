using Microsoft.Maui.Graphics;

namespace Water_reminder.Controls;

public class AuthBackgroundView : GraphicsView
{
    public AuthBackgroundView()
    {
        Drawable = new AuthBackgroundDrawable();
        InputTransparent = true;
    }
}

internal class AuthBackgroundDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var w = dirtyRect.Width;
        var h = dirtyRect.Height;

        var basePaint = new LinearGradientPaint
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops =
            [
                new PaintGradientStop(0, Color.FromArgb("#02050A")),
                new PaintGradientStop(0.44f, Color.FromArgb("#07101F")),
                new PaintGradientStop(1, Color.FromArgb("#111C33"))
            ]
        };
        canvas.SetFillPaint(basePaint, dirtyRect);
        canvas.FillRectangle(dirtyRect);

        DrawRibbon(canvas, w, h, -0.12f, "#05070D", "#0A1222", 0.92f);
        DrawRibbon(canvas, w, h, 0.24f, "#0B172A", "#15233D", 0.72f);
        DrawRibbon(canvas, w, h, 0.62f, "#08101E", "#182744", 0.54f);

        var softShade = new PathF();
        softShade.MoveTo(w * -0.12f, h * 1.08f);
        softShade.CurveTo(w * 0.02f, h * 0.76f, w * -0.08f, h * 0.60f, w * 0.24f, h * 0.42f);
        softShade.CurveTo(w * 0.58f, h * 0.23f, w * 0.62f, h * 0.10f, w * 0.74f, h * -0.04f);
        softShade.LineTo(w * -0.12f, h * -0.04f);
        softShade.Close();
        canvas.FillColor = Color.FromRgba(0, 0, 0, 0.26f);
        canvas.FillPath(softShade);
    }

    private static void DrawRibbon(ICanvas canvas, float w, float h, float xOffset, string dark, string light, float opacity)
    {
        var path = new PathF();
        path.MoveTo(w * (xOffset + 0.08f), h * 1.05f);
        path.CurveTo(w * (xOffset + 0.18f), h * 0.84f, w * (xOffset + 0.02f), h * 0.63f, w * (xOffset + 0.30f), h * 0.48f);
        path.CurveTo(w * (xOffset + 0.58f), h * 0.32f, w * (xOffset + 0.50f), h * 0.15f, w * (xOffset + 0.82f), h * -0.08f);
        path.LineTo(w * (xOffset + 1.05f), h * -0.08f);
        path.CurveTo(w * (xOffset + 0.88f), h * 0.22f, w * (xOffset + 1.02f), h * 0.45f, w * (xOffset + 0.64f), h * 0.60f);
        path.CurveTo(w * (xOffset + 0.32f), h * 0.72f, w * (xOffset + 0.42f), h * 0.93f, w * (xOffset + 0.20f), h * 1.08f);
        path.Close();

        var bounds = new RectF(Math.Min(0, w * xOffset), 0, w * 1.4f, h);
        var paint = new LinearGradientPaint
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops =
            [
                new PaintGradientStop(0, Color.FromArgb(dark).WithAlpha(opacity)),
                new PaintGradientStop(1, Color.FromArgb(light).WithAlpha(opacity))
            ]
        };

        canvas.SetFillPaint(paint, bounds);
        canvas.FillPath(path);
    }
}
