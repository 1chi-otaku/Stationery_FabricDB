using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
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
using System.Xml.Linq;

namespace Stationery_FabricDB
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ExecuteSelectionNoParam(string procedureName)
        {
            SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True");
            SqlCommand command = new SqlCommand();

            try
            {
                connect.Open();
                command.Connection = connect;
                command.CommandText = "EXEC " + procedureName + ";";
                SqlDataReader reader = command.ExecuteReader();


                DataTable dt = new DataTable();
                dt.Load(reader);

                datagrid.ItemsSource = dt.DefaultView;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public void ExecuteSelectionOneParam(string procedureName, string param)
        {
            SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True");
            SqlCommand command = new SqlCommand();

            try
            {
                connect.Open();
                command.Connection = connect;
                command.CommandText = "EXEC " + procedureName +  " '" + param + "';";
                SqlDataReader reader = command.ExecuteReader();


                DataTable dt = new DataTable();
                dt.Load(reader);

                datagrid.ItemsSource = dt.DefaultView;
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

        }
        private void MenuItemShowAll_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowAllStationeryInfo");
        }

        private void MenuItemShowTypes_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowAllTypes");
        }

        private void MenuItemShowManagers_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowAllManagers");
        }

        private void MenuItemShowMaxQuantity_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowStationeryWithMaxQuantity");
        }

        private void MenuItemShowMinQuantity_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowStationeryWithMinQuantity");
        }

        private void MenuItemShowMaxPrice_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowStationeryWithMaxPrice");
        }

        private void MenuItemShowMinPrice_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowStationeryWithMinPrice");
        }

        private void MenuItemShowAVGTypeQua_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowAverageQuantityByType");
        }

        private void MenuItemShowSelectedType_Click(object sender, RoutedEventArgs e)
        {
            SelectiveWindow selective = new SelectiveWindow("Type");
            selective.ShowDialog();

            if (selective.DialogResult == true)
            {
                ExecuteSelectionOneParam("ShowStationeryByType", selective.Value);
            }
            else if (selective.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemShowSelectedManagerSold_Click(object sender, RoutedEventArgs e)
        {
            SelectiveWindow selective = new SelectiveWindow("Manager");
            selective.ShowDialog();

            if (selective.DialogResult == true)
            {
                ExecuteSelectionOneParam("ShowStationerySoldByManager", selective.Value);
            }
            else if (selective.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemShowSelectedFirmBought_Click(object sender, RoutedEventArgs e)
        {
            SelectiveWindow selective = new SelectiveWindow("Firm");
            selective.ShowDialog();

            if (selective.DialogResult == true)
            {
                ExecuteSelectionOneParam("ShowStationeryBoughtByFirm", selective.Value);
            }
            else if (selective.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemShowRecentTransaction_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowLatestTransactionInfo");
        }

        private void MenuItemCreateNew_Click(object sender, RoutedEventArgs e)
        {
            CreateEditWindow create = new CreateEditWindow();
            create.ShowDialog();

            if (create.DialogResult == true)
            {
                ExecuteSelectionNoParam("ShowAllStationeryInfo");
            }
            else if (create.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemCreateNewType_Click(object sender, RoutedEventArgs e)
        {
            EnterWindow enter = new EnterWindow();
            enter.ShowDialog();

            if (enter.DialogResult == true)
            {
                ExecuteSelectionOneParam("InsertNewType", enter.Value);
                ExecuteSelectionNoParam("ShowAllTypes");
            }
            else if (enter.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemCreateNewManager_Click(object sender, RoutedEventArgs e)
        {
            EnterWindow enter = new EnterWindow();
            enter.ShowDialog();

            if (enter.DialogResult == true)
            {
                ExecuteSelectionOneParam("InsertNewManager", enter.Value);
                ExecuteSelectionNoParam("ShowAllManagers");
            }
            else if (enter.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemCreateNewFirm_Click(object sender, RoutedEventArgs e)
        {
            EnterWindow enter = new EnterWindow();
            enter.ShowDialog();

            if (enter.DialogResult == true)
            {
                ExecuteSelectionOneParam("InsertNewFirm", enter.Value);
                ExecuteSelectionNoParam("ShowAllStationeryInfo");
            }
            else if (enter.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemUpdateStationery_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("stationery",true);
            settings.ShowDialog();

            
        }

        private void MenuItemUpdateType_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("type",true);
            settings.ShowDialog();
        }

        private void MenuItemUpdateManager_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("manager", true);
            settings.ShowDialog();
        }

        private void MenuItemUpdateFirm_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("firm", true);
            settings.ShowDialog();
        }

        private void MenuItemDeleteStationery_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("stationery");
            settings.ShowDialog();
        }

        private void MenuItemDeleteType_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("type");
            settings.ShowDialog();
        }

        private void MenuItemDeleteManager_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("manager");
            settings.ShowDialog();
        }

        private void MenuItemDeleteFirm_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow("firm");
            settings.ShowDialog();
        }

        private void MenuItemShowManagerWithMaxSoldQuantity_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowManagerWithMaxSoldQuantity");
        }

        private void MenuItemShowManagerWithMaxTotalProfit_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowManagerWithMaxTotalProfit");
        }

        private void MenuItemShowManagerWithMaxTotalProfitInPeriod_Click(object sender, RoutedEventArgs e)
        {
            DataRangeInput dataRangeInput = new DataRangeInput();
            dataRangeInput.ShowDialog();

            if (dataRangeInput.DialogResult == true)
            {
                DateTime? startDate = dataRangeInput.StartDate;
                DateTime? endDate = dataRangeInput.EndDate;

                if (startDate.HasValue && endDate.HasValue)
                {
                    try
                    {
                        using (SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True"))
                        using (SqlCommand cmd = new SqlCommand("ShowManagerWithMaxTotalProfitInPeriod", connect))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate.Value;
                            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate.Value;

                            connect.Open();

                            SqlDataReader reader = cmd.ExecuteReader();

                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            datagrid.ItemsSource = dt.DefaultView;
                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please select both start and end dates.");
                }
            }
            else if (dataRangeInput.DialogResult == false)
            {
                return;
            }
        }

        private void MenuItemShowFirmWithMaxTotalPurchase_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowFirmWithMaxTotalPurchase");
        }

        private void MenuItemShowTypeWithMaxSoldQuantity_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowTypeWithMaxSoldQuantity");
        }

        private void MenuItemShowMostProfitableType_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowMostProfitableType");
        }

        private void MenuItemShowMostPopularStationery_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectionNoParam("ShowMostPopularStationery");
        }

        private void MenuItemShowStationeryNotSoldForDays_Click(object sender, RoutedEventArgs e)
        {
            EnterWindow window = new EnterWindow();
            window.ShowDialog();

            if (window.DialogResult == true)
            {
                if (int.TryParse(window.Value, out int result))
                {
                    ExecuteSelectionOneParam("ShowStationeryNotSoldForDays", window.Value);
                }
                else
                {
                    MessageBox.Show("Invalid numeric value.");
                }
               
            }
        }
    }
}
