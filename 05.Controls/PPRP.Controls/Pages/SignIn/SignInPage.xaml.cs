#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib.Services;
using PPRP.Services;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class SignInPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SignInPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sUserName = string.Empty;
        private string sPassword = string.Empty;

        #endregion

        #region Button Handlers

        private void txtSignIn_Click(object sender, RoutedEventArgs e)
        {
            SignIn();
        }

        #endregion

        #region Textbox/Passwordbox handlers

        private void txtUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUserName.Text) &&
                !string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                if (txtUserName.Text.Trim() != sUserName || txtPassword.Password.Trim() != sPassword)
                {
                    sUserName = txtUserName.Text.Trim();
                    sPassword = txtPassword.Password.Trim();
                    SignIn();
                }
            }
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUserName.Text) &&
                !string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                if (txtUserName.Text.Trim() != sUserName || txtPassword.Password.Trim() != sPassword)
                {
                    sUserName = txtUserName.Text.Trim();
                    sPassword = txtPassword.Password.Trim();
                    SignIn();
                }
            }
        }

        #endregion

        #region Private Methods

        private void FocusControl(TextBox ctrl)
        {
            if (null == ctrl)
                return;
            // Set focus to text box this invoked when the application has rendered
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ctrl.SelectAll();
                ctrl.Focus();
            }), System.Windows.Threading.DispatcherPriority.Render);
        }
        private void FocusControl(PasswordBox ctrl)
        {
            if (null == ctrl)
                return;
            // Set focus to text box this invoked when the application has rendered
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ctrl.SelectAll();
                ctrl.Focus();
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void SignIn()
        {
            string userName = txtUserName.Text;
            if (string.IsNullOrWhiteSpace(userName))
            {
                var win = PPRPWindows.Windows.MessageBox;
                win.Setup("กรุณาป้อน ชื่อบัญชีผู้ใช้", "PPRP");
                win.ShowDialog();

                FocusControl(txtUserName);
                return;
            }

            string password = txtPassword.Password;
            if (string.IsNullOrWhiteSpace(password))
            {
                var win = PPRPWindows.Windows.MessageBox;
                win.Setup("กรุณาป้อน รหัสผ่าน", "PPRP");
                win.ShowDialog();

                FocusControl(txtPassword);
                return;
            }

            SignInStatus status = SignInManager.Instance.SignIn(userName, password);

            bool success = false;
            string msg = string.Empty;
            switch (status)
            {
                case SignInStatus.UserNotFound:
                    msg = "ไม่พบข้อมูล ชื่อผู้ใช้งาน กรุณาตรวจสอบ";
                    break;
                case SignInStatus.InvalidPassword:
                    msg = "รหัสผ่านไม่ถูกต้อง กรุณาตรวจสอบ";
                    break;
                case SignInStatus.Success:
                    success = true;
                    break;
            }

            if (!success)
            {
                // login failed show message.
                var win = PPRPWindows.Windows.MessageBox;
                win.Setup(msg, "PPRP");
                win.ShowDialog();

                if (status == SignInStatus.InvalidPassword)
                    FocusControl(txtPassword);
                else FocusControl(txtUserName);

                return;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            sUserName = string.Empty;
            sPassword = string.Empty;

            FocusControl(txtUserName);
        }

        #endregion
    }
}
