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
  /// Interaction logic for BusinessUnitOld.xaml
  /// </summary>
  public partial class BusinessUnitOld : Page
  {
    private ConnectionDetails conn = new ConnectionDetails();
    private BusinessUnitHierarchy hierarchy = new BusinessUnitHierarchy();
    private BusinessUnitSet data;
    private ViewStatus vStat = new ViewStatus();

    public BusinessUnitOld()
    {
      InitializeComponent();
      actionButton.Content = "Connect";
      dbMenuGrid.DataContext = conn;
      statusBar.DataContext = vStat;
      busUnitTree.ItemsSource = hierarchy.AllLevels;
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

          hierarchy.ConvertToHierarchy(data);
          busUnitTree.UpdateLayout();
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



    // ########################################################################
    //
    // The TreeView actions
    //
    private void expDivisonChange(object sender, RoutedEventArgs e)
    {
      using (new WaitCursor())
      {
        foreach (HierarchyLevel item in busUnitTree.Items)
        {
          TreeViewItem tvi = busUnitTree.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
          if (tvi != null && item.Key == 1)
            tvi.IsExpanded = (bool)expDivison.IsChecked;
        }
        busUnitTree.UpdateLayout();
      }
    }
    //
    private void expRegionChange(object sender, RoutedEventArgs e)
    {
      using (new WaitCursor())
      {
        var level2 = from HierarchyLevel hl in busUnitTree.Items
                     from HierarchyLevel sl in hl.SubLevel
                     where sl.Key == 2
                     select sl;
        //
        foreach (HierarchyLevel l2 in level2)
        {
          TreeViewItem tvi = busUnitTree.ItemContainerGenerator.ContainerFromItem(l2) as TreeViewItem;
          if (tvi != null)
            tvi.IsExpanded = (bool)expRegion.IsChecked;
        }
        busUnitTree.UpdateLayout();
      }
    }
    //
    // ########################################################################

  }
}
