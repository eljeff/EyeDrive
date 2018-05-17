using Microsoft.Shell;
using Squirrel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;

namespace EyeDrive
{
    public partial class App : Application, ISingleInstanceApp
    {
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            return true;
        }

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance("{35BBF621-4276-48A8-AEEB-71011A44C476}"))
            {
                var application = new App();

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
            else
            {
                MessageBox.Show($"{Assembly.GetEntryAssembly().GetName().Name} is already running.");
            }
        }

        public App()
        {
            DispatcherUnhandledException += (o, e) =>
            {
                var ex = e.Exception;

                // Grabbing the InnerException if it's there to get closer to the source.
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                MessageBox.Show(ex.ToString());
            };
        }
    }
    public partial class App : Application
    {
        private Host _host;
        private WpfInteractorAgent _wpfInteractorAgent;

        protected override void OnStartup(StartupEventArgs e)
        {
            _host = new Host();
            _wpfInteractorAgent = _host.InitializeWpfAgent();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
