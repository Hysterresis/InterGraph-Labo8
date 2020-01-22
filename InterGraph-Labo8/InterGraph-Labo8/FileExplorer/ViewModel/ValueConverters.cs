using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using FileExplorer.Model;

namespace FileExplorer.ViewModel
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class DirInfoConverter : BaseConverter, IValueConverter
    {
        public DirInfoConverter()
            :base()
        {}
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                DirInfo nodeToExpand = value as DirInfo;
                if (nodeToExpand == null)
                    return null;

                 //return the subdirectories of the Current Node
                 if ((ObjectType)nodeToExpand.DirType == ObjectType.MyComputer)
                 {
                     return (from sd in DirectoryService.GetDrives()
                                     select new DirInfo(sd)).ToList();
                 }
                 else
                 {
                    IList<DirInfo> childDirList = new List<DirInfo>();
                    IList<DirInfo> childFileList = new List<DirInfo>();
                    //Combine all the subdirectories and files of the current directory
                    childDirList = (from dir in DirectoryService.GetDirectories(nodeToExpand.Path)
                                    select new DirInfo(dir)).ToList();

                    childFileList = (from fobj in DirectoryService.GetFiles(nodeToExpand.Path)
                                     select new DirInfo(fobj)).ToList();

                    return childDirList.Concat(childFileList).ToList();
                }
                 
            }
            catch
            { 
                return null; 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
