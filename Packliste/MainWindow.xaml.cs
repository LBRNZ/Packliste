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
            //var testData = CreateSampleData();
            //testData.Save();

            data = new XmlData().Load();
        }

        public XmlData CreateSampleData()
        {
            var data = new XmlData();

            var item = new Item (data) { Name = "Teller" };

            var person = new Person (data) { Name = "Jens" };
            data.Persons.Add(person);

            var itemSet = new ItemSet { Item = item, Count = 3 };
            var traveler = new Traveler(person);
            traveler.itemSets.Add(itemSet);
            var journey = new Journey (data)
            {
                Destination = "Harz",
                StartDate = new DateOnly(2022, 02, 21),
                EndDate = new DateOnly(2022, 03, 05)
            };
            journey.Travelers.Add(traveler);
            data.Journeys.Add(journey);

            data.Items.Add(item);

            return data;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            data.Save();
        }
    }
}
