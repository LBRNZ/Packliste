using Packliste.Data;
using Packliste.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Packliste.Helper;

namespace Packliste.Pages
{
    /// <summary>
    /// Interaktionslogik für PackinglistPage.xaml
    /// </summary>
    public partial class PackinglistPage : Page, INotifyPropertyChanged
    {
        private XmlData _Data;

        public XmlData Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                NotifyPropertyChanged();
            }
        }

        private Journey _currentJourney;

        public PackinglistPage()
        {
            InitializeComponent();
            Data = (Application.Current.MainWindow as MainWindow).data;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void JourneySelector_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PersonsTabControl.Items.Clear();
            _currentJourney = ((ComboBox)sender).SelectedItem as Journey;
            if (_currentJourney != null)
            {
                foreach (Traveler traveler in _currentJourney.Travelers)
                {
                    TravelerTabItem tabItem = new TravelerTabItem()
                    {
                        Traveler = traveler
                    };
                    PersonsTabControl.Items.Add(tabItem);
                }
            }

        }

        private void SavePdf_Click(object sender, RoutedEventArgs e)
        {
            new PdfCreator(_currentJourney);
        }
    }
}
