using GNalogRuSharp.GUI.ViewModels;
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

namespace GNalogRuSharp.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabItem = tabControl.SelectedItem as TabItem;
            if (tabItem != null)
            {
                switch (tabItem.Header)
                {
                    case "ИНН":
                        Title = "Получение ИНН по паспортным данным";
                        break;
                    case "Статус самозанятого":
                        Title = "Получение статуса самозанятого";
                        break;
                    default:
                        Title = "ФНС";
                        break;
                }
            }
            else
            {
                Title = "ФНС";
            }
        }
    }
}
