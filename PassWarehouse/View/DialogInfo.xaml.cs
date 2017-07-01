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
using System.IO;
using System.Drawing;

namespace PassWarehouse.View
{
    /// <summary>
    /// Lógica de interacción para DialogInfo.xaml
    /// </summary>
    public partial class DialogInfo : Window
    {
        public DialogInfo(TypeMsg pType, string pMsg)
        {
            InitializeComponent();
            this.txtMsg.Document.Blocks.Add(new Paragraph(new Run(pMsg)));

            switch (pType)
            {
                case TypeMsg.Error:
                    this.imgBox.Source = Convert(Properties.Resources.errorV2);
                    this.imgBox.Stretch = Stretch.Uniform;
                    break;
                case TypeMsg.Info:
                    this.imgBox.Source = Convert(Properties.Resources.infoV2);
                    this.imgBox.Stretch = Stretch.Uniform;
                    break;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public enum TypeMsg
        {
            Error,
            Info
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
