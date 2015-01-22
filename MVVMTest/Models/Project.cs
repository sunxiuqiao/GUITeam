using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMTest.Models
{
    public class Project
    {
        string prijectFilePath;

        public string FilePath
        {
            get { return prijectFilePath; }
            set { prijectFilePath = value; }
        }
    }
}
