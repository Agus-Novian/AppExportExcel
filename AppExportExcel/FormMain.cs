﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppExportExcel
{
    public partial class FormMain : Form
    {
        public static string lConnectionString = "";
        public static string lDataBase = "";

        public FormMain(string ConnStr, string DB)
        {
            InitializeComponent();
            lConnectionString = ConnStr;
            lDataBase = DB;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private DataSet getDataSet()
        {
            SqlConnection conn = new SqlConnection(lConnectionString);
            conn.Open();

            string from = dtpFrom.Value.ToString("yyyy-MM-dd");
            string to = dtpTo.Value.ToString("yyyy-MM-dd");

            SqlDataAdapter da = new SqlDataAdapter("SELECT t.kode 'TRX ID', t.tgl_entri 'TGL ENTRY', t.kode_produk 'PRODUK', t.qty 'QTY', t.tujuan 'TUJUAN', t.kode_reseller 'KODE RESELLER', m.label 'MODUL', CASE WHEN t.status = 20 THEN 'Sukses' ELSE 'Gagal' END AS STATUS, t.harga_beli 'HARGA BELI', t.harga 'HARGA JUAL', t.harga - t.harga_beli 'LABA', t.sn 'SN', t.saldo_awal-t.harga 'SALDO AKHIR' FROM transaksi t INNER JOIN modul m ON t.kode_modul = m.kode WHERE t.status = 20 AND CAST(tgl_entri as date) BETWEEN '" + from + "' AND '" + to + "' ORDER BY kode_reseller", conn);

            DataSet ds = new DataSet();
            da.Fill(ds);
            ds.Tables[0].TableName = "Transaksi";

            return ds;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // Prepare the output filenames
            string timeMark = DateTime.Now.ToString("yyyyMMdd");
            string cellByCellFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Laporan_" + timeMark + ".xls");

            this.ResultListBox.Items.Clear();

            // Object to mark the times for each process
            Stopwatch stopwatch = new Stopwatch();
            this.UseWaitCursor = true;

            try
            {
                // Get the demo DataSet
                stopwatch.Start();
                DataSet demoDataSet = this.getDataSet();
                stopwatch.Stop();

                this.ResultListBox.Items.Add("-> Proccessing Export... " + stopwatch.Elapsed.ToString());
                stopwatch.Reset();

                // Use the "Copy-cell-by-cell" method
                stopwatch.Start();
                ClassExport.ExportToExcel(demoDataSet, cellByCellFilePath);
                stopwatch.Stop();

                this.ResultListBox.Items.Add("-> Export successfully " + stopwatch.Elapsed.ToString());
                stopwatch.Reset();
                MessageBox.Show("File saved in "+cellByCellFilePath+"", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            finally
            {
                this.UseWaitCursor = false;
            }
        }

    }
}
