using MahApps.Metro.SimpleChildWindow;
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

namespace moasreport
{
    
    public partial class BaronialMoASChildWindow : ChildWindow
    {
        public event Action<List<string>> MoASInformation;

        public BaronialMoASChildWindow() {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e) {
            // TODO: save data and send to main window, then close
            List<string> moasList = new List<string> {
                baronialMoASName.Text,
                baronialMoASEmail.Text
            };
            MoASInformation?.Invoke(moasList);
            Close();
        }
    }
}
