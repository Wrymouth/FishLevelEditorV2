using Avalonia.Labs.Controls;
using FishLevelEditor2.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.Controls
{
    public class ObjectDefinitionsViewControl : SKCanvasView
    {
        public const int OBJECTS_IMAGE_WIDTH = 128;
        public ObjectsViewModel ObjectsViewModel { get; set; }
        public bool ShowGrid { get; set; }
        public int TileSize { get; set; } = 16;

        public ObjectDefinitionsViewControl()
        {
            PaintSurface += OnPaint;
        }

        private void OnPaint(object? sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            var surfaceWidth = e.Info.Width;
            var surfaceHeight = e.Info.Height;

            // Destination rect: fit entire control size (stretch)
            //var destRect = new SKRect(0, 0, surfaceWidth, surfaceHeight);

            canvas.Clear(SKColors.Gray);
            int posX = 0;
            int posY = 0;
            foreach (var objectDefinition in ObjectsViewModel.ObjectDefinitions)
            {
                // drawObject()
                // a 16x16 preview of the top left of the object image
                SKBitmap objectBitmap = CropObjectBitmap(BitmapUtils.LoadSKBitmapFromFile(objectDefinition.SpriteFilePath));
                var destRect = new SKRect(posX, posY, posX + 32, posY + 32);
                canvas.DrawBitmap(objectBitmap, destRect);
                posX += 32;
                if (posX >= OBJECTS_IMAGE_WIDTH)
                {
                    posX = 0;
                    posY += 32;
                }
            }

            if (ShowGrid)
            {
                DrawGrid(canvas);
            }
        }

        private void DrawGrid(SKCanvas canvas)
        {
            //int bitmapWidth = LevelViewModel.LevelBitmap.Bitmap.Width;
            //int bitmapHeight = LevelViewModel.LevelBitmap.Bitmap.Height;

            //int cols = bitmapWidth / TileSize;
            //int rows = bitmapHeight / TileSize;

            //using var paint = new SKPaint
            //{
            //    Color = new SKColor(255, 255, 255, 60),
            //    IsStroke = true,
            //    StrokeWidth = 1
            //};

            //for (int x = 0; x <= cols; x++)
            //    canvas.DrawLine(x * TileSize, 0, x * TileSize, LevelViewModel.LevelBitmap.Bitmap.Height, paint);

            //for (int y = 0; y <= rows; y++)
            //    canvas.DrawLine(0, y * TileSize, LevelViewModel.LevelBitmap.Bitmap.Width, y * TileSize, paint);
        }

        private SKBitmap CropObjectBitmap(SKBitmap bitmap)
        {
            using var pixmap = new SKPixmap(bitmap.Info, bitmap.GetPixels());
            SKRectI rectI = new(0, 0, 16, 16);

            var subset = pixmap.ExtractSubset(rectI);
            var returnBitmap = new SKBitmap();
            returnBitmap.InstallPixels(subset);
            return returnBitmap;
        }
    }
}
