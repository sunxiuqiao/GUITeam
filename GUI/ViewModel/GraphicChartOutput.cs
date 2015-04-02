using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    public class GraphicChartOutput : MVVMBase.ObservableObject
    {
        private void BatchOutput_Executed()
        {

            string path = System.Windows.Forms.Application.StartupPath + @"\GraphicOutput/ExportFigures.exe";
            System.Diagnostics.Process.Start(path); 
        }

        private bool BatchOutput_CanExecute()
        {
            return true;
        }

        public System.Windows.Input.ICommand BatchOutputCommand { get { return new MVVMBase.RelayCommand(BatchOutput_Executed, BatchOutput_CanExecute); } }
        
    }
}
