#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Models
{
    #region Error Consts

    public enum ErrNums : int
    {
        Success = 0,
        // Local Database Connection (100-149)
        DbConenctFailed = 100,
        // Web Service Connection (150-199)
        RestConenctFailed = 150,
        // Note. remove err num and used http status code instead.
        //RestResponseError = 151,
        RestInvalidConfig = 152,
        // Models - Common (200-210)
        ParameterIsNull = 200,
        // Common Exception
        Exception = 900,
        // Unknown (999)
        UnknownError = 999
    }

    public class ErrConsts
    {
        private static Dictionary<ErrNums, string> _msgs;

        // TODO: Required to add more error message.
        static ErrConsts()
        {
            _msgs = new Dictionary<ErrNums, string>();
            _msgs.Add(ErrNums.Success, "Success.");
            // Local Database
            _msgs.Add(ErrNums.DbConenctFailed, "Database connection failed.");

            // Web Service Connection
            _msgs.Add(ErrNums.RestConenctFailed, "Web Service connection failed.");
            // Note. remove err message and used http error message instead. 
            //_msgs.Add(ErrNums.RestResponseError, "Web Service response error.");

            // Models - common
            _msgs.Add(ErrNums.ParameterIsNull, "Parameter is null.");
            // Common Exception
            _msgs.Add(ErrNums.Exception, "Exception detected.");
            // Unknown
            _msgs.Add(ErrNums.UnknownError, "Unknown error.");
        }

        public static string ErrMsg(ErrNums value)
        {
            if (_msgs.ContainsKey(value))
                return _msgs[value];
            else return _msgs[ErrNums.UnknownError];
        }
    }

    #endregion

    #region NDbError

    /// <summary>
    /// The NDbError class.
    /// </summary>
    public class NDbError
    {
        #region Public Properties

        /// <summary>
        /// Checks has errors.
        /// </summary>
        public bool hasError
        {
            get { return (errNum != 0); }
            set { }
        }
        /// <summary>
        /// Gets or sets error number.
        /// </summary>
        public int errNum { get; set; }
        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        public string errMsg { get; set; }

        #endregion
    }

    #endregion

    #region NDbResult

    /// <summary>
    /// The NDbResult class.
    /// </summary>
    public class NDbResult
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base()
        {
            this.errors = new NDbError();
            UnknownError();
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Set Database (local) Connection Error.
        /// </summary>
        public virtual void DbConenctFailed()
        {
            var err = ErrNums.DbConenctFailed;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Unknown Error.
        /// </summary>
        public virtual void UnknownError()
        {
            var err = ErrNums.UnknownError;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Parameter Is Null Error.
        /// </summary>
        public virtual void ParameterIsNull()
        {
            var err = ErrNums.ParameterIsNull;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Success.
        /// </summary>
        public virtual void Success()
        {
            var err = ErrNums.Success;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Error.
        /// </summary>
        /// <param name="ex">The exception instance.</param>
        public virtual void Error(Exception ex)
        {
            var err = ErrNums.Exception;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ex.Message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets error instance.
        /// </summary>
        public NDbError errors { get; set; }
        /// <summary>
        /// Checks if operation is success.
        /// </summary>
        public virtual bool Ok
        {
            get { return !this.errors.hasError; }
            set { }
        }
        /// <summary>
        /// Checks if operation is failed.
        /// </summary>
        public virtual bool Failed
        {
            get { return this.errors.hasError; }
            set { }
        }
        /// <summary>
        /// Gets or sets Err Number.
        /// </summary>
        public int ErrNum 
        {
            get { return errors.errNum; }
            set { errors.errNum = value; }
        }
        /// <summary>
        /// Gets or sets Err Message.
        /// </summary>
        public string ErrMsg 
        {
            get { return errors.errMsg; }
            set { errors.errMsg = value; }
        }
        /// <summary>
        /// Checks Has Error.
        /// </summary>
        public bool HasError
        {
            get { return ErrNum != 0; }
            set { }
        }
        /// <summary>
        /// Gets or sets Page No.
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// Gets or sets Row Per Page.
        /// </summary>
        public int RowsPerPage { get; set; }
        /// <summary>
        /// Gets or sets Max Page.
        /// </summary>
        public int MaxPage { get; set; }
        /// <summary>
        /// Gets or sets Total Records.
        /// </summary>
        public int TotalRecords { get; set; }

        #endregion
    }

    #endregion

    #region NDbResult<T>

    /// <summary>
    /// The NDbResult class.
    /// </summary>
    /// <typeparam name="T">The Type of data.</typeparam>
    public class NDbResult<T> : NDbResult
        where T : new()
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base()
        {
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Set Database (local) Connection Error.
        /// </summary>
        public override void DbConenctFailed()
        {
            base.DbConenctFailed();
            this.data = Default();
        }
        /// <summary>
        /// Set Unknown Error.
        /// </summary>
        public override void UnknownError()
        {
            base.UnknownError();
            this.data = Default();
        }
        /// <summary>
        /// Set Parameter Is Null Error.
        /// </summary>
        public override void ParameterIsNull()
        {
            base.ParameterIsNull();
            this.data = Default();
        }
        /// <summary>
        /// Set Success.
        /// </summary>
        /// <param name="data">The Data instance.</param>
        public void Success(T data)
        {
            base.Success();
            this.data = (null != data) ? data : Default();
        }
        /// <summary>
        /// Set Error.
        /// </summary>
        /// <param name="ex">The exception instance.</param>
        public override void Error(Exception ex)
        {
            base.Error(ex);
            this.data = Default();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Data Instance.
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// Checks if has data (not null).
        /// </summary>
        public bool HasData
        {
            get { return (null != this.data); }
            set { }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets default instance.
        /// </summary>
        /// <returns>Returns default instance. If T is IList return new instance.</returns>
        public static T Default()
        {
            return (typeof(T) == typeof(IList)) ? new T() : default(T);
        }

        #endregion
    }

    #endregion

    #region NDbResult<T, O>

    /// <summary>
    /// The NDbResult class.
    /// </summary>
    /// <typeparam name="T">The Type of data.</typeparam>
    /// <typeparam name="O">The Type of output.</typeparam>
    public class NDbResult<T, O> : NDbResult
        where T : new()
        where O : new()
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base()
        {
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Set Database (local) Connection Error.
        /// </summary>
        public override void DbConenctFailed()
        {
            base.DbConenctFailed();
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }
        /// <summary>
        /// Set Unknown Error.
        /// </summary>
        public override void UnknownError()
        {
            base.UnknownError();
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }
        /// <summary>
        /// Set Parameter Is Null Error.
        /// </summary>
        public override void ParameterIsNull()
        {
            base.ParameterIsNull();
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }
        /// <summary>
        /// Set Success.
        /// </summary>
        /// <param name="data">The Data instance.</param>
        /// <param name="output">The Output instance.</param>
        public void Success(T data, O output)
        {
            base.Success();
            this.data = (null != data) ? data : DefaultData();
            this.Output = (null != output) ? output : DefaultOutput();
        }
        /// <summary>
        /// Set Error.
        /// </summary>
        /// <param name="ex">The exception instance.</param>
        public override void Error(Exception ex)
        {
            base.Error(ex);
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Data Instance.
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// Checks if has data (not null).
        /// </summary>
        public bool HasData
        {
            get { return (null != this.data); }
            set { }
        }
        /// <summary>
        /// Gets Output Instance (for serialized).
        /// </summary>
        public O @out { get; set; }
        /// <summary>
        /// Gets Output Instance.
        /// </summary>
        public O Output
        {
            get { return this.@out; }
            set { this.@out = value; }
        }
        /// <summary>
        /// Checks if has ouput (not null)
        /// </summary>
        public bool HasOutput
        {
            get { return (null != this.Output); }
            set { }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets default instance.
        /// </summary>
        /// <returns>Returns default instance. If T is IList return new instance.</returns>
        public static T DefaultData()
        {
            return (typeof(T) == typeof(IList)) ? new T() : default(T);
        }
        /// <summary>
        /// Gets default instance.
        /// </summary>
        /// <returns>Returns default instance. If T is IList return new instance.</returns>
        public static O DefaultOutput()
        {
            return (typeof(O) == typeof(IList)) ? new O() : default(O);
        }

        #endregion
    }

    #endregion

    #region NDbResult Extension Methods

    /// <summary>
    /// The NDbResult Extension Methods class.
    /// </summary>
    public static class NDbResultExtensionMethods
    {
        #region Public Methods (static)

        /// <summary>
        /// Gets defult value of data.
        /// </summary>
        /// <typeparam name="T">Tht data type.</typeparam>
        /// <returns>Returns default instance. If type is IList new instance returns.</returns>
        public static T DefaultData<T>()
            where T : new()
        {
            return (typeof(T) == typeof(IList)) ? new T() : default(T);
        }
        /// <summary>
        /// Gets defult value of output.
        /// </summary>
        /// <typeparam name="O">The output type.</typeparam>
        /// <returns>Returns default instance. If type is IList new instance returns.</returns>
        public static O DefaultOutput<O>()
            where O : new()
        {
            return (typeof(O) == typeof(IList)) ? new O() : default(O);
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets Data instance.
        /// </summary>
        /// <typeparam name="T">The type for data instance.</typeparam>
        /// <param name="value">The instance to get data.</param>
        /// <returns>Returns data instance.</returns>
        public static T Value<T>(this NDbResult<T> value)
            where T : new()
        {
            T ret = (null != value && !value.errors.hasError && null != value.data) ?
                value.data : DefaultData<T>();
            return ret;
        }
        /// <summary>
        /// Gets Data instance.
        /// </summary>
        /// <typeparam name="T">The type for data instance.</typeparam>
        /// <typeparam name="O">The type for output instance.</typeparam>
        /// <param name="value">The instance to get data.</param>
        /// <returns>Returns data instance.</returns>
        public static T Value<T, O>(this NDbResult<T, O> value)
            where T : new()
            where O : new()
        {
            T ret = (null != value && !value.errors.hasError && null != value.data) ?
                value.data : DefaultData<T>();
            return ret;
        }
        /// <summary>
        /// Gets Output instance.
        /// </summary>
        /// <typeparam name="T">The type for data instance.</typeparam>
        /// <typeparam name="O">The type for output instance.</typeparam>
        /// <param name="value">The instance to get output.</param>
        /// <returns>Returns output instance.</returns>
        public static O Output<T, O>(this NDbResult<T, O> value)
            where T : new()
            where O : new()
        {
            O ret = (null != value && !value.errors.hasError && null != value.Output) ?
                value.Output : DefaultOutput<O>();
            return ret;
        }

        #endregion
    }

    #endregion

    #region ImportError

    /// <summary>
    /// The ImportError class.
    /// </summary>
    public class ImportError
    {
        #region Public Properties

        /// <summary>
        /// Gets or set RowNo.
        /// </summary>
        public int RowNo { get; set; }
        /// <summary>
        /// Gets or set ErrMsg.
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// Gets or set DataString.
        /// </summary>
        public string DataString { get; set; }

        #endregion
    }

    #endregion
}
