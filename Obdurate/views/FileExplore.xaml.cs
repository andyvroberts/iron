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
using System.Windows.Forms;
using System.IO;
using Obdurate.viewmodels;

namespace Obdurate.views
{
  /// <summary>
  /// Interaction logic for FileExplore.xaml
  /// </summary>
  public partial class FileExplore : Page
  {
    private FolderBrowserDialog fDial;
    private ViewStatus vStat = new ViewStatus();
    private FileSet filesModel;
    private CollectionViewSource fileViewSource;
    private List<string> chosenExtensions;
    private List<string> sortedExtensions;
    private string[] dataOps = { ".csv", ".dat", ".txt" };

    //
    // Initializers
    // 1. set the data contexts (for status messages)
    // 2. set the collection view source so that chosen files can be filtered
    //
    public FileExplore()
    {
      InitializeComponent();
      statusBar.DataContext = vStat;
      statusEllipse.DataContext = vStat;
      fileViewSource = FindResource("fileListView") as CollectionViewSource;
    }

    // #####################################################################################
    // OPEN FOLDER DIALOGUE METHODS
    //
    // Enable the Open folder Application Command
    //
    private void OpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }
    //
    // perform the open folder dialogue (and Application Command)
    //
    private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      fDial = new FolderBrowserDialog();
      fDial.Description = "Select the directory containing the data files";
      fDial.ShowNewFolderButton = false;

      if (vStat.FolderPath != null)
        fDial.SelectedPath = vStat.FolderPath;
      else
        fDial.RootFolder = Environment.SpecialFolder.MyComputer;

      DialogResult res = fDial.ShowDialog();

      if (res == DialogResult.OK)
      {
        vStat.FolderPath = fDial.SelectedPath;
        ReadDirectoryForFiles(fDial.SelectedPath);
      }
      else
      {
        vStat.FolderPath = "No folder selected";
      }
      folderText.Text = vStat.FolderPath;
    }
    //
    // allow manual entry of folder location on press of "enter" key in text box
    //
    private void ReturnExecuted()
    {
      if (folderText.Text != string.Empty && folderText.Text != null)
      {
        ReadDirectoryForFiles(folderText.Text.ToString());
      }
    }
    //
    // Validates the give location is a directory otherwise show an error
    //
    private void ReadDirectoryForFiles(string folder)
    {
      if (Directory.Exists(folder))
      {
        vStat.IsError = false;
        vStat.IsActivity = true;
        vStat.StatusMessage = string.Empty;
        fileTypesCheck.Visibility = Visibility.Hidden;
        ProcessDirectoryContent(folder);
        vStat.IsActivity = false;
      }
      else
      {
        vStat.StatusMessage = string.Format("Not a directory {0}", folder);
        vStat.IsError = true;
      }
    }
    // #####################################################################################



    // #####################################################################################
    // PROCESS DIRECTORY FILES AND BUILD EXTENSIONS CHECKBOX'S
    //
    // Read all the files in the given directory location
    //
    private void ProcessDirectoryContent(string dir)
    {
      filesModel = new FileSet();
      fileViewSource.Source = filesModel;
      FileInfo[] iFiles = null;

      using (new WaitCursor())
      {
        try
        {
          if (subFolders.IsChecked == true)
            iFiles = new DirectoryInfo(dir).GetFiles("*.*", SearchOption.AllDirectories);
          else
            iFiles = new DirectoryInfo(dir).GetFiles("*.*", SearchOption.TopDirectoryOnly);

          foreach (FileInfo fi in iFiles)
          {
            FileItem item = new FileItem();
            item.FilePath = fi.FullName;
            item.FileName = fi.Name;
            item.FileSize = fi.Length;
            item.FileExtension = fi.Extension.ToLower();
            item.DisplayFile = true;

            filesModel.Add(item);
          }

          if (filesModel.FileCount > 0)
          {
            BuildCheckListOfFileTypes();
          }
          vStat.StatusMessage = string.Format("{0} Files | Specified directory has been read OK", 
            filesModel.FileCount);
          vStat.IsError = false;
        }
        catch (Exception e)
        {
          vStat.StatusMessage = string.Format("ERROR - {0}", e.Message);
          vStat.IsError = true;
        }
      } // dispose WaitCursor
    }
    //
    // retrieve the ordered list of distinct file extensions from the file set
    // build a collection of ticked checkboxes and display on UI
    //
    private void BuildCheckListOfFileTypes()
    {
      chosenExtensions = new List<string>();
      fileTypesCheckPanel.Children.Clear();

      Thickness m = new Thickness();
      m.Left = 10;
      // show extensions in alphabetical order (default sort)
      sortedExtensions = filesModel.ExtensionTypes;
      sortedExtensions.Sort();

      foreach (string eType in sortedExtensions)
      {
        System.Windows.Controls.CheckBox cb = new System.Windows.Controls.CheckBox();
        cb.Tag = eType;
        cb.Content = eType;
        cb.IsChecked = true;
        cb.Margin = m;
        cb.Style = FindResource("checkList") as Style;
        chosenExtensions.Add(eType);
        cb.Click += SelectOrDeselectExtension;
        fileTypesCheckPanel.Children.Add(cb);
      }

      selectAllFileTypes.IsChecked = true;
      fileTypesCheck.Visibility = Visibility.Visible;
    }
    //
    // create the 'click' event for the constructed checkbox's.
    // remove/add extensions from/to the filtered set passed to the collection view.
    //
    private void SelectOrDeselectExtension(object sender, RoutedEventArgs e)
    {
      System.Windows.Controls.CheckBox chx = sender as System.Windows.Controls.CheckBox;
      if (chx.IsChecked == true)
        chosenExtensions.Add(chx.Tag.ToString());
      else
      {
        chosenExtensions.Remove(chx.Tag.ToString());
        // if all options have been unchecked make sure the 'all' is also unchecked.
        if (chosenExtensions.Count == 0)
          selectAllFileTypes.IsChecked = false;
      }
      // modify the contents of the collection view source.
      filesModel.SetDisplayedFiles(chosenExtensions);
      fileViewSource.Filter += new FilterEventHandler(FilterFileDisplay);
      GetFilteredCount();
    }
    //
    // select or deselect EVERY checkbox without firing each event handler
    //
    private void SelectOrDeselectAllExtensions(object sender, RoutedEventArgs e)
    {
      using (new WaitCursor())
      {
        System.Windows.Controls.CheckBox chx = sender as System.Windows.Controls.CheckBox;
        if (chx.IsChecked == true)
        {
          chosenExtensions.AddRange(sortedExtensions);
          foreach (System.Windows.Controls.CheckBox cb in fileTypesCheckPanel.Children)
          {
            cb.IsChecked = true;
          }
        }
        else
        {
          chosenExtensions.Clear();
          foreach (System.Windows.Controls.CheckBox cb in fileTypesCheckPanel.Children)
          {
            cb.IsChecked = false;
          }
        }
        // modify the contents of the collection view source.
        filesModel.SetDisplayedFiles(chosenExtensions);
        fileViewSource.Filter += new FilterEventHandler(FilterFileDisplay);
        GetFilteredCount();
      }
    }
    //
    // a filter method for the collection view.
    //
    private void FilterFileDisplay(object sender, FilterEventArgs e)
    {
      FileItem shallWeFilterThis = e.Item as FileItem;
      if (shallWeFilterThis != null)
      {
        e.Accepted = shallWeFilterThis.DisplayFile;
      }
    }
    //
    // return the count of the current filtered collection view.
    //
    private void GetFilteredCount()
    {
      int filterCount = fileViewSource.View.Cast<FileItem>().Count();
      vStat.StatusMessage = string.Format("{0} Files | File type filtering is active", 
        filterCount);
      vStat.IsError = false;
    }
    // #####################################################################################



    // #####################################################################################
    // UI CONTROL CLICK HANDLERS
    //
    // if the enter key is pressed in the folder selection text box then execute the folder search.
    //
    private void FolderTextKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        ReturnExecuted();
      }
    }
    //
    // de-select the file list to avoid complications in collection view filters
    //
    private void fileListLostFocus(object sender, RoutedEventArgs e)
    {
      fileList.SelectedItem = null;
    }
    // #####################################################################################




    // #####################################################################################
    // NAVIGATION AND MENU HANDLING
    //
    // Open a file.
    // 1. if a data file the go to data grid processing
    // 2. if other file then go to document viewer processing
    //
    private void ViewFile(object sender, MouseButtonEventArgs e)
    {
      FileItem chosenFile = (FileItem)fileList.SelectedItem;
      string sep = string.Empty;
      bool hasNoHeaders = false;
      bool colQuoted = true;
      // Do we have a data file type
      if (dataOps.Any(x => chosenFile.FileName.EndsWith(x)))
      {
        if (chosenFile.FileExtension == ".csv")
        {
          sep = ",";
          colQuoted = true;
          hasNoHeaders = false;
        }
        else
        {
          sep = string.Empty;
          hasNoHeaders = true;
          colQuoted = false;
        }
        //
        NavigationService ns = NavigationService.GetNavigationService(this);
        FileData fd = new FileData(chosenFile.FilePath, sep, hasNoHeaders, colQuoted);
        ns.Navigate(fd);
      }
      else
      {
        NavigationService ns = NavigationService.GetNavigationService(this);
        DocViewer dv = new DocViewer(chosenFile.FilePath, chosenFile.FileExtension);
        ns.Navigate(dv);
      }
    }
    // #####################################################################################


  }
}
