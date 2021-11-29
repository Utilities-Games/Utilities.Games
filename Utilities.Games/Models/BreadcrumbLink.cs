using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.Games.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <see href="https://stackoverflow.com/a/64721882/4585104"/>
    public class BreadcrumbLink
    {
        public int OrderIndex { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }
    }
    public class BreadcrumbConfig {
        public string Address { get; set; }

        public string Title { get; set; }
    }
}
