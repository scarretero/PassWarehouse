using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PassWarehouse.View
{
    /// <summary>
    /// Lógica de interacción para DialogPassword.xaml
    /// </summary>
    public partial class DialogPassword : Window
    {
        public string mPass;

        public DialogPassword()
        {
            InitializeComponent();
        }

        private void btnMasterPass_Click(object sender, RoutedEventArgs e)
        {
            this.mPass = this.txtPassBox.Password;
            this.DialogResult = true;
            
        }
    }
}
