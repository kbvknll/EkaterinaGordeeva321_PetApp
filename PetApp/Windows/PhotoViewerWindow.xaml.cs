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

namespace PetApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для PhotoViewerWindow.xaml
    /// </summary>
    public partial class PhotoViewerWindow : Window
    {
        public PhotoViewerWindow(BitmapImage image, string description)
        {
            InitializeComponent();
            PhotoImage.Source = image;
            DescriptionTextBlock.Text = description;
        }
    }
}
