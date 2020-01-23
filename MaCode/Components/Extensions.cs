using System;
using System.Collections.Generic;
using System.Text;

namespace MaCode.Components
{
    public static class Extensions
    {
        public static T GetService<T>(this Dictionary<string,object> model,string serviceName)
        {
            return (T)model[serviceName];
        }
    }
}
