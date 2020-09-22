using System;
using System.Data;
using System.Data.OleDb;

namespace Stock_Care_D
{
    class TableBuilder
    {
        private const string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";

        public DataTable fillCurrTableInv(string inv_number)
        {


            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();

                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"SELECT * FROM sellertablw
                WHERE(" + inv_number + " = sellertablw.[invoice number]); ";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                /*Just in case a wrong charachter happen it's by default 0*/
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();

                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"SELECT * FROM sellertablw
                WHERE(" + "0" + " = sellertablw.[invoice number]); ";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
        }
        public DataTable fillCurrTablePur(string pur_number)
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"SELECT importer.ID, importer.code,importer.item,importer.peice,importer.package,importer.dateofimport,importer.supplier
                FROM importer
                WHERE(" + pur_number + " = importer.DN); ";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"SELECT importer.ID, importer.code,importer.item,importer.peice,importer.package,importer.dateofimport,importer.supplier
                FROM importer
                WHERE(" + "0" + " = importer.DN); ";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;

            }
        }
        public DataTable fillTableTrace()
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = connection_string;
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"Select * FROM trace";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public DataTable searchStockTable(string item)
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = connection_string;
                connect.Open();
                string query = "SELECT * FROM current_stock WHERE item like '%" + item + "%'";
                cmd.Connection = connect;
                cmd.CommandText = query;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable fillAllTablePur()
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"SELECT importer.ID, importer.code,importer.item,importer.peice,importer.package,importer.dateofimport,importer.supplier
            FROM importer;";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable fillAllTableInv()
        {
            try
            {
                OleDbConnection connect = new OleDbConnection();
                OleDbCommand cmd = new OleDbCommand();
                connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = @"SELECT * FROM sellertablw; ";
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                connect.Close();
                return data_table;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
