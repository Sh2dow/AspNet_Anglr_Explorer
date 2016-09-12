using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FSExplorer.Models;
using System.IO;
using System.Net.Http;

namespace FSExplorer.BL.Logic
{
    public class LocalFSItemManager : IFSItemManager
    {

        private string workingFolder { get; set; }
        public LocalFSItemManager()
        {

        }

        public LocalFSItemManager(string workingFolder)
        {
            this.workingFolder = workingFolder;
            CheckTargetDirectory();
        }

        public bool FileExists(string fileName)
        {
            var file = Directory.GetFiles(this.workingFolder, fileName)
                                .FirstOrDefault();

            return file != null;
        }

        private void CheckTargetDirectory()
        {
            if (!Directory.Exists(this.workingFolder))
            {
                throw new ArgumentException("the destination path " + this.workingFolder + " could not be found");
            }
        }

        public async Task<IEnumerable<FSItem>> Get()
        {
            List<FSItem> fsitems = new List<FSItem>();

            DirectoryInfo fsFolder = new DirectoryInfo(this.workingFolder);

            await Task.Factory.StartNew(() =>
            {
                fsitems = fsFolder.EnumerateFiles()
                                            .Select(fi => new FSItem
                                            {
                                                Name = fi.Name,
                                                Size = fi.Length / 1024
                                            })
                                            .ToList();
            });

            return fsitems;
        }

        public async Task<IEnumerable<FSItem>> Add(HttpRequestMessage request)
        {
            var provider = new PhotoMultipartFormDataStreamProvider(this.workingFolder);

            await request.Content.ReadAsMultipartAsync(provider);

            var fsitems = new List<FSItem>();

            foreach (var file in provider.FileData)
            {
                var fileInfo = new FileInfo(file.LocalFileName);

                fsitems.Add(new FSItem
                {
                    Name = fileInfo.Name,
                    Size = fileInfo.Length / 1024
                });
            }

            return fsitems;
        }
    }
}