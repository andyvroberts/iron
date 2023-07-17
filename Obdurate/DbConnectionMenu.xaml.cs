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
using System.Globalization;

namespace Obdurate
{
  /// <summary>
  /// Interaction logic for DbConnectionMenu.xaml
  /// </summary>
  public partial class DbConnectionMenu : UserControl
  {
    public DbConnectionMenu()
    {
      InitializeComponent();
    }

    // ########################################################################
    //
    // the open menu panel button actions
    //
    private void OpenDbMenuPanel(object sender, RoutedEventArgs e)
    {
      connectionPanel.IsOpen = true;
      openDbMenu.Visibility = Visibility.Collapsed;
      closeDbMenu.Visibility = Visibility.Visible;
    }
    //
    // the close menu panel button actions
    //
    private void CloseDbMenuPanel(object sender, RoutedEventArgs e)
    {
      connectionPanel.IsOpen = false;
      closeDbMenu.Visibility = Visibility.Collapsed;
      openDbMenu.Visibility = Visibility.Visible;
    }
    //
    // When the popup opens and has focus
    //
    private void connectionPanel_Closed(object sender, EventArgs e)
    {
      closeDbMenu.Visibility = Visibility.Collapsed;
      openDbMenu.Visibility = Visibility.Visible;
    }
    //
    // When any textbox has focus pre-select the entire contents
    //
    private void TextBoxHasFocus(object sender, RoutedEventArgs e)
    {
      (sender as TextBox).SelectAll();
    }
    //
    // ########################################################################


  }
}
