using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace Stationery_FabricDB
{
    /// <summary>
    /// Interaction logic for CreateEditWindow.xaml
    /// </summary>
    public partial class CreateEditWindow : Window
    {
        bool Edit;
        int Id;
        public string Name, Type, Quantity, Price;
        public CreateEditWindow(bool isEdit  = false, int id = 0, string name = "", string type = "", string quantity = "", string price = "")
        {
            InitializeComponent();

            SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True");
            SqlCommand command = new SqlCommand();
            try
            {
                connect.Open();
                command.Connection = connect;
                command.CommandText = "Select Name from Types;";
                SqlDataReader reader = command.ExecuteReader();

                cmbGoodType.Items.Clear();

                while (reader.Read())
                {
                    string typeName = reader["Name"].ToString();
                    cmbGoodType.Items.Add(typeName);
                }

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

            
            if (!isEdit)
            {
                txtName.Text = "NewName";
                txtCost.Text = "100";
                txtQuantity.Text = "5";
            }
            else
            {
                txtName.Text = name;
                txtCost.Text = price;
                txtQuantity.Text = quantity;
            }

            cmbGoodType.SelectedIndex = 0;

            Name = name;
            Type = type;
            Quantity = quantity;
            Price = price;
            Id = id;
            Edit = isEdit;

        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Edit)
            {
                Name = txtName.Text;
                Type = cmbGoodType.Text;
                Quantity = txtQuantity.Text;
                Price = txtCost.Text;
                DialogResult = true;
                Close();
                return;
            }

            using (SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True"))
            using (SqlCommand cmd = new SqlCommand("InsertNewStationery", connect))
            {
                try
                {
                    connect.Open();

                    cmd.Parameters.Add("Name", SqlDbType.NVarChar);
                    cmd.Parameters["Name"].Value = txtName.Text;

                    cmd.Parameters.Add("TypeID", SqlDbType.Int);
                    cmd.Parameters["TypeID"].Value = GetIDByName("Types", cmbGoodType.SelectedItem.ToString());

                    cmd.Parameters.Add("Quantity", SqlDbType.Int);
                    cmd.Parameters["Quantity"].Value = Convert.ToInt32(txtQuantity.Text);

                    cmd.Parameters.Add("Price", SqlDbType.Decimal);
                    cmd.Parameters["Price"].Value = Convert.ToDecimal(txtCost.Text);

                    cmd.CommandType = CommandType.StoredProcedure;

                    int n = cmd.ExecuteNonQuery();
                    if (n == 1)
                        MessageBox.Show("Success!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
           

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private int GetIDByName(string tableName, string name)
        {
            int id = -1;

            using (SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True"))
            {
                connect.Open();

                string query = $"SELECT ID FROM {tableName} WHERE Name = @Name;";
                using (SqlCommand idCommand = new SqlCommand(query, connect))
                {
                    idCommand.Parameters.AddWithValue("@Name", name);

                    try
                    {
                        object result = idCommand.ExecuteScalar();
                        if (result != null)
                        {
                            id = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            return id;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtCost.Text) ||
                string.IsNullOrEmpty(txtQuantity.Text) ||
                cmbGoodType.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the fields.");
                return false;
            }


            if (!double.TryParse(txtCost.Text, out _) || !int.TryParse(txtQuantity.Text, out _))
            {
                MessageBox.Show("Invalid numeric values for Cost or Quantity.");
                return false;
            }

            return true;
        }
    }
}

