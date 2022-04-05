using Packliste.Data;
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

namespace Packliste.Controls
{
    /// <summary>
    /// Interaktionslogik für TravelerTabItem.xaml
    /// </summary>
    public partial class TravelerTabItem : TabItem, INotifyPropertyChanged
    {


        public Traveler Traveler
        {
            get { return (Traveler)GetValue(TravelerProperty); }
            set 
            {
                SetValue(TravelerProperty, value); 
                NotifyPropertyChanged();
            }
        }

        public static readonly DependencyProperty TravelerProperty =
            DependencyProperty.Register("Traveler", typeof(Traveler), typeof(TravelerTabItem));

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TravelerTabItem()
        {
            InitializeComponent();
            this.PropertyChanged += TravelerTabItem_PropertyChanged;
        }

        private void TravelerTabItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Traveler")
            {
                Header = Traveler.Person.Name;
            }
        }
    }
}
