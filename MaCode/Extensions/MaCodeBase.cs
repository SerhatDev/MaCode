using Google.Apis.AnalyticsReporting.v4.Data;
using MaCode.Analytics;
using MaCode.Components;
using MaCode.Plugins;
using MaCode.Plugins.MaImage;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace MaCode.Extensions
{
    public static class MaCodeBase
    {
        public static IServiceCollection AddMaCode(this IServiceCollection services)
        {
            return services.AddSingleton<ISweetAlerts2, SweetAlerts2>();
        }
        public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services)
        {
            return services.AddScoped<IGoogleAnalytics, GoogleAnalytics>();
        }

        public static IServiceCollection AddMaComponents(this IServiceCollection services,Action<IComponentSettings> config)
        {
            ComponentSettings cps = new ComponentSettings();
            cps.ServicesNamespace ??= new List<string>();
            config.Invoke(cps);
            services.AddSingleton<IComponentSettings>(cps);

            try
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddSingleton<Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor, Microsoft.AspNetCore.Mvc.Infrastructure.ActionContextAccessor>();
            }
            catch
            {
            }
            services.AddSingleton<IIMaComponent, IMaComponent>();
            return services.AddScoped<RazorLightService>();
        }
        public static IServiceCollection AddMaImage(this IServiceCollection services,Action<ImageProcessSettings> config)
        {
            ImageProcessSettings ims = new ImageProcessSettings();
            config.Invoke(ims);
            services.AddSingleton<IImageProcessSettings>(ims);
            return services.AddScoped<IImageProcess, ImageProcess>();
        }


        internal static string GetString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
        internal static int GetTotal(this ReportData reportData)
        {
            return int.Parse(reportData.Totals[0].Values[0]);
        }
        internal static string GetReportDate(this DateTime dateTime)
        {
            return $"{dateTime.Year}-{dateTime.Month.ToString("d2")}-{dateTime.Day.ToString("d2")}";
        }
    }
}
