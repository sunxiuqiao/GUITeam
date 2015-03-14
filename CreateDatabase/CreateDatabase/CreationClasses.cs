using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;

namespace CreateDatabase
{
    class CGeoDatasetCreation : ICreation
    {
        public void Create(ITable table)
        {
            string shapeFieldName = null;
            IFeatureDataset featureDataset = ((CSpatialDatabase)table.Database).CurrentFeatureDataset;
            IGeoDataset geoDataset = featureDataset as IGeoDataset;
            IDatabase dataBase = table.Database;
            IFeatureWorkspace wsp = dataBase.Workspace as IFeatureWorkspace;
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            IField field = new FieldClass();
            IFieldEdit2 fieldEdit = (IFieldEdit2)field;
            fieldEdit.Name_2 = "ObjectID";
            fieldEdit.AliasName_2 = "FID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(field);
            foreach(IColumn column in table.Columns)
            {
                esriFieldType fieldType = (esriFieldType)column.Type;
                field = new FieldClass();
                fieldEdit.Name_2 = column.Name;
                fieldEdit.AliasName_2 = column.AliasName;
                fieldEdit.Type_2 = fieldType;
                fieldEdit.IsNullable_2 = column.IsNullable;
                fieldEdit.Length_2 = column.Precision;
                if(fieldType==esriFieldType.esriFieldTypeGeometry)
                {
                    shapeFieldName = column.Name;
                    IGeometryDef geometryDef = new GeometryDefClass();
                    IGeometryDefEdit gemertryDefEdit = geometryDef as IGeometryDefEdit;
                    gemertryDefEdit.GeometryType_2 = column.GeometryType;
                    gemertryDefEdit.SpatialReference_2 = geoDataset.SpatialReference;
                    fieldEdit.GeometryDef_2 = geometryDef;
                }
                fieldsEdit.AddField(field);
            }
            
            IFeatureClassDescription fcDesc = new FeatureClassDescriptionClass();
            IObjectClassDescription ocDesc = (IObjectClassDescription)fcDesc;
            featureDataset.CreateFeatureClass(table.Name, fields, ocDesc.InstanceCLSID, ocDesc.ClassExtensionCLSID, esriFeatureType.esriFTSimple, shapeFieldName, "");
        }

   
    }

    class CAttributeTableCreation : ICreation
    {
        public void Create(ITable table)
        {
            IDatabase database = table.Database;
            IColumn column;
            string cmdStr = @"CREATE TABLE " + table.Name + "(";
            for (int i = 0; i < table.Columns.Count - 1; i++)
            {
                column = table.Columns[i];
                if (column.IsPrimaryKey == true)
                {
                    cmdStr += string.Format("{0} {1}({2}) NOT NULL PRIMARY KEY, ", column.Name, column.Type.ToString(), column.Precision);
                }
                else
                {
                    string isNull = column.IsNullable ? "" : "NOT NULL";
                    if (column.Scale <= 0)
                        cmdStr += string.Format("{0} {1}({2}) {3}, ", column.Name, column.Type.ToString(), column.Precision, isNull);
                    else
                        cmdStr += string.Format("{0} {1}({2},{3}) {4}, ", column.Name, column.Type.ToString(), column.Precision, column.Scale, isNull);
                }
            }
            column = table.Columns[table.Columns.Count - 1];
            if (column.Scale <= 0)
                cmdStr += string.Format("{0} {1}({2}) {3})", column.Name, column.Type.ToString(), column.Precision, column.IsNullable ? "" : "NOT NULL");
            else
                cmdStr += string.Format("{0} {1}({2},{3}) {4})", column.Name, column.Type.ToString(), column.Precision, column.Scale, column.IsNullable ? "" : "NOT NULL");
                
            OracleCommand cmd = new OracleCommand(cmdStr, ((CODPNETDbConnection)table.Database.Connection).Conn);
            cmd.ExecuteNonQuery();
        }
    }

    class CBusinessTableCreation : ICreation
    {
        public void Create(ITable table)
        {
            IDatabase database = table.Database;
            string cmdStr = @"CREATE TABLE " + table.Name + "(";
            foreach (IColumn column in table.Columns)
            {
                if (column.IsPrimaryKey == true)
                {
                    cmdStr += string.Format("{0} {1}({2}) NOT NULL PRIMARY KEY, ", column.Name, column.Type.ToString(), column.Precision);
                }
                else
                {
                    string isNull = column.IsNullable ? "" : "NOT NULL";
                    cmdStr += string.Format("{0} {1}({2}) {3}, ", column.Name, column.Type.ToString(), column.Precision, isNull);
                }
            }
            cmdStr += ")";

            OracleCommand cmd = new OracleCommand(cmdStr, ((CODPNETDbConnection)table.Database.Connection).Conn);
            cmd.ExecuteNonQuery();
        }
    }
}
