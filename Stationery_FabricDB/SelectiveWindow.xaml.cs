using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Stationery_FabricDB
{
    /// <summary>
    /// Interaction logic for SelectiveWindow.xaml
    /// </summary>
    public partial class SelectiveWindow : Window
    {
        public string Value { get; private set; }
        public SelectiveWindow(string selectiveType)
        {
            InitializeComponent();

            SqlConnection connect = new SqlConnection(@"Data Source=PECHKA\SQLEXPRESS;Initial Catalog=Stationery_Fabric;Integrated Security=True");
            SqlCommand command = new SqlCommand();

            try
            {
                connect.Open();
                command.Connection = connect;

                if(selectiveType.ToLower() == "manager")
                {
                    command.CommandText = "Select Name from Managers;";  
                }
                else if(selectiveType.ToLower() == "type")
                {
                    command.CommandText = "Select Name from Types;";
                }
                else if(selectiveType.ToLower() == "firm")
                {
                    command.CommandText = "Select Name from Firms;";
                }
                else
                {
                    throw (new Exception("Identifiend param"));
                }

                SqlDataReader reader = command.ExecuteReader();

                comboBox.Items.Clear();

                while (reader.Read())
                {
                    string typeName = reader["Name"].ToString();
                    comboBox.Items.Add(typeName);
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
            comboBox.SelectedIndex = 0;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Value = comboBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
