using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Core.MVC
{
    public class RouteMVC
    {
        public String Name     { get; set; }
        public String Template { get; set; }
        public String Default { get; set; } = String.Empty;

        public RouteMVC(String name, String template,String _default)
        {
            Name = name;
            Template = template;
            Default = _default;
        }
    }
}
