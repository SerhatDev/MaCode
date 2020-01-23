using Microsoft.AspNetCore.Html;

namespace MaCode.Plugins
{
    public interface ISweetAlerts2
    {
        /// <summary>
        /// Adds Required JS file reference
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IHtmlContent UseSwal(string options = "");


        IHtmlContent Dialog(
            string title,
            string message,
            string icon = "question",
            bool showCancelButton = true,
            string confirmButtonColor = "#3085d6",
            string cancelButtonColor = "#d33",
            string cancelButtonText = "Cancel",
            string confirmButtonText = "Yes",
            string successCallback = "",
            string cancelCallback = "",
            bool addScriptTag = false,
            bool when=true);


        IHtmlContent Error(
            string title,
            string message,
            bool showButton = false,
            string buttonText = "Okay",
            bool toast = false,
            int toastTimeout = 1500,
            string toastPosition = "top-end",
            bool addScriptTag = false,
            bool when = true,
            string afterClosed = "");


        IHtmlContent Success(
            string title,
            string message,
            bool showButton = false,
            string buttonText = "Okay",
            bool toast = false,
            int toastTimeout = 1500,
            string toastPosition = "top-end",
            bool addScriptTag = false,
            bool when = true,
            string afterClosed = "",
            int timer = 0);
        IHtmlContent Success(
           string title,
           string message,
           IHtmlContent afterClosed,
           bool showButton = false,
           string buttonText = "Okay",
           bool toast = false,
           int toastTimeout = 1500,
           string toastPosition = "top-end",
           bool addScriptTag = false,
           bool when = true,
           int timer = 0);
    }
}