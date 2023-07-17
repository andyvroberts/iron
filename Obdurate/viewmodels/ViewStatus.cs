using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Obdurate.viewmodels
{
  public class ViewStatus: INotifyPropertyChanged
  {
    // status properties
    private string statusMessage;
    private bool isError;
    private string folderPath;
    private bool isActivity = false;

    public string StatusMessage
    {
      get { return statusMessage; }
      set { statusMessage = value; OnPropChanged("statusMessage"); }
    }

    public bool IsError
    {
      get { return isError; }
      set { isError = value; OnPropChanged("IsError"); }
    }

    public string FolderPath
    {
      get { return folderPath; }
      set { folderPath = value; OnPropChanged("FolderPath"); }
    }

    public bool IsActivity
    {
      get { return isActivity; }
      set { isActivity = value; OnPropChanged("IsActivity"); }
    }

    //
    // create mandatory event and event handler for Notify property changed
    //
    public event PropertyChangedEventHandler PropertyChanged;
    //
    protected void OnPropChanged(string name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
