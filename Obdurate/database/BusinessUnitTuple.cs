using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Obdurate.database
{
  internal class BusinessUnitTuple: INotifyPropertyChanged, IDataErrorInfo
  {
    private string division;
    private string region;
    private string branch;
    private string satellite;
    private string department;
    private string level6;
    private string level7;
    private string businessunit;

    public string Division
    {
      get { return division; }
      set { division = value; OnPropChanged("Division"); }
    }
    public string Region
    {
      get { return region; }
      set { region = value; OnPropChanged("Region"); }
    }
    public string Branch
    {
      get { return branch; }
      set { branch = value; OnPropChanged("Branch"); }
    }
    public string Satellite
    {
      get { return satellite; }
      set { satellite = value; OnPropChanged("Satellite"); }
    }
    public string Department
    {
      get { return department; }
      set { department = value; OnPropChanged("Department"); }
    }
    public string Level6
    {
      get { return level6; }
      set { level6 = value; OnPropChanged("Level6"); }
    }
    public string Level7
    {
      get { return level7; }
      set { level7 = value; OnPropChanged("Level7"); }
    }
    public string Businessunit
    {
      get { return businessunit; }
      set { businessunit = value; OnPropChanged("Businessunit"); }
    }



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



    // ########################################################################
    //
    // create mandatory data error Info methods.
    //
    public string Error
    {
      get { throw new NotImplementedException(); }
    }

    public string this[string attributeName]
    {
      get
      {
        string retErr = null;
        switch (attributeName)
        {
          case "Division":
            if (division == null || division.Length == 0)
              retErr = "Division must contain a value";
            break;
          case "Region":
            if (region == null || region.Length == 0)
              retErr = "Region must contain a value";
            break;
          case "Branch":
            if (branch == null || branch.Length == 0)
              retErr = "Branch must contain a value";
            break;
          case "Satellite":
            if (satellite == null || satellite.Length == 0)
              retErr = "Satellite must contain a value";
            break;
          case "Department":
            if (department == null || department.Length == 0)
              retErr = "Department must contain a value";
            break;
          case "Businessunit":
            if (businessunit == null || businessunit.Length == 0)
              retErr = "Business Unit must contain a value";
            break;
        }
        return retErr;
      }
    }
    //
    // ########################################################################


  }
}
