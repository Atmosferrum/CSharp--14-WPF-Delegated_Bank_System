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

namespace Bank_System.Windows
{
    /// <summary>
    /// Interaction logic for EditClientWindow.xaml
    /// </summary>
    public partial class EditClientWindow : Window
    {
        private MainWindow mainWindow;
        private Client client;
        private int clientClassIndex;

        private int deposit; //Variable to get PARSED Deposit Data

        private double percent; //Variable to get PARSED Percent Data

        private DateTime dateOfDeposit; //Variable to get PARSED date of deposit

        private bool parsedDeposit => Int32.TryParse(TB_EditClientDeposit.Text, out deposit); //Bool to PARSE Deposit Data

        private bool parsedPercent => Double.TryParse(TB_EditClientPercent.Text, out percent); //Bool to PARSE Percent Data

        private bool parsedDate => DateTime.TryParse(DP_EditClientDateOfDeposit.Text, out dateOfDeposit); //Bool to PARSE Date Data

        private bool depositIsValid => parsedDeposit //Bool to CHECK if Deposit Data is correct
                                    && deposit >= Bank.minDeposit
                                    && deposit <= Bank.maxDeposit;

        private bool percentIsValid => parsedPercent //Bool to CHECK if Percent Data is correct
                                    && percent >= Bank.minPercent
                                    && percent <= Bank.maxPercent;

        private bool dateIsValid => parsedDate //Bool to CHECK if Date Data is correct
                                 && dateOfDeposit <= DateTime.Now;

        private bool inputDataIsCorrect => TB_EditClientName.Text != ""  //Bool to CHECK if input Data is correct
                                        && TB_EditClientLastName.Text != ""
                                        && depositIsValid
                                        && percentIsValid
                                        && dateIsValid;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainWindow">MainWindow</param>
        /// <param name="client">Client to EDIT reference</param>
        /// <param name="clientClassIndex">Client Class</param>
        public EditClientWindow(MainWindow mainWindow,
                                Client client,
                                int clientClassIndex)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.client = client;
            this.clientClassIndex = clientClassIndex;

            TB_EditClientDeposit.Text = Convert.ToString(client.Deposit);
            TB_EditClientName.Text = client.Name;
            TB_EditClientLastName.Text = client.LastName;
            TB_EditClientPercent.Text = Convert.ToString(client.Percent);
            DP_EditClientDateOfDeposit.Text = Convert.ToString(client.DateOfDeposit);
        }

        /// <summary>
        /// Button Method to EDIT Client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Clients_EditClient(object sender, RoutedEventArgs e)
        {
            if (inputDataIsCorrect)
            {
                Bank.EditClient(client,
                                clientClassIndex,
                                TB_EditClientName.Text,
                                TB_EditClientLastName.Text,
                                Convert.ToString(deposit),
                                Convert.ToString(percent),
                                Convert.ToString(dateOfDeposit));

                CloseWindow();
            }
            else
                MessageBox.Show($"The DATA you are entering is wrong!",
                                $"{EditClientWindow.TitleProperty.Name}",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
        }

        /// <summary>
        /// Method to CLOSE this Window
        /// </summary>
        private void CloseWindow()
        {
            mainWindow.LoadClientsToLV();
            this.Close();
        }
    }
}
