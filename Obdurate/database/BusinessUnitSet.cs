using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace Obdurate.database
{
  internal class BusinessUnitSet: ObservableCollection<BusinessUnitTuple>
  {
    private ConnectionDetails conDetails;

    public BusinessUnitSet(ConnectionDetails inConn)
    {
      conDetails = inConn;
    }

    // ########################################################################
    //
    // Database read to construct all the business unit tuples.
    //
    public void FetchBusinessUnits()
    {
      string SqlSelect = "SELECT division, region, branch, satellite, department, " +
        "level6, level7, businessunit_name";
      string SqlFrom = string.Format(" FROM {0}.businessunit", conDetails.DatabaseSchema);

      using (SqlConnection conn = new SqlConnection(conDetails.Builder.ConnectionString))
      {
        SqlCommand cmd = new SqlCommand(SqlSelect + SqlFrom, conn);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          ConvertRow((IDataRecord)reader);
        }
        reader.Close();
      }
    }
    //
    // Decode each reader row, map to an object tuple.
    //
    private void ConvertRow(IDataRecord row)
    {
      BusinessUnitTuple bu = new BusinessUnitTuple();
      bu.Division = row.GetString(0);
      bu.Region = row.GetString(1);
      bu.Branch = row.GetString(2);
      bu.Satellite = row.GetString(3);
      bu.Department = row.GetString(4);
      bu.Level6 = row.GetString(5);
      bu.Level7 = row.GetString(6);
      bu.Businessunit = row.GetString(7);

      this.Add(bu);
    }
    //
    // ########################################################################

  }
}
