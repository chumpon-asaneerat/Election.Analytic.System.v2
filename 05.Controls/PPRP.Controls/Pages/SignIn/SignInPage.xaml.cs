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

        #region Button Handlers

        private void txtSignIn_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Password;
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
                MessageBox.Show(msg);
                return;
            }
            // login success.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {

        }

        #endregion
    }
}
