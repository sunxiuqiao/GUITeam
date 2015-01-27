using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using MicroMvvm;
using ESRI.ArcGIS.Carto;

namespace MVVMTest.ViewModels
{
    public abstract class LayerViewModel : ObservableObject
    {
        #region LayerName;
        abstract protected string layerName();
        public string LayerName { get { return layerName(); } }
        #endregion

        #region DataLayer
        protected ILayer dataLayer;
        public ILayer DataLayer { get { return dataLayer; } }
        #endregion       

        #region IsValid
        abstract protected bool isValid();
        public bool IsValid { get { return isValid(); } }
        #endregion
    }
}
