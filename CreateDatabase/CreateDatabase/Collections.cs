using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CreateDatabase
{
    public class ColumnsCollection : CollectionBase
    {
        public int Count
        {
            get { return this.List.Count; }
        }

        public void Add(IColumn column)
        {
            this.List.Add(column);
        }

        public void Remove(IColumn column)
        {
            this.List.Remove(column);
        }
        public IColumn this[int columnIndex] 
        {
            get 
            {
                return (IColumn)this.List[columnIndex];
            }
            set
            {
                this.List[columnIndex] = value;
            }
        }
    }

    public class TableCollection : CollectionBase
    {
        public int Count
        {
            get { return this.List.Count; }
        }

        public void Add(ITable table)
        {
            this.List.Add(table);
        }

        public void Remove(ITable table)
        {
            this.List.Remove(table);
        }

        public ITable this[int tableIndex]
        {
            get
            {
                return (ITable)this.List[tableIndex];
            }
            set
            {
                this.List[tableIndex] = value;
            }
        }

        public void Create()
        {
            foreach (ITable table in this.List)
            {
                table.Create();
            }
        }

        //TODO
        public void LoadSchemaFromXml(string file)
        {
            XDocument XMLDoc = XDocument.Load(file);
            XElement ROOTChild = XMLDoc.Element("ROOT");
            IEnumerable<XElement> DatabaseChildren = ROOTChild.Elements("DATABASE");
            foreach(XElement databaseChild in DatabaseChildren)
            {
                IEnumerable<XElement> tableChildren = databaseChild.Elements("TABLE");
                foreach(XElement tableChild in tableChildren)
                {
                    ITable table = new CTable();
                    foreach(XAttribute attribute in tableChild.Attributes())
                    {
                        switch(attribute.Name.ToString().ToUpper())
                        {
                            case "NAME":
                                table.Name = attribute.Value;
                                break;
                            case "ALIASNAME":
                                table.AliasName = attribute.Value;
                                break;
                            default:
                                break;
                        }

                    }

                    IEnumerable<XElement> fieldChildren = tableChild.Elements("FIELD");
                    foreach(XElement fieldChild in fieldChildren)
                    {
                        IColumn column = new CColumn();
                        foreach (XAttribute attribute in fieldChild.Attributes())
                        {
                            switch (attribute.Name.ToString().ToUpper())
                            {
                                case "NAME":
                                    column.Name = attribute.Value;
                                    break;
                                case "ALIASNAME":
                                    column.AliasName = attribute.Value;
                                    break;
                                case "TYPE" :
                                    column.Type = Parse(attribute.Value);
                                    break;
                                case "LENGTH":
                                    int length;
                                    int.TryParse(attribute.Value, out length);
                                    column.Precision = length;
                                    break;
                                case "SCALE":
                                    int scale;
                                    int.TryParse(attribute.Value, out scale);
                                    column.Scale = scale;
                                    break;
                                case "ISNULLABLE":
                                    int isNull;
                                    int.TryParse(attribute.Value, out isNull);
                                    column.IsNullable = Parse(isNull);
                                    break;
                                case "ISPRIMARYKEY":
                                    int isPrimaryKey;
                                    int.TryParse(attribute.Value, out isPrimaryKey);
                                    column.IsPrimaryKey = Parse(isPrimaryKey);
                                    break;
                            }
                        }
                        table.Columns.Add(column);
                    }
                    this.Add(table);
                }
            }
            

        }

        private string Parse(string type)
        {
            switch (type.ToUpper())
            {
                case "CHAR":
                    return "varchar2";
                case "INT":
                case "FLOAT":
                    return "number";
                case "DATE":
                    return "date";
                case "VARBIN":
                    return "blob";
            }
            return null;
        }

        private bool Parse(int type)
        {
            switch (type)
            {
                case 0:
                    return false;
                case 1:
                    return true;
            }
            return false;
        }
    }
}
