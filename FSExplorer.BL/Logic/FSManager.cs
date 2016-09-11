using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FSExplorer.Models;
using System.IO;

namespace FSExplorer.BL.Logic
{
    public class FSManager : IFSManager
    {
        public FSManager()
        {

        }

        public bool FileExists(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FSItem>> Get()
        {
            var fsitems = new FSItem();

            DirectoryInfo fsFolder = new DirectoryInfo(this.location);

            await Task.Factory.StartNew(() =>
            {
                fsitems = photoFolder.EnumerateFiles()
                                            .Select(fi => new FSItem
                                            {
                                                Name = fi.Name,
                                                Size = fi.Length / 1024
                                            })
                                            .ToList();
            });

            return photos;
        }
    }
}