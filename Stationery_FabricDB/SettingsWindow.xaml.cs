using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Stationery_FabricDB
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public bool Edit;
        public string Type;
        public SettingsWindow(string type, bool isEdit = false)
        {
            InitializeComponent();

            SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True");
            SqlCommand command = new SqlCommand();
            try
            {
                if (type.ToLower() == "stationery")
                {
                    ExecuteSelectionNoParam("ShowAllStationeryInfo");
                    label.Text = "Select stationery to ";
                }
                else if(type.ToLower() == "firm")
                {
                    ExecuteSelectionNoParam("ShowAllFirms");
                    label.Text = "Select firm to ";
                }
                else if(type.ToLower() == "type")
                {
                    ExecuteSelectionNoParam("ShowAllTypes");
                    label.Text = "Select type to ";
                }
                else if(type.ToLower() == "manager")
                {
                    ExecuteSelectionNoParam("ShowAllManagers");
                    label.Text = "Select manager to ";
                }
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

            if (isEdit) label.Text += "edit";
            else label.Text += "delete";

            Edit = isEdit;
            Type = type;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid selectedDataGrid = datagrid;
            var selectedItems = selectedDataGrid.SelectedItems;

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Choose something first!");
                return;
            }

            var selectedRow = selectedItems[0] as DataRowView;




            int id = (int)selectedRow["ID"];



            if (Edit)
            {
                if (Type.ToLower() == "stationery")
                {
                    
                    string name = selectedRow["Name"].ToString();
                    string typeId = selectedRow["TypeID"].ToString();
                    string quantity = selectedRow["Quantity"].ToString();
                    string price = selectedRow["Price"].ToString();

                    CreateEditWindow window = new CreateEditWindow(true, id, name, typeId, quantity, price);
                    window.ShowDialog();

                    if (window.DialogResult == true)
                    {
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True"))
                            using (SqlCommand cmd = new SqlCommand("UpdateStationery", connect))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@StationeryID", SqlDbType.Int).Value = id;
                                cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = window.Name;
                                cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(window.Quantity); ;
                                cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Convert.ToDouble(window.Price);

                                connect.Open();
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Stationery updated successfully.");
                                }
                                else
                                {
                                    MessageBox.Show("No records were updated.");
                                }
                                ExecuteSelectionNoParam("ShowAllStationeryInfo");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                }
                else if (Type.ToLower() == "firm")
                {
                    int firmID = (int)selectedRow["ID"];
                    string name = selectedRow["Name"].ToString();

                    EnterWindow window = new EnterWindow(name);
                    window.ShowDialog();

                    if (window.DialogResult == true)
                    {
                        ExecuteTwoParam("UpdateFirm", firmID, window.Value);
                    }

                    ExecuteSelectionNoParam("ShowAllFirms");
                }
                else if (Type.ToLower() == "type")
                {
                    int typeId = (int)selectedRow["ID"];
                    string name = selectedRow["Name"].ToString();

                    EnterWindow window = new EnterWindow(name);
                    window.ShowDialog();

                    if (window.DialogResult == true)
                    {
                        ExecuteTwoParam("UpdateType", typeId, window.Value);
                    }

                    ExecuteSelectionNoParam("ShowAllTypes");
                }
                else if (Type.ToLower() == "manager")
                {
                    int managerId = (int)selectedRow["ID"];
                    string name = selectedRow["Name"].ToString();

                    EnterWindow window = new EnterWindow(name);
                    window.ShowDialog();

                    if (window.DialogResult == true)
                    {
                        ExecuteTwoParam("UpdateManager", managerId, window.Value);
                    }

                    ExecuteSelectionNoParam("ShowAllManagers");
                }

            }
            else
            {
                if (Type.ToLower() == "stationery")
                {
                    ExecuteOneParam("DeleteStationery", id);
                    ExecuteSelectionNoParam("ShowAllStationeryInfo");
                }
                else if (Type.ToLower() == "firm")
                {
                    ExecuteOneParam("DeleteFirm", id);
                    ExecuteSelectionNoParam("ShowAllFirms");
                }
                else if (Type.ToLower() == "type")
                {
                    ExecuteOneParam("DeleteType", id);
                    ExecuteSelectionNoParam("ShowAllTypes");
                }
                else if (Type.ToLower() == "manager")
                {
                    ExecuteOneParam("DeleteManager", id);
                    ExecuteSelectionNoParam("ShowAllManagers");
                }
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
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

        public void ExecuteTwoParam(string procedureName, int id, string Name)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True"))
                using (SqlCommand cmd = new SqlCommand(procedureName, connect))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;

                    connect.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Success!");
                    }
                    else
                    {
                        MessageBox.Show("No records were updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ExecuteOneParam(string procedureName, int param)
        {
            SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True");
            SqlCommand command = new SqlCommand();

            try
            {
                connect.Open();
                command.Connection = connect;
                command.CommandText = "EXEC " + procedureName + " '" + param + "';";
                int n = command.ExecuteNonQuery();

                if(n == 0) { MessageBox.Show("Something is bruh"); }
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
    }
}
