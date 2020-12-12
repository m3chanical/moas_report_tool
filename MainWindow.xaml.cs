using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;

namespace moasreport
{
    public partial class MainWindow : MetroWindow
    {
        private MoasSettings _moasSettingsExport = new MoasSettings();
        private MoasSettings _moasSettingsImport = new MoasSettings();


        private readonly string[] _groupsStrings = 
        {
            "Abhainn Iarthair",
            "Berley Cort",
            "Black Diamond",
            "Bright Hills",
            "Caer Gelynniog",
            "Caer Mear",
            "Dun Carraig",
            "Greenlion Bay",
            "Highland Foorde",
            "Isenfir",
            "Lochmere",
            "Marinus",
            "Ponte Alto",
            "Rencester",
            "Rivers Point",
            "Roxbury Mill",
            "Seven Hills",
            "Spiaggia Levantina",
            "Stierbach",
            "Storvik",
            "Sudentorre",
            "Tir-y-Don",
            "Yarnvid"
        };

        private readonly string[] _quarterStrings =
        {
            "1st Quarter (Jan - Mar)",
            "2nd Quarter (Apr - Jun)",
            "3rd Quarter (Jul - Sep)",
            "4th Quarter (Oct - Dec)"
        };

        private readonly string[] _positionStrings =
        {
            "Baronial MoAS",
            "Canton MoAS",
            "Shire MoAS",
            "College MoAS",
            "Branch Seneschal",
            "Deputy MoAS",
            "Other"
        };

        private string _deputyName;
        private string _deputySca;
        private string _deputyEmail;

        private string _moasStreet;
        private string _moasStreetTwo;
        private string _moasCity;
        private string _moasState;
        private string _moasZip;
        private string _moasArea;
        private string _moasPhone;
        private string _moasCountry = "United States";

        private string _baronialMoasName;
        private string _baronialMoasEmail;


        public MainWindow() {
            InitializeComponent();

            // check is valid email: https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address

            // TODO: Add more tooltips, e.g. "If you are a seneschal..."

            InitializeData();
        }

        private void InitializeData()
        {
            ReportYear.Text = DateTime.Now.Year.ToString();
            ExpirationDate.SelectedDate = DateTime.Today; // TODO: check if expiration date is Today

            CmbGroup.ItemsSource = _groupsStrings;
            CmbGroup.SelectedIndex = 10;

            CmbQuarter.ItemsSource = _quarterStrings;
            CmbQuarter.SelectedIndex = ((DateTime.Now.Month + 2) / 3) - 1;

            CmbPosition.ItemsSource = _positionStrings;
            CmbPosition.SelectedIndex = 0;

            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            Stream importStream = File.Exists(currentDir + "default_settings.json") ? File.OpenRead(currentDir + "default_settings.json") : null;
            ImportFile(importStream);
        }

        private void btnImport_Click(object sender, RoutedEventArgs e) 
        {
            Stream importStream;
            OpenFileDialog openFileDialog = new OpenFileDialog {
                DefaultExt = "*.json",
                FileName = "settings.json",
                Filter = "json Files|*.json|All Files|*.*",
                Multiselect = true,
                Title = "Open json to import"
            };

            if (openFileDialog.ShowDialog() == true) {
                //do stuff
                if ((importStream = openFileDialog.OpenFile()) != null) {
                    ImportFile(importStream);
                }
            }
        }

        private async void ImportFile(Stream importStream) {            
            if (importStream == null) return;
            string import;

            using (StreamReader file = new StreamReader(importStream)) {            
                import = await file.ReadToEndAsync();                             
            }
            try {
                _moasSettingsImport = JsonConvert.DeserializeObject<MoasSettings>(import);
                importStream.Close();
                ApplySettings();
            } catch {
                MessageBox.Show("Invalid Json Input", "Error");
            }                  
        }

        private void ApplySettings() {
            #region General
            CmbGroup.SelectedIndex = Enumerable.Range(0, _groupsStrings.Length).Contains(_moasSettingsImport.GroupName) ?  _moasSettingsImport.GroupName : 0; //
            CmbQuarter.SelectedIndex = Enumerable.Range(0, _quarterStrings.Length).Contains(_moasSettingsImport.Quarter) ? _moasSettingsImport.Quarter : ((DateTime.Now.Month + 2) / 3) - 1;
            ReportYear.Text = _moasSettingsImport.Year.ToString();
            CmbPosition.SelectedIndex = Enumerable.Range(0, _positionStrings.Length).Contains(_moasSettingsImport.Position) ? _moasSettingsImport.Position : 0;
            ScaName.Text = (_moasSettingsImport?.ScaName) ?? "";
            ModernName.Text = (_moasSettingsImport?.ModernName) ?? "";
            EmailAddress.Text = (_moasSettingsImport?.EmailAddress) ?? "";
            MembershipNumber.Text = _moasSettingsImport.MembershipNumber.ToString();
            WarrantYes.IsChecked = _moasSettingsImport.WarrantCopy;
            ExpirationDate.SelectedDate = _moasSettingsImport.MembershipExpiration;
            ModernNameShare.IsChecked = _moasSettingsImport.SharePermission.FirstOrDefault(kvp => kvp.Key == ModernNameShare.Name).Value;
            MailingShare.IsChecked = _moasSettingsImport.SharePermission.FirstOrDefault(kvp => kvp.Key == MailingShare.Name).Value;
            PhoneShare.IsChecked = _moasSettingsImport.SharePermission.FirstOrDefault(kvp => kvp.Key == PhoneShare.Name).Value;
            EmailShare.IsChecked = _moasSettingsImport.SharePermission.FirstOrDefault(kvp => kvp.Key == EmailShare.Name).Value;
            #endregion
            #region Other
            SeneschalScaName.Text = (_moasSettingsImport?.SeneschalScaName) ?? "";
            SeneschalEmail.Text = (_moasSettingsImport?.SeneschalEmail) ?? "";
            UpdateYes.IsChecked = _moasSettingsImport.UpdateContactInformation;
            DeputyYes.IsChecked = _moasSettingsImport.HaveDeputy;
            RecognitionText.Text = (_moasSettingsImport?.NeedRecognition) ?? "";
            NeedText.Text = (_moasSettingsImport?.NeedFromKingdom) ?? "";
            GoalsText.Text = (_moasSettingsImport?.GroupGoals) ?? "";
            #endregion
            #region Activities            
            ShopText.Text = (_moasSettingsImport?.WorkshopsOccurred) ?? "";
            EventText.Text = (_moasSettingsImport?.ASEvents) ?? "";
            UniText.Text = (_moasSettingsImport?.CollegiaEvents) ?? "";
            MiscText.Text = (_moasSettingsImport?.MiscASActivities) ?? "";
            #endregion
        }

        private async void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Stream exportStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "*.json",
                FileName = "settings.json",
                Filter = "json files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
                Title = "Save data to json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                if ((exportStream = saveFileDialog.OpenFile()) != null)
                {
                    
                    ExportSettings(); // should i expose this out a bit more like i do with import?
                    using (StreamWriter file = new StreamWriter(exportStream)) 
                    {
                        await file.WriteAsync(JsonConvert.SerializeObject(_moasSettingsExport)); 
                    }
                    // log file written?
                    exportStream.Close();
                }
            }
            
        }

        private void ExportSettings() 
        {
            #region General
            _moasSettingsExport.GroupName = CmbGroup.SelectedIndex;
            _moasSettingsExport.Quarter = CmbQuarter.SelectedIndex;
            _moasSettingsExport.Year = int.Parse(ReportYear.Text); 
            _moasSettingsExport.Position = CmbPosition.SelectedIndex;
            _moasSettingsExport.ScaName = ScaName?.Text ?? "";
            _moasSettingsExport.ModernName = ModernName?.Text ?? "";
            _moasSettingsExport.EmailAddress = EmailAddress?.Text ?? "";
            _moasSettingsExport.MembershipNumber = String.IsNullOrEmpty(MembershipNumber.Text) ? 0 : int.Parse(MembershipNumber.Text);
            _moasSettingsExport.WarrantCopy = (WarrantYes.IsChecked ?? false);
            _moasSettingsExport.MembershipExpiration = ExpirationDate.SelectedDate??DateTime.Now;
            List<KeyValuePair<string, bool>> share = new List<KeyValuePair<string, bool>>() {
                new KeyValuePair<string, bool>(ModernNameShare.Name, ModernNameShare.IsChecked ?? false),
                new KeyValuePair<string, bool>(MailingShare.Name, MailingShare.IsChecked ?? false),
                new KeyValuePair<string, bool>(PhoneShare.Name, PhoneShare.IsChecked ?? false),
                new KeyValuePair<string, bool>(EmailShare.Name, EmailShare.IsChecked ?? false)
            };
            _moasSettingsExport.SharePermission = share;
            #endregion
            #region Other
            _moasSettingsExport.SeneschalScaName = SeneschalScaName?.Text ?? "";
            _moasSettingsExport.SeneschalEmail = SeneschalEmail?.Text ?? "";
            _moasSettingsExport.UpdateContactInformation = UpdateYes.IsChecked ?? false;
            _moasSettingsExport.HaveDeputy = DeputyYes.IsChecked ?? false;
            _moasSettingsExport.NeedRecognition = RecognitionText?.Text ?? "";
            _moasSettingsExport.NeedFromKingdom = NeedText?.Text ?? "";
            _moasSettingsExport.GroupGoals = GoalsText?.Text ?? "";
            #endregion
            #region Activities
            _moasSettingsExport.WorkshopsOccurred = ShopText?.Text ?? "";
            _moasSettingsExport.ASEvents = EventText?.Text ?? "";
            _moasSettingsExport.CollegiaEvents = UniText?.Text ?? "";
            _moasSettingsExport.MiscASActivities = MiscText?.Text ?? "";
            #endregion
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e) 
        {
            
            if (!ValidForm()) 
            {
                // message box
                return;
            }

            // break this out: 
            var client = new RestClient("https://submit.jotform.us");
            var request = new RestRequest("/submit/50690741078155/");
            request.AddParameter("formID", 50690741078155);
            request.AddParameter("q4_whichGroup", CmbGroup.Text);
            request.AddParameter("q5_whichQuarter", CmbQuarter.Text);
            request.AddParameter("q6_whatYear", ReportYear.Text);
            request.AddParameter("q41_whatIs", CmbPosition.Text);
            request.AddParameter("q9_scaName", ScaName.Text);
            request.AddParameter("q10_modernName", ModernName.Text);

            if (UpdateYes.IsChecked ?? false) 
            {
                request.AddParameter("q14_hasAny", "Yes");
                request.AddParameter("q20_moasMailing[addr_line1]", _moasStreet);
                request.AddParameter("q20_moasMailing[addr_line2]", _moasStreetTwo);
                request.AddParameter("q20_moasMailing[city]", _moasCity);
                request.AddParameter("q20_moasMailing[state]", _moasState);
                request.AddParameter("q20_moasMailing[postal]", _moasZip);
                request.AddParameter("q20_moasMailing[country]", _moasCountry); // hardcoded to United States. 
                request.AddParameter("q21_moasPhone21[area]", _moasArea);
                request.AddParameter("q21_moasPhone21[phone]", _moasPhone);
            } 
            else 
            {
                request.AddParameter("q14_hasAny", "No");
            }

            request.AddParameter("q18_emailAddress", EmailAddress.Text);

            if (ModernNameShare.IsChecked ?? false) 
            {
                request.AddParameter("q47_permissionIs[]", "Modern Name");
            }
            if (MailingShare.IsChecked ?? false) 
            {
                request.AddParameter("q47_permissionIs[]", "Mailing Address");
            }
            if (PhoneShare.IsChecked ?? false) 
            {
                request.AddParameter("q47_permissionIs[]", "Phone#");
            }
            if (EmailShare.IsChecked ?? false) 
            {
                request.AddParameter("q47_permissionIs[]", "Email Address");
            }

            if (WarrantYes.IsChecked ?? false) 
            {
                request.AddParameter("q39_doYou39", "Yes");
            } else if (WarrantNo.IsChecked ?? false) 
            {
                request.AddParameter("q39_doYou39", "No");
            }
            // need to make sure at least one is checked. 

            request.AddParameter("q19_membershipNumber", MembershipNumber.Text);
            request.AddParameter("q17_expirationDate[month]", ExpirationDate.SelectedDate?.Month);
            request.AddParameter("q17_expirationDate[day]", ExpirationDate.SelectedDate?.Day);
            request.AddParameter("q17_expirationDate[year]", ExpirationDate.SelectedDate?.Year);

            request.AddParameter("q42_seneschalsSca", SeneschalScaName.Text);
            request.AddParameter("q43_seneschalsEmail", SeneschalEmail.Text);

            if (CmbPosition.SelectedIndex == 1) // Canton MoAS
            {
                request.AddParameter("q44_baronialMoass44", _baronialMoasName); // name
                request.AddParameter("q45_baronialMoass45", _baronialMoasEmail); // email
            }

            if (DeputyYes.IsChecked ?? false) 
            {
                request.AddParameter("q26_doYou", "Yes");
                request.AddParameter("q23_deputysSca", _deputySca);
                request.AddParameter("q24_deputysModern", _deputyName);
                request.AddParameter("q25_deputysEmail", _deputyEmail);
            } 
            else 
            {
                request.AddParameter("q26_doYou", "No");
            }
            var response = 
                await client.ExecuteAsync(request);

            if(response.Content.Contains("Thank You!")) {
                MessageBox.Show("You've successfully submitted your report! Please check the email you entered for a copy!", "Success");

            }
            else if(response.Content.Contains("Incomplete Values")) {
                MessageBox.Show("There are incomplete values or errors in your form. Please correct them.", "Error");
            }
        }

        private bool ValidForm()
        {
            /* do form validation:
                    required (not null >.>):
                        group *
                        quarter *
                        year *
                        position *
                        sca name * 
                        modern name *
                        contact info yes/no *
                        email address * 
                        warrant *
                        membership number *
                        expiration date *
                        seneschal's sca name *
                        seneschal's email *
                        deputy yes/no 
            */
            return true;
        }

        private async void UpdateYes_Click(object sender, RoutedEventArgs e) 
        {
            ContactChildWindow contactChildWindow = new ContactChildWindow();
            contactChildWindow.MoASContactInformation += moasInfo =>
            {
                _moasStreet = moasInfo[0]; // TODO: Check null/empty
                _moasStreetTwo = moasInfo[1];
                _moasCity = moasInfo[2];
                _moasState = moasInfo[3];
                _moasZip = moasInfo[4];
                _moasArea = moasInfo[5];
                _moasPhone = moasInfo[6];
            };
            await this.ShowChildWindowAsync(contactChildWindow);
        }

        private async void DeputyYes_Click(object sender, RoutedEventArgs e) 
        {
            DeputyChildWindow deputyChildWindow = new DeputyChildWindow();
            deputyChildWindow.DeputyInformation += depInfo =>
            {
                _deputySca = depInfo[0]; // TODO: Check null/empty.
                _deputyName = depInfo[1];
                _deputyEmail = depInfo[2];
            };
            await this.ShowChildWindowAsync(deputyChildWindow);
        }

        private async void CmbPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // e.g. MessageBox.Show((string)CmbPosition.SelectedValue, "Baronial Position");
            //TODO: if canton moas - prompt for baronial moas information. create new child window
            if(CmbPosition.SelectedIndex == 1) {
                BaronialMoASChildWindow baronialMoASChildWindow = new BaronialMoASChildWindow();
                baronialMoASChildWindow.MoASInformation += barInfo => {
                    _baronialMoasName = barInfo[0];
                    _baronialMoasEmail = barInfo[1];
                };
                await this.ShowChildWindowAsync(baronialMoASChildWindow);
            }
        }

        private void ReportYear_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void MembershipNumber_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ReportYear_TextChanged(object sender, TextChangedEventArgs e) {
            ReportYear.Text = ReportYear.Text.Replace(" ", string.Empty);
        }

        private void MembershipNumber_TextChanged(object sender, TextChangedEventArgs e) {
            MembershipNumber.Text = MembershipNumber.Text.Replace(" ", string.Empty);
        }
    }
}
