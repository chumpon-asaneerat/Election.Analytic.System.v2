#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Reflection;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using NLib;
using Dapper;
using System.Windows.Media.Imaging;

using Newtonsoft.Json;

#endregion

namespace PPRP
{
    /// <summary>
    /// The Byte Utils class.
    /// </summary>
    public class ByteUtils
    {
        #region Public Methods (static)

        #region Buffers

        /// <summary>
        /// Gets Buffers from Full File Name.
        /// </summary>
        /// <param name="fileName">The Full File Name.</param>
        /// <returns>Returns file buffer array.</returns>
        public static byte[] GetFileBuffer(string fileName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            byte[] buffers = null;

            if (string.IsNullOrEmpty(fileName))
            {
                med.Err("No File Name assigned.");
                return buffers;
            }
            if (!File.Exists(fileName))
            {
                med.Err("File Not Found. File Name: {0}", fileName);
                return buffers;
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        buffers = reader.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return buffers;
        }
        /// <summary>
        /// Convert Resource Uri Image to Buffers.
        /// </summary>
        /// <param name="resourceUri">The resource Uri.</param>
        /// <returns>Returns ImageSource uri buffer in byte array.</returns>
        public static byte[] GetResourceUriBuffer(Uri resourceUri)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            byte[] ret = null;
            if (null == resourceUri)
            {
                med.Err("Uri is null.");
                return ret;
            }
            try
            {

                var info = Application.GetResourceStream(resourceUri);
                var memoryStream = new MemoryStream();
                info.Stream.CopyTo(memoryStream);
                ret = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                ret = null;
                //throw ex;
                med.Err(ex);
            }

            return ret;
        }

        #endregion

        #region Image Source

        /// <summary>
        /// Convert Buffers to Image source.
        /// </summary>
        /// <param name="buffers">The buffer array.</param>
        /// <returns>Returns ImageSource instance.</returns>
        public static ImageSource GetImageSource(byte[] buffers /*, int decodePixelWidth = 0 */)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            ImageSource ret = null;
            if (null == buffers)
            {
                med.Err("Buffer is null.");
                return ret;
            }
            if (buffers.Length <= 0)
            {
                med.Err("Buffer length is zero.");
                return ret;
            }

            try
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

                using (Stream stream = new MemoryStream(buffers))
                {
                    BitmapImage image = new BitmapImage();
                    stream.Position = 0;
                    image.BeginInit();

                    image.CacheOption = BitmapCacheOption.OnLoad;
                    //image.DecodePixelWidth = (decodePixelWidth > 0) ? decodePixelWidth : 0;

                    image.StreamSource = stream;

                    image.EndInit();
                    image.Freeze();

                    ret = image;
                }
            }
            catch (Exception ex)
            {
                ret = null;
                //throw ex;
                med.Err(ex);
            }

            return ret;
        }
        /// <summary>
        /// Convert Resourec Uri to Image source.
        /// </summary>
        /// <param name="resourceUri">The resource uri.</param>
        /// <returns>Returns ImageSource instance.</returns>
        public static ImageSource GetImageSource(Uri resourceUri /*, int decodePixelWidth = 0 */)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            ImageSource ret = null;
            if (null == resourceUri)
            {
                med.Err("Uri is null.");
                return ret;
            }
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();

                image.CacheOption = BitmapCacheOption.OnLoad;
                //image.DecodePixelWidth = (decodePixelWidth > 0) ? decodePixelWidth : 0;

                image.UriSource = resourceUri;

                image.EndInit();
                image.Freeze();
                ret = image;
            }
            catch (Exception ex)
            {
                ret = null;
                //throw ex;
                med.Err(ex);
            }

            return ret;
        }

        #endregion

        #region Json

        /// <summary>
        /// Gets Json String from Byte array.
        /// </summary>
        /// <param name="buffers">The Byte array.</param>
        /// <returns>Returns Json data in string.</returns>
        public static string GetJsonString(byte[] buffers)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string ret = null;
            try
            {
                ret = System.Text.Encoding.UTF8.GetString(buffers);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return ret;
        }
        /// <summary>
        /// Format Json string.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>Return new formated json string</returns>
        public static string FormatJson(string json)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string ret = string.Empty;
            try
            {
                // formatting json
                dynamic dJson = JsonConvert.DeserializeObject(json);
                ret = JsonConvert.SerializeObject(dJson, Formatting.Indented);
            }
            catch (Exception ex)
            {
                med.Err(ex);
                ret = json;
            }
            return ret;
        }

        #endregion

        #endregion
    }
}
