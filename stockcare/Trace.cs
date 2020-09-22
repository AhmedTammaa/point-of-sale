using System;
using System.Data;
using System.Data.OleDb;

namespace Stock_Care_D
{
    class Trace
    {
        private const string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";

        public DataTable filterCodeTrace(string code)
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"Select [date], [DnInv], [VC],[In],[Out],[Balance] FROM trace WHERE code = '" + code + "'";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                TableBuilder tb = new TableBuilder();
                return tb.fillTableTrace();
            }
        }
        public DataTable filterVendorTrace(string vendor)
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"Select [date], [DnInv], [code],[item],[In],[Out],[Balance] FROM trace WHERE VC = '" + vendor + "'";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                TableBuilder tb = new TableBuilder();
                return tb.fillTableTrace();
            }
        }

        public DataTable  filterItemTrace(string item)
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"Select [date], [DnInv], [VC],[In],[Out],[Balance] FROM trace WHERE Item = '" + item + "';";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                TableBuilder tb = new TableBuilder();
                return tb.fillTableTrace();
            }
        }
    }
}
