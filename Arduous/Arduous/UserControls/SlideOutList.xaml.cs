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

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Arduous.UserControls
{
  /// <summary>
  /// Interaction logic for SlideOutList.xaml
  /// </summary>
  public partial class SlideOutList : UserControl
  {
    public SlideOutList()
    {
      InitializeComponent();
    }
    //
    // the open menu panel button actions
    //
    private void OpenMenuPanel(object sender, RoutedEventArgs e)
    {
      slidePanel.Focus();
      openMenuButton.Visibility = Visibility.Collapsed;
      closeMenuButton.Visibility = Visibility.Visible;
    }
    //
    // the close menu panel button actions

    private void CloseMenuPanel(object sender, RoutedEventArgs e)
    {
      closeMenuButton.Visibility = Visibility.Collapsed;
      openMenuButton.Visibility = Visibility.Visible;
    }
    //
    // go back one page.
    //
    private void BackButton(object sender, RoutedEventArgs e)
    {
      NavigationService ns = NavigationService.GetNavigationService(this);
      if (ns.CanGoBack)
        ns.GoBack();
    }
    //
    // menu naviation for function choices
    //
    private void filesMenuButton(object sender, RoutedEventArgs e)
    {
      NavigationService ns = NavigationService.GetNavigationService(this);
      //FileExplore fe = new FileExplore();
      //ns.Navigate(fe);
    }
    private void BusinessUnitButton(object sender, RoutedEventArgs e)
    {
      NavigationService ns = NavigationService.GetNavigationService(this);
      //BusinessUnit bu = new BusinessUnit();
      //ns.Navigate(bu);
    }



    // #####################################################################################
    // ROUTED EVENT HANDLING FOR GO
    //
    // register the routed event and its properties
    //
    public static readonly RoutedEvent GoEvent =
      EventManager.RegisterRoutedEvent("ClickGo", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SlideOutList));
    //
    // declare the CLR accessors for the routed event SlideOutList.ClickGo
    //
    public event RoutedEventHandler ClickGo
    {
      add { AddHandler(GoEvent, value); }
      remove { RemoveHandler(GoEvent, value); }
    }
    //
    // capture the click on the viewbox (the GO button).
    //
    private void Viewbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      // first close the slide out menu
      ButtonAutomationPeer bap = new ButtonAutomationPeer(closeMenuButton);
      IInvokeProvider invoker = bap.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
      invoker.Invoke();
      //raise the routed event to bubble up the element tree.
      RaiseEvent(new RoutedEventArgs(GoEvent, this));
    }
    // #####################################################################################

  }
}
