using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Care_D
{
    class Master
    {
        private const string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";

        public void AddClient(string name)
        {
            try
            {

                OleDbConnection connect = new OleDbConnection();
                connect.ConnectionString = connection_string;
                OleDbCommand cmd = new OleDbCommand();
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"INSERT INTO [client] (client ) VALUES ('" + name + "'); ";
                cmd.ExecuteNonQuery();
                connect.Close();
                MessageBox.Show("Datasaved", "Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void setPrice(string cust, string code, string item, string price)
        {
            try
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = connection_string;
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO priceMaster (customer,code,item,price) VALUES ('" + cust + "','" + code + "','" + item + "','" + price + "');";
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data Saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void AddItem(string code,string item, string uom)
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                connect.ConnectionString = connection_string;
                OleDbCommand cmd = new OleDbCommand();
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"INSERT INTO stock(code, item, peice, [peice per package])  VALUES ('"
                                 + code
                                 + @"','"
                                 + item
                                 + @"',"
                                 + "0"
                                 + @",'" + uom + "'); ";
                cmd.ExecuteNonQuery();
                cmd.Connection = connect;
                connect.Close();
                
                MessageBox.Show("Datasaved", "Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
