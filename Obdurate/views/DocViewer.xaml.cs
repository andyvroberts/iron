using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Markup;

namespace Obdurate.views
{
  /// <summary>
  /// Interaction logic for DocViewer.xaml
  /// </summary>
  public partial class DocViewer : Page
  {
    public DocViewer(string filePath, string extType)
    {
      InitializeComponent();
      LoadFileToFlowReader(filePath);
    }

    //
    // create a flow document.  Open the file.  Load into viewer.
    //
    private void LoadFileToFlowReader(string aPath)
    {
      using (FileStream fs = File.OpenRead(aPath))
      {
        TextRange tr = new TextRange(fDoc.ContentStart, fDoc.ContentEnd);
        tr.Load(fs, DataFormats.Text);
        docView.Document = fDoc;

      }
    }
    //
    // determine the data format based on the extension type
    //
    private void DetermineDataFormat(string ext)
    { }


  }
}
