using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaCode.Plugins.MaImage
{
    public static class ImageExtensions
    {
        public static IImageProcessingContext DrawWatermark(this IImageProcessingContext ctx, Image watermark, Image original, int destWidth, int destHeight)
        {

            SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifValue eVal;
            var size = original.Size();
          
            try
            {
                original.Metadata.ExifProfile.TryGetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.SceneCaptureType, out eVal);
                if (eVal.ToString() == "Portrait")
                {
                    int _width = size.Width;
                    size.Width = size.Height;
                    size.Height = _width;
                }
            }
            catch
            {

            }
            int waterX = size.Width - 540;
            int waterY = size.Height - 70;
            if (size.Width > size.Height)
            {
                return ctx.DrawImage(watermark, new SixLabors.Primitives.Point(waterX, waterY), PixelColorBlendingMode.Normal, .6f);
            }
            else
            {
                return ctx.DrawImage(watermark, .6f);
            }
        }
    }
}
