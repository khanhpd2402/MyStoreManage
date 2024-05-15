using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyStoreManage.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider serviceProvider { get; set; }
        public IConfiguration configuration { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollecction = new ServiceCollection();
            serviceCollecction.AddTransient<WindowMyProfile>();
            serviceCollecction.AddScoped<MyStoreContext>();
            serviceProvider = serviceCollecction.BuildServiceProvider();
            serviceProvider.GetRequiredService<WindowMyProfile>().Show();
        }
    }

}
