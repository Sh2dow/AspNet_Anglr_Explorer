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
        private string localPath { get; set; }

        public LocalFSItemManager(string path)
        {
            this.localPath = path == "secret" ? "secret" : HttpUtility.UrlDecode(path);
        }

        public async Task<IEnumerable<FSItem>> Get(string path)
        {
            List<FSItem> fsitems = null;
            var fsFolder = new DirectoryInfo(this.localPath);
            path = HttpUtility.UrlPathEncode(fsFolder.FullName);
            fsitems = new List<FSItem>();

            if (localPath != "secret" && new DriveInfo(Directory.GetDirectoryRoot(localPath)).IsReady)
            {
                string fipath;
                if (Directory.GetDirectoryRoot(localPath) == localPath)
                    fipath = "secret";
                else
                    fipath = HttpUtility.UrlPathEncode(fsFolder.Parent.FullName);

                await Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var files = fsFolder.EnumerateFiles()
                                                    .Select(fi => new FItem
                                                    {
                                                        name = fi.Name,
                                                        path = path + @"\" + fi.Name
                                                    })
                                                    .ToList();

                            var folders = fsFolder.EnumerateDirectories()
                                                        .Select(di => new DItem
                                                        {
                                                            name = di.Name,
                                                            path = path + @"\" + di.Name
                                                        })
                                                        .ToList();
                            fsitems.Add(new PItem
                            {
                                path = fipath,
                                name = localPath,
                                NestedItems = getNestedItems(fsFolder)
                            });
                            fsitems.AddRange(folders);
                            fsitems.AddRange(files);
                        }
                        catch
                        {
                            fsitems.Add(new PItem
                            {
                                path = fipath,
                                name = localPath,
                            });
                        }
                    });

            }
            else
            {
                await Task.Factory.StartNew(() =>
                {
                    var drives = Environment.GetLogicalDrives()
                                                .Select(di => new DItem
                                                {
                                                    name = di.FirstOrDefault().ToString() + @":\",
                                                    path = di.FirstOrDefault().ToString() + @":\"
                                                })
                                                .ToList();
                    fsitems.Add(new PItem
                    {
                        path = "secret",
                        name = "My Computer",
                    });
                    fsitems.AddRange(drives);
                });
                return fsitems;
            }
            return fsitems;
        }

        public List<Int64> getNestedItems(DirectoryInfo di)
        {
            var ni = new List<Int64> { 0, 0, 0 };

            var files = new List<FileInfo>();
            var dirs = di.GetDirectories();
            foreach (var dir in dirs)
            {
                try
                {
                    files.AddRange(dir.GetFiles("*.*", SearchOption.AllDirectories));
                }
                catch { }
            }

            ni[0] += (files.Where(x => x.Length <= (1024 * 1024 * 10)).ToArray().LongLength);
            ni[1] += (files.Where(x => x.Length >= (1024 * 1024 * 10)).Where(x => x.Length <= (1024 * 1024 * 50)).ToArray().LongLength);
            ni[2] += (files.Where(x => x.Length >= (1024 * 1024 * 100)).ToArray().LongLength);
            return ni;
        }

        public bool FileExists(string fileName)
        {
            var file = Directory.GetFiles(this.localPath, fileName)
                                .FirstOrDefault();
            return file != null;
        }
    }
}
