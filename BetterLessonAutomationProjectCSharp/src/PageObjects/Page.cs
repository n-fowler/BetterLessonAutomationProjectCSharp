using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterLessonAutomationProjectCSharp.src.PageObjects
{
    public static class Page
    {
        private static T GetPage<T>() where T : new()
        {
            var page = new T();
            return page;
        }

        public static HomePage Home => GetPage<HomePage>();
    }
}
