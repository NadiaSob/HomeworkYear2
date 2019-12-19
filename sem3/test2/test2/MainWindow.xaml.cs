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

namespace test2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool turn;

        private Player player = new Player();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = player;
            turn = true;

            Label.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move1 = 'X';
            }
            else
            {
                player.Move1 = '0';
            }
            GameLogic();
            turn = !turn;
            Button1.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move2 = 'X';
            }
            else
            {
                player.Move2 = '0';
            }
            GameLogic();
            turn = !turn;
            Button2.IsEnabled = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move3 = 'X';
            }
            else
            {
                player.Move3 = '0';
            }
            GameLogic();
            turn = !turn;
            Button3.IsEnabled = false;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move4 = 'X';
            }
            else
            {
                player.Move4 = '0';
            }
            GameLogic();
            turn = !turn;
            Button4.IsEnabled = false;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move5 = 'X';
            }
            else
            {
                player.Move5 = '0';
            }
            GameLogic();
            turn = !turn;
            Button5.IsEnabled = false;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move6 = 'X';
            }
            else
            {
                player.Move6 = '0';
            }
            GameLogic();
            turn = !turn;
            Button6.IsEnabled = false;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move7 = 'X';
            }
            else
            {
                player.Move7 = '0';
            }
            GameLogic();
            turn = !turn;
            Button7.IsEnabled = false;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move8 = 'X';
            }
            else
            {
                player.Move8 = '0';
            }
            GameLogic();
            turn = !turn;
            Button8.IsEnabled = false;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (turn)
            {
                player.Move9 = 'X';
            }
            else
            {
                player.Move9 = '0';
            }
            GameLogic();
            turn = !turn;
            Button9.IsEnabled = false;
        }

        private void GameLogic()
        {
            if ((Equals(Button1.Content, Button5.Content) && Equals(Button5.Content, Button9.Content) && !Equals(Button1.Content, ' '))
                || (Equals(Button3.Content, Button5.Content) && Equals(Button5.Content, Button7.Content) && !Equals(Button7.Content, ' '))
                || (Equals(Button1.Content, Button2.Content) && Equals(Button2.Content, Button3.Content) && !Equals(Button1.Content, ' '))
                || (Equals(Button1.Content, Button4.Content) && Equals(Button4.Content, Button7.Content) && !Equals(Button1.Content, ' '))
                || (Equals(Button4.Content, Button5.Content) && Equals(Button5.Content, Button6.Content) && !Equals(Button6.Content, ' '))
                || (Equals(Button7.Content, Button8.Content) && Equals(Button8.Content, Button9.Content) && !Equals(Button9.Content, ' '))
                || (Equals(Button2.Content, Button5.Content) && Equals(Button5.Content, Button8.Content) && !Equals(Button8.Content, ' '))
                || (Equals(Button3.Content, Button6.Content) && Equals(Button6.Content, Button9.Content) && !Equals(Button9.Content, ' '))
                || (!Button1.IsEnabled && !Button2.IsEnabled && !Button3.IsEnabled && !Button4.IsEnabled && !Button5.IsEnabled && !Button6.IsEnabled
                && !Button7.IsEnabled && !Button8.IsEnabled && !Button9.IsEnabled)
                )
            {
                Label.Visibility = Visibility.Visible;
                PlayButton.Visibility = Visibility.Visible;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Label.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Hidden;
            player.Move1 = ' ';
            player.Move2 = ' ';
            player.Move3 = ' ';
            player.Move4 = ' ';
            player.Move5 = ' ';
            player.Move6 = ' ';
            player.Move7 = ' ';
            player.Move8 = ' ';
            player.Move9 = ' ';

            Button1.IsEnabled = true;
            Button2.IsEnabled = true;
            Button3.IsEnabled = true;
            Button4.IsEnabled = true;
            Button5.IsEnabled = true;
            Button6.IsEnabled = true;
            Button7.IsEnabled = true;
            Button8.IsEnabled = true;
            Button9.IsEnabled = true;
        }
    }
}
