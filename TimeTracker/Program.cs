using Keystroke.API;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Windows.Forms;
using TimeTracker.Services;
using TimeTracker.Services.SignIn;
using TimeTracker.Services.Storage;
using TimeTracker.Services.Sync;
using TimeTracker.Services.Tracking;
using TimeTracker.Services.Tracking.Applications;
using TimeTracker.Services.Tracking.Hooks;
using TimeTracker.Services.Tracking.System;

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
                .AddSingleton<ISignInService, SignInService>()
                .AddSingleton<ITaskRunner, TaskRunner>()
                .AddSingleton<ITrackInstalledApplicationsService, TrackInstalledApplicationsService>()
                .AddSingleton<ITrackOpenedApplicationsService, TrackOpenedApplicationsService>()
                .AddSingleton<ITrackKeystrokeService, TrackKeystrokeService>()
                .AddSingleton<ITrackMouseClickService, TrackMouseClickService>()
                .AddSingleton<ITrackDnsCacheService, TrackDnsCacheService>()
                .AddSingleton(x => (ITakeSnapshot<KeyboardClicksSnapshot>)x.GetService<ITrackKeystrokeService>())
                .AddSingleton(x => (ITakeSnapshot<MouseClicksSnapshot>)x.GetService<ITrackMouseClickService>())
                .AddSingleton<ITakeSnapshot<InstalledApplicationsSnapshot>>(x => x.GetService<ITrackInstalledApplicationsService>())
                .AddSingleton<ITakeSnapshot<OpenedApplicationsSnapshot>>(x => x.GetService<ITrackOpenedApplicationsService>())
                .AddSingleton<ITakeSnapshot<DnsCacheSnapshot>>(x => x.GetService<ITrackDnsCacheService>())
                .AddSingleton<ISyncService, SyncService>()
                .AddSingleton<KeystrokeAPI>()
                .AddTransient<ITrackApiWrapper, ApiStubWrapper>()
                .BuildServiceProvider();
        }
    }
}
