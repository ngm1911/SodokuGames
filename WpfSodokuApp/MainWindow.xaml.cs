using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfSodokuApp.ViewModel;

namespace WpfSodokuApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = App.AppHost.Services.GetRequiredService<MainWindowViewModel>();
        }
    }
}