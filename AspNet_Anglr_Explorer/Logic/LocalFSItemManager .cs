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

            var relPath = fsFolder.FullName.Replace(@"\", "/"); ;

            //DriveInfo drives = new DriveInfo(this.currentDir);

            //// checks if the logical drive is ready
            //if (!drives.IsReady)
            //{
            //    this.currentDir = String.Empty;
            //}
            string parRelPath = "";
            if (Directory.GetParent(currentDir).Exists)
            {
                parRelPath = ExtensionMethods.RelativeFromAbsolutePath(fsFolder.Parent.FullName);
            }
            else
            {
                parRelPath = ExtensionMethods.RelativeFromAbsolutePath(fsFolder.Root.FullName);
            }
            parRelPath = Path.Combine(parRelPath);

            await Task.Factory.StartNew(() =>
            {
                fsitems = new List<FSItem>();
                var files = fsFolder.EnumerateFiles()
                                        .Select(fi => new FItem
                                        {
                                            name = fi.Name,
                                            nameAs = fi.Name,
                                            Size = fi.Length
                                        })
                                        .ToList();

                var folders = fsFolder.EnumerateDirectories()
                                            .Select(di => new DItem
                                            {
                                                name = di.Name,
                                                nameAs = di.Name,
                                            })
                                            .ToList();
                fsitems.Add(new RItem
                {
                    name = parRelPath,
                    nameAs = "..",
                    curPath = currentDir,
                    relPath = Path.Combine(currentDir),
                    NestedItems = getNestedItems(fsFolder)
                });
                fsitems.AddRange(folders);
                fsitems.AddRange(files);
            });

            return fsitems;
        }
        public List<Int64> getNestedItems(DirectoryInfo di)
        {
            var ni = new List<Int64> { 0, 0, 0 };

            List<FileInfo> fileDetails = new List<FileInfo>();
            var files = di.GetFiles("*.*", SearchOption.AllDirectories);
            ni[0] += (files.Where(x => x.Length <= (1024 * 1024 * 10)).ToArray().LongLength);
            ni[1] += (files.Where(x => x.Length >= (1024 * 1024 * 10)).Where(x => x.Length <= (1024 * 1024 * 50)).ToArray().LongLength);
            ni[2] += (files.Where(x => x.Length >= (1024 * 1024 * 100)).ToArray().LongLength);
            return ni;
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
                //return path.Replace(@"\", "/");
                return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
            }

            throw new InvalidOperationException("We can only map an absolute back to a relative path if an HttpContext is available.");
        }
    }
}