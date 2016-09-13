using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSExplorer.Models;
using System.IO;

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
                var files = fsFolder.EnumerateFiles()
                                            .Select(fi => new FSItem
                                            {
                                                Name = fi.Name,
                                                Location = fi.DirectoryName,
                                                Size = fi.Length / 1024
                                            })
                                            .ToList();

                var folders = fsFolder.EnumerateDirectories()
                                            .Select(fi => new FSItem
                                            {
                                                Name = fi.Name,
                                                Location = fi.FullName,
                                                Size = 0
                                            })
                                            .ToList();

                var parentFolder = new FSItem
                {
                    Location = new DirectoryInfo(this.workingFolder).Parent.Name,
                    Name = ".."
                };
                fsitems.Add(parentFolder);
                fsitems.AddRange(files);
                fsitems.AddRange(folders);
                //fsitems.Add(new DirectoryInfo(this.workingFolder));
            });

            return fsitems;
        }

    }
}