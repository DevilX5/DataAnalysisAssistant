using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAnalysisAssistant.Models.客户信息;
using DataAnalysisAssistant.Services.客户信息;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataAnalysisAssistant
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            var dt = ExcelHelper.GetDataTable();
            if (dt != null)
            {
                var tp = new 创建与导入();
                tp.Dt = dt;
                var p = Globals.ThisAddIn.CustomTaskPanes.Add(tp, "创建与导入");
                //p.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionFloating;
                tp.Dock = System.Windows.Forms.DockStyle.Fill;
                p.Width = 1000;
                p.Visible = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请选择一个以上的单元格");
            }
        }

        private async void btnkhxx_Click(object sender, RibbonControlEventArgs e)
        {
            var range = ExcelHelper.SelectRange;
            if (range != null)
            {
                Excel.Range f = ExcelHelper.App.InputBox("选择放置位置", Type: 8);
                if (!string.IsNullOrEmpty(f.Address))
                {
                    var start = f.Address.Replace("$", "").Split(':')[0];
                    var data = range.Cast<Excel.Range>().Select((s, i) =>
                    {
                        return new Khxx { Jxsbm = s.Value == null ? "" : s.Value.ToString() };
                    }).ToList();

                    var services = new KhxxServices();
                    data = await services.GetKhxxes(data);

                    var dt = await data.ListToDtAsync();
                    dt.Columns.Remove("Zgsmc");
                    dt.Columns.Remove("Qlywzrbm");
                    dt.Columns.Remove("Qlywzrmc");

                    var rows = dt.Rows.Count;
                    var cols = dt.Columns.Count;
                    var result = new object[rows, cols];
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            result[i, j] = dt.Rows[i][j].ToString();
                        }
                    }
                    //ws.Range[start].get_Resize(rows, cols).Value2 = s;
                    ExcelHelper.Worksheet.Range[start].get_Resize(dt.Rows.Count, dt.Columns.Count).Value = result;
                }
            }
        }
    }
}
