using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyDesktopSort
{
    public class MoveDirective
    {
        public string Foldername { get; set; }
        public string[] Keywords { get; set; }
        public bool ReplaceKeywords { get; set; }
    }
}
