using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DataAnalysisAssistant
{
    public partial class 创建与导入 : UserControl
    {
        public 创建与导入()
        {
            InitializeComponent();            
        }
        public DataTable Dt{ get; set; }
        List<string> columnsMatching = new List<string>();
        static string driver = "";
        private void 创建与导入_Load(object sender, EventArgs e)
        {
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            toolStripLabel2.Text = ConstHelper.ConnString.Replace("DataSource=", "").Replace(";", "");
            SetDatas();
            if (!string.IsNullOrEmpty(ConstHelper.ConnString))
            {
                textBox1.AutoCompleteCustomSource.Clear();
                textBox1.AutoCompleteCustomSource.AddRange(SQLiteHelper.GetAllTableName());
            }
        }

        void SetDatas()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Dt;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;

            columnsMatching.Clear();
            foreach (DataColumn dc in Dt.Columns)
            {
                columnsMatching.Add(dc.ColumnName);
            }
            flowLayoutPanel1.Controls.Clear();
            listView1.Items.Clear();
            var cklst = new List<Control>();
            foreach (var cm in columnsMatching)
            {
                var lab = new Label();
                lab.Text = cm;
                lab.Padding = new Padding(2);
                cklst.Add(lab);
                listView1.Items.Add(new ListViewItem(new string[] { cm, "-->" }));
            }
            flowLayoutPanel1.Controls.AddRange(cklst.ToArray());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Dt = ExcelHelper.GetDataTable();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            SetDatas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var tablename = textBox1.Text.Trim();
                if (!string.IsNullOrEmpty(tablename))
                {
                    var isexists = SQLiteHelper.IsExists(tablename);
                    if (checkBox3.Checked)
                    {
                        if (isexists)
                        {
                            SQLiteHelper.DropTable(tablename);
                        }
                        SQLiteHelper.CreateTable(Dt, tablename);
                        textBox1.AutoCompleteCustomSource.Clear();
                        textBox1.AutoCompleteCustomSource.AddRange(SQLiteHelper.GetAllTableName());
                        MessageBox.Show("创建成功");
                    }
                    else if (!isexists)
                    {
                        SQLiteHelper.CreateTable(Dt, tablename);
                        textBox1.AutoCompleteCustomSource.Clear();
                        textBox1.AutoCompleteCustomSource.AddRange(SQLiteHelper.GetAllTableName());
                        MessageBox.Show("创建成功");
                    }
                    else
                    {
                        MessageBox.Show("表名已存在");
                    }
                }
                else
                {
                    MessageBox.Show("请输入表名");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConstHelper.ConnString))
            {
                var tablename = textBox1.Text.Trim();
                if (!string.IsNullOrEmpty(tablename))
                {
                    var dbcn = SQLiteHelper.GetColumnNames(tablename);
                    listView1.Items.Clear();
                    foreach (var n in columnsMatching)
                    {
                        listView1.Items.Add(new ListViewItem(new string[] { n, "-->", dbcn.FirstOrDefault(f => f == n) }));
                    }
                }
                else
                {
                    MessageBox.Show("请输入表名");
                }
            }
            else
            {
                MessageBox.Show("请先设置数据库");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"正在导入";
            var tablename = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(tablename))
            {
                if (SQLiteHelper.IsExists(tablename))
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    if (checkBox1.Checked)
                    {
                        SQLiteHelper.ClearTable(tablename,checkBox2.Checked);
                    }
                    var t = Task.Run(() =>
                    {
                        var rows = SQLiteHelper.BulkInsert(Dt, tablename);
                        sw.Stop();
                        Invoke((MethodInvoker)delegate
                        {
                            toolStripStatusLabel1.Text = $"成功向{tablename}导入{rows}条数据,耗时{sw.Elapsed}";
                        });
                    });
                }
                else
                {
                    MessageBox.Show("表名不存在，请先创建");
                }
            }
            else
            {
                MessageBox.Show("请输入表名");
            }
        }

        private void 创建数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                toolStripLabel2.Text = fbd.SelectedPath+"\\app.db";
            }
            var dbpath = toolStripLabel2.Text;
            ConstHelper.ConnString = $"DataSource={dbpath};";
            var result = SQLiteHelper.CreateDb();
            if (result)
            {
                toolStripStatusLabel1.Text = $"成功创建数据库文件{dbpath}";
            }
            else
            {
                toolStripStatusLabel1.Text = $"创建失败,{dbpath}已存在";
            }
        }

        private void 选择数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择文件";
            dialog.Filter = "SQLite数据库文件(*.db)|*.db";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                toolStripLabel2.Text = dialog.FileName;
                var dbpath = toolStripLabel2.Text;
                ConstHelper.ConnString = $"DataSource={dbpath};";
                textBox1.AutoCompleteCustomSource.AddRange(SQLiteHelper.GetAllTableName());
            }
        }

        private void toolStripLabel2_TextChanged(object sender, EventArgs e)
        {
            var path = toolStripLabel2.Text;
            if (!string.IsNullOrEmpty(path))
            {
                driver = $"driver={{SQLite3 ODBC Driver}};database={path};longnames=0;timeout=1000;notxn=0;dsn=SQLite3 Datasource";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(driver);
            toolStripStatusLabel1.Text = "驱动连接字符串已复制";
        }
    }
}
