#region Using

using System;
using System.Collections.Generic;
using System.Windows;

using NLib.Services;
using PPRP.Services;

#endregion

namespace PPRP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start Server
            DbServer.Instance.Start();

            #region Test Connection
            /*
            var rets = Domains.MTitle.Gets();
            if (null != rets)
            {
                Console.WriteLine("Count: {0}", rets.Value.Count);
            }
            */
            #endregion

            // Initial Page Content Manager
            PageContentManager.Instance.ContentChanged += new EventHandler(Instance_ContentChanged);
            PageContentManager.Instance.Start();

            // Init SignIn Manager
            SignInManager.Instance.UserChanged += Instance_UserChanged;

            // Sign In.
            var page = PPRPApp.Pages.SignIn;
            PageContentManager.Instance.Current = page;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Release SignIn Manager
            SignInManager.Instance.Signout();
            SignInManager.Instance.UserChanged -= Instance_UserChanged;

            // Release Page Content Manager
            PageContentManager.Instance.Shutdown();
            PageContentManager.Instance.ContentChanged -= new EventHandler(Instance_ContentChanged);

            // Shutdown Server
            DbServer.Instance.Shutdown();
        }

        #endregion

        #region Page Content Manager Handlers

        void Instance_ContentChanged(object sender, EventArgs e)
        {
            this.container.Content = PageContentManager.Instance.Current;
        }

        #endregion

        #region SignIn Manager Handlers

        private void Instance_UserChanged(object sender, EventArgs e)
        {
            if (null == SignInManager.Instance.User)
            {
                // Signout - show Sign In page.
                var page = PPRPApp.Pages.SignIn;
                PageContentManager.Instance.Current = page;
            }
            else
            {
                // SignIn OK - show thailand page.
                //var page = PPRPApp.Pages.Thailand;
                //page.Setup();
                //PageContentManager.Instance.Current = page;
            }
        }

        #endregion
    }
}
