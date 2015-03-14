using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDatabase
{
    class CColumn : IColumn
    {
        private string name;
        private string aliasName;
        private object type;
        private int precision;
        private bool isNullable;
        private esriGeometryType geometryType;
        private bool isPrimaryKey;
        private int scale;

        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string AliasName
        {
            get { return aliasName; }
            set { aliasName = value; }
        }
        public object Type
        {
            get { return type; }
            set { type = value; }
        }
        public int Precision
        {
            get { return precision; }
            set { precision = value; }
        }
        public bool IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        public esriGeometryType GeometryType
        {
            get { return geometryType; }
            set { geometryType = value; }
        }

        public bool IsPrimaryKey
        {
            get { return isPrimaryKey; }
            set { isPrimaryKey = value; }
        }
    }
}
