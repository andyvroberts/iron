using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataReader.Common;
using DataReader.Models;
using System.IO;

namespace DataReader.FlatData
{
    internal class ReadController : IDisposable
    {
        private ColumnSplit splitter = new ColumnSplit();
        private DataFile df = new DataFile();
        private string dataFile;
        private string searchText;
        private bool searched = false;
        private bool headerRow = false;

        private int QueueCounter;
        internal int RowCounter;
        internal int MaxCols;

        internal Queue<List<string>> FileRecords
        {
            get { return df.SampleSet; }
        }


        /// <summary>
        /// Constructor performs required initializations and mandatory guard clauses.
        /// </summary>
        /// <param name="filePath">the requested file path</param>
        /// <param name="sampleRecordCount">the number of sample records required</param>
        internal ReadController(string filePath, int sampleRecordCount)
        {
            FileUtils utils = new FileUtils();

            if (sampleRecordCount == 0)
                throw new Exception("A sample record count of zero is invalid");
            if (!utils.DoesFileExist(filePath))
                throw new Exception("The specified file does not exist");
            if (utils.IsFileEncrypted(filePath))
                throw new Exception("Selected file is encrypted");

            dataFile = filePath;
            df.SampleSize = sampleRecordCount;
        }


        /// <summary>
        /// Return the sample number of requested records from the file.
        /// </summary>
        /// <param name="columnSeperator">the string or character that splits the file columns</param>
        /// <param name="quoted">indicates if the columns may be double quoted</param>
        internal void ReadSampleData(string columnSeperator, bool quoted)
        {
            DecodeFile(columnSeperator, quoted);
        }


        /// <summary>
        /// Return the sample number of requested records that match the required search term from the file.
        /// </summary>
        /// <param name="columnSeperator">the string or character that splits the file columns</param>
        /// <param name="searchTerm">the required string to search for</param>
        /// <param name="quoted">indicates if the columns may be double quoted</param>
        internal void SearchSampleData(string columnSeperator, string searchTerm, bool quoted, bool headers)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                searched = false;
            else
            {
                searchText = searchTerm;
                searched = true;
            }
            headerRow = headers;

            DecodeFile(columnSeperator, quoted);
        }


        /// <summary>
        /// If the file is a flat data structure this method controls the data records decode process.
        /// </summary>
        /// <param name="columnSeperator"></param>
        /// <param name="quoted"></param>
        internal void DecodeFile(string columnSeperator, bool quoted)
        {
            RowCounter = 0;
            QueueCounter = 0;
            bool openQuote = false;
            DataRow dr = new DataRow();

            if (string.IsNullOrWhiteSpace(columnSeperator))
                df.IsSeperated = false;
            else
            {
                df.IsSeperated = true;
                df.SeperatorValue = columnSeperator;
            }
            df.IsQuoted = quoted;

            using (StreamReader sr = new StreamReader(dataFile))
            {
                while (sr.Peek() >= 0)
                {
                    char c1 = (char)sr.Read();
                    CharType ctd = CharacterTypeDecode.DetermineChar(c1);

                    switch (ctd)
                    {
                        case CharType.DoubleQuote:
                            {
                                if (df.IsQuoted)
                                {
                                    openQuote = (openQuote) ? false : true;
                                }
                                dr.Characters.Add(Tuple.Create(c1, ctd));
                            }
                            break;
                        case CharType.CR:
                            {
                                if (openQuote)
                                {
                                    // within a quote delimited column.
                                    dr.Characters.Add(Tuple.Create(c1, ctd));
                                }
                                else
                                {
                                    // remove spurious CR and ensure we only break on Newline
                                    if (sr.Peek() != '\n')
                                    {
                                        // next char is not EOL so the CR is a part of the data
                                        dr.Characters.Add(Tuple.Create(c1, ctd));
                                    }
                                }
                            }
                            break;
                        case CharType.Newline:
                            {
                                if (openQuote)
                                {
                                    // within a quote delimited column.
                                    dr.Characters.Add(Tuple.Create(c1, ctd));
                                }
                                else
                                {
                                    if (QueueCounter <= df.SampleSize)
                                    {
                                        QueueCounter += DataRowToColumns(dr);
                                    }
                                    RowCounter++;
                                    dr = new DataRow();
                                }
                            }
                            break;
                        default:
                            dr.Characters.Add(Tuple.Create(c1, ctd));
                            break;
                    }
                } // end While Loop
            } // end stream reader
        }

        /// <summary>
        /// convert the data line into seperated data columns.  If searching for a value then only keep records with a match.
        /// </summary>
        /// <param name="line"></param>
        private int DataRowToColumns(DataRow line)
        {
            List<string> row;

            if (df.IsSeperated)
            {
                row = splitter.ConvertSeperatedLineToColumns(line, df.SeperatorValue, df.IsQuoted);
                MaxCols = Math.Max(MaxCols, row.Count());
            }
            else
            {
                var charArray = line.Characters.Select(c => c.Item1).ToArray();
                string noCols = string.Concat(charArray);
                row = new List<string> { noCols };
                MaxCols = Math.Max(MaxCols, 1);
            }
            // if there is a header then always keep the first record.
            if (headerRow)
            {
                df.SampleSet.Enqueue(row);
                headerRow = false;
                return 0;
            }
            else if (searched)
            {
                foreach (string s1 in row)
                {
                    if (s1.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        df.SampleSet.Enqueue(row);
                        return 1;
                    }
                }
                // if we reach this far either the column array was empty or the search term was not found.
                return 0;
            }
            else
            {
                df.SampleSet.Enqueue(row);
                return 1;
            }
        }




        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    df = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ReadController() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

