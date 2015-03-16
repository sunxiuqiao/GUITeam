using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace CreateDatabase
{
    public abstract class CConnection :IConnection
    {
        public abstract bool Open(IDatabase database);
        public abstract void Close(IDatabase database);
    }

    public class CODPNETDbConnection : CConnection
    {
        private OracleConnection conn = null;

        public OracleConnection Conn
        {
            get { return conn; }
        }

        public override bool Open(IDatabase database)
        {
            try
            {
                string connectionStr = string.Format(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))
                                (CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4}", database.Server, database.PortNumber, database.ServiceName, database.User, database.Password);
                conn = new OracleConnection(connectionStr);
                conn.Open();
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
            }
        }

        public override void Close(IDatabase database)
        {
            conn.Close();
        }

       
    }

    public class CSparialDbConnection : CConnection
    {
        public override bool Open(IDatabase database)
        {
            IPropertySet propertySet = new PropertySetClass();
            propertySet.SetProperty("SERVER", database.Server);
            propertySet.SetProperty("INSTANCE", database.ServiceName);
            propertySet.SetProperty("USER",database.User);
            propertySet.SetProperty("PASSWORD", database.Password);
            propertySet.SetProperty("VERSION", database.Version);
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.SdeWorkspaceFactory");
            try
            {
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                database.Workspace = workspaceFactory.Open(propertySet, 0);
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
            }
        }

        public override void Close(IDatabase database)
        {

        }
    }
}
