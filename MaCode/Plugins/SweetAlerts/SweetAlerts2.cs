using MaCode.Extensions;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaCode.Plugins
{
    public class SweetAlerts2 : ISweetAlerts2
    {
        private string scriptTagRoot = @"<script type='text/javascript' defer>
                                            function _maCodeSwAlert(){
                                                {0}
                                            };
                                            _maCodeSwAlert();       
                                         </script>";
        public ISweetAlerts2 Instance()
        {
            return this;
        }

        public IHtmlContent UseSwal(string options = "")
        {
            var content = new HtmlContentBuilder()
                    .AppendHtml("<script src='https://cdn.jsdelivr.net/npm/sweetalert2@9'></script>");
            return content;
        }
        /// <summary>
        /// Shows a dialog
        /// </summary>
        /// <param name="title">Title of dialog</param>
        /// <param name="message">Message</param>
        /// <param name="icon">can be 'warning','success','question'</param>
        /// <param name="showCancelButton">Set if cancel button should be shown</param>
        /// <param name="confirmButtonColor">Set the color of confirm button</param>
        /// <param name="cancelButtonColor">Set the color of cancel button</param>
        /// <param name="cancelButtonText">Set the text of cancel button</param>
        /// <param name="confirmButtonText">Set the text of the confirm button</param>
        /// <param name="successCallback">Set which JS method will be called after confirm button click</param>
        /// <param name="cancelCallback">Set which JS method will be called after cancel button click</param>
        /// <param name="addScriptTag">Set if script tag should be rendered also</param>
        /// <returns></returns>
        public IHtmlContent Dialog(
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
            bool when=true)
        {
            if (!when)
                return null;

            string questionRoot = @$"Swal.fire({{
                                          title: '{title}',
                                          text: '{message}',
                                          icon: '{icon}',
                                          showCancelButton: {showCancelButton.ToString().ToLower()},
                                          confirmButtonColor: '{confirmButtonColor}',
                                          cancelButtonColor: '{cancelButtonColor}',
                                          cancelButtonText: '{cancelButtonText}',
                                          confirmButtonText: '{confirmButtonText}'
                                        }}).then((result) => {{
                                            if (result.value)
                                            {{
                                                {successCallback}
                                            }}else{{
                                                {cancelCallback}                
                                            }}
                                        }})";


            var content = new HtmlContentBuilder();

            if (addScriptTag)
            {
                content.AppendHtml(scriptTagRoot.Replace("{0}", questionRoot));
            }
            else
            {
                content.AppendHtml(questionRoot);
            }

            return content;
        }

        /// <summary>
        /// Shows an Error Message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="showButton"></param>
        /// <param name="buttonText"></param>
        /// <param name="toast"></param>
        /// <param name="toastTimeout"></param>
        /// <param name="toastPosition"></param>
        /// <param name="addScriptTag"></param>
        /// <returns></returns>
        public IHtmlContent Error(string title, string message, bool showButton = false, string buttonText = "Okay", bool toast = false, int toastTimeout = 1500, string toastPosition = "top-end", bool addScriptTag = false, bool when = true,string afterClosed="")
        {
            if (when == false)
                return null;
            string btnText = showButton switch
            {
                bool sb when sb => $",confirmButtonText: '{buttonText}'",
                _ => string.Empty
            };

            if (toast)
                showButton = false;
            string toastText = toast switch
            {
                bool t when t => $",position: '{toastPosition}',timer:{toastTimeout},timerProgressBar: true",
                _ => string.Empty
            };

            string closedCallback = afterClosed switch
            {
                string s when !string.IsNullOrEmpty(s) => $",onAfterClose:{afterClosed}",
                _ => string.Empty
            };

            string contentHtml = @$"Swal.fire(
                                    {{
                                    title: '{title}',
                                    text: '{message}',
                                    showConfirmButton:{showButton.ToString().ToLower()},
                                    icon: 'error'
                                    {closedCallback}
                                    {toastText}
                                    {btnText}
                        }});";
            var content = new HtmlContentBuilder();

            if (addScriptTag)
            {
                content.AppendHtml(scriptTagRoot.Replace("{0}", contentHtml));
            }
            else
            {
                content.AppendHtml(contentHtml);
            }

            return content;
        }

        /// <summary>
        /// Shows a Success Message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="showButton"></param>
        /// <param name="buttonText"></param>
        /// <param name="toast"></param>
        /// <param name="toastTimeout"></param>
        /// <param name="toastPosition"></param>
        /// <param name="addScriptTag"></param>
        /// <returns></returns>
        public IHtmlContent Success(string title, string message, bool showButton = false, string buttonText = "Okay", bool toast = false, int toastTimeout = 1500, string toastPosition = "top-end", bool addScriptTag = false, bool when=true,string afterClosed="",int timer=0)
        {
            if (when == false)
                return null;
            string btnText = showButton switch
            {
                bool sb when sb => $",confirmButtonText: '{buttonText}'",
                _ => string.Empty
            };
            if (toast)
                showButton = false;
            string toastText = toast switch
            {
                bool t when t => $",position: '{toastPosition}',timer:{toastTimeout},timerProgressBar: true",
                _ => string.Empty
            };

            string closedCallback = afterClosed switch
            {
                string s when !string.IsNullOrEmpty(s) => $",onAfterClose:() => {{ {afterClosed} }}",
                _ => string.Empty
            };
            string timerText = "";
            if(string.IsNullOrEmpty(toastText) && timer > 0)
            {
                timerText = $",timer: {timer},timerProgressBar: true";
            }
            string contentHtml = @$"Swal.fire(
                                    {{
                                    title: '{title}',
                                    text: '{message}',
                                    showConfirmButton:{showButton.ToString().ToLower()},
                                    icon: 'success'
                                    {timerText}
                                    {closedCallback}
                                    {toastText}
                                    {btnText}
                        }});";
            var content = new HtmlContentBuilder();
            if (addScriptTag)
            {
                content.AppendHtml(scriptTagRoot.Replace("{0}", contentHtml));
            }
            else
            {
                content.AppendHtml(contentHtml);
            }

            return content;
        }

        public IHtmlContent Success(string title, string message, IHtmlContent afterClosed, bool showButton = false, string buttonText = "Okay", bool toast = false, int toastTimeout = 1500, string toastPosition = "top-end", bool addScriptTag = false, bool when = true,  int timer = 0)
        {
            if (when == false)
                return null;
            string btnText = showButton switch
            {
                bool sb when sb => $",confirmButtonText: '{buttonText}'",
                _ => string.Empty
            };
            if (toast)
                showButton = false;
            string toastText = toast switch
            {
                bool t when t => $",position: '{toastPosition}',timer:{toastTimeout},timerProgressBar: true",
                _ => string.Empty
            };

            string closedCallback = afterClosed switch
            {
                IHtmlContent s when s is { } => $",onAfterClose:() => {{ {afterClosed.GetString()} }}",
                _ => string.Empty
            };
            string timerText = "";
            if (string.IsNullOrEmpty(toastText) && timer > 0)
            {
                timerText = $",timer: {timer},timerProgressBar: true";
            }
            string contentHtml = @$"Swal.fire(
                                    {{
                                    title: '{title}',
                                    text: '{message}',
                                    showConfirmButton:{showButton.ToString().ToLower()},
                                    icon: 'success'
                                    {timerText}
                                    {closedCallback}
                                    {toastText}
                                    {btnText}
                        }});";
            var content = new HtmlContentBuilder();
            if (addScriptTag)
            {
                content.AppendHtml(scriptTagRoot.Replace("{0}", contentHtml));
            }
            else
            {
                content.AppendHtml(contentHtml);
            }

            return content;
        }

    }

}
