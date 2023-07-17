using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace Arduous.Models
{
  internal class DataTuple<T>: INotifyPropertyChanged
  {
    private T[] dataBuffer;
    private bool ofInterest = false;

    // constructor defines data tuple size (and data type by creating statement).
    public DataTuple(int capacity)
    {
      dataBuffer = new T[capacity];
    }

    // fields
    public T[] DataBuffer
    {
      get { return dataBuffer; }
    }
    public bool OfInterest
    {
      get { return ofInterest; }
      set { ofInterest = value; OnPropChanged("OfInterest"); }
    }

    // methods
    public void AddToThisTuple(T iVal, int arrayX)
    {
      dataBuffer[arrayX] = iVal;
    }
    //
    public void SearchThisTuple(T sVal)
    {
      int foundIndex = -1;
      foundIndex = Array.IndexOf<T>(dataBuffer, sVal);

      if (foundIndex >= 0)
        ofInterest = true;
      else
        ofInterest = false;
    }


    //
    // create the required event and handler to notify of underlying data changes.
    //
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropChanged(string name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
