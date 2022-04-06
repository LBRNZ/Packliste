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

        private ICollectionView _ItemsView;
        public ICollectionView ItemsView
        {
            get { return _ItemsView; }
            set
            {
                _ItemsView = value;
                NotifyPropertyChanged();
            }
        }
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
            var Data = (Application.Current.MainWindow as MainWindow).data;
            ItemsView = new CollectionViewSource { Source = Data.Items }.View;
            ItemsView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
        }

        private void TravelerTabItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Traveler")
            {
                Header = Traveler.Person.Name;
            }
        }

        private void Resources_dg_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item selectedItem = ((DataGridRow)sender).Item as Item;
            Traveler.itemSets.Add(new ItemSet()
            {
                Item = selectedItem,
                Count = 1
            });
        }

        private void DeleteItemSet_Click(object sender, RoutedEventArgs e)
        {
            ItemSet selectedItemSet =  GetAncestorOfType<ListBoxItem>(sender as Button).Content as ItemSet;
            Traveler.itemSets.Remove(selectedItemSet);
        }

        public T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }
    }
}
