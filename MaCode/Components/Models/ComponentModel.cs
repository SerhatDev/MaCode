using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaCode.Components.Models
{
    public class ComponentModel
    {
        public string name { get; set; }
        public string view { get; set; }
        public bool useCache { get; set; }
        public List<string> injectables { get; set; }

        public Dictionary<string, object> values { get; set; }

        public string ComponentValues
        {
            get {
                if (values == null && values.Count < 1)
                    return string.Empty;
                else
                    return values.Select(x => $"{x.Key}@{x.Value}").Aggregate((a, b) => a + "|" + b);
            }
        }
    }
}
