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
    /// Lógica de interacción para DialogQuestion.xaml
    /// </summary>
    public partial class DialogQuestion : Window
    {
        public DialogQuestion(string pMsg)
        {
            InitializeComponent();
            this.txtMsg.Document.Blocks.Add(new Paragraph(new Run(pMsg)));
            this.imgBox.Source = DialogInfo.Convert(Properties.Resources.preguntaIcon);
            this.imgBox.Stretch = Stretch.Uniform;
        }

        private void buttonAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
