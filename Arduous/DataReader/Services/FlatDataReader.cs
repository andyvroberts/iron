using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataReader.FlatData;

namespace DataReader.Services
{
    public class FlatDataReader: IDataFileReader
    {
        //
        // This is the default file reader that will be chosen by the reader factory.
        // It expects to decode textual flat files that may be seperated into columns.
        //

        private int maxColumnCount = 1;
        public int MaxColumnCount
        {
            get { return maxColumnCount; }
        }

        private int recordCount;
        public int RecordCount
        {
            get { return recordCount; }
        }


        /// <summary>
        /// For a given file, return the requested sample number of records.
        /// </summary>
        /// <param name="filePath">the file location</param>
        /// <param name="sampleRecordCount">the number of requested reocords</param>
        /// <param name="columnSeperator">the string value that separates the data columns</param>
        /// <param name="quoted">true if the columns are optionally quoted</param>
        /// <returns></returns>
        public Queue<List<string>> GetSample(string filePath, 
                                             int sampleRecordCount,
                                             string columnSeperator, 
                                             bool quoted)
        {
            ReadController flatController = new ReadController(filePath, sampleRecordCount);
            flatController.ReadSampleData(columnSeperator, quoted);
            
            maxColumnCount = flatController.MaxCols;
            recordCount = flatController.RowCounter;

            return flatController.FileRecords;
        }

        /// <summary>
        /// For a given file, perform a search and return no more than the requested sample number
        /// of records.
        /// </summary>
        /// <param name="filePath">the file location</param>
        /// <param name="sampleRecordCount">the number of requested records</param>
        /// <param name="columnSeperator">the string value that separates the data columns</param>
        /// <param name="searchTerm">the search value</param>
        /// <param name="quoted">true if the columns are optionally quoted</param>
        /// <param name="headers">true if the file has a header record</param>
        /// <returns></returns>
        public Queue<List<string>> GetSearched(string filePath, 
                                               int sampleRecordCount,
                                               string columnSeperator, 
                                               string searchTerm,
                                               bool quoted,
                                               bool headers)
        {
            ReadController flatController = new ReadController(filePath, sampleRecordCount);
            flatController.SearchSampleData(columnSeperator, searchTerm, quoted, headers);

            maxColumnCount = flatController.MaxCols;
            recordCount = flatController.RowCounter;

            return flatController.FileRecords;
        }



    }
}
