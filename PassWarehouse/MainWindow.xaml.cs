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
using System.IO;




namespace PassWarehouse
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Controller.Controller controller;
        private static int previousTabIndex = 0;
        private static ObservableCollection<GridDataRow> mData;

        public MainWindow()
        {
            InitializeComponent();

            controller = new Controller.Controller();
            mData = new ObservableCollection<GridDataRow>();

        }

        #region events

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                controller.callWrite(this.txtName.Text + ";" + this.txtPass.Password);
                this.txtName.Text = string.Empty;
                this.txtPass.Password = string.Empty;
            }
            catch (Exception ex)
            {
                controller.callLogger("Método btnSave_Click() de la clase MainWindow => " + ex.ToString() + "__" + DateTime.Now);
            }

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tabConsultar.IsSelected && tabControl.SelectedIndex != previousTabIndex)
                {
                    this.dataGrid.ItemsSource = null;

                    mData = controller.callRead();

                    if (mData != null && mData.Count > 0)
                    {
                        if (this.dataGrid.Columns.Count == 0)
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
                        }

                        this.dataGrid.ItemsSource = mData;
                    }
                }
            }
            catch (Exception ex)
            {
                controller.callLogger("Método TabControl_SelectionChanged() de la clase MainWindow => " + ex.ToString() + "__" + DateTime.Now);
            }

            previousTabIndex = tabControl.SelectedIndex;
        }

        private void MenuBorrar_Click(object sender, RoutedEventArgs e)
        {
            controller.callRemoveData();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.textBoxSearch.Text != "")
                this.dataGrid.ItemsSource = mData.Where(x => x.nombre.Contains(this.textBoxSearch.Text));
            else
                this.dataGrid.ItemsSource = mData;
        }

        private void MenuConfigDataFile_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath;

                    controller.callEditConfig("dataPath", path + "\\data");
                }
            }
        }

        private void txtPass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btSave_Click(null, null);
            }
        }

        #endregion

        public struct GridDataRow
        {
            public string nombre { set; get; }
            public string pass { set; get; }
        }

        
    }
}
