#region Using

using System;
using System.Collections.Generic;
using System.Reflection;

using NLib;

#endregion

namespace PPRP.Services
{
    #region SignInManager

    /// <summary>
    /// SignInManager class.
    /// </summary>
    public class SignInManager : NSingelton<SignInManager>
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SignInManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~SignInManager()
        {
            Signout();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sign In.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>Returns True if signin success.</returns>
        public bool SignIn(string userName, string password)
        {
            User = new object();
            bool success = (null != User);
            if (success)
            {
                // Raise Event.
                UserChanged.Call(this, EventArgs.Empty);
            }
            return success;
        }
        /// <summary>
        /// Signout.
        /// </summary>
        public void Signout()
        {
            this.User = null;
            // Raise Event.
            UserChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current user.
        /// </summary>
        public object User { get; private set; }

        #endregion

        #region Public Events

        /// <summary>
        /// UserChanged event.
        /// </summary>
        public event EventHandler UserChanged;

        #endregion
    }

    #endregion
}
