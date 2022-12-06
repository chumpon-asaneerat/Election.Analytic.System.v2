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
            page.Setup();
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

        #region Window Closing

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != PageContentManager.Instance.Current)
            {
                Type curr = PageContentManager.Instance.Current.GetType();
                if (curr != PPRPApp.Pages.SignIn.GetType())
                {
                    var win = PPRPApp.Windows.MessageBoxOKCancel;
                    win.Setup("ต้องการปิดโปรแกรมใช่หรือไม่", "PPRP");
                    if (win.ShowDialog() == true)
                    {
                        // signout
                        SignInManager.Instance.Signout();
                        e.Cancel = false;
                    }
                    else
                    {
                        // stay on current page.
                        e.Cancel = true;
                    }
                }
                else
                {
                    // on signin page so allow close
                    e.Cancel = false;
                }
            }
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
                page.Setup();
                PageContentManager.Instance.Current = page;
            }
            else
            {
                // SignIn OK - show main menu.
                var page = PPRPApp.Pages.MainMenu;
                page.Setup();
                PageContentManager.Instance.Current = page;
            }
        }

        #endregion
    }
}
