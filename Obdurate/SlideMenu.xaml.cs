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
using Obdurate.views;

namespace Obdurate
{
  /// <summary>
  /// Interaction logic for SlideMenu.xaml
  /// </summary>
  public partial class SlideMenu : UserControl
  {
    public SlideMenu()
    {
      InitializeComponent();
    }
    //
    // the open menu panel button actions
    //
    private void OpenMenuPanel(object sender, RoutedEventArgs e)
    {
      menuPanel.Focus();
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
    // Use UI automation to remotely call the close menu button click event.
    //
    private void MenuPanelLostFocus(object sender, RoutedEventArgs e)
    {
      ButtonAutomationPeer bap = new ButtonAutomationPeer(closeMenuButton);
      IInvokeProvider invoker = bap.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
      invoker.Invoke();
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
      FileExplore fe = new FileExplore();
      ns.Navigate(fe);
    }
    private void BusinessUnitButton(object sender, RoutedEventArgs e)
    {
      NavigationService ns = NavigationService.GetNavigationService(this);
      BusinessUnit bu = new BusinessUnit();
      ns.Navigate(bu);
    }

  }
}
