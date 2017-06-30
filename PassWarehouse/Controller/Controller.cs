using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PassWarehouse.Model;

namespace PassWarehouse.Controller
{
    public class Controller
    {
        private ReadWrite readWriteClass;

        public Controller()
        {
            string dataFile = Path.Combine(System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             ), "PassWarehouse\\data");

            readWriteClass = new ReadWrite(dataFile);
        }

        public void callWrite(string pMessage)
        {
            if (!readWriteClass.checkDataExists())
                readWriteClass.createDataFile();

            readWriteClass.write(pMessage);
        }
    }
}
