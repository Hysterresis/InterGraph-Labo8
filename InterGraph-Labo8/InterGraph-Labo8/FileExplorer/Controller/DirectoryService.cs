using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FileExplorer.Model
{
    class DirectoryService
    {
        public static IList<FileInfo> GetFiles(string directory)
        {
            try
            {
                return (from x in Directory.GetFiles(directory)
                        select new FileInfo(x)).ToList();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return new List<FileInfo>();
        }

        public static IList<DirectoryInfo> GetDirectories(string directory)
        {
            try
            {
                return (from x in Directory.GetDirectories(directory)
                        select new DirectoryInfo(x)).ToList();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return new List<DirectoryInfo>();
        }

        public static IList<DriveInfo> GetDrives()
        {
            return (from x in DriveInfo.GetDrives() select x).ToList();
        }
    }
}