using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMTest.Models
{
    public class Project
    {
        string proijectName;
        string prijectFilePath;

        public string ProjectName
        {
            get{return proijectName;}
            set{proijectName = value;}
        }

        public string FilePath
        {
            get { return prijectFilePath; }
            set { prijectFilePath = value; }
        }
    }
}
