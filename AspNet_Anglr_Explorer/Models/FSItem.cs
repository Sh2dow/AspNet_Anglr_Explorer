﻿using System;
using System.Collections.Generic;

namespace AspNet_Anglr_Explorer.Models
{
    public class FSItem
    {
        public string Name { get; set; }
        public string relPath { get; set; }
        public bool isDirectory { get; set; }
        public Int64? Size { get; set; }
        public List<Int64> NestedItems { get; set; }
    }
}