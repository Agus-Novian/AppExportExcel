using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppExportExcel
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        public static string ConnectionString = "";
        public static string DataBase = "";

        private void frmLogin_Load(object sender, EventArgs e)
        {
            cmbAuthentication.SelectedIndex = 0;
            cmbServer.Items.Add(@"192.168.1.3\MSSQLMMI,1455");
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (cmbServer.Text.Trim() == "")
            {
                MessageBox.Show("Please input form required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (cmbAuthentication.SelectedIndex == 0)
                {
                    cmbDB.Items.Clear();
                    SqlConnection conn = new SqlConnection("Data Source = " + cmbServer.Text + "; Initial Catalog = Master; Integrated Security = True;");
                    SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases", conn);
                    SqlDataReader rdr;
                    try
                    {
                        conn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            cmbDB.Items.Add(rdr[0].ToString());
                        }
                        MessageBox.Show("Test connection success", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                        cmd.Dispose();
                    }
                }
                if (cmbAuthentication.SelectedIndex == 1)
                {
                    cmbDB.Items.Clear();
                    SqlConnection conn = new SqlConnection("Data Source = " + cmbServer.Text + "; Initial Catalog = Master; User ID = " + txtUID.Text + "; Password =" + txtPWD.Text + ";");
                    SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases", conn);
                    SqlDataReader rdr;
                    try
                    {
                        conn.Open();
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            cmbDB.Items.Add(rdr[0].ToString());
                        }
                        if (txtUID.Text == "pusat")
                        {
                            cmbDB.SelectedIndex = 10;
                        }
                        else if (txtUID.Text == "retail")
                        {
                            cmbDB.SelectedIndex = 7;
                        }
                        else if (txtUID.Text == "ritnas")
                        {
                            cmbDB.SelectedIndex = 8;
                        }
                        else if (txtUID.Text == "h2h")
                        {
                            cmbDB.SelectedIndex = 9;
                        }
                        MessageBox.Show("Test connection success", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                        cmd.Dispose();
                    }
                }
            }
        }

        private void cmbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAuthentication.SelectedIndex == 0)
            {
                txtUID.Enabled = false;
                txtPWD.Enabled = false;
            }
            else
            {
                txtUID.Enabled = true;
                txtPWD.Enabled = true;
            }
        }

        private void cmbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnConnect.Enabled = true;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cmbDB.Text != "")
            {
                if (cmbAuthentication.SelectedIndex == 0)
                {
                    DataBase = cmbDB.Text;
                    ConnectionString = "Data Source = " + cmbServer.Text + "; Initial Catalog = " + cmbDB.Text + "; Integrated Security = True;";
                }
                else
                {
                    DataBase = cmbDB.Text;
                    ConnectionString = "Data Source = " + cmbServer.Text + "; Initial Catalog = " + cmbDB.Text + "; User ID=" + txtUID.Text + "; Password=" + txtPWD.Text + ";";
                }
                this.Hide();
                FormMain frmMain = new FormMain(ConnectionString, DataBase);
                frmMain.Show();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close application?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                e.Cancel = true;
        }
    }
}
