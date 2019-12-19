using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2
{
    public class Player : INotifyPropertyChanged
    {
        private char[] move = new char[9];

        public Player()
        {
            for (var i = 0; i < 9; ++i)
            {
                move[i] = ' ';
            }
        }

        public char Move1
        {
            get
                => move[0];
            set
            {
                move[0] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move1)));
            }
        }

        public char Move2
        {
            get
                => move[1];
            set
            {
                move[1] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move2)));
            }
        }

        public char Move3
        {
            get
                => move[2];
            set
            {
                move[2] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move3)));
            }
        }

        public char Move4
        {
            get
                => move[3];
            set
            {
                move[3] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move4)));
            }
        }

        public char Move5
        {
            get
                => move[4];
            set
            {
                move[4] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move5)));
            }
        }

        public char Move6
        {
            get
                => move[5];
            set
            {
                move[5] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move6)));
            }
        }

        public char Move7
        {
            get
                => move[6];
            set
            {
                move[6] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move7)));
            }
        }

        public char Move8
        {
            get
                => move[7];
            set
            {
                move[7] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move8)));
            }
        }

        public char Move9
        {
            get
                => move[8];
            set
            {
                move[8] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Move9)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
