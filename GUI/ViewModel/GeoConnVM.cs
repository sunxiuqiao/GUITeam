using GUI.MVVMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDatabase;

namespace GUI.ViewModel
{
    class GeoConnVM : ConnBaseVM
    {
        private bool isCanSave = false;

        public bool IsCanSave
        {
            get { return isCanSave; }
            set { isCanSave = value; }
        }


        private bool ConnTestCommand_CanExecute()
        {
            return true;
        }
        private void ConnTestCommand_Executed()
        {
            CreateDatabase.IDatabase SpatialDatabase = CreateDatabase.CSpatialDatabase.GetInstance();
            SpatialDatabase.Name = Name;
            SpatialDatabase.Password = PassWord;
            if (string.IsNullOrEmpty(PortNumber))
            {
                SpatialDatabase.PortNumber = 0;
            }
            else
            {
                SpatialDatabase.PortNumber = int.Parse(PortNumber);
            }
            SpatialDatabase.Server = Server;
            SpatialDatabase.ServiceName = ServiceName;
            SpatialDatabase.User = User;
            SpatialDatabase.Version = "SDE.DEFAULT";

            IsCanSave = SpatialDatabase.Open();
            if (IsCanSave)
                System.Windows.MessageBox.Show("connect is ok");
            //System.Windows.MessageBox.Show(string.Format("Server:{0},ServiceName:{1},User:{2},PortNumber:{3},PassName:{4}", SpatialDatabase.Server, SpatialDatabase.ServiceName, SpatialDatabase.User, SpatialDatabase.PortNumber, SpatialDatabase.Password));
        }
        public System.Windows.Input.ICommand ConnTestCommand { get { return new RelayCommand(ConnTestCommand_Executed, ConnTestCommand_CanExecute); } }


        private bool ConnSaveCommand_CanExecute()
        {
            return IsCanSave;
        }
        private void ConnSaveCommand_Executed()
        {

        }
        public System.Windows.Input.ICommand ConnSaveCommand { get { return new RelayCommand(ConnSaveCommand_Executed, ConnSaveCommand_CanExecute); } }

    }
}
