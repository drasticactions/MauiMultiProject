using Microsoft.Maui.Controls;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MauiMultiProject
{
    class Program
    {
        [STAThread]
        static async Task<int> Main(string[] args)
        {
            WinRT.ComWrappersSupport.InitializeComWrappers();
            bool isRedirect = await DecideRedirection();
            if (!isRedirect)
            {
                Microsoft.UI.Xaml.Application.Start((p) =>
                {
                    var context = new DispatcherQueueSynchronizationContext(
                        DispatcherQueue.GetForCurrentThread());
                    SynchronizationContext.SetSynchronizationContext(context);
                    Application.Current = new App();
                });
            }

            return 0;
        }

        private static async Task<bool> DecideRedirection()
        {
            bool isRedirect = false;
            AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
            ExtendedActivationKind kind = args.Kind;
            AppInstance keyInstance = AppInstance.FindOrRegisterForKey("mainInstance");

            if (keyInstance.IsCurrent)
            {
                keyInstance.Activated += KeyInstance_Activated;
            }
            else
            {
                isRedirect = true;
                await keyInstance.RedirectActivationToAsync(args);
            }

            return isRedirect;
        }

        private static void KeyInstance_Activated(object? sender, AppActivationArguments e)
        {
            ExtendedActivationKind kind = e.Kind;
            switch (kind)
            {
                case ExtendedActivationKind.Protocol:

                    break;
            }
        }
    }
}
