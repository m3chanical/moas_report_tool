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
    public partial class ContactChildWindow : ChildWindow
    {
        public event Action<List<string>> MoASContactInformation;
        public ContactChildWindow() 
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e) 
        {
            // TODO: save data and send to main window, then close
            List<string> moasList = new List<string> {
                moasStreet.Text,
                moasStreetTwo.Text,
                moasCity.Text,
                moasState.Text,
                moasZip.Text,
                moasArea.Text,
                moasPhone.Text
            };
            MoASContactInformation?.Invoke(moasList);
            Close();
        }
    }
}
