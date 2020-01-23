using System;
using System.Collections.Generic;
using System.Text;

namespace MaCode.Components
{
    public interface IComponentSettings
    {
        string BaseComponentDir { get; set; }
        void SetUrl(string basePath);
        string BaseComponentFullDir { get; set; }
        List<string> ServicesNamespace { get; set; }
    }

    public class ComponentSettings : IComponentSettings
    {
        public string BaseComponentDir { get; set; }
        public List<string> ServicesNamespace { get; set; }
        public string BaseComponentFullDir
        {
            get; set;
        }

        public void SetUrl(string basePath)
        {
            if (string.IsNullOrEmpty(BaseComponentFullDir))
            {
                BaseComponentFullDir = basePath + "\\" + BaseComponentDir;
            }
        }
    }
}
