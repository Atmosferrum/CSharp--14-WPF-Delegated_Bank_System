using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bank_System.Windows
{
    /// <summary>
    /// Interaction logic for TransferWindow.xaml
    /// </summary>
    public partial class TransferWindow : Window
    {
        private MainWindow mainWindow; //MainWindow Reference
        private Client fromClient; //Client to Transfer from
        private int clientClassIndex; //Client index to GET its Department

        private float Amount; //Amount to TRANSSFER
        private float From; //Deposit FROM wich will GIVE Transfer
        private float FromResult; //Result for Client wich GIVES Transfer
        private float To; //Deposit TO wich will GET transfer
        private float ToResult; //Result for Client wich GETS Transfer

        private bool parsedAmount => float.TryParse(TB_AmountToTransfer.Text, out Amount); //Bool to PARSE Amount

        private bool amountIsValid => parsedAmount //Bool to CHECK if Amount Data is correct
                                   && Amount > 0
                                   && Amount <= From;

        private bool inputDataIsCorrect => CB_ToClient.SelectedIndex > -1  //Bool to CHECK if input Data is correct
                                        && amountIsValid;
                                        //&& TB_AmountToTransfer.Text != null
                                        //&& TB_AmountToTransfer.Text != "";

        private Department<Client> allClients = new Department<Client>("Temp"); //List of ALL Clients for ComboBox


        #region Constructor;

        /// <summary>
        /// Constgructor for TransferWindow
        /// </summary>
        /// <param name="MainWindow"></param>
        /// <param name="FromClient"></param>
        /// <param name="ClientClassIndex"></param>
        public TransferWindow(MainWindow MainWindow,
                  Client FromClient,
                  int ClientClassIndex)
        {
            InitializeComponent();

            this.mainWindow = MainWindow;
            this.fromClient = FromClient;
            this.clientClassIndex = ClientClassIndex;

            TB_FromClient.Text = $"{fromClient.Name} {fromClient.LastName} {fromClient.Balance} $";

            foreach (Department<Client> allDepartments in Bank.Departments[0].Departments)
            {
                for (int i = 0; i < allDepartments.Count(); i++)
                {
                    allClients.Add(allDepartments[i]);
                }
            }

            From = fromClient.Balance;

            CB_ToClient.ItemsSource = allClients;
        } 

        #endregion Constructor

        #region Element's Mthods;

        /// <summary>
        /// Button Method to TRANSFER money
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Clients_Transfer(object sender, RoutedEventArgs e)
        {
            if (inputDataIsCorrect)
            {
                Bank.Departments[0].Departments[clientClassIndex].Transfer(fromClient, 
                                                                          (CB_ToClient.SelectedItem as Client),
                                                                          Amount);
                CloseWindow();
            }
            else
            {
                MessageBox.Show($"The DATA you are entering is wrong!",
                $"{TransferWindow.TitleProperty.Name}",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// ComboBox Method on SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_ToClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            To = (CB_ToClient.SelectedItem as Client).Balance;

            if (TB_AmountToTransfer.Text != null &&
                TB_AmountToTransfer.Text != "")
                ShowResults();
        }

        /// <summary>
        /// TextBlock Method on SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TB_AmountToTransfer_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (TB_AmountToTransfer.Text != null &&
                TB_AmountToTransfer.Text != "")
                ShowResults();
        } 

        #endregion Element's Mthods

        #region Methods;

        /// <summary>
        /// Method to SHOW Transfer Results
        /// </summary>
        private void ShowResults()
        {
            if (inputDataIsCorrect)
            {
                FromResult = From - Amount;
                ToResult = To + Amount;
                TB_AmountResult.Text = $"{From} - {Amount} = {FromResult} -> {To} + {Amount} = {ToResult}";
            }
            else
            {
                MessageBox.Show($"The DATA you are entering is wrong!",
                $"{TransferWindow.TitleProperty.Name}",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

                TB_AmountResult.Text = "";
            }            
        }

        /// <summary>
        /// Method to CLOSE this Window
        /// </summary>
        private void CloseWindow()
        {
            mainWindow.LoadClientsToLV();
            this.Close();
        }

        #endregion Methods
    }
}
