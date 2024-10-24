using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using WpfSodokuApp.ViewModel;

namespace WpfSodokuApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; } = default;

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((HostBuilderContext, services) =>
                {
                    services.AddSingleton<MainWindowViewModel>();
                })
                .Build();
        }   
    }
}
