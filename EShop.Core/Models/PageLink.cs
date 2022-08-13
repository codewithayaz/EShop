using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Models
{
    public class PageLink
    {
        public PageLink()
        {

        }
        public PageLink(string name)
        {
            Name = name;
        }
        public SidebarMenuType Type { get; set; }
        public bool IsActive { get; set; } = false;
        public string Name { get; set; }
        public string IconClassName { get; set; }
        public string URLPath { get; set; }
        public List<PageLink> TreeChild { get; set; }
        public Tuple<int, int, int> LinkCounter { get; set; }
    }

    public enum SidebarMenuType
    {
        Header,
        Link,
        Tree
    }
}
