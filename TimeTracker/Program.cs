using Keystroke.API;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTracker.Services;
using TimeTracker.Services.SignIn;
using TimeTracker.Services.Storage;
using TimeTracker.Services.Sync;
using TimeTracker.Services.Tracking;
using TimeTracker.Services.Tracking.Hooks;

namespace TimeTracker
{
    static class Program
    {
        #region Private

        static IServiceProvider _serviceProvider;

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BootstrapIoCContainer();

            Application.Run(_serviceProvider.GetService<Main>());
        }

        static void BootstrapIoCContainer()
        {
            //setup our DI
            _serviceProvider = new ServiceCollection()
                .AddLogging(x =>
                {
                    x.SetMinimumLevel(LogLevel.Debug);
                    x.AddNLog();
                })
                .AddTransient<Main>()
                .AddTransient<SignInService>()
                .AddTransient<ITaskRunner, TaskRunner>()
                .AddTransient<ITrackApplicationsService, TrackApplicationsService>()
                .AddTransient<ITrackKeystrokeService, TrackKeystrokeService>()
                .AddTransient<ITrackMouseClickService, TrackMouseClickService>()
                .AddSingleton<ISyncService, SyncService>()
                .AddSingleton<KeystrokeAPI>()
                .AddTransient<ITrackApiWrapper, ApiStubWrapper>()
                .BuildServiceProvider();
        }
    }
}
