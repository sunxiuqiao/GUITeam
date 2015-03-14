using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace CreateDatabase
{
    public interface IConnection
    {
        bool Open(IDatabase database);
        void Close(IDatabase database);
    }

    public interface IDatabase
    {
        string Server { get; set; }
        int PortNumber{ get; set; }
        //用户名
        string User{ get; set; }
        //密码
        string Password { get; set; }
        IConnection Connection { get; set; }
        TableCollection Tables { get; set; }
        string Name { get; set; }
        string ServiceName { get; set; }
        string Version { get; set; }
        EDatabaseType Type { get; set; }
        IWorkspace Workspace { get; set; }
        bool Open();
        void Close();
        void CreateTable();
    }

    public interface ITable
    {
        string Name{ get; set; }
        string AliasName { get; set; }
        IDatabase Database { get; set; }
        ICreation Creation { get; set; }
        ColumnsCollection Columns { get; set; }
        void Create();
    }

    public interface IColumn
    {
        int Scale { get; set; }
        bool IsPrimaryKey { get; set; }
        string Name { get; set; }
        string AliasName { get; set; }
        object Type { get; set; }
        int Precision { get; set; }
        bool IsNullable { get; set; }
        esriGeometryType GeometryType { get; set; }
    }

    public interface ICreation
    {
        void Create(ITable table);
    }
}
