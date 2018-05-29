using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTracker.Services.SignIn;

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
                .AddLogging()
                .AddTransient<Main>()
                .AddTransient<SignInService>()
                .BuildServiceProvider();
        }
    }
}
