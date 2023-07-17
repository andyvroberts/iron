using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Obdurate.database;

namespace Obdurate.viewmodels
{
  internal class BusinessUnitHierarchy
  {
    private ObservableCollection<HierarchyLevel> fullHierarchy = new ObservableCollection<HierarchyLevel>();
    private ObservableCollection<HierarchyLevel> subs = new ObservableCollection<HierarchyLevel>();

    public ObservableCollection<HierarchyLevel> AllLevels
    {
      get { return fullHierarchy; }
    }

    // constructor
    public BusinessUnitHierarchy()
    {

    }


    // ########################################################################
    //
    // From the incoming data we need to construct non-repeating hierarchical
    // relationships.  i.e. if the data contains 50 rows with only 2 level 1 keys
    // there should only be 2 level 1 class entries (not 50 as exists in the data).
    //
    // work backwards so no need to compare against multiple-nested relationships.
    //
    //private void BuildLevels()
    //{
    //  // Level 7to8: Unique Level7 and each distinct child BusinessUnitOld. 1398
    //  var level1 = from row in bus
    //               group new { row.Level7, row.Businessunit } by new { row.Level7, row.Businessunit } into l7
    //               select new Relationship() { Parent = l7.Key.Level7, Child = l7.Key.Businessunit };
    //  // 
    //  MapRelationshipToHierarchy(level1);
    //}
    //
    //
    //
    //private void MapRelationshipToHierarchy(IEnumerable<Relationship> relations)
    //{
    //  int counter = relations.Count();

    //  foreach (var rel in relations)
    //  {
    //    // does the parent already exist as a child?
    //    string parent = rel.Parent;
    //  }
    //}
    //
    //
    //
    public void ConvertToHierarchy(BusinessUnitSet inSet)
    {
      foreach (BusinessUnitTuple row in inSet)
      {
        HierarchyLevel h8 = new HierarchyLevel() { Key = 8, Name = row.Businessunit };
        HierarchyLevel h7 = new HierarchyLevel() { Key = 7, Name = row.Level7 };
        subs = new ObservableCollection<HierarchyLevel>();
        subs.Add(h8);
        h7.SubLevel = subs;
        HierarchyLevel h6 = new HierarchyLevel() { Key = 6, Name = row.Level6 };
        subs = new ObservableCollection<HierarchyLevel>();
        subs.Add(h7);
        h6.SubLevel = subs;
        HierarchyLevel h5 = new HierarchyLevel() { Key = 5, Name = row.Department };
        subs = new ObservableCollection<HierarchyLevel>();
        subs.Add(h6);
        h5.SubLevel = subs;
        HierarchyLevel h4 = new HierarchyLevel() { Key = 4, Name = row.Satellite };
        subs = new ObservableCollection<HierarchyLevel>();
        subs.Add(h5);
        h4.SubLevel = subs;
        HierarchyLevel h3 = new HierarchyLevel() { Key = 3, Name = row.Branch };
        subs = new ObservableCollection<HierarchyLevel>();
        subs.Add(h4);
        h3.SubLevel = subs;
        HierarchyLevel h2 = new HierarchyLevel() { Key = 2, Name = row.Region };
        subs = new ObservableCollection<HierarchyLevel>();
        subs.Add(h3);
        h2.SubLevel = subs;
        HierarchyLevel h1 = new HierarchyLevel() { Key = 1, Name = row.Division };
        subs = new ObservableCollection<HierarchyLevel>();
        subs.Add(h2);
        h1.SubLevel = subs;
        //
        fullHierarchy.Add(h1);
      }
    }
    //
    // ########################################################################
  }


  // ########################################################################
  //
  // container classes for repeating hierarchy.  Contained in this code file for 
  // ease of understanding logic.
  //
  public class HierarchyLevel
  {
    public int Key { get; set; }
    public string Name { get; set; }

    public ObservableCollection<HierarchyLevel> SubLevel { get; set; }
  }
  //public class Relationship
  //{
  //  public string Parent { get; set; }
  //  public string Child { get; set; }
  //}


}
