﻿using System;
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

namespace Packliste.Pages
{
    /// <summary>
    /// Interaktionslogik für DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        public DataPage()
        {
            InitializeComponent();
            var data = (Application.Current.MainWindow as MainWindow).data;
            Items_dg.ItemsSource = data.Items;
            Persons_dg.ItemsSource = data.Persons;
            Journeys_dg.ItemsSource = data.Journeys;
        }
    }
}
