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
using System.Windows.Shapes;
using MahApps.Metro.SimpleChildWindow;

namespace moasreport
{
    public partial class DeputyChildWindow : ChildWindow
    {
        public event Action<List<string>> DeputyInformation;
        public DeputyChildWindow() {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: save data and send to main window, then close
            List<string> addrList = new List<string> {
                depSCA.Text,
                depModern.Text,
                depEmail.Text
            };
            DeputyInformation?.Invoke(addrList);
            Close();
        }
    }
}