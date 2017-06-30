using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PassWarehouse.Model
{
 
    class ReadWrite
    {
        private string mDataFile;

        public ReadWrite(string pDataFile)
        {
            this.mDataFile = pDataFile;
        }

        public bool checkDataExists()
        {
            try
            {
                return File.Exists(this.mDataFile);
            }
            catch (System.IO.IOException ex)
            {
                return false;
            }
        }

        public void write(string pTextToWrite)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(mDataFile, true))
                {
                    file.WriteLine(pTextToWrite);
                }
            }
            catch (System.IO.IOException ex)
            {

            }
           
        }

        public void createDataFile()
        {
            try
            {
                if(!Directory.Exists(mDataFile.Substring(0, mDataFile.LastIndexOf("\\"))));
                    Directory.CreateDirectory(mDataFile.Substring(0, mDataFile.LastIndexOf("\\")));

                File.Create(this.mDataFile);
            }
            catch (System.IO.IOException ex)
            {

            }
            
        }
    }
}
