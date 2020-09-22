using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stock_Care_D
{
    public partial class Form1 : Form
    {
        private TableBuilder tb = new TableBuilder();
        private Purchase pur = new Purchase();
        private Invoice inv = new Invoice();
        private Trace trc = new Trace();
        private Master master = new Master();
        private Export export = new Export();
        private void load()
        {
            //trace loading
            pur.fillsupplier(txt_trc_vend);
            Facility.fillComboName(txt_trc_item);
            Facility.fillCodeComboBox(txt_trc_code);
            //Facility.CodeAutoComplete(codeTrace);
            tbl_trc.DataSource = tb.fillTableTrace();
            //Purchase loading
            pur.fillDelNum(txt_pur_dlvry_num);
            Facility.fillCodeComboBox(txt_pur_code);
            pur.fillid(txt_pur_id);
            Facility.fillComboName(txt_pur_item);
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
            pur.fillsupplier(txt_pur_sup);
            tbl_all_pur.DataSource = tb.fillAllTablePur();
            //stock loading
            tbl_stck.DataSource = tb.searchStockTable(txt_stock_srch.Text);
            //Invoice Loading
            inv.FillInvoiceNumber(txt_inv_num);
            inv.fillIdInv(txt_inv_id);
            pur.fillsupplier(txt_inv_cust);
            Facility.fillCodeComboBox(txt_inv_code);
            Facility.fillComboName(txt_inv_item);
            tbl_all_inv.DataSource = tb.fillAllTableInv();
            //Master Loading
            /*Master Price*/
            pur.fillsupplier(txt_master_cust);
            Facility.fillCodeComboBox(txt_master_price_code);
            Facility.fillComboName(txt_master_price_item);
        }
        public Form1()
        {
            InitializeComponent();
        }
        
        
        private void Form1_Load(object sender, EventArgs e)
        {
            load();   
        }
        private void BunifuFlatButton1_Click(object sender, EventArgs e)
        {
            menu.Visible = false;
        }

        private void BunifuFlatButton2_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbpur;
        }
        private void BunifuFlatButton3_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbstk;
        }

        private void BunifuFlatButton4_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbinv;
        }

        private void Txtidinv_TextChanged(object sender, EventArgs e)
        {
            if (txt_pur_id.Text != "")
            {
                btn_save_pur.Hide();

                btn_save_pur.Enabled = false;

            }
            else
            {
                btn_save_pur.Show();
                btn_save_pur.Enabled = true;
            }
        }

        private void Invoice_numberTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbl_curr_inv.DataSource = tb.fillCurrTableInv(txt_inv_num.Text);
        }

        private void BunifuFlatButton14_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Tuple<int, float> result = pur.newPurchase(txt_pur_code.Text,
                                                       txt_pur_item.Text,
                                                       txt_pur_dlvry_num.Text,
                                                       txt_pur_sup.Text,
                                                       float.Parse(txt_pur_pkg.Text),
                                                       int.Parse(txt_pur_uom.Text),
                                                       int.Parse(txt_pur_in_stock.Text),
                                                       int.Parse(txt_pur_unit.Text),
                                                       txt_pur_date.Value.ToString());
            txt_pur_code.Text = "";
            txt_pur_item.Text = "";
            txt_pur_unit.Text = "0";
            txt_pur_pkg.Text = "";
            pur.fillid(txt_pur_id);
            if (result.Item1 <= 0)
            {

                txt_pur_in_stock.ForeColor = Color.Red;
            }
            else
            {
                txt_pur_in_stock.ForeColor = Color.White;
            }
            txt_pur_in_stock.Text = result.Item1.ToString();
            txt_pur_pkg.Text = result.Item2.ToString();
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
        }

        private void BunifuFlatButton5_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbtrc;
        }

        private void CodeTrace_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbl_trc.DataSource = trc.filterCodeTrace(txt_trc_code.Text);
        }

        private void CodeTrace_TextChanged(object sender, EventArgs e)
        {
            if (txt_trc_code.Text == "")
            {
                tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
            }
        }

        private void BunifuFlatButton7_Click(object sender, EventArgs e)
        {
            menu.Visible = false;
        }

        private void BunifuFlatButton11_Click_1(object sender, EventArgs e)
        {
            Tuple<int, float> result = inv.newInvoice(txt_inv_code.Text,
                                                       txt_inv_item.Text,
                                                       txt_inv_num.Text,
                                                       txt_inv_cust.Text,
                                                       float.Parse(txt_inv_pkg.Text),
                                                       int.Parse(txt_inv_uom.Text),
                                                       int.Parse(txt_inv_in_stock.Text),
                                                       int.Parse(txt_inv_unit.Text),
                                                       txt_pur_date.Value.ToString(),
                                                       int.Parse(txt_inv_unt_price.Text),
                                                       decimal.Parse(txt_inv_dicount.Text),
                                                       decimal.Parse(txt_inv_total_price.Text));

            MessageBox.Show("Data Saved!");
            txt_inv_code.Text = "";
            txt_inv_item.Text = "";
            txt_inv_id.Text = "";
            txt_inv_unit.Text = "";
            tbl_curr_inv.DataSource = tb.fillCurrTableInv(txt_inv_num.Text);
            tbl_all_inv.DataSource = tb.fillAllTableInv();
            txt_inv_in_stock.Text = result.Item1.ToString();
            txt_inv_pkg.Text = result.Item2.ToString();
           
            if (result.Item1 <= 0)
            {

                txt_inv_in_stock.ForeColor = Color.Red;
            }
            else
            {
                txt_inv_in_stock.ForeColor = Color.LightGreen;
            }
            

        }

        private void Txtid_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string[] result = pur.fillByID(txt_pur_id.Text);
            txt_pur_code.Text = result[0];
            txt_pur_item.Text = result[1];
            txt_pur_unit.Text = result[2];
            txt_pur_pkg.Text = result[3];
            txt_pur_date.Text = result[4];
            txt_pur_sup.Text = result[5];
            txt_pur_dlvry_num.Text = result[6];
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            Tuple<int, float> result = pur.updatePurchase(txt_pur_id.Text, txt_pur_code.Text,
                                                        txt_pur_item.Text,
                                                        txt_pur_dlvry_num.Text,
                                                        txt_pur_sup.Text,
                                                        float.Parse(txt_pur_pkg.Text),
                                                        int.Parse(txt_pur_uom.Text),
                                                        int.Parse(txt_pur_in_stock.Text),
                                                        int.Parse(txt_pur_unit.Text),
                                                        txt_pur_date.Value.ToString(),
                                                        int.Parse(txt_pur_update.Text));
            MessageBox.Show("Updated!");
            pur.fillid(txt_pur_id);
            txt_pur_code.Text = "";
            txt_pur_item.Text = "";
            txt_pur_unit.Text = "0";
            txt_pur_pkg.Text = "";
            txt_pur_pkg.Text = result.Item1.ToString();
            txt_pur_in_stock.Text = result.Item2.ToString();
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
        }

        private void BunifuFlatButton10_Click(object sender, EventArgs e)
        {
            pur.deletePurchase(txt_pur_id.Text, txt_pur_code.Text, txt_pur_item.Text, txt_pur_dlvry_num.Text,int.Parse(txt_pur_unit.Text), txt_pur_date.Value.ToString());
            MessageBox.Show("Data deleted!");
            pur.fillid(txt_pur_id);
            txt_pur_code.Text = "";
            txt_pur_item.Text = "";
            txt_pur_unit.Text = "0";
            txt_pur_pkg.Text = "";
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
        }

        private void Dlvryntnum_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
        }

        private void Dlvryntnum_TextChanged(object sender, EventArgs e)
        {
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
        }

        private void CodeTextBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dynamic result = Facility.fillFieldByCode(txt_pur_code.Text);
            txt_pur_uom.Text = result.uom;
            txt_pur_in_stock.Text = result.stock;
            txt_pur_item.Text = result.name;
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
            txt_pur_pkg.Text = (float.Parse(txt_pur_in_stock.Text) / float.Parse(txt_pur_uom.Text)).ToString();
            txt_pur_pkg.Text = (float.Parse(txt_pur_in_stock.Text) / float.Parse(txt_pur_uom.Text)).ToString();
        }

        private void Com_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dynamic result = Facility.fillFieldByName(txt_pur_item.Text);
            txt_pur_uom.Text = result.uom;
            txt_pur_in_stock.Text = result.stock;
            txt_pur_code.Text = result.code;
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
            txt_pur_pkg.Text = (float.Parse(txt_pur_in_stock.Text) / float.Parse(txt_pur_uom.Text)).ToString();
        }

        private void BunifuFlatButton20_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbpur;
        }

        private void Srchtxt_OnValueChanged_1(object sender, EventArgs e)
        {
            tbl_stck.DataSource = export.Search(txt_stock_srch.Text);
        }

        private void BunifuFlatButton9_Click_2(object sender, EventArgs e)
        {
            export.Excel(tbl_stck);
            MessageBox.Show("File Exported to excel!");
        }

        private void BunifuFlatButton19_Click(object sender, EventArgs e)
        {
            tbl_stck.DataSource = export.Search(txt_stock_srch.Text);
            menu.Visible = true;
            menu.SelectedTab = tbstk;
        }

        private void Txtidinv_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string[] result = inv.FillByID(txt_inv_id.Text);
            txt_inv_num.Text = result[0];
            txt_inv_cust.Text = result[1];
            txt_pur_code.Text = result[2];
            txt_inv_item.Text = result[3];
            txt_inv_unit.Text = result[4];
            txt_inv_update.Text = result[5];
            txt_inv_unt_price.Text = result[6];
            txt_inv_total_price.Text = result[7];
            txt_inv_dicount.Text = result[8];
            txt_inv_date.Text = result[9];
            txt_inv_payment.Text = result[10];
        }

        private void Invoice_numberTextBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            tbl_curr_inv.DataSource = tb.fillCurrTableInv(txt_inv_num.Text);
        }

        private void Invoice_numberTextBox_TextChanged(object sender, EventArgs e)
        {
            tbl_curr_inv.DataSource = tb.fillCurrTableInv(txt_inv_num.Text);
        }

        private void CustomerTextBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
           txt_inv_unt_price.Text= inv.CustPrice(txt_inv_cust.Text, txt_inv_code.Text).ToString();
        }

        private void CodeTextBox1inv_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dynamic result = Facility.fillFieldByCode(txt_inv_code.Text);
            txt_inv_uom.Text = result.uom;
            txt_inv_in_stock.Text = result.stock;
            txt_inv_item.Text = result.name;
            tbl_curr_inv.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
            txt_inv_pkg.Text = (float.Parse(txt_inv_in_stock.Text) / float.Parse(txt_inv_uom.Text)).ToString();
            txt_inv_unt_price.Text = inv.CustPrice(txt_inv_cust.Text, txt_inv_code.Text).ToString();
        }

        private void Cominv_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dynamic result = Facility.fillFieldByName(txt_inv_item.Text);
            txt_inv_uom.Text = result.uom;
            txt_inv_in_stock.Text = result.stock;
            txt_inv_code.Text = result.code;
            tbl_curr_inv.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
            txt_inv_pkg.Text = (float.Parse(txt_inv_in_stock.Text) / float.Parse(txt_inv_uom.Text)).ToString();
            txt_inv_unt_price.Text = inv.CustPrice(txt_inv_cust.Text, txt_inv_code.Text).ToString();
        }

        private void BunifuFlatButton12_Click_1(object sender, EventArgs e)
        {
            Tuple<int,float> result = inv.updateInvoice(txt_inv_id.Text,
                                                       txt_inv_code.Text,
                                                       txt_inv_item.Text,
                                                       txt_inv_num.Text,
                                                       txt_inv_cust.Text,
                                                       float.Parse(txt_inv_pkg.Text),
                                                       int.Parse(txt_inv_uom.Text),
                                                       int.Parse(txt_inv_in_stock.Text),
                                                       int.Parse(txt_inv_unit.Text),
                                                       txt_pur_date.Value.ToString(),
                                                       int.Parse(txt_inv_unt_price.Text),
                                                       decimal.Parse(txt_inv_dicount.Text),
                                                       decimal.Parse(txt_inv_total_price.Text),
                                                       int.Parse(txt_inv_update.Text));
            MessageBox.Show("Updated!");
            if (result.Item1 <= 0)
            {

                txt_inv_in_stock.ForeColor = Color.Red;
            }
            else
            {
                txt_inv_in_stock.ForeColor = Color.Green;
            }
            txt_inv_code.Text = "";
            txt_inv_item.Text = "";
            txt_inv_id.Text = "";
            txt_inv_unit.Text = "";
            txt_inv_update.Text = "";
            txt_inv_in_stock.Text = result.Item1.ToString();
            txt_inv_pkg.Text = result.Item2.ToString();
            tbl_curr_inv.DataSource = tb.fillCurrTableInv(txt_inv_num.Text);
        }

        private void BunifuFlatButton13_Click_1(object sender, EventArgs e)
        {
            inv.DeleteInvoice(txt_inv_id.Text,
                     txt_inv_code.Text,
                     txt_inv_item.Text,
                     txt_inv_num.Text,
                     txt_inv_cust.Text,
                     float.Parse(txt_inv_pkg.Text),
                     int.Parse(txt_inv_uom.Text),
                     int.Parse(txt_inv_in_stock.Text),
                     int.Parse(txt_inv_unit.Text),
                     txt_pur_date.Value.ToString(),
                     int.Parse(txt_inv_unt_price.Text),
                     decimal.Parse(txt_inv_dicount.Text),
                     decimal.Parse(txt_inv_total_price.Text));
            MessageBox.Show("Data deleted!");
            tbl_curr_pur.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
        }

        private void BunifuFlatButton18_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbinv;
        }

        private void Cominv_TextChanged(object sender, EventArgs e)
        {
            dynamic result = Facility.fillFieldByName(txt_inv_item.Text);
            txt_inv_uom.Text = result.uom;
            txt_inv_in_stock.Text = result.stock;
            txt_inv_code.Text = result.code;
            tbl_curr_inv.DataSource = tb.fillCurrTablePur(txt_pur_dlvry_num.Text);
            txt_inv_pkg.Text = (float.Parse(txt_inv_in_stock.Text) / float.Parse(txt_inv_uom.Text)).ToString();
            txt_inv_unt_price.Text = inv.CustPrice(txt_inv_cust.Text, txt_inv_code.Text).ToString();
        }

        private void CodeTrace_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            tbl_trc.DataSource = trc.filterCodeTrace(txt_trc_code.Text);
        }

        private void ItemTrace_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbl_trc.DataSource = trc.filterItemTrace(txt_trc_item.Text);
        }

        private void SupTrace_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbl_trc.DataSource = trc.filterVendorTrace(txt_trc_vend.Text);
        }

        private void BunifuFlatButton17_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbtrc;
        }

        private void BunifuFlatButton9_Click_3(object sender, EventArgs e)
        {
            master.AddClient(txt_master_client.Text);
        }

        private void BunifuFlatButton11_Click_2(object sender, EventArgs e)
        {
            master.setPrice(txt_master_cust.Text, txt_master_price_code.Text, txt_master_price_item.Text, txt_master_price.Text);
        }

        private void MastCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamic result = Facility.fillFieldByCode(txt_master_price_code.Text);
            txt_master_price_item.Text = result.name;

        }

        private void MastIt_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamic result = Facility.fillFieldByName(txt_master_price_item.Text);
            txt_master_price_code.Text = result.code;
        }

        private void BunifuFlatButton16_Click(object sender, EventArgs e)
        {
            menu.Visible = true;
            menu.SelectedTab = tbmstrC;
        }

        private void BunifuFlatButton10_Click_1(object sender, EventArgs e)
        {
            master.AddItem(txt_master_item_code.Text, txt_master_item_name.Text, txt_master_item_uom.Text);
            load();
        }

        private void BunifuFlatButton14_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}