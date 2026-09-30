namespace CodexUsage.App.UI;

public static class DpiFonts
{
    private const float PointsPerInch = 72f;

    // Owner-drawn text must follow the control's DeviceDpi. Point-sized fonts are converted with the
    // Graphics DPI, which is the process system DPI and can differ from the monitor DPI (for example
    // after reconnecting over Remote Desktop at a different scale).
    public static Font Create(FontFamily family, float points, FontStyle style, int deviceDpi)
        => new(family, points * deviceDpi / PointsPerInch, style, GraphicsUnit.Pixel);
}
