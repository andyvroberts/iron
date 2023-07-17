using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.SqlClient;

namespace Obdurate.database
{
  public class ConnectionDetails: INotifyPropertyChanged
  {
    private SqlConnectionStringBuilder conBuilder = new SqlConnectionStringBuilder();
    private string schema;

    // constructor
    public ConnectionDetails()
    {
      conBuilder.DataSource = "DESKTOP-DUBDIPS\\SQLEXPRESS";
      conBuilder.IntegratedSecurity = true;
      conBuilder.InitialCatalog = "Slam";
      schema = "ods";
    }

    public string DatabaseServer
    {
      get { return conBuilder.DataSource; }
      set { conBuilder.DataSource = value; OnPropChanged("DatabaseServer"); }
    }
    public string DatabaseCatalog
    {
      get { return conBuilder.InitialCatalog; }
      set { conBuilder.InitialCatalog = value; OnPropChanged("DatabaseCatalog"); }
    }
    public string DatabaseSchema
    {
      get { return schema; }
      set { schema = value; OnPropChanged("DatabaseSchema"); }
    }
    public bool DatabaseTrusted
    {
      get { return conBuilder.IntegratedSecurity; }
      set { conBuilder.IntegratedSecurity = value; OnPropChanged("DatabaseTrusted"); }
    }
    public SqlConnectionStringBuilder Builder
    {
      get { return conBuilder; }
    }


    // ########################################################################
    //
    // Connection string utility methods
    //
    private static string GetConnectionString()
    {
      // old style.  can place in configuration file if required
      // conBuilder = new SqlConnectionStringBuilder(GetConnectionString());
      //
      return "Server=(local);Integrated Security=SSPI;Initial Catalog=Slam;";
    }
    //
    // ########################################################################


    // ########################################################################
    //
    // create mandatory event and event handler for Notify property changed
    //
    public event PropertyChangedEventHandler PropertyChanged;
    //
    protected void OnPropChanged(string name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
    //
    // ########################################################################

  }

}
