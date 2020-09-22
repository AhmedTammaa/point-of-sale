using System;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
namespace Stock_Care_D
{
    class Export
    {
        private const string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";
        public void Excel(DataGridView dv)
        {
            if (dv.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel._Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = xcelApp.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Report";
                for (int i = 1; i < dv.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = dv.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dv.Rows.Count; i++)
                {
                    for (int j = 0; j < dv.Columns.Count; j++)
                    {
                        if (dv.Rows[i].Cells[j].Value != null)
                        {
                            worksheet.Cells[i + 2, j + 1] = dv.Rows[i].Cells[j].Value.ToString();
                        }
                        else
                        {
                            worksheet.Cells[i + 2, j + 1] = "";
                        }
                    }
                }
                SaveFileDialog saver = new SaveFileDialog();
                saver.FileName = "Stock Report " + Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Day);
                saver.DefaultExt = "xlsx";
                if (saver.ShowDialog() == DialogResult.OK)
                {
                    worksheet.SaveAs(saver.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing);

                }
                xcelApp.Quit();
            }
        }
        public DataTable Search(string item)
        {
            OleDbConnection connect = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Database\project101.accdb; Persist Security Info=False;";
            connect.Open();
            string query = "SELECT * FROM current_stock WHERE item LIKE '%" + item + "%'";
            command.Connection = connect;
            command.CommandText = query;
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable data_table = new DataTable();
            adapter.Fill(data_table);
            connect.Close();
            return data_table;
        }
    }
}
