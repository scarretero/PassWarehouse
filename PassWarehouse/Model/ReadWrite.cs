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
        private string mConfigFile;
        private Cryptography CriptoClass;

        public ReadWrite(string pConfigFile)
        {
            this.mConfigFile = pConfigFile;
            CriptoClass = new Cryptography();
        }

        public bool checkDataExists()
        {
            bool result = false;

            try
            {
                if (!File.Exists(mDataFile))
                    File.Create(mDataFile);
                else
                    result = true;
            }
            catch (System.IO.IOException ex)
            {
                Logger("Método checkDataExists() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
            }

            return result;
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
                Logger("Método writeData() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
            }

            return result;
        }

        public bool passwordCheck(string pPass)
        {
            try
            {
                bool lEmptyFile = isDataEmpty();

                if (lEmptyFile)
                {
                    writeData(pPass, pPass);
                    return true;
                }
                else
                {
                    using (System.IO.StreamReader file = new StreamReader(mDataFile))
                    {
                        return CriptoClass.Decrypt(file.ReadLine(), pPass) == pPass;
                    }
                }
            }
            catch (System.IO.IOException ex)
            {
                Logger("Método passWordCheck() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
                return false;
            }
            catch (System.Security.Cryptography.CryptographicException ex)
            {
                Logger("Método passWordCheck() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
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
                Logger("Método createDataFile() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
            }

        }

        public string getDataPathSetting()
        {
            string lPath = "";

            try
            {
                if (File.Exists(mConfigFile))
                {
                    using (System.IO.StreamReader file = new StreamReader(mConfigFile))
                    {
                        string lLine = file.ReadLine();

                        while (lLine != null)
                        {
                            if (lLine.Split(';')[0] == "dataPath")
                            {
                                lPath = lLine.Split(';')[1];
                                break;
                            }
                        }
                    }
                }
                else
                    lPath = "";

            }
            catch (Exception ex)
            {
                Logger("Método getDataPathSettinge() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
            }

            return lPath;
        }

        public void editConfigFile(string pName, string pValue)
        {
            try
            {
                string[] lLines = File.ReadAllLines(mConfigFile);

                bool optionExists = false;

                for (int i = 0; i < lLines.Length; i++)
                {
                    if (lLines[i].Split(';')[0] == pName)
                    {
                        lLines[i] = pName + ";" + pValue;
                        optionExists = true;
                        break;
                    }
                }

                if (!optionExists)
                {
                    List<string> lList = lLines.ToList();
                    lList.Add(pName + ";" + pValue);
                    lLines = lList.ToArray();
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(mConfigFile, false))
                {
                    foreach (string setting in lLines)
                    {
                        file.WriteLine(setting);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger("Método editConfigFile() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
            }
        }

        public void checkConfigExists()
        {
            try
            {
                if (!File.Exists(mConfigFile))
                    using (var file = File.Create(this.mConfigFile));
            }
            catch (Exception ex)
            {
                Logger("Método checkConfigExists() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
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
                Logger("Método isDataEmpty() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
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
            catch (System.IO.IOException ex)
            {
                Logger("Método readData() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
                data = null;
            }

            return data;
        }

        public bool removeData()
        {
            bool result = true;

            try
            {
                File.Delete(mDataFile);
            }
            catch (System.IO.IOException ex)
            {
                Logger("Método removeData() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, Controller.Controller.mLogFile);
                result = false;
            }

            return result;
        }

        public void Logger(string pText, string pLogFile)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pLogFile, true))
                {
                    file.WriteLine(pText);
                }
            }
            catch (System.IO.IOException ex)
            {
                Logger("Método Logger() de la clase ReadWrite => " + ex.ToString() + "__" + DateTime.Now, pLogFile);
            }


        }

        public string getDataFile()
        {
            return this.mDataFile;
        }

        public void setDataFile(string pPath)
        {
            this.mDataFile = pPath;
        }
    }
}
