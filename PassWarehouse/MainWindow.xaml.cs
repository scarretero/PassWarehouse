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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using PassWarehouse.View;


namespace PassWarehouse
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Controller.Controller controller;

        public MainWindow()
        {
            InitializeComponent();

            controller = new Controller.Controller();

        }

        #region events

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            controller.callWrite(this.txtPass.Password);
        }

        #endregion


    }
}
