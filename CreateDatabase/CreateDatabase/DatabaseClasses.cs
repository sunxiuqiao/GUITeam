using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CreateDatabase
{
    public abstract class CDatabase : IDatabase
    {
        private string server;
        private int portNumber;
        private string user;
        private string password;
        private IConnection connection;
        private TableCollection tables = new TableCollection();
        private string name;
        private string serviceName;
        private string version;
        private EDatabaseType type;
        private IWorkspace workspace;

        public string Server
        {
            get { return this.server; }
            set { this.server = value; }
        }
        public int PortNumber
        {
            get { return this.portNumber; }
            set { this.portNumber = value; }
        }
        //用户名
        public string User
        {
            get { return this.user; }
            set { this.user = value; }
        }
        //密码
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }
        public IConnection Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }
        public TableCollection Tables
        {
            get { return tables; }
            set { tables = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string ServiceName
        {
            get { return this.serviceName; }
            set { this.serviceName = value; }
        }
        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }
        public EDatabaseType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public IWorkspace Workspace
        {
            get { return this.workspace; }
            set { this.workspace = value; }
        }

        public bool Open()
        {
            return this.Connection.Open(this);
        }

        public void Close()
        {
            this.Connection.Close(this);
        }

        public void CreateTable()
        {
            this.Tables.Create();
        }
    }

    public class CSpatialDatabase : CDatabase
    {
        private ArrayList featureDatasets;
        private IFeatureDataset currentFeatureDataset;

        public ArrayList FeatureDatasets
        {
            get { return this.featureDatasets; }
            set { this.featureDatasets = value; }
        }

        public IFeatureDataset CurrentFeatureDataset
        {
            get { return this.currentFeatureDataset; }
            set { this.currentFeatureDataset = value; }
        }

        private static CSpatialDatabase spatialDatabase;

        private CSpatialDatabase()
        {
            this.Type = EDatabaseType.SpatialDatabase;
            this.Connection = new CSparialDbConnection();
            this.Tables = new TableCollection();
        }

        public static CSpatialDatabase GetInstance()
        {
            if (spatialDatabase == null)
                spatialDatabase = new CSpatialDatabase();
            return spatialDatabase;
        }

        
    }
    public class CAttributeDatabase : CDatabase
    {
        private static CAttributeDatabase attributeDatabase;

        private CAttributeDatabase()
        {
            this.Type = EDatabaseType.AttributeDatabase;
            this.Connection = new CODPNETDbConnection();
            this.Tables = new TableCollection();
        }

        public static CAttributeDatabase GetInstance()
        {
            if (attributeDatabase == null)
                attributeDatabase = new CAttributeDatabase();
            return attributeDatabase;
        }
    }

    public class CBusinessDatabase : CDatabase
    {
        private static CBusinessDatabase businessDatabase;

        private CBusinessDatabase()
        {
            this.Type = EDatabaseType.BusinessDatabase;
            this.Connection = new CODPNETDbConnection();
            this.Tables = new TableCollection();
        }

        public static CBusinessDatabase GetInstance()
        {
            if (businessDatabase == null)
                businessDatabase = new CBusinessDatabase();
            return businessDatabase;
        }
    }
}
