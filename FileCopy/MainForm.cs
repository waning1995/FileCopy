﻿using FileCopy.Config;
using FileCopy.Handle;
using FileCopy.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileCopy
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeDataSource();
            InitializeHandleFactory();
        }

        private void InitializeDataSource()
        {
            var options = ConfigManager.Options;
            this.dataGridView1.DataSource = new BindingList<Options>(options);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new OptionsForm();
            if(form.ShowDialog(this) == DialogResult.OK)
            {
                InitializeDataSource();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var rows = this.dataGridView1.SelectedRows.Count;
            if (rows <= 0)
                return;
            var row = this.dataGridView1.SelectedRows[0];
            this.dataGridView1.Rows.Remove(row);
            var item = row.DataBoundItem as Options;
            if (item != null)
            {
                ConfigManager.Options.Remove(item);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfigManager.Save();
            MessageBox.Show(this, "保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            this.notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void InitializeHandleFactory()
        {
            var factory = HandleFactory.Instance;
            factory.Register(ConfigManager.Options);
            factory.Start();
        }
    }
}