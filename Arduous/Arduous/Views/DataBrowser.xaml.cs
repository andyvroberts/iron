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
using Arduous.Models;
using Arduous.ViewModels;

namespace Arduous.Views
{
    /// <summary>
    /// Interaction logic for DataBrowser.xaml
    /// </summary>
    public partial class DataBrowser : Page
    {
        private FolderBrowserDialog fDial;
        private CollectionViewSource fileViewSource;

        private Status pageStatus = new Status();
        private DataBrowserFileSet pageModel = new DataBrowserFileSet();

        private List<string> chosenExtensions;
        private List<string> sortedExtensions;
        private string[] dataOps = { ".csv", ".dat", ".txt", ".json", ".bson" };
        private string csv = ".csv";

        public DataBrowser()
        {
            InitializeComponent();
            fileViewSource = FindResource("FileListView") as CollectionViewSource;
            browserStatusBar.DataContext = pageStatus;
            fileViewSource.Source = pageModel.ListedFiles;
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
        // Execute the Open Application Command
        //
        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFolderBrowseDialogue();
        }

        //
        // execute the windows forms folder browse dialogue.
        //
        private void OpenFolderBrowseDialogue()
        {
            fDial = new FolderBrowserDialog();
            fDial.Description = "Select the directory containing the data files";
            fDial.ShowNewFolderButton = false;

            if (pageStatus.FolderPath != string.Empty)
                fDial.SelectedPath = pageStatus.FolderPath;
            else
                fDial.RootFolder = Environment.SpecialFolder.MyComputer;


            DialogResult res = fDial.ShowDialog();

            if (res == DialogResult.OK)
            {
                pageStatus.FolderPath = fDial.SelectedPath;
                folderText.Text = fDial.SelectedPath;
                ReadDirectoryForFiles(fDial.SelectedPath);
            }
            else
            {
                folderText.Text = "No folder selected";
                pageStatus.FolderPath = string.Empty;
            }
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
        // Call the ViewModel to obtain the file list.
        //
        private void ReadDirectoryForFiles(string folder)
        {
            using (new WaitCursor())
            {
                try
                {
                    pageModel.GetFilesInDirectory(folderPath: folder,
                                                  fileTypes: dataOps,
                                                  withSubFolders: (bool)subFolders.IsChecked);

                    if (pageModel.ListedFiles.FileCount > 0)
                    {
                        BuildCheckListOfFileTypes();
                    }
                    pageStatus.IsError = false;
                    pageStatus.StatusMessage = string.Format("{0} Files", pageModel.ListedFiles.FileCount);
                }
                catch (Exception e)
                {
                    pageStatus.IsError = true;
                    pageStatus.StatusMessage = string.Format("ERROR - {0}", e.Message);
                }
            }
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
            sortedExtensions = pageModel.ListedFiles.ExtensionTypes;
            sortedExtensions.Sort();

            foreach (string eType in sortedExtensions)
            {
                System.Windows.Controls.CheckBox cb = new System.Windows.Controls.CheckBox();
                cb.Tag = eType;
                cb.Content = eType;
                cb.IsChecked = true;
                cb.Margin = m;
                cb.Style = FindResource("CheckList") as Style;
                chosenExtensions.Add(eType);
                cb.Click += SelectOrDeselectExtension;
                fileTypesCheckPanel.Children.Add(cb);
            }

            selectAllFileTypes.IsChecked = true;
            fileTypesCheck.Visibility = Visibility.Visible;
        }

        // #####################################################################################



        // #####################################################################################
        // MANIPULATE THE APPEARANCE OF CHECKBOXES AND THE COLLECTION VIEW SOURCE OF FILES
        //
        // create the handler for the 'click' event for the constructed checkbox's.
        // remove/add file extensions from/to the filtered set passed to the collection view.
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
            pageModel.ListedFiles.SetDisplayedFiles(chosenExtensions);
            //filesModel.SetDisplayedFiles(chosenExtensions);
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
                pageModel.ListedFiles.SetDisplayedFiles(chosenExtensions);
                //filesModel.SetDisplayedFiles(chosenExtensions);
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
            pageStatus.IsError = false;
            int filterCount = fileViewSource.View.Cast<FileItem>().Count();
            pageStatus.StatusMessage = string.Format("{0} Files", filterCount);
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
        // click event from the folder browse button
        //
        private void FolderClick(object sender, RoutedEventArgs e)
        {
            OpenFolderBrowseDialogue();
        }
        //
        // de-select the file list to avoid complications in collection view filters
        //
        private void fileListLostFocus(object sender, RoutedEventArgs e)
        {
            fileList.SelectedItem = null;
        }
        //
        // The include sub-folders checkbox has been clicked so re-run the folder read
        //
        private void SubFoldersClick(object sender, RoutedEventArgs e)
        {
            ReadDirectoryForFiles(folderText.Text);
        }
        // #####################################################################################



        // #####################################################################################
        // NAVIGATION
        //
        // Open a data file.
        //
        private void ViewFile(object sender, MouseButtonEventArgs e)
        {
            pageStatus.FieldSeperator = null;
            pageStatus.HasHeaders = false;
            pageStatus.IsQuoted = false;

            FileItem chosenFile = (FileItem)fileList.SelectedItem;
            // Do we have a data file type
            pageStatus.ChosenFile = chosenFile.FilePath;
            pageStatus.Extension = chosenFile.FileExtension;
            //
            using (new WaitCursor())
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                DataViewer viewer = new DataViewer(pageStatus);
                ns.Navigate(viewer);
            }
        }
        // #####################################################################################

    }
}
