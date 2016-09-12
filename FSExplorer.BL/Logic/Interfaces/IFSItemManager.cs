using System.Collections.Generic;
using System.Threading.Tasks;
using FSExplorer.Models;
using System.Net.Http;

namespace FSExplorer.BL.Logic
{
    public interface IFSItemManager
    {
        Task<IEnumerable<FSItem>> Get();
        Task<IEnumerable<FSItem>> Add(HttpRequestMessage request);
        bool FileExists(string fileName);
    }
}