using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Obdurate.viewmodels
{
  public class FileToDataTable
  {
    private DataTable fileDT;
    private string filePath;
    private string columnSeperator;
    private List<string> columnHeaders;

    // logic decision booleans with global scope
    private bool noHeaders = false;
    private bool isQuoted = false;
    private bool isOneColumn = false;

    // public properties
    public DataTable FileDT
    {
      get { return fileDT; }
    }

    // constructor
    public FileToDataTable(string path)
    {
      filePath = path;
    }


    // #####################################################################################
    // PUBLICLY ACCESSIBLE CONTROLING LOGIC TO DECODE FILE CONTENTS
    //
    public void FormatContents(string seperator, bool hasNoHeaders, bool quoted)
    {
      ResetDataTable();
      isQuoted = quoted;
      noHeaders = hasNoHeaders;

      if (seperator == string.Empty)
        isOneColumn = true;
      else
      {
        columnSeperator = seperator;
        isOneColumn = false;
      }

      FileToTable();
    }
    //
    // #####################################################################################



    // #####################################################################################
    // READ THE DATA FILE.  CONVERT CONTENTS TO DATA TABLE
    //
    private void FileToTable()
    {
      string dataLine;
      bool firstRow = true;
      columnHeaders = new List<string>();

      using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        fileDT.BeginLoadData();
        using (StreamReader sr = new StreamReader(fs))
        {
          while ((dataLine = sr.ReadLine()) != null)
          {
            List<string> columnData = new List<string>();
            // the first row or record may have column headers.
            if (firstRow)
            {
              // if the file has only one column
              if (isOneColumn)
              {
                if (noHeaders)
                {
                  // the first row is a data row.  Add fake header row then the data row.
                  columnHeaders.Add("File Line");
                  AddHeaderToTable();
                  columnData.Add(dataLine);
                  AddRowToTable(columnData.ToArray());
                }
                else
                {
                  // the first row is an actual header.
                  columnHeaders.Add(dataLine);
                  AddHeaderToTable();
                }
              }
              // if the file has multiple columns.
              else
              {
                if (isQuoted)
                  columnData = DecodeQuotedColumns(dataLine);
                else
                  columnData = DecodeColumns(dataLine);

                if (noHeaders)
                {
                  // the first row is a data row.  Add fake headers and then data row.
                  columnHeaders = CreateColumnHeaderNames(columnData);
                  AddHeaderToTable();
                  AddRowToTable(columnData.ToArray());
                }
                else
                {
                  // the first row is an actual header.
                  columnHeaders = DeduplicateColumnHeaderNames(columnData);
                  AddHeaderToTable();
                }
              }
              firstRow = false;
            }
            // not the first row so assume everything is a data row.
            else
            {
              if (isOneColumn)
              {
                columnData.Add(dataLine);
                AddRowToTable(columnData.ToArray());
              }
              else
              {
                if (isQuoted)
                  columnData = DecodeQuotedColumns(dataLine);
                else
                  columnData = DecodeColumns(dataLine);

                AddRowToTable(columnData.ToArray());
              }
            }
          } // end while loop
        } // end stream reader
        fileDT.EndLoadData();
      } // end file stream
    }
    //
    // Decode a CSV row which may be comma, other character or string seperated.
    // Each column may or may not be quoted.
    //
    private List<string> DecodeQuotedColumns(string dataLine)
    {
      List<string> colSet = new List<string>();
      bool colQuoted = false;
      StringBuilder entry = new StringBuilder();
      char quote = '"';
      int dataLen = dataLine.Length;
      int finalCol = dataLen - 1;
      int sepLen = columnSeperator.Length;
      int currPos = 0;

      while (currPos < dataLen)
      {
        if (currPos == finalCol)
        {
          colSet.Add(entry.ToString());
          currPos++;
        }
        else if (dataLine[currPos] == quote)
        {
          if (colQuoted)
            colQuoted = false;
          else
            colQuoted = true;

          currPos++;
        }
        else if (dataLine.Substring(currPos, sepLen) == columnSeperator)
        {
          // if this is a seperator then save the column value and continue
          if (colQuoted)
          {
            // if the seperator is within the quoted string to save it within column
            entry.Append(dataLine.ElementAt(currPos));
            currPos++;
          }
          else
          {
            colSet.Add(entry.ToString());
            entry = new StringBuilder();
            currPos += sepLen;
          }
        }
        else
        {
          entry.Append(dataLine.ElementAt(currPos));
          currPos++;
        }
      } // end while loop
      return colSet;
    }      
    //
    // Decode row by the seperator value.
    //
    private List<string> DecodeColumns(string dataLine)
    {
      return dataLine.Split(columnSeperator.ToCharArray(), StringSplitOptions.None).ToList();
    }
    //
    // #####################################################################################




    // #####################################################################################
    // DATA TABLE MAINTENANCE METHODS
    //
    // Add any number of column headers to the data table.
    //
    private void AddHeaderToTable()
    {
      foreach(string header in columnHeaders)
      {
        fileDT.Columns.Add(header, typeof(string));
      }
    }
    //
    // Add a data row to the data table.
    //
    private void AddRowToTable(string[] colData)
    {
      DataRow row = fileDT.NewRow();
      int dataIndex = 0;
      int maxCol = colData.Count();

      foreach (string header in columnHeaders)
      {
        if (dataIndex < maxCol)
        {
          row[header] = colData[dataIndex];
          dataIndex++;
        }
      }
      fileDT.Rows.Add(row);
    }
    //
    // #####################################################################################


    // #####################################################################################
    // UTILITY FUNCTIONS
    //
    // Clear the old data table by creating a new one.
    //
    private void ResetDataTable()
    {
      if (fileDT != null)
      {
        fileDT.Columns.Clear();
        fileDT.Rows.Clear();
        fileDT.Dispose();
      }
      fileDT = new DataTable();
    }
    //
    // When there are no headers, generate a unique number for each column - in sequence.
    //
    private List<string> CreateColumnHeaderNames(List<string> headers)
    {
      List<string> newHeaders = new List<string>();
      int colCount = 1;
      foreach(string col in headers)
      {
        newHeaders.Add(colCount.ToString());
        colCount++;
      }
      return newHeaders;
    }
    //
    // A data table cannot have duplicate column headers so check the heading names
    // from the header row and if any duplicates are found prefix each heading name with
    // a sequential numeric to ensure uniqueness.
    //
    private List<string> DeduplicateColumnHeaderNames(List<string> headers)
    {
      int maxGroupByCount = (from string col in headers
                             group new { col } by col into colGroup
                             orderby colGroup.Count() descending
                             select colGroup.Count()
                             ).FirstOrDefault();

      if (maxGroupByCount > 1)
      {
        // the group by of column names returned a value greater than 1 (not unique)
        List<string> modifyHeaders = new List<string>();
        int colCount = 1;
        foreach(string col in headers)
        {
          modifyHeaders.Add(string.Format("{0}-{1}", colCount, col));
          colCount++;
        }
        return modifyHeaders;
      }
      else
        return headers;
    }
    //
    // #####################################################################################


  }
}
