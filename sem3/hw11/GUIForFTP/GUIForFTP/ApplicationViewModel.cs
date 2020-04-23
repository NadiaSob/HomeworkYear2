using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GUIForFTP
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string hostname;
        private int port;

        /// <summary>
        /// Server address.
        /// </summary>
        public string Address
        {
            get
                => hostname;
            set
                => hostname = value;
        }

        /// <summary>
        /// Server port.
        /// </summary>
        public string Port
        {
            get
                => port.ToString();
            set
                => port = Convert.ToInt32(value);
        }

        public ApplicationViewModel(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
