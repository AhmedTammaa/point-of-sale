using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Stock_Care_D
{
    static class Facility
    {
        private const string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";
        public static void fillCodeComboBox(ComboBox box)
        {
            box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            box.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = " SELECT * FROM stock ";
            cmd.ExecuteNonQuery();
            DataTable data_table = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(data_table);
            foreach (DataRow dr in data_table.Rows)
            {
                box.Items.Add(dr["code"]);
                string name = Convert.ToString(dr["code"]);
                collection.Add(name);
            }
            connect.Close();
            box.AutoCompleteCustomSource = collection;
        }
        public static void fillComboName(ComboBox box)
        {
            box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            box.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = " SELECT DISTINCT * FROM stock ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string collname = Convert.ToString(dr["item"]);
                box.Items.Add(dr["item"]);
                collection.Add(collname);
            }
            connect.Close();
            box.AutoCompleteCustomSource = collection;
        }
        public static dynamic fillFieldByCode(string code)
        {
            string _uom = "0", _stock = "0", _name = "";
            OleDbConnection connect = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader reader;
            connect.ConnectionString = connection_string;
            connect.Open();
            cmd.Connection = connect;
            cmd.CommandText = "SELECT * FROM [current_stock] WHERE [code] = '" + code + "'; ";
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int temp = reader.GetOrdinal("item");
                _name = reader.GetString(temp);
                temp = reader.GetOrdinal("peice balance");
                _stock = reader.GetInt32(temp).ToString();
                temp = reader.GetOrdinal("peice per package");
                _uom = reader.GetInt32(temp).ToString();
            }
            connect.Close();
            return new { uom = _uom, stock = _stock, name = _name };
        }
        public static dynamic fillFieldByName(string name)
        {
            string _uom = "0", _stock = "0", _code = "";
            OleDbConnection connect = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader reader;
            connect.ConnectionString = connection_string;
            connect.Open();
            cmd.Connection = connect;
            cmd.CommandText = "SELECT * FROM [current_stock] WHERE [item] = '" + name + "'; ";
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int temp = reader.GetOrdinal("code");
                _code = reader.GetString(temp);
                temp = reader.GetOrdinal("peice balance");
                _stock = reader.GetInt32(temp).ToString();
                temp = reader.GetOrdinal("peice per package");
                _uom = reader.GetInt32(temp).ToString();
            }
            connect.Close();
            return new { uom = _uom, stock = _stock, code = _code };
        }
    }

}

