using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Obdurate
{
  /// <summary>
  /// Interaction logic for CurrentStatus.xaml
  /// </summary>
  public partial class CurrentStatus : UserControl, INotifyPropertyChanged
  {
    private bool showProg = false;
    public bool ShowProgress
    {
      get { return showProg; }
      set { showProg = value; OnPropChanged("ShowProgress"); }
    }

    public CurrentStatus()
    {
      InitializeComponent();
    }

    // ########################################################################
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
    //
    // ########################################################################

  }
}
