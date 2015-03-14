using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDatabase
{
    class CTable : ITable
    {
        string name;
        string aliasName;
        IDatabase database;
        ICreation creation;
        ColumnsCollection columns;

        public string AliasName
        {
            get { return aliasName; }
            set { aliasName = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public IDatabase Database
        {
            get { return database; }
            set { database = value; }
        }
        public ICreation Creation
        {
            get { return creation; }
            set { creation = value; }
        }
        public ColumnsCollection Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public void Create()
        {
            creation.Create(this);
        }
    }
}
