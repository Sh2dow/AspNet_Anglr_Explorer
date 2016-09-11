using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FSExplorer.Models;

namespace FSExplorer.BL.Logic
{
    public interface IFSManager
    {
        Task<IEnumerable<FSItem>> Get();
        bool FileExists(string fileName);
    }
}