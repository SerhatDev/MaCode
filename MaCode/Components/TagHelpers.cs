using MaCode.Components.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MaCode.Components
{
    [HtmlTargetElement("Ma", Attributes = "comp-name")]
    public class MaComponentRenderer : TagHelper
    {
        private readonly RazorLightService viewRenderer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IComponentSettings _componentSettings;
        private readonly IIMaComponent _imaComponent;

        public string CompName { get; set; }

        [HtmlAttributeName("view-model")]
        public object ViewModel { get; set; }
        public MaComponentRenderer(
            IIMaComponent imaComponent,
            RazorLightService viewRenderService, 
            IServiceProvider serviceProvider, 
            IWebHostEnvironment hostingEnvironment, 
            IComponentSettings componentSettings)
        {
            _imaComponent = imaComponent;
            _componentSettings = componentSettings;
            viewRenderer = viewRenderService;
            _serviceProvider = serviceProvider;
            _hostingEnvironment = hostingEnvironment;
        }


        private ComponentInfoModel GetFilePath()
        {
            string jsonText = File.ReadAllText(_componentSettings.BaseComponentFullDir + "\\viewBindings.json");

            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ComponentInfoModel>>(jsonText);

            var currentView = list.Where(x => x.name == CompName).Single();

            return currentView;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var _compBasicInfo = GetFilePath();
            var componentInfo = _imaComponent.GetComponentSettings(_compBasicInfo);

            var data = new Dictionary<string, object>();

            string attrValue = "";
            try
            {
                attrValue = context.AllAttributes.Single(x => x.Name == "values").Value.ToString();
            }
            catch
            {
            }

            if (!string.IsNullOrEmpty(attrValue))
            {
                List<string> values = attrValue.Split('|').ToList();

                values.ForEach(x =>
                {
                    data.Add(x.Split('@')[0], x.Split('@')[1]);
                });
            }
            if (componentInfo.values != null)
                componentInfo.values.ToList().ForEach(x =>
                {
                    data.Add(x.Key, x.Value);
                });

            var fDic = context.AllAttributes.ToDictionary(
                (keySelect => keySelect.Name),
                (elSelect => elSelect.Value)
                );
            for (int i = 0; i < fDic.Count; i++)
            {
                data.Add(fDic.Keys.ElementAt(i), fDic.Values.ElementAt(i));
            }
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly();



            componentInfo.injectables.ForEach(serviceName =>
            {
                Type _type = null;
                int i = 0;
                while ((_type = Type.GetType($"{_componentSettings.ServicesNamespace[i]}.{serviceName}, {assemblyName}")) == null)
                {
                    if (i + 1 == _componentSettings.ServicesNamespace.Count)
                    {
                        throw new ArgumentException($"Type {serviceName} can not be loaded!");
                    }
                    i++;
                }
                data.Add(serviceName, _serviceProvider.GetService(_type));
            });


            string rawHtml = viewRenderer.GetTemplate(componentInfo.view, data, componentInfo.useCache);

            output.Content.SetHtmlContent(rawHtml);
        }
    }
}
