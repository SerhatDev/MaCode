using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace MaCode.Plugins.MaImage
{
    public interface IImageProcess
    {
        void Process(IFormFile file, FileStream outStream);
        void Process(IFormFile file, FileStream outStream, Action<IImageProcessSettings> settings);
    }

    public class ImageProcess : IImageProcess
    {
        private readonly IImageProcessSettings _settings;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ImageProcess(IImageProcessSettings settings, IHostingEnvironment hostingEnvironment)
        {
            _settings = settings;
            _hostingEnvironment = hostingEnvironment;
            _settings.SetUrl(_hostingEnvironment.WebRootPath);
        }
        public void Process(IFormFile file, FileStream outStream, Action<IImageProcessSettings> settings)
        {
            IImageProcessSettings currentSettings = new ImageProcessSettings()
            {
                DestImageSize = _settings.DestImageSize,
                EncodeQuality = _settings.EncodeQuality,
                UseWatermark = _settings.UseWatermark,
                EncodeSample = _settings.EncodeSample,
                WatermarkFullPath = _settings.WatermarkFullPath,
                WatermarkUrl = _settings.WatermarkUrl
            };
            settings.Invoke(currentSettings);

            if (currentSettings.UseWatermark)
            {
                currentSettings.SetUrl(_hostingEnvironment.WebRootPath);
            }

            Image watermark = null;
            if (currentSettings.UseWatermark)
            {
                watermark = Image.Load(currentSettings.WatermarkFullPath);
            }

            var inStream = file.OpenReadStream();

            using (var image = Image.Load(inStream, out IImageFormat format))
            {
                Action<IImageProcessingContext> processAction = (i) =>
                {
                    i.Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Max,
                        Compand = true,
                        Size = new SixLabors.Primitives.Size(currentSettings.DestImageSize.Width, currentSettings.DestImageSize.Height)
                    });
                    if (currentSettings.UseWatermark)
                    {
                        i.DrawWatermark(watermark, image, currentSettings.DestImageSize.Width, currentSettings.DestImageSize.Height);
                    }
                };
                image.Mutate(processAction);
                image.Save(outStream,
                    new JpegEncoder()
                    {
                        Quality = currentSettings.EncodeQuality,
                        Subsample = ((JpegSubsample)(int)currentSettings.EncodeSample)
                    });
            }
        }
        public void Process(IFormFile file, FileStream outStream)
        {
            Image watermark = null;
            if (_settings.UseWatermark)
            {
                watermark = Image.Load(_settings.WatermarkFullPath);
            }
            var inStream = file.OpenReadStream();

            using (var image = Image.Load(inStream, out IImageFormat format))
            {
                Action<IImageProcessingContext> processAction = (i) =>
                {
                    i.Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Max,
                        Compand = true,
                        Size = new SixLabors.Primitives.Size(_settings.DestImageSize.Width, _settings.DestImageSize.Height)
                    });
                    if (_settings.UseWatermark)
                    {
                        i.DrawWatermark(watermark, image, _settings.DestImageSize.Width, _settings.DestImageSize.Height);
                    }
                };
                image.Mutate(processAction);
                image.Save(outStream,
                    new JpegEncoder()
                    {
                        Quality = _settings.EncodeQuality,
                        Subsample = ((JpegSubsample)(int)_settings.EncodeSample)
                    });
            }
        }
    }
}