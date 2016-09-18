using System.Threading.Tasks;
using AspNet_Anglr_Explorer.Models;
using System.Collections.Generic;

namespace AspNet_Anglr_Explorer.Logic
{
    public interface IFSItemManager
    {
        //Task<IEnumerable<FSItem>> Get();
        Task<IEnumerable<FSItem>> Get(string path);
        bool FileExists(string fileName);
    }
}