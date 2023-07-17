using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataReader.Models;

namespace DataReader.Common
{
    internal class ColumnSplit
    {
        private DataRow workingSet;
        private List<string> outCols;

        /// <summary>
        /// Convert the data row from a single line to a set of columns, split by the given seperator.
        /// This method is designed to be called multiple times for many rows given only one instantiation of 
        /// the class.
        /// </summary>
        /// <param name="inRow">Tuple of characters and character types</param>
        /// <param name="seperator">The string used to seperate a line into columns</param>
        /// <param name="quoted">Are the columns in the given line double quoted</param>
        /// <returns></returns>
        internal List<string> ConvertSeperatedLineToColumns(DataRow inRow, string seperator, bool quoted)
        {
            if (inRow == null)
                throw new Exception("Cannot convert data line to columns as DataRow is null");
            if (inRow.Characters.Count() == 0)
                throw new Exception("Cannot convert data line to columns as there are no characters");

            workingSet = inRow;
            outCols = new List<string>();

            if (quoted)
                ConvertQuotedLine(seperator);
            else
            {
                // there are no quotes to be concerned with so just string split the entire text
                var allChar = workingSet.Characters.Select(c => c.Item1).ToArray();
                string lineString = string.Concat(allChar);

                outCols = lineString.Split(seperator.ToCharArray(), StringSplitOptions.None).ToList();
            }
            
            return outCols;
        }

        /// <summary>
        /// A quoted line is always assumed to be double quotes such as CSV format
        /// Consider quotes to be mandatory in this logic
        /// </summary>
        /// <param name="seperator">The string used to seperate a line into columns</param>
        private void ConvertQuotedLine(string seperator)
        {
            StringBuilder aCol = new StringBuilder();
            bool withinCol = false;
            int quoteDistance = 0;

            foreach (Tuple<char, CharType> t in workingSet.Characters)
            {
                switch (t.Item2)
                {
                    case CharType.DoubleQuote:
                        if (withinCol)
                        {
                            // encountered closing quote so save stringbuilder as column
                            if (quoteDistance == 0)
                            {
                                // however, if a close quote is directly followed by an open quote 
                                // then we have an empty column.
                                outCols.Add(string.Empty);
                            }
                            else
                            {
                                outCols.Add(aCol.ToString());
                                aCol = new StringBuilder();
                            }
                            withinCol = false;
                            quoteDistance = 0;
                        }
                        else
                        {
                            withinCol = true;
                            quoteDistance = 0;
                        }
                        break;
                    default:
                        // any other character except a double quote
                        if (withinCol) 
                        {
                            // if inside a quoted section add to the string builder
                            aCol.Append(t.Item1);
                            quoteDistance++;
                        }
                        else
                        {
                            // anything that is not within quotes is the seperator so ignore 
                            quoteDistance++;
                        }
                        break;
                }
            }
            // if the final text segment has no close quote then save it as a column anyway.
            if (aCol.Length > 0)
                outCols.Add(aCol.ToString());
        }


    }
}
