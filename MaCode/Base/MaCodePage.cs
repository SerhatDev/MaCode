using MaCode.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace MaCode
{
    public abstract class RazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
       
        private ISweetAlerts2 _Swal;
      
        /// <summary>
        /// Hmmm ?
        /// </summary>
        public ISweetAlerts2 Swal
        {
            get {
                if (_Swal != null)
                    return _Swal;
                else
                {
                    _Swal= Context.RequestServices.GetRequiredService<ISweetAlerts2>();
                    return _Swal;
                }
            }
        }
    }
}
