using Microsoft.AspNetCore.Hosting;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaCode.Components
{
    public class RazorLightService
    {
        public IRazorLightEngine Engine { get; private set; }
        private readonly IComponentSettings _componentSettings;
        public RazorLightService(IComponentSettings componentSettings, IWebHostEnvironment host)
        {
            _componentSettings = componentSettings;
            _componentSettings.SetUrl(host.ContentRootPath);
            Engine = new RazorLightEngineBuilder()
              .UseFileSystemProject(componentSettings.BaseComponentFullDir)
              .UseMemoryCachingProvider()
              .Build();
            //Engine = engine;
        }

        public string GetTemplate(string path, Dictionary<string, object> data, bool useCache)
        {
            string result = "";

            if (useCache)
            {
                var cacheResult = Engine.Handler.Cache.RetrieveTemplate(path);

                if (cacheResult.Success)
                {
                    result = RenderCachedTemplate(data, cacheResult);
                }
                else
                {
                    result = RenderNewTemplate(path, data);
                }
            }
            else
            {
                result = RenderNewTemplate(path, data);
            }
            return result;
        }
        public async Task<string> GetTemplateAsync(string path, Dictionary<string, object> data, bool useCache)
        {
            if (useCache)
            {
                var cacheResult = Engine.Handler.Cache.RetrieveTemplate(path);

                if (cacheResult.Success)
                {
                    return await RenderCachedTemplateAsync(data, cacheResult);
                }
                else
                {
                    return await RenderNewTemplateAsync(path, data);
                }
            }
            else
            {
                return await RenderNewTemplateAsync(path, data);
            }
        }

        private async Task<string> RenderCachedTemplateAsync(Dictionary<string, object> data, RazorLight.Caching.TemplateCacheLookupResult cacheResult)
        {
            return await Engine.RenderTemplateAsync(cacheResult.Template.TemplatePageFactory(), data);

        }

        private async Task<string> RenderNewTemplateAsync(string path, Dictionary<string, object> data)
        {
            return await Engine.CompileRenderAsync(path, data);
        }

        private string RenderCachedTemplate(Dictionary<string, object> data, RazorLight.Caching.TemplateCacheLookupResult cacheResult)
        {
            return Engine.RenderTemplateAsync(cacheResult.Template.TemplatePageFactory(), data)
                .GetAwaiter()
                .GetResult();
        }

        private string RenderNewTemplate(string path, Dictionary<string, object> data)
        {
            return Engine.CompileRenderAsync(path, data)
                                            .GetAwaiter()
                                            .GetResult();
        }
    }

}
