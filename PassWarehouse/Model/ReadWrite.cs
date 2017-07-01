using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PassWarehouse.Model;
using System.Collections.ObjectModel;

namespace PassWarehouse.Model
{

    class ReadWrite
    {
        private string mDataFile;
        private Cryptography CriptoClass;

        public ReadWrite(string pDataFile)
        {
            this.mDataFile = pDataFile;
            CriptoClass = new Cryptography();
        }

        public void checkDataExists()
        {
            try
            {
                if (!File.Exists(mDataFile))
                {
                    File.Create(mDataFile);
                }
            }
            catch (System.IO.IOException ex)
            {
            }
        }

        public int writeData(string pTextToWrite, string pPass)
        {
            int result = 0;

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(mDataFile, true))
                {
                    file.WriteLine(CriptoClass.Encrypt(pTextToWrite, pPass));
                }
            }
            catch (System.IO.IOException ex)
            {
                result = 1;
            }

            return result;
        }

        public bool passwordCheck(string pPass)
        {
            try
            {
                using (System.IO.StreamReader file = new StreamReader(mDataFile))
                {
                    return CriptoClass.Decrypt(file.ReadLine(), pPass) == pPass;
                }
            }
            catch (System.IO.IOException ex)
            {
                return false;
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                return false;
            }
        }

        public void createDataFile()
        {
            try
            {
                if (!Directory.Exists(mDataFile.Substring(0, mDataFile.LastIndexOf("\\"))))
                    Directory.CreateDirectory(mDataFile.Substring(0, mDataFile.LastIndexOf("\\")));

                using (var file = File.Create(this.mDataFile)) ;

            }
            catch (System.IO.IOException ex)
            {

            }

        }

        public bool isDataEmpty()
        {
            try
            {
                using (System.IO.StreamReader file = new StreamReader(mDataFile))
                {
                    return file.EndOfStream;
                }

            }
            catch (System.IO.IOException ex)
            {
                return true;
            }
        }

        public ObservableCollection<MainWindow.GridDataRow> readData(string pPass)
        {
            ObservableCollection<MainWindow.GridDataRow> data = new ObservableCollection<MainWindow.GridDataRow>();

            try
            {
                using (System.IO.StreamReader file = new StreamReader(mDataFile))
                {
                    string lHeader = file.ReadLine();
                    string lLine = file.ReadLine();

                    while (lLine != null)
                    {
                        lLine = CriptoClass.Decrypt(lLine, pPass);

                        data.Add(new MainWindow.GridDataRow { nombre = lLine.Split(';')[0], pass = lLine.Split(';')[1] });

                        lLine = file.ReadLine();
                    }
                }
            }
            catch (System.IO.IOException)
            {

            }

            return data;
        }
    }
}
