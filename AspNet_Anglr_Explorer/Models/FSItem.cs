using System;
using System.Collections.Generic;

namespace AspNet_Anglr_Explorer.Models
{
    public abstract class FSItem
    {
        public string name { get; set; }
        public string nameAs { get; set; }
        public bool isDirectory { get; set; }
    }

    public class FItem : FSItem
    {
        public Int64? Size { get; set; }
        public FItem()
        {
            isDirectory = false;
        }
    }

    public class DItem : FSItem
    {
        public List<Int64> NestedItems { get; set; }
        public DItem()
        {
            isDirectory = true;
        }
    }

    public class RItem : FSItem
    {
        public string curPath { get; set; }
        public string relPath { get; set; }
        public string parPath { get; set; }
        public List<Int64> NestedItems { get; set; }
        public RItem()
        {
            isDirectory = false;
        }
    }
}