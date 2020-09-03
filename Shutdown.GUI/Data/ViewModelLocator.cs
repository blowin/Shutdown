using Microsoft.Extensions.DependencyInjection;
using Shutdown.GUI.Util;
using Shutdown.GUI.ViewModel;

namespace Shutdown.GUI.Data
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();

        public static void Init()
        {
            var services = new ServiceCollection()
                .AddTransient<MainViewModel>()
                .AddSingleton<ReflectionExtractor>()
                .AddSingleton<UnitUtil>();

            _provider = services.BuildServiceProvider(true);
        }
    }
}