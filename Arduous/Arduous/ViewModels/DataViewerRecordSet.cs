using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arduous.Models;
using DataReader.Services;

namespace Arduous.ViewModels
{
    internal class DataViewerRecordSet
    {
        private Queue<List<string>> displaySet;
        public Queue<List<string>> DisplaySet
        {
            get { return displaySet; }
        }

        private int columnCount;
        public int ColumnCount
        {
            get { return columnCount; }
        }

        private int recordCount;
        public int RecordCount
        {
            get { return recordCount; }
        }


        /// <summary>
        /// Constructor to initialise the record set.
        /// </summary>
        public DataViewerRecordSet()
        {
            displaySet = new Queue<List<string>>();
        }

        /// <summary>
        /// Retrieve a sample number of records from a given data file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recs"></param>
        /// <param name="seperator"></param>
        /// <param name="quoted"></param>
        /// <param name="headers"></param>
        public void GetRecordSet(string extension,
                                 string path, 
                                 int recs, 
                                 string seperator, 
                                 bool quoted, 
                                 bool headers)
        {
            // Use the reader factory to return the correct read implementation
            IDataFileReader repo = ReaderFactory.GetRepository(extension);

            displaySet = repo.GetSample(path, recs, seperator, quoted);
            columnCount = repo.MaxColumnCount;
            recordCount = repo.RecordCount;
        }

        /// <summary>
        /// Retrieve a sample number of records from a data file, given a search string.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recs"></param>
        /// <param name="seperator"></param>
        /// <param name="quoted"></param>
        /// <param name="headers"></param>
        public void SearchRecordSet(string extension,
                                    string path,
                                    int recs,
                                    string seperator,
                                    string searchTerm,
                                    bool quoted,
                                    bool headers)
        {
            // Use the reader factory to return the correct read implementation
            IDataFileReader repo = ReaderFactory.GetRepository(extension);

            displaySet = repo.GetSearched(path, recs, seperator, searchTerm, quoted, headers);
            columnCount = repo.MaxColumnCount;
            recordCount = repo.RecordCount;
        }

        /// <summary>
        /// Return (pop) the next data row from the Queue.
        /// </summary>
        /// <returns>One of more column values</returns>
        public string[] GetRow()
        {
            if (displaySet.Count() > 0)
            {
                List<string> row = displaySet.Dequeue();
                return row.ToArray();
            }
            else
                throw new Exception("The data file reader decoded zero columns from the data file");
        }

    }
}
