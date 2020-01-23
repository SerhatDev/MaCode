using MaCode.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace MaCode
{
    public abstract class Page : Microsoft.AspNetCore.Mvc.RazorPages.Page
    {

        private ISweetAlerts2 _Swal;
        public ISweetAlerts2 Swal
        {
            get {
                if (_Swal != null)
                    return _Swal;
                else
                {
                    _Swal = HttpContext.RequestServices.GetRequiredService<ISweetAlerts2>();
                    return _Swal;
                }
            }
        }
    }
}
