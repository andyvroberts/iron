using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Obdurate.viewmodels
{
  public class FileItem: INotifyPropertyChanged
  {
    private string fileName;
    private string filePath;
    private string fileSizeDisplay;
    private double fileSize;
    private string fileExtension;
    private bool displayFile;

    public string FileName
    {
      get { return fileName; }
      set { fileName = value; OnPropChanged("FileName"); }
    }
    public string FilePath
    {
      get { return filePath; }
      set { filePath = value; OnPropChanged("FilePath"); }
    }
    public string FileSizeDisplay
    {
      get
      {
        string level = string.Empty;
        if (fileSize < 1024)
          return string.Format("{0} bytes", fileSize);

        if (fileSize > 1024 * 1024 * 1024)
          return string.Format("{0} Gb", Math.Round(fileSize / (1024 * 1024 * 1024), 2));

        if (fileSize > 1024 * 1024)
          return string.Format("{0} Mb", Math.Round(fileSize / (1024 * 1024), 2));

        if (fileSize > 1024)
          return string.Format("{0} Kb", Math.Round(fileSize / 1024, 2));

        return "too big";
      }
      set { fileSizeDisplay = value; OnPropChanged("FileSizeDisplay"); }
    }
    public double FileSize
    {
      get { return fileSize; }
      set { fileSize = value; OnPropChanged("FileSize"); }
    }
    public string FileExtension
    {
      get { return fileExtension; }
      set { fileExtension = value; OnPropChanged("FileExtension"); }
    }
    public bool DisplayFile
    {
      get { return displayFile; }
      set { displayFile = value; OnPropChanged("DisplayFile"); }
    }


    //
    // create the required event and handler to notify of underlying data changes.
    //
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropChanged(string name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
