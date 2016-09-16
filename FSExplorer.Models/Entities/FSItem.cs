using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSExplorer.Models
{
    public class FSItem
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public long Size { get; set; }
    }

    public class FolderItem : FSItem
    {
        public List<FSItem> NestedItems { get; set; }
    }
}