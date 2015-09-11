using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace AGM.Web.Infrastructure
{
    public class AppBundleOrderer : IBundleOrderer
    {
        private IEnumerable<string> _loadFirstFiles = new List<string>();

        public AppBundleOrderer()
        { }

        public AppBundleOrderer(IEnumerable<string> loadFirstFiles)
        {
            _loadFirstFiles = loadFirstFiles;
        }

        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            List<BundleFile> list = new List<BundleFile>();

            var bundleFiles = files as IList<BundleFile> ?? files.ToList();
            foreach (var file in _loadFirstFiles)
            {
                if (file.Contains('.'))
                {
                    var fileToCheck = string.Format("/{0}", file);
                    if (bundleFiles.Any(f => f.VirtualFile.VirtualPath.EndsWith(fileToCheck)))
                        bundleFiles.Where(f => f.VirtualFile.VirtualPath.EndsWith(fileToCheck)).ToList().ForEach(f => list.Add(f));
                }
                else
                {
                    var pathToCheck = string.Format("/{0}/", file);
                    if (bundleFiles.Any(f => f.VirtualFile.VirtualPath.Contains(pathToCheck)))
                        bundleFiles.Where(f => f.VirtualFile.VirtualPath.Contains(pathToCheck)).ToList().ForEach(f => list.Add(f));
                }
            }

            list.ForEach(f => bundleFiles.Remove(f));

            if (bundleFiles.Any())
                list.AddRange(bundleFiles);

            return list;
        }
    }
}