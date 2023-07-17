using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Obdurate.viewmodels
{
  internal class FileSet: ObservableCollection<FileItem>
  {
    // count the files in this container
    public int FileCount
    {
      get { return this.Count(); }
    }

    // return a collection of distinct extension types in this container
    public List<string> ExtensionTypes
    {
      get
      {
        return
          (from FileItem fi in this
           select fi.FileExtension).Distinct().ToList();
      }
    }

    //
    // accept a collection of extension types and for any matching extensions in 
    // this container, set the DisplayFile boolean value to true.  For all others
    // set it to false.  Made verbose for readability.
    //
    public void SetDisplayedFiles(List<string> extensionSet)
    {
      foreach (FileItem fi in this)
      {
        if (extensionSet.Any(x => fi.FileExtension.Equals(x)))
          fi.DisplayFile = true;
        else
          fi.DisplayFile = false;
      }
    }
    // and overloaded method to accept a single extension or sting.
    public void SetDisplayedFiles(string extension)
    {
      if (extension == "*")
      {
        foreach (FileItem fi in this)
        {
          fi.DisplayFile = true;
        }
      }
      else
      {
        foreach (FileItem fi in this)
        {
          if (fi.FileExtension.ToUpper() == extension.ToUpper())
            fi.DisplayFile = true;
          else
            fi.DisplayFile = false;
        }
      }
    }
    //
  }
}
