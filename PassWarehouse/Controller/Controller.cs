using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PassWarehouse.Model;
using PassWarehouse.View;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace PassWarehouse.Controller
{
    public class Controller
    {
        private ReadWrite readWriteClass;
        public static string mLogFile;
        private static string mPass = "";

        public Controller()
        {
            string configFile = Path.Combine(System.Environment.
                            GetFolderPath(
                                Environment.SpecialFolder.CommonApplicationData
                            ), "PassWarehouse\\config");

            readWriteClass = new ReadWrite(configFile);

            string dataFile = readWriteClass.getDataPathSetting();

            readWriteClass.setDataFile(dataFile == "" ? Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PassWarehouse\\data") : dataFile);


            mLogFile = Path.Combine(System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             ), "PassWarehouse\\log");
        }

        public void callWrite(string pMessage)
        {
            try
            {
                if (mPass == "" || mPass == null)
                {
                    mPass = checkCeredentials();
                }

                if (mPass != null && mPass != "")
                {
                    int lResult = readWriteClass.writeData(pMessage, mPass);

                    DialogInfo msg = lResult == 0 ? new DialogInfo(DialogInfo.TypeMsg.Info, "Contraseña guardada") : new DialogInfo(DialogInfo.TypeMsg.Error, "Se ha produdcido un erro al guardar la contraseña");

                    msg.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    msg.ShowDialog();
                }
                else if (mPass != null)
                {
                    DialogInfo passError = new DialogInfo(DialogInfo.TypeMsg.Error, "El password maestro introducido no es correcto");
                    passError.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                callLogger("Método callWrite() de la clase Controller => " + ex.ToString() + "__" + DateTime.Now);
            }


        }

        public ObservableCollection<MainWindow.GridDataRow> callRead()
        {
            ObservableCollection<MainWindow.GridDataRow> lData = null;

            try
            {
                readWriteClass.checkDataExists();

                if (mPass == "" || mPass == null)
                {
                    mPass = checkCeredentials();
                }

                if (mPass != null && mPass != "")
                    lData = readWriteClass.readData(mPass);
                else if (mPass != null)
                {
                    DialogInfo passError = new DialogInfo(DialogInfo.TypeMsg.Error, "El password maestro introducido no es correcto");
                    passError.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                callLogger("Método callRead() de la clase Controller => " + ex.ToString() + "__" + DateTime.Now);
                lData = null;
            }

            return lData;

        }

        private string checkCeredentials()
        {
            string lMasterPass = null;

            try
            {          
                bool dataExist = readWriteClass.checkDataExists();

                if (!dataExist || (mPass == null || mPass == ""))
                {
                    DialogPassword passWindow = new DialogPassword();
                    bool? resultDialog = passWindow.ShowDialog();

                    if (resultDialog != null && resultDialog.ToString().ToUpper() == "TRUE")
                    {
                        lMasterPass = passWindow.mPass;

                        if (readWriteClass.passwordCheck(lMasterPass))
                            return lMasterPass;
                    }
                }
                else
                    lMasterPass = mPass;


            }
            catch (Exception ex)
            {
                callLogger("Método checkCredentials() de la clase Controller => " + ex.ToString() + "__" + DateTime.Now);
                lMasterPass = null;
            }

            return lMasterPass;
        }

        public void callRemoveData()
        {
            try
            {
                DialogQuestion msg = new DialogQuestion("Se va a borrar toda la información almacenada, quiere continuar ?");

                bool? lResult = msg.ShowDialog();

                if (lResult != null && lResult.ToString().ToUpper() == "TRUE")
                {
                    string lPass = checkCeredentials();

                    if (lPass != null && lPass != "")
                    {
                        bool lRemoveResult = readWriteClass.removeData();
                        DialogInfo resultMsg = new DialogInfo(lRemoveResult ? DialogInfo.TypeMsg.Info : DialogInfo.TypeMsg.Error, lRemoveResult ? "Los datos han sido borrados" : "No se han podido borrar los datos");
                        resultMsg.ShowDialog();
                        mPass = "";
                    }
                    else if (lPass != null)
                    {
                        DialogInfo passError = new DialogInfo(DialogInfo.TypeMsg.Error, "El password maestro introducido no es correcto");
                        passError.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                callLogger("Método callRemoveData() de la clase Controller => " + ex.ToString() + "__" + DateTime.Now);
            }
        }

        public void callEditConfig(string pName, string pValue)
        {
            readWriteClass.checkConfigExists();
            readWriteClass.editConfigFile(pName, pValue);
            readWriteClass.setDataFile(pValue);
        }

        public void callLogger(string pText)
        {
            readWriteClass.Logger(pText, mLogFile);
        }
    }
}
