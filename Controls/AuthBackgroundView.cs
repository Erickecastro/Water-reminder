using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

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
        float w = dirtyRect.Width;
        float h = dirtyRect.Height;

        // Fundo
        var background = new LinearGradientPaint
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops =
            [
                new PaintGradientStop(0f, Color.FromArgb("#050A14")),
                new PaintGradientStop(0.5f, Color.FromArgb("#0A1930")),
                new PaintGradientStop(1f, Color.FromArgb("#10294A"))
            ]
        };

        canvas.SetFillPaint(background, dirtyRect);
        canvas.FillRectangle(dirtyRect);

        // Brilho superior direito
        canvas.FillColor = Color.FromArgb("#4FC3F7").WithAlpha(0.08f);
        canvas.FillCircle(w * 0.92f, h * 0.12f, w * 0.28f);

        // Círculo central
        canvas.FillColor = Color.FromArgb("#81D4FA").WithAlpha(0.05f);
        canvas.FillCircle(w * 0.30f, h * 0.35f, w * 0.22f);

        // Círculo inferior
        canvas.FillColor = Color.FromArgb("#4FC3F7").WithAlpha(0.04f);
        canvas.FillCircle(w * 0.80f, h * 0.72f, w * 0.30f);

        DrawWave(canvas, w, h, 0.78f, "#2B79C2", 0.18f);
        DrawWave(canvas, w, h, 0.84f, "#4AA3F0", 0.12f);
    }

    private static void DrawWave(
        ICanvas canvas,
        float w,
        float h,
        float y,
        string color,
        float alpha)
    {
        var path = new PathF();

        path.MoveTo(0, h);

        path.LineTo(0, h * y);

        path.CurveTo(
            w * 0.20f, h * (y - 0.05f),
            w * 0.45f, h * (y + 0.04f),
            w * 0.70f, h * (y - 0.03f));

        path.CurveTo(
            w * 0.85f, h * (y - 0.08f),
            w * 0.95f, h * (y + 0.03f),
            w, h * y);

        path.LineTo(w, h);
        path.Close();

        canvas.FillColor = Color.FromArgb(color).WithAlpha(alpha);
        canvas.FillPath(path);
    }
}