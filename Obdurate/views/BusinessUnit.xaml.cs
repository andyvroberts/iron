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
using Obdurate.database;
using Obdurate.viewmodels;

namespace Obdurate.views
{
  /// <summary>
  /// Interaction logic for BusinessUnit.xaml
  /// </summary>
  public partial class BusinessUnit : Page
  {
    private ConnectionDetails conn = new ConnectionDetails();
    private BusinessUnitSet data;
    private ViewStatus vStat = new ViewStatus();

    public BusinessUnit()
    {
      InitializeComponent();
      actionButton.Content = "Connect";
      dbMenuGrid.DataContext = conn;
      statusBar.DataContext = vStat;
    }

    // ########################################################################
    //
    // the popup button actions
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
    // ########################################################################



    // ########################################################################
    //
    // Connect or Reload to/from Database
    //
    private void ReadDatabase(object sender, RoutedEventArgs e)
    {
      using (new WaitCursor())
      {
        try
        {
          data = new BusinessUnitSet(conn);
          data.FetchBusinessUnits();
          vStat.StatusMessage = string.Format("Database connection OK | {0} business units found.",
            data.Count());
          actionButton.Content = "Reload";

          treeSearchText.Focus();
        }
        catch (Exception err)
        {
          vStat.StatusMessage = err.Message;
        }
      }
    }
    //
    // ########################################################################


  }
}
