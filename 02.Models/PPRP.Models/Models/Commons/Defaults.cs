#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Media;
using System.Windows.Threading;

using NLib;

#endregion

namespace PPRP.Models
{
    /// <summary>
    /// The Defaults Utilities class.
    /// </summary>
    public class Defaults
    {
        #region Internal Variables (static)

        // Person vars
        private static bool _PersonImageLoading = false;
        private static ImageSource _PersonImage = null;
        private static bool _PersonBufferLoading = false;
        private static byte[] _PersonBuffer = null;

        // Party vars
        private static bool _PartyImageLoading = false;
        private static ImageSource _PartyImage = null;
        private static bool _PartyBufferLoading = false;
        private static byte[] _PartyBuffer = null;

        #endregion

        #region Public Properties (static)

        /// <summary>
        /// Execute action in background thread.
        /// </summary>
        /// <param name="action">The target action delegate.</param>
        public static void RunInBackground(Action action)
        {
            if (null != action)
            {
                if (null != Dispatcher)
                {
                    Dispatcher.BeginInvoke((Action)(() => { action(); }), DispatcherPriority.Background);
                }
            }
        }

        #endregion

        #region Public Properties (static)

        #region Dispatcher

        /// <summary>
        /// Gets Main Window Dispatcher
        /// </summary>
        public static Dispatcher Dispatcher
        {
            get
            {
                if (null != System.Windows.Application.Current)
                    return System.Windows.Application.Current.Dispatcher;
                else return null;
            }
        }

        #endregion

        #region Person

        /// <summary>
        /// Gets Default Person ImageSource.
        /// </summary>
        public static ImageSource Person
        {
            get
            {
                if (null == _PersonImage && !_PersonImageLoading)
                {
                    lock (typeof(Defaults))
                    {
                        _PersonImageLoading = true;

                        RunInBackground(() =>
                        {
                            var uri = new Uri("pack://application:,,,/PPRP.Models;component/Images/Default/person.jpg", UriKind.Absolute);
                            _PersonImage = ByteUtils.GetImageSource(uri);
                            _PersonImageLoading = false;
                        });
                    }

                }
                return _PersonImage;
            }
        }
        /// <summary>
        /// Gets Default Person Image Buffers.
        /// </summary>
        public static byte[] PersonBuffer
        {
            get
            {
                if (null == _PersonBuffer && !_PersonBufferLoading)
                {
                    lock (typeof(Defaults))
                    {
                        _PersonImageLoading = true;
                        var uri = new Uri("pack://application:,,,/PPRP.Models;component/Images/Default/person.jpg", UriKind.Absolute);
                        _PersonBuffer = ByteUtils.GetResourceUriBuffer(uri);
                        _PersonImageLoading = false;
                    }

                }
                return _PersonBuffer;
            }
        }

        #endregion

        #region Party

        /// <summary>
        /// Gets Default Party ImageSource.
        /// </summary>
        public static ImageSource Party
        {
            get
            {
                if (null == _PartyImage && !_PartyImageLoading)
                {
                    lock (typeof(Defaults))
                    {
                        _PartyImageLoading = true;

                        RunInBackground(() =>
                        {
                            var uri = new Uri("pack://application:,,,/PPRP.Models;component/Images/Default/party.png", UriKind.Absolute);
                            _PartyImage = ByteUtils.GetImageSource(uri);
                            _PartyImageLoading = false;
                        });
                    }

                }
                return _PersonImage;
            }
        }
        /// <summary>
        /// Gets Default Person Image Buffers.
        /// </summary>
        /// <summary>
        /// Gets Default Party Image Buffers.
        /// </summary>
        public static byte[] PartyBuffer
        {
            get
            {
                if (null == _PartyBuffer && !_PartyBufferLoading)
                {
                    lock (typeof(Defaults))
                    {
                        _PartyBufferLoading = true;
                        var uri = new Uri("pack://application:,,,/PPRP.Models;component/Images/Default/party.png", UriKind.Absolute);
                        _PartyBuffer = ByteUtils.GetResourceUriBuffer(uri);
                        _PartyBufferLoading = false;
                    }

                }
                return _PersonBuffer;
            }
        }

        #endregion

        #endregion
    }
}
