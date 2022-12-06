#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP
{
    public class AreaNavi : NSingelton<AreaNavi>
    {
        #region Internal Variables

        private List<PakMenuItem> _regions = null;
        
        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AreaNavi() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~AreaNavi()
        {
            Current = null;
            _regions = null;
        }

        #endregion

        #region Public Methods

        public int GetPakByMenuItem(PakMenuItem item)
        {
            return null != item ? GetPakByRegionId(item.RegionId) : -1;
        }

        public int GetPakByRegionId(string regionId)
        {
            int idx = -1;
            if (!HasRegions) return idx;

            idx = Regions.FindIndex((pak) => { return pak.RegionId == regionId; });
            if (idx == -1 || idx >= Regions.Count) idx = -1; // out of range.

            return idx;
        }

        public void GotoPak(string regionId)
        {
            if (!HasRegions) return;
            int idx = GetPakByRegionId(regionId);
            if (idx == -1) return;

            Current = Regions[idx]; // set current;
        }

        public void GoPrev()
        {
            if (!HasRegions) return;

            int idx = GetPakByMenuItem(Current);
            if (idx == -1) return;
            idx--;
            if (idx < 0)
                Current = Regions[0];
            else Current = Regions[idx++];
        }

        public void GoNext()
        {
            if (!HasRegions) return;

            int idx = GetPakByMenuItem(Current);
            if (idx == -1) return;
            idx++;
            if (idx >= Regions.Count)
                Current = Regions[0];
            else Current = Regions[idx];
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Pak Menu items.
        /// </summary>
        public List<PakMenuItem> Regions
        {
            get 
            {
                if (null == _regions)
                {
                    lock (typeof(AreaNavi))
                    {
                        _regions = PakMenuItem.Gets().Value();
                    }
                }
                return _regions;
            }
            set { }
        }

        public bool HasRegions
        {
            get { return (null != Regions && Regions.Count > 0); }
        }

        public PakMenuItem Current { get; set; }

        #endregion
    }
}
