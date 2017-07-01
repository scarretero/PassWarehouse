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
using System.Data;
using System.Collections.ObjectModel;
using PassWarehouse.Model;



namespace PassWarehouse
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Controller.Controller controller;
        private static int previousTabIndex = 0;

        public MainWindow()
        {
            InitializeComponent();

            controller = new Controller.Controller();

        }

        #region events

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            controller.callWrite(this.txtName.Text + ";" + this.txtPass.Password);
            this.txtName.Text = string.Empty;
            this.txtPass.Password = string.Empty;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabConsultar.IsSelected && tabControl.SelectedIndex != previousTabIndex)
            {
                ObservableCollection<GridDataRow> lData = controller.callRead();

                if (lData != null && lData.Count > 0)
                {
                    DataGridTextColumn columnName = new DataGridTextColumn();
                    columnName.Header = "Nombre";
                    columnName.Binding = new System.Windows.Data.Binding("nombre");
                    columnName.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);

                    DataGridTextColumn columnPass = new DataGridTextColumn();
                    columnPass.Header = "Contraseña";
                    columnPass.Binding = new System.Windows.Data.Binding("pass");
                    columnPass.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);

                    this.dataGrid.Columns.Add(columnName);
                    this.dataGrid.Columns.Add(columnPass);

                    this.dataGrid.ItemsSource = lData;
                }
            }
       
            previousTabIndex = tabControl.SelectedIndex;
        }

        #endregion

        public struct GridDataRow
        {
            public string nombre { set; get; }
            public string pass { set; get; }
        }
    }
}
