using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Stock_Care_D
{
    class Invoice
    {
        private const string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";
        public void FillInvoiceNumber(ComboBox inv_number)
        {
            inv_number.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            inv_number.AutoCompleteSource = AutoCompleteSource.CustomSource;
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = " SELECT DISTINCT * FROM sellertablw ";
            cmd.ExecuteNonQuery();
            DataTable data_table = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            da.Fill(data_table);
            SortedSet<string> arr = new SortedSet<string>();
            foreach (DataRow dr in data_table.Rows)
            {
                string collname = Convert.ToString(dr["invoice number"]);

                string temp = dr["invoice number"].ToString();
                arr.Add(temp);
                collection.Add(collname);

            }
            foreach (string i in arr)
            {
                inv_number.Items.Add(i);

            }
            connect.Close();
            inv_number.AutoCompleteCustomSource = collection;

        }
        public void fillIdInv(ComboBox inv_id)
        {
            inv_id.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            inv_id.AutoCompleteSource = AutoCompleteSource.CustomSource;
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = " SELECT * FROM sellertablw ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string collname = Convert.ToString(dr["ID"]);
                inv_id.Items.Add(dr["ID"]);
                collection.Add(collname);
            }
            connect.Close();
            inv_id.AutoCompleteCustomSource = collection;
        }
        public Tuple<int, float> newInvoice(string code, string item, string inv_num, string cust, float pkg, int uom, int in_stock, int pcs_count, string date, int u_price,decimal discount,decimal payment)
        {
            in_stock -= pcs_count;
            pkg = pcs_count / uom;
            decimal total_price = pcs_count * u_price;
            total_price -= total_price * discount;
            //Establish the coneection and save data
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = @"INSERT INTO sellertablw(package,code,[invoice number],Customer,[item name],units,[price of peice],[total price],[total discont],[date of the operation],[total payment]) VALUES ("
            +
            pkg
            + ",'"
            + code
            + "',"
            + inv_num
            + ",'"
            + cust
            + "','"
            + item
            + "',"
            + pcs_count
            + ","
            + u_price
            + ","
            + total_price
            + ","
            + discount
            + ",'"
            + date
            +
            "',"
            +
            payment
            + "); ";
            cmd.ExecuteNonQuery();

            connect.Close();
            connect.ResetState();
            connect.Open();
            //Update the sold table
            cmd.Connection = connect;
            cmd.CommandText = @"UPDATE stock INNER JOIN ([input] INNER JOIN sold ON input.code = sold.Code) ON (stock.code = input.code) AND (stock.code = sold.Code) SET sold.[Out] = sold.[Out]+" + pcs_count + ", sold.[quantity package] = [sold]![Out]/[stock]![peice per package] WHERE(([sold].[Code] = '" + code + "') AND ([sold].[code]=[stock].[code]));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"INSERT INTO [trace] ( code, item, [Out], [date], [DNInv], VC,Balance) VALUES ('"
                              + code
                              + @"','"
                              + item
                              + @"','"
                              + pcs_count
                              + @"','"
                              + date
                              + @"','"
                              + inv_num
                              + @"','"
                              + cust
                              + @"','"
                              + in_stock
                              +
                              "' ); ";
            cmd.ExecuteNonQuery();
            connect.Close();
            return Tuple.Create(in_stock, pkg);
        }
        public Tuple<int, float> updateInvoice(string id,string code, string item, string inv_num, string cust, float pkg, int uom, int in_stock, int pcs_count, string date, int u_price, decimal discount, decimal payment,int update_to)
        {
            int final = update_to - pcs_count;
            pkg = update_to / uom;
            
            in_stock -= (int)final;
            decimal total_price = pcs_count * u_price;
            total_price -= total_price * discount;
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = @"UPDATE sellertablw SET sellertablw.[invoice number] ='"
                              + inv_num
                              + "', sellertablw.Customer ='"
                              + cust
                              + "', sellertablw.code ='"
                              + code
                              + "', sellertablw.[item name] = '"
                              + item
                              + "', sellertablw.units = '"
                              + update_to
                              + "', sellertablw.package ='"
                              + pkg
                              + "', sellertablw.[price of peice] ='"
                              + u_price
                              + "', sellertablw.[total discont] ='"
                              + discount
                              + "', sellertablw.[date of the operation] ='"
                              + date
                              + "', sellertablw.[total payment] = '"
                              + payment
                              + "' WHERE ((ID="
                              + id
                              + "));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE stock INNER JOIN ([input] INNER JOIN sold ON input.code = sold.Code) ON (stock.code = input.code) AND (stock.code = sold.Code) SET sold.[Out] = sold.[Out] - " + pcs_count+ ", sold.[quantity package] = [sold]![Out]/[stock]![peice per package] WHERE(([sold].[Code] = '" + code + "') AND ([sold].[code]=[stock].[code]));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE stock INNER JOIN ([input] INNER JOIN sold ON input.code = sold.Code) ON (stock.code = input.code) AND (stock.code = sold.Code) SET sold.[Out] = sold.[Out] + " + update_to + ", sold.[quantity package] = [sold]![Out]/[stock]![peice per package] WHERE(([sold].[Code] = '" + code + "') AND ([sold].[code]=[stock].[code]));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE trace SET trace.code ='"
            + code
            + "', trace.Item ='"
            + item
            + "', trace.[Out] = trace.[Out] - "
            + pcs_count
            + ", trace.[date] = '"
            + date
            + "' , trace.[Balance] = trace.[Balance] -"
            + pcs_count
            + " WHERE ((code='"
            + code
            +
            "' AND (trace.date) >=#"
            + date
            + "#"
            + "AND trace.[In] < 0"
            +
            "));";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE trace SET trace.code ='"
             + code
             + "', trace.Item ='"
             + item
             + "', trace.[Out] = trace.[Out] + "
             + update_to
             + ", trace.[date] = '"
             + date
             + "' , trace.[Balance] = trace.[Balance] +"
             + update_to
             + " WHERE ((DNInv="
             + inv_num
             + "AND code='"
             + code
             +
             "' AND (trace.date) >=#"
             + date
             + "#"
             + "AND trace.[In] < 0"
             +
              "));";
            cmd.ExecuteNonQuery();
            connect.Close();
            return Tuple.Create(in_stock, pkg);
        }
        public Decimal CustPrice(string cust, string code)
        {
            OleDbConnection con = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader reader;
            con.ConnectionString = connection_string;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM priceMaster WHERE (customer = " + "'" + cust + "' AND code = '" + code + "');";
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int temp = reader.GetOrdinal("price");
                return reader.GetDecimal(temp);
            }
            return 0;
        }
        public void DeleteInvoice(string id, string code, string item, string inv_num, string cust, float pkg, int uom, int in_stock, int pcs_count, string date, int u_price, decimal discount, decimal payment)
        {
            OleDbConnection connect = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connect.ConnectionString = connection_string;
            connect.Open();
            command.Connection = connect;
            command.CommandText = "DELETE FROM sellertablw WHERE (ID = " + id + ");";
            command.ExecuteNonQuery();
            command.CommandText = @"UPDATE stock INNER JOIN ([input] INNER JOIN sold ON input.code = sold.Code) ON (stock.code = input.code) AND (stock.code = sold.Code) SET sold.[Out] = sold.[Out] - "
                                  + pcs_count
                                  + ", sold.[quantity package] = [sold]![Out]/[stock]![peice per package] WHERE(([sold].[Code] = '"
                                  + code
                                  + "') AND ([sold].[code]=[stock].[code]));";
            command.ExecuteNonQuery();
            command.CommandText = @"UPDATE trace SET trace.code ='"
            + code
            + "', trace.Item ='"
            + item
            + "', trace.[Out] = trace.[Out] -"
            + pcs_count
            + ", trace.date = '"
            + date
            + "' WHERE ((DNInv= "
            + inv_num
            + " AND code= '"
            + code
            + "'));";
            command.ExecuteNonQuery();
            connect.Close();

        }
        public string[] FillByID(string id)
        {
            string[] result = new string[11];
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = connection_string;
            connect.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connect;
            cmd.CommandText = "SELECT * FROM sellertablw WHERE (ID =" + id + ");";
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result[0] = reader.GetInt32(1).ToString();
                for(int i = 1; i < 9; ++i)
                {
                    if(i<4)
                        result[i] = reader.GetString(i + 1);
                    else
                        result[i] = reader.GetInt32(i + 1).ToString();
                }
                result[9] = reader.GetDateTime(10).ToString();
                result[10] = reader.GetInt32(11).ToString();
            }
            connect.Close();
            return result;
        }
    }
}
