using Packliste.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using WPFUI.Controls;

namespace Packliste.Pages
{
    /// <summary>
    /// Interaktionslogik für DataPage.xaml
    /// </summary>
    public partial class DataPage : Page, INotifyPropertyChanged
    {
        private Category _selectedCategory;

        public event PropertyChangedEventHandler? PropertyChanged;

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


        private XmlData _Data;

        public XmlData Data
        {
            get { return _Data; }
            set {
                _Data = value;
                NotifyPropertyChanged();
            }
        }

        public DataPage()
        {
            Data = (Application.Current.MainWindow as MainWindow).data;
            InitializeComponent();
            
        }

        private void RefreshItemsView()
        {
            ItemsView = new CollectionViewSource { Source = Data.Items }.View;
            ItemsView.Filter = item => SelectedCategory == null || SelectedCategory == ((Item)item).Category;
        }

        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                NotifyPropertyChanged();
                RefreshItemsView();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Items_dg_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new Item(Data)
            {
                Category = SelectedCategory
            };
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var category = new Category();
            Data.Categories.Add(category);
            SelectedCategory = category;
        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.RemoveCategory(SelectedCategory);
            }
            catch (Exception ex)
            {
                Snackbar rootSnackbar = (((MainWindow)Application.Current.MainWindow)!).RootSnackbar;
                rootSnackbar.Message = ex.Message;
                rootSnackbar.Title = "Fehler";
                rootSnackbar.Icon = WPFUI.Common.Icon.ErrorCircle24;

                rootSnackbar.Expand();
            }
           
        }

        private void Items_dg_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGrid)sender;
            if (Key.Delete == e.Key)
            {
                try
                {
                    Data.RemoveItem(grid.Items[grid.SelectedIndex] as Item);
                }
                catch (Exception ex)
                {

                    Snackbar rootSnackbar = (((MainWindow)Application.Current.MainWindow)!).RootSnackbar;
                    rootSnackbar.Message = ex.Message;
                    rootSnackbar.Title = "Fehler";
                    rootSnackbar.Icon = WPFUI.Common.Icon.ErrorCircle24;

                    rootSnackbar.Expand();
                } finally
                {
                    e.Handled = true;
                }
            }
        }
    }
}
