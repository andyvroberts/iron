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
using Obdurate.viewmodels;

namespace Obdurate.views
{
  /// <summary>
  /// Interaction logic for FileData.xaml
  /// </summary>
  public partial class FileData : Page
  {
    private ViewStatus vStat = new ViewStatus();
    private FileToDataTable fileContent;
    private string fileName;

    // constructor
    public FileData(string file, string seperator, bool hasNoHeaders, bool quoted)
    {
      InitializeComponent();
      vStat.IsError = false;
      statusBar.DataContext = vStat;
      statusEllipse.DataContext = vStat;

      fileName = file;
      dataHeaders.IsChecked = hasNoHeaders;
      dataQuoted.IsChecked = quoted;
      dataSeperator.Text = seperator;
      fileContent = new FileToDataTable(file);

      DisplayDataTableInGrid(seperator, hasNoHeaders, quoted);
    }


    // #####################################################################################
    // DATAGRID METHODS
    //
    private void DisplayDataTableInGrid(string seperator, bool hasNoHeaders, bool quoted)
    {
      using (new WaitCursor())
      {
        try
        {
          fileContent.FormatContents(seperator, hasNoHeaders, quoted);
          dataContent.ItemsSource = fileContent.FileDT.DefaultView;
          vStat.StatusMessage = string.Format("{0} records loaded from: {1}", 
            dataContent.Items.Count, fileName);
          vStat.IsError = false;
        }
        catch (Exception e)
        {
          vStat.StatusMessage = string.Format("Error: {0}", e.Message);
          vStat.IsError = true;
        }
      }

    }
    //
    // #####################################################################################


    // #####################################################################################
    // RELOAD DATA METHODS
    //
    // reload button click
    //
    private void ReloadDataFile(object sender, RoutedEventArgs e)
    {
      bool headers = (bool)dataHeaders.IsChecked;
      bool quoted = (bool)dataQuoted.IsChecked;

      statusBar.ShowProgress = true;
      DisplayDataTableInGrid(dataSeperator.Text, headers, quoted);
      dataSeperator.Focus();
      statusBar.ShowProgress = false;
    }
    //
    // #####################################################################################
  }
}
