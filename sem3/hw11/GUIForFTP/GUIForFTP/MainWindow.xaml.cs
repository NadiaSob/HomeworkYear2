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

namespace GUIForFTP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationViewModel viewModel;

        public MainWindow()
        {
            viewModel = new ApplicationViewModel("localhost", 1234);
            DataContext = viewModel;
            InitializeComponent();
        }

        private async void HandleServerDoubleClick(object sender, RoutedEventArgs e) =>
            await viewModel.OpenOrDownloadServerItem((sender as ListBoxItem).Content.ToString());

        private void HandleClientDoubleClick(object sender, RoutedEventArgs e) =>
            viewModel.OpenClientFolder((sender as ListBoxItem).Content.ToString());
    }
}
