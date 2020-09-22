using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Stock_Care_D
{
    class Purchase
    {
        private const string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";
        
        public void fillDelNum(ComboBox delivery_notes)
        {
            delivery_notes.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            delivery_notes.AutoCompleteSource = AutoCompleteSource.CustomSource;
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = " SELECT DISTINCT * FROM importer ";
            cmd.ExecuteNonQuery();
            DataTable data_table = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            da.Fill(data_table);
            SortedSet<string> arr = new SortedSet<string>();
            foreach (DataRow dr in data_table.Rows)
            {
                string collname = Convert.ToString(dr["DN"]);
                string temp = dr["DN"].ToString();
                arr.Add(temp);
                collection.Add(collname);

            }
            foreach (string i in arr)
            {
                delivery_notes.Items.Add(i);

            }
            connect.Close();
            delivery_notes.AutoCompleteCustomSource = collection;
        }
        public void fillid(ComboBox txt_id)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = " SELECT * FROM importer ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string collname = Convert.ToString(dr["ID"]);
                txt_id.Items.Add(dr["ID"]);
                collection.Add(collname);
            }
            connect.Close();
            txt_id.AutoCompleteCustomSource = collection;
        }
        public void fillsupplier(ComboBox box)
        { 
            box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            box.AutoCompleteSource = AutoCompleteSource.CustomSource;
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = " SELECT DISTINCT * FROM client ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string collname = Convert.ToString(dr["client"]);
                box.Items.Add(dr["client"]);
                collection.Add(collname);
            }
            connect.Close();
            box.AutoCompleteCustomSource = collection;

        }
        public Tuple<int,float> newPurchase(string code,string item,string del_num,string sup,float pkg, int uom, int in_stock,int pcs_count,string date)
        {
            try
            {
                in_stock += pcs_count;
                int stock = in_stock + pcs_count;
                pkg = (float)pcs_count / uom;
                OleDbConnection connect = new OleDbConnection();
                connect.ConnectionString = connection_string;
                OleDbCommand cmd = new OleDbCommand();
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"INSERT INTO [importer] ( code, item, peice, [dateofimport], [package],[DN],supplier ) VALUES ('"
                                   + code
                                   + @"','"
                                   + item
                                   + @"','"
                                   + pcs_count
                                   + @"','"
                                   + date
                                   + @"','"
                                   + pkg
                                   + @"','"
                                   + del_num
                                   + @"','"
                                   + sup
                                   +
                                   "' ); ";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"INSERT INTO [trace] ( code, item, [In], [date], [DNInv], VC,Balance) VALUES ('"
                                  + code
                                  + @"','"
                                  + item
                                  + @"','"
                                  + pcs_count
                                  + @"','"
                                  + date
                                  + @"','"
                                  + del_num
                                  + @"','"
                                  + sup
                                  + @"','"
                                  + stock.ToString()
                                  +
                                  "' ); ";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"UPDATE stock INNER JOIN [input] ON stock.code = input.code SET [input].peiceIn = [input]![peiceIn] + "
                                  + pcs_count
                                  + " WHERE (([input].[code]= '"
                                  + code
                                  + "' ) AND ([stock].[code]=[input].[code]));";
                cmd.ExecuteNonQuery();
                connect.Close();
                MessageBox.Show("Saved!");
                return Tuple.Create(in_stock, pkg);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public Tuple<int,float> updatePurchase(string id,string code, string item, string del_num, string sup, float pkg, int uom, int in_stock, int pcs_count, string date, int update_to)
        {
            int final = update_to - pcs_count;
            in_stock += final; 
            pkg = update_to / uom;
            
            OleDbConnection connect = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            connect.ConnectionString = connection_string;
            connect.Open();
            cmd.Connection = connect;
            cmd.CommandText = @"UPDATE importer SET importer.code ='"
                + code
                + "', importer.item ='"
                + item
                + "', importer.peice ='"
                + update_to
                + "', importer.package ='"
                + pkg
                + "', importer.dateofimport = '"
                + date
                + "' WHERE ((ID="
                + id
                + "));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE stock INNER JOIN [input] ON stock.code = input.code SET [input].peiceIn = [input]![peiceIn] - " + pcs_count + " WHERE (([input].[code]= '" + code + "' ) AND ([stock].[code]=[input].[code]));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE stock INNER JOIN [input] ON stock.code = input.code SET [input].peiceIn = [input]![peiceIn] + " + update_to + " WHERE (([input].[code]= '" + code + "' ) AND ([stock].[code]=[input].[code]));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE trace SET trace.code ='"
              + code
              + "', trace.Item ='"
              + item
              + "', trace.[In] = trace.[In] - "
              + pcs_count
              + ", trace.[date] = '"
              + date
              + "' , trace.[Balance] = trace.[Balance] -"
              + pcs_count
              + " WHERE ((code='"
              + code
              + "' AND (trace.date) >=#"
              + date
              + "#"
              + "AND trace.[Out] < 0"
              + "));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE trace SET trace.code ='"
             + code
             + "', trace.Item ='"
             + item
             + "', trace.[In] = trace.[In] + "
             + update_to
             + ", trace.[date] = '"
             + date
             + "' , trace.[Balance] = trace.[Balance] +"
             + update_to
             + " WHERE ((DNInv="
             + del_num
             + "AND code='"
             + code
             +
             "' AND (trace.date) >= #"
             + date
             + "#"
             +
              "));";
            cmd.ExecuteNonQuery();
            connect.Close();
            return Tuple.Create(in_stock, pkg);
        }
        public void deletePurchase(string id, string code, string item, string del_num, int pcs_count, string date)
        {
            OleDbConnection connect = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connect.ConnectionString = connection_string;
            connect.Open();
            command.Connection = connect;
            command.CommandText = "DELETE FROM importer WHERE (ID = " + id + ");";
            command.ExecuteNonQuery();
            command.CommandText = @"UPDATE stock INNER JOIN [input] ON stock.code = input.code SET [input].peiceIn = [input]![peiceIn] - " + pcs_count + " WHERE (([input].[code]= '" + code + "' ) AND ([stock].[code]=[input].[code]));";
            command.ExecuteNonQuery();
            command.CommandText = @"UPDATE trace SET trace.code ='"
            + code
            + "', trace.Item ='"
            + item
            + "', trace.In = trace.In -"
            + pcs_count
            + ", trace.date = '"
            + date
            + "' WHERE ((DNInv= "
            + del_num
            + " AND code= '"
            + code
            + "'));";
            command.ExecuteNonQuery();
            connect.Close();
        }
        public string[] fillByID(string id)
        {
            string[] result = new string[7];
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = "SELECT * FROM importer WHERE (ID =" + id + ");";
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result[0] = reader.GetString(1);
                result[1] = reader.GetString(2);
                result[2] = reader.GetInt32(3).ToString();
                result[3] = reader.GetInt32(4).ToString();
                result[4] = reader.GetDateTime(5).ToString();
                result[5] = reader.GetString(6).ToString();
                result[6] = reader.GetInt32(7).ToString();
            }
            connect.Close();
            return result;
        }
    }
}
