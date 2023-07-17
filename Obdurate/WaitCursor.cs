using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Obdurate
{
  public class WaitCursor : IDisposable
  {
    private Cursor previousCursor;

    public WaitCursor()
    {
      previousCursor = Mouse.OverrideCursor;
      Mouse.OverrideCursor = Cursors.Wait;
    }

    public void Dispose()
    {
      Mouse.OverrideCursor = previousCursor;
    }

  }
}
