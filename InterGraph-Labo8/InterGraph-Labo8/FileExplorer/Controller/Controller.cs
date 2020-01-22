using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FileExplorer.Model;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Input;
using FileExplorer.ViewModel;
using InterGraph_Labo8.Properties;

namespace FileExplorer.Controller
{
    public class Controller
    {
        #region Members;
        private DirectoryViewModel _directoryViewModel;
        private DirInfo _rootDirectory;
        #endregion

        #region Constructor 
        public Controller(DirInfo rootDirectory = null)
        {
            RootDirectory = rootDirectory ?? new DirInfo(Resources.My_Computer_String);
            DirectoryViewModel = new DirectoryViewModel();
        }
        #endregion

        #region // Public Properties
        ///// <summary>
        ///// Name of the current directory user is in
        ///// </summary>
        public DirInfo SelectedDirectory { get; set; }

        public DirInfo RootDirectory { 
            get
            {
                return _rootDirectory;
            }
            set
            {
                _rootDirectory = value;
                DirectoryViewModel = new DirectoryViewModel();
            }
        }

        public DirectoryViewModel DirectoryViewModel
        {
            get { return _directoryViewModel; }
            set
            {
                _directoryViewModel = value;

                if (_directoryViewModel.Children == null)
                {
                    _directoryViewModel.Children = new List<DirInfo>() { RootDirectory };
                }
            }
        }

        #endregion


        #region Methods
        /// <summary>
        /// this method gets the children of current directory and stores them in the CurrentItems Observable collection
        /// </summary>
        protected IList<DirInfo> GetDirAndFiles()
        {
            IList<DirInfo> childDirList = new List<DirInfo>();
            IList<DirInfo> childFileList = new List<DirInfo>();

            //If current directory is "My computer" then get the all logical drives in the system
            if (SelectedDirectory.Name.Equals(Resources.My_Computer_String))
            {
                childDirList = (from rd in DirectoryService.GetDrives()
                                select new DirInfo(rd)).ToList();
            }
            else
            {
                //Combine all the subdirectories and files of the current directory
                childDirList = (from dir in DirectoryService.GetDirectories(SelectedDirectory.Path)
                                select new DirInfo(dir)).ToList();

                childFileList = (from fobj in DirectoryService.GetFiles(SelectedDirectory.Path)
                                 select new DirInfo(fobj)).ToList();

                childDirList = childDirList.Concat(childFileList).ToList();
            }

            return childDirList;
            //CurrentItems = childDirList;
        }

      
        #endregion
    }
}
