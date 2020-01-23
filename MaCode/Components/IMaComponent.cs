using MaCode.Components.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace MaCode.Components
{
    public interface IIMaComponent
    {
        Task<bool> AddComponentAsync(IFormFile zipFile);
        List<ComponentInfoModel> GetComponentList();
        ComponentModel GetComponentSettings(ComponentInfoModel component);
    }

    public class IMaComponent : IIMaComponent
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IComponentSettings _componentSettings;
        public IMaComponent(IWebHostEnvironment hostingEnvironment, IComponentSettings componentSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _componentSettings = componentSettings;
            _componentSettings.SetUrl(_hostingEnvironment.ContentRootPath);
        }

        public List<ComponentInfoModel> GetComponentList()
        {
            string jsonText = File.ReadAllText(_componentSettings.BaseComponentFullDir + "\\viewBindings.json");

            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ComponentInfoModel>>(jsonText);
            return list;
        }

        public ComponentModel GetComponentSettings(ComponentInfoModel component)
        {
            string jsonText = File.ReadAllText($"{_componentSettings.BaseComponentFullDir}/{component.name}/settings.json");

            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ComponentModel>(jsonText);
            return model;
        }

        public async Task<bool> AddComponentAsync(IFormFile zipFile)
        {
            string extractPath = _componentSettings.BaseComponentFullDir + "\\" + Path.GetFileNameWithoutExtension(zipFile.FileName);
            

            if (!Directory.Exists(extractPath))
                Directory.CreateDirectory(extractPath);

            string zipPath = extractPath + "\\comp.zip";


            using (var fileStream = new FileStream(zipPath, FileMode.Create))
            {
                await zipFile.CopyToAsync(fileStream);
            }


            //bool addedInfo = UpdateViewBindings(componentInfo);

            //if(!addedInfo)
            //{
            //    return false;
            //}

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));
                    entry.ExtractToFile(destinationPath, true);

                    //if (entry.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    // json Settings, merge it with viewBindings.json

                    //    // Gets the full path to ensure that relative segments are removed.
                    //    string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                    //    // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                    //    // are case-insensitive.
                    //    if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                    //        entry.ExtractToFile(destinationPath);
                    //}
                }
            }
            var compModel = GetComponentSettingsFromFile(extractPath + "\\settings.json");
            var compInfo = new ComponentInfoModel() { name = compModel.name, view = compModel.view };
            bool infoUpdated = UpdateViewBindings(compInfo,compModel);

            File.Delete(zipPath);

            return true;
        }

        private ComponentModel GetComponentSettingsFromFile(string path)
        {
            string jsonText = File.ReadAllText(path);

            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ComponentModel>(jsonText);
            return model;
        }

        private bool UpdateViewBindings(ComponentInfoModel model,ComponentModel component)
        {
            try
            {
                string basePathToComp = _componentSettings.BaseComponentFullDir + "\\" + model.name;
                string baseViewJson = _componentSettings.BaseComponentFullDir + "\\viewBindings.json";
               

                string readViewSettings = File.ReadAllText(baseViewJson);
                var info = JsonConvert.DeserializeObject<List<ComponentInfoModel>>(readViewSettings);

                info.Add(new ComponentInfoModel()
                {
                    name = component.name,
                    view = component.view
                });

                string newJsonViewSettings = JsonConvert.SerializeObject(info);

                File.WriteAllText(baseViewJson, newJsonViewSettings);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
