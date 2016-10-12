using System;
using System.Collections.Generic;

namespace AspNet_Anglr_Explorer.Models
{
    public abstract class FSItem
    {
        public string name { get; set; }
        public string path { get; set; }
        public bool isDirectory { get; set; }
    }

    public class FItem : FSItem
    {
        public FItem()
        {
            isDirectory = false;
        }
    }

    public class DItem : FSItem
    {
        public DItem()
        {
            isDirectory = true;
        }
    }

    public class PItem : FSItem
    {
        public List<Int64> NestedItems { get; set; }
        public PItem()
        {
            isDirectory = false;
        }
    }
}