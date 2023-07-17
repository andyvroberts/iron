using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Services
{
    public interface IDataFileReader
    {
        int MaxColumnCount
        {
            get;
        }

        int RecordCount
        {
            get;
        }

        // retrieve some content of a data file.
        Queue<List<string>> GetSample(string filePath, 
                                      int sampleRecordCount, 
                                      string columnSeperator, 
                                      bool quoted);

        // retrieve search results
        Queue<List<string>> GetSearched(string filePath, 
                                        int sampleRecordCount, 
                                        string columnSeperator, 
                                        string searchTerm,
                                        bool quoted, 
                                        bool headers);
    }
}
