using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    public class ModifyDKBMVM : MVVMBase.ObservableObject
    {
        private string dkbm;
        private static bool isShow = false;

        public string DKBM
        {
            get { return dkbm; }
            set 
            { 
                dkbm = value;
                RaisePropertyChanged("DKBM");
            }
        }

        public bool IsShow
        {
            get { return isShow; }
            set 
            {
                isShow = value;
                RaisePropertyChanged("IsShow");
            }
        }

        private void ModifyFinished_Executed()
        {
            try
            {
                IMap map = ControlsVM.MapControl().Map;
                IEnumFeature features = map.FeatureSelection as IEnumFeature;
                features.Reset();
                IFeature feature = features.Next();
                IFields fields = feature.Fields;
                int index = fields.FindField("DKBM");
                if (index == -1)
                {
                    System.Windows.MessageBox.Show("字段名错误！");
                    return;
                }
                //object value = feature.get_Value(index);
                //DKBM = value.ToString();
                int number = 0;
               
                if (string.IsNullOrEmpty(DKBM))
                {
                    number = 0;
                }
                else
                {
                    try
                    {
                        number = int.Parse(DKBM);
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show("输入的应该是数字：123");
                    }
                }
                feature.set_Value(index, number.ToString());
                feature.Store();
                System.Windows.MessageBox.Show("修改成功！");
                IsShow = false;
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        private bool ModifyFinished_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand ModifyFinishedCommand { get { return new MVVMBase.RelayCommand(ModifyFinished_Executed, ModifyFinished_CanExecute); } }

        private void ModifyCancel_Executed()
        {
            DKBM = null;
            IsShow = false;
        }
        private bool ModifyCancel_CanExecute()
        {
            return true;
        }
        public System.Windows.Input.ICommand ModifyCancelCommand { get { return new MVVMBase.RelayCommand(ModifyCancel_Executed, ModifyCancel_CanExecute); } }

    }
}
