using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace WYSDumper
{
    public static class Constants
    {
        public static string ObjectYYTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "YYObjectTemplate.yy"));
        public static string ObjectYYTemplateSpriteless = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "YYObjectTemplateSpriteless.yy"));
        public static string ProjectStringEntry = "{\"id\":{\"name\":\"#object#\",\"path\":\"objects/#object#/#object#.yy\",},\"order\":0,},";
    }
}
