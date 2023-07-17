using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace Arduous.ViewModels
{
    public class Status : INotifyPropertyChanged
    {
        // fields
        private string statusMessage;
        private bool isError = false;
        private string folderPath;
        private string chosenFile;
        private string seperator = string.Empty;
        private string extension;
        private bool headers = false;
        private bool quoted = false;
        private int displays = 20000;

        // properties
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

        public string ChosenFile
        {
            get { return chosenFile; }
            set { chosenFile = value; OnPropChanged("ChosenFile"); }
        }

        public string FieldSeperator
        {
            get { return seperator; }
            set { seperator = value; OnPropChanged("FieldSeperator"); }
        }

        public bool HasHeaders
        {
            get { return headers; }
            set { headers = value; OnPropChanged("HasHeaders"); }
        }

        public bool IsQuoted
        {
            get { return quoted; }
            set { quoted = value; OnPropChanged("IsQuoted"); }
        }

        public int DisplayRows
        {
            get { return displays; }
            set { displays = value; OnPropChanged("DisplayRows"); }
        }

        public string Extension
        {
            get { return extension; }
            set { extension = value; OnPropChanged("Extension"); }
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
