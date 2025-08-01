using Avalonia.Labs.Controls;
using FishLevelEditor2;
using FishLevelEditor2.ViewModels;
using SkiaSharp;

namespace FishLevelEditor2.Controls
{
    public class LevelViewControl : SKCanvasView
    {
        private string _entryIconFilePath = "Content/entry.png";
        public LevelViewModel LevelViewModel { get; set; }
        public bool ShowGrid { get; set; }
        public int TileSize { get; set; } = 16;

        public LevelViewControl()
        {
            PaintSurface += OnPaint;
        }

        private void OnPaint(object? sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            var surfaceWidth = e.Info.Width;
            var surfaceHeight = e.Info.Height;

            // Destination rect: fit entire control size (stretch)
            var destRect = new SKRect(0, 0, surfaceWidth, surfaceHeight);

            canvas.Clear(SKColors.Black);
            LevelViewModel.Display();
            canvas.DrawBitmap(LevelViewModel.LevelBitmap.Bitmap, destRect);

            if (ShowGrid)
            {
                DrawGrid(canvas);
            }

            DrawEntries(canvas);
        }

        private void DrawGrid(SKCanvas canvas)
        {
            int bitmapWidth = LevelViewModel.LevelBitmap.Bitmap.Width;
            int bitmapHeight = LevelViewModel.LevelBitmap.Bitmap.Height;

            int cols = bitmapWidth / TileSize;
            int rows = bitmapHeight / TileSize;

            using var paint = new SKPaint
            {
                Color = new SKColor(255, 255, 255, 60),
                IsStroke = true,
                StrokeWidth = 1
            };

            for (int x = 0; x <= cols; x++)
                canvas.DrawLine(x * TileSize, 0, x * TileSize, LevelViewModel.LevelBitmap.Bitmap.Height, paint);

            for (int y = 0; y <= rows; y++)
                canvas.DrawLine(0, y * TileSize, LevelViewModel.LevelBitmap.Bitmap.Width, y * TileSize, paint);
        }

        private void DrawEntries(SKCanvas canvas)
        {
            foreach (var entry in LevelViewModel.Level.Entries)
            {
                float x = entry.PosX * TileSize * 2;
                float y = entry.PosY * TileSize * 2;
                var destRect = new SKRect(x, y, x + (TileSize*2), y + (TileSize*2));

                canvas.DrawBitmap(BitmapUtils.LoadSKBitmapFromFile(_entryIconFilePath), destRect);
            }
        }
    }
}