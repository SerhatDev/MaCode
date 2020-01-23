using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaCode.Plugins.MaImage
{
    public interface IImageProcessSettings
    {
        (int Width, int Height) DestImageSize { get; set; }
        int EncodeQuality { get; set; }
        bool UseWatermark { get; set; }
        ImageProcessSettings.JpegSample EncodeSample { get; set; }
        string WatermarkFullPath { get; }
        string WatermarkUrl { get; set; }
        void SetUrl(string basePath);
    }


    public class ImageProcessSettings : IImageProcessSettings
    {
        public enum JpegSample : int
        {
            High = 0,
            Medium = 1
        }

        public void SetUrl(string basePath)
        {
            if(string.IsNullOrEmpty(WatermarkFullPath))
            {
                WatermarkFullPath = basePath + "\\" + WatermarkUrl;
            }
        }

        public bool UseWatermark { get; set; }
        public string WatermarkUrl { get; set; }
        public (int Width, int Height) DestImageSize { get; set; }
        public int EncodeQuality { get; set; }

        public JpegSample EncodeSample { get; set; }

        public string WatermarkFullPath { get; internal set; }

    }
}
