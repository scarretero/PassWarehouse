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
            string lPass = checkCeredentials();

            if (lPass != null && lPass != "")
            {
                int lResult = readWriteClass.writeData(pMessage, lPass);

                DialogInfo msg = lResult == 0 ? new DialogInfo(DialogInfo.TypeMsg.Info, "Contraseña guardada") : new DialogInfo(DialogInfo.TypeMsg.Error, "Se ha produdcido un erro al guardar la contraseña");

                msg.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                msg.ShowDialog();
            }
            else if (lPass != null)
            {
                DialogInfo passError = new DialogInfo(DialogInfo.TypeMsg.Error, "El password maestro introducido no es correcto");
                passError.ShowDialog();
            }
        }

        public ObservableCollection<MainWindow.GridDataRow> callRead()
        {
            ObservableCollection<MainWindow.GridDataRow> lData = null;

            readWriteClass.checkDataExists();

            string lPass = checkCeredentials();

            if (lPass != null && lPass != "")
                lData = readWriteClass.readData(lPass);
            else if (lPass != null)
            {
                DialogInfo passError = new DialogInfo(DialogInfo.TypeMsg.Error, "El password maestro introducido no es correcto");
                passError.ShowDialog();
            }

            return lData;

        }

        private string checkCeredentials()
        {
            string lMasterPass = null;

            readWriteClass.checkDataExists();

            DialogPassword passWindow = new DialogPassword();

            bool? resultDialog = passWindow.ShowDialog();

            if (resultDialog != null && resultDialog.ToString().ToUpper() == "TRUE")
            {
                lMasterPass = passWindow.mPass;

                if (!readWriteClass.passwordCheck(lMasterPass))
                    lMasterPass = "";
            }

            return lMasterPass;


        }
    }
}
