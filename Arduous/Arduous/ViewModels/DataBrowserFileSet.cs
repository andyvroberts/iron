using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Arduous.Models;

namespace Arduous.ViewModels
{
    internal class DataBrowserFileSet
    {
        private FileSet listedFiles;
        public FileSet ListedFiles
        {
            get { return listedFiles; }
        }

        /// <summary>
        /// Constructor used to initialize the FileSet.
        /// This occurs only once during entire lifecycle of calling page process.
        /// </summary>
        public DataBrowserFileSet()
        {
            listedFiles = new FileSet();
        }


        /// <summary>
        /// Controlling logic to open a directory and list the specified file types.
        /// </summary>
        /// <param name="folderPath">The chosen directory</param>
        /// <param name="fileTypes">The requirested file types to list</param>
        /// <param name="withSubFolders">include or exclude sub-folders of the directory</param>
        public void GetFilesInDirectory(string folderPath, string[] fileTypes, bool withSubFolders)
        {
            // guard clauses (no need for bool or fileTypes array).
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new Exception("No directory path was specified");

            listedFiles.Clear();

            if (CheckPathIsDirectory(folderPath))
            {
                FileInfo[] allFiles = FetchFilesFromDirectory(folderPath, withSubFolders);
                if (fileTypes.Count() == 0)
                    SelectAllFileTypes(allFiles);
                else
                    SelectFileTypes(allFiles, fileTypes);
            }
        }

        /// <summary>
        /// If the given path string is a directory on this device then return true.
        /// Otherwise throw an exception to be handled by the calling class.
        /// </summary>
        /// <param name="path">file system directory path</param>
        /// <returns>true or exception</returns>
        private bool CheckPathIsDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                throw new Exception(string.Format("The given path is not a directory: '{0}'", path));
            }
        }

        /// <summary>
        /// Use DirectoryInfo to retrieve a list of files in the given directory
        /// </summary>
        /// <param name="path">file system directory path</param>
        /// <param name="subFolders">top level or all nested directories</param>
        /// <returns>FileInfo[]</returns>
        private FileInfo[] FetchFilesFromDirectory(string path, bool subFolders)
        {
            FileInfo[] foundFiles = null;
            if (subFolders)
                foundFiles = new DirectoryInfo(path).GetFiles("*.*", SearchOption.AllDirectories);
            else
                foundFiles = new DirectoryInfo(path).GetFiles("*.*", SearchOption.TopDirectoryOnly);
            return foundFiles;
        }

        /// <summary>
        /// convert the file info array to an observable collection for the GUI.
        /// </summary>
        /// <param name="files">file information array: FileInfo[]</param>
        private void SelectAllFileTypes(FileInfo[] files)
        {
            foreach (FileInfo fi in files)
            {
                FileItem item = new FileItem();
                item.FilePath = fi.FullName;
                item.FileName = fi.Name;
                item.FileSize = fi.Length;
                item.FileExtension = fi.Extension.ToLower();
                item.DisplayFile = true;
                listedFiles.Add(item);
            }
        }

        /// <summary>
        /// for the specified file types only, convert the info array to an observable 
        /// collection for the GUI.
        /// </summary>
        /// <param name="files">file information array: FileInfo[]</param>
        /// <param name="types">file extension array: string[]</param>
        private void SelectFileTypes(FileInfo[] files, string[] types)
        {
            foreach (FileInfo fi in files)
            {
                if (types.Any(f => fi.Extension.ToLower().EndsWith(f)))
                {
                    FileItem item = new FileItem();
                    item.FilePath = fi.FullName;
                    item.FileName = fi.Name;
                    item.FileSize = fi.Length;
                    item.FileExtension = fi.Extension.ToLower();
                    item.DisplayFile = true;
                    listedFiles.Add(item);
                }
            }
        }




    }
}
