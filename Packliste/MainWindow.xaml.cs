using Packliste.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Packliste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public XmlData data { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            data = new XmlData().Load();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            data.Save();
        }
    }
}
