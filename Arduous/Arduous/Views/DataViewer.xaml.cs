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

using Arduous.ViewModels;
using Arduous.UserControls;

namespace Arduous.Views
{
    /// <summary>
    /// Interaction logic for DataViewer.xaml
    /// </summary>
    public partial class DataViewer : Page
    {
        private Status pageStatus;
        private DataViewerRecordSet pageModel = new DataViewerRecordSet();

        private string _previousMessage;
        private bool _previousStatus;

        public DataViewer(Status chosenStatus)
        {
            InitializeComponent();
            this.WindowTitle = chosenStatus.ChosenFile;
            // add the specific (routed) events handles used in this page.
            slideOut.AddHandler(SlideOutList.GoEvent, new RoutedEventHandler(SlideOutEventToReReadData));

            pageStatus = chosenStatus;
            viewerStatusBar.DataContext = pageStatus;
            slideOut.DataContext = pageStatus;
            _previousMessage = pageStatus.StatusMessage;
            _previousStatus = pageStatus.IsError;

            ReadFile();
        }


        // #####################################################################################
        // PERFORM METHODS TO READ THE FILE CONTENT
        //
        // Use the view-model to interract with file reader logic.
        //
        private void ReadFile()
        {
            try
            {
                using (new WaitCursor())
                {
                    pageModel.GetRecordSet(pageStatus.Extension,
                                           pageStatus.ChosenFile,
                                           pageStatus.DisplayRows,
                                           pageStatus.FieldSeperator,
                                           pageStatus.IsQuoted,
                                           pageStatus.HasHeaders);

                    BuildDataGridColumns();
                }

                pageStatus.IsError = false;
                pageStatus.StatusMessage = string.Format("Showing {0:N0} of {1:N0} records.", 
                    pageModel.DisplaySet.Count(), pageModel.RecordCount);
            }
            catch (Exception e)
            {
                pageStatus.IsError = true;
                pageStatus.StatusMessage = e.Message;
            }
        }

        private void SearchFile()
        {
            try
            {
                using (new WaitCursor())
                {
                    pageModel.SearchRecordSet(pageStatus.Extension,
                                              pageStatus.ChosenFile,
                                              pageStatus.DisplayRows,
                                              pageStatus.FieldSeperator,
                                              dispRows.Text,
                                              pageStatus.IsQuoted,
                                              pageStatus.HasHeaders);

                    BuildDataGridColumns();
                }

                pageStatus.IsError = false;
                pageStatus.StatusMessage = string.Format("Showing {0:N0} of {1:N0} records.",
                    pageModel.DisplaySet.Count(), pageModel.RecordCount);
            }
            catch (Exception e)
            {
                pageStatus.IsError = true;
                pageStatus.StatusMessage = e.Message;
            }
        }

        //
        // #####################################################################################



        // #####################################################################################
        // CONSTRUCT THE LIST VIEW ITEMS TEMPLATE
        //
        private void BuildDataGridColumns()
        {
            string[] headerNames = new string[pageModel.ColumnCount];
            recordList.Columns.Clear();

            try
            {
                // set the header row.
                if (pageStatus.HasHeaders)
                {
                    // pop first record from queue.
                    string[] firstRow = pageModel.GetRow();
                    firstRow.CopyTo(headerNames, 0);
                }
                else
                {
                    for (int i = 0; i < pageModel.ColumnCount; i++)
                    {
                        headerNames[i] = string.Format("Column {0}", i + 1);
                    }
                }

                // set the data grid column bindings to each array element that exists.
                for (int i = 0; i < pageModel.ColumnCount; i++)
                {
                    DataGridTextColumn col = new DataGridTextColumn();
                    col.Header = string.Format("{0}", headerNames[i]);
                    col.Binding = new Binding(string.Format("[{0}]", i));
                    recordList.Columns.Add(col);
                }

                recordList.ItemsSource = pageModel.DisplaySet;
            }
            catch (Exception e)
            {
                pageStatus.IsError = true;
                pageStatus.StatusMessage = e.Message;
            }
        }
        //
        // #####################################################################################



        // #####################################################################################
        // HANDLE ROUTED EVENTS FROM USER CONTROLS ON THIS PAGE
        //
        private void SlideOutEventToReReadData(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            using (new WaitCursor())
            {
                if (dispRows.Text.Length > 0)
                    SearchFile();
                else
                    ReadFile();
            }
        }
        //
        // #####################################################################################



        // #####################################################################################
        // UI CONTROL CLICK HANDLERS
        //
        // if the enter key is pressed in the search text box then execute another read.
        //
        private void SearchTextKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                using (new WaitCursor())
                {
                    SearchFile();
                }
            }
        }
        //
        // #####################################################################################



        // #####################################################################################
        // NAVIGATION METHODS
        //
        // when navigating back to file list, display the appropriate status message.
        //
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            pageStatus.StatusMessage = _previousMessage;
            pageStatus.IsError = _previousStatus;
        }
        //
        // #####################################################################################

    }
}
