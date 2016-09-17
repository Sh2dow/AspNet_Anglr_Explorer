using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using AspNet_Anglr_Explorer.Models;

namespace AspNet_Anglr_Explorer.Logic
{
    public class LocalFSItemManager : IFSItemManager
    {

        private string currentDir { get; set; }
        public LocalFSItemManager()
        {
        }

        public LocalFSItemManager(string currentDir)
        {
            this.currentDir = currentDir;
            CheckTargetDirectory();
        }

        public bool FileExists(string fileName)
        {
            var file = Directory.GetFiles(this.currentDir, fileName)
                                .FirstOrDefault();

            return file != null;
        }

        private void CheckTargetDirectory()
        {
            if (!Directory.Exists(this.currentDir))
            {
                throw new ArgumentException("the destination path " + this.currentDir + " could not be found");
            }
        }

        public async Task<IEnumerable<FSItem>> Get()
        {
            List<FSItem> fsitems = null;
            DirectoryInfo fsFolder = new DirectoryInfo(this.currentDir);
            await Task.Factory.StartNew(() =>
            {
                fsitems = new List<FSItem>();
                var files = fsFolder.EnumerateFiles()
                                        .Select(fi => new FSItem
                                        {
                                            Name = fi.Name,
                                            isDirectory = false,
                                            Size = fi.Length
                                        })
                                        .ToList();

                var folders = fsFolder.EnumerateDirectories()
                                            .Select(di => new FSItem
                                            {
                                                Name = di.Name,
                                                isDirectory = true,
                                                Size = 0
                                            })
                                            .ToList();
                fsitems.Add(new FSItem
                {
                    Name = "..",
                    isDirectory = false,
                    relPath = fsFolder.FullName
                });
                fsitems.AddRange(folders);
                fsitems.AddRange(files);
            });

            return fsitems;
        }

        public async Task<IEnumerable<FSItem>> Get(string path)
        {
            this.currentDir = path;
            return await Get();
        }
    }

    public static class ExtensionMethods
    {
        public static string RelativeFromAbsolutePath(string path)
        {
            if (HttpContext.Current != null)
            {
                var request = HttpContext.Current.Request;
                var applicationPath = request.PhysicalApplicationPath;
                var virtualDir = request.ApplicationPath;
                virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
                return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
            }

            throw new InvalidOperationException("We can only map an absolute back to a relative path if an HttpContext is available.");
        }
    }
}