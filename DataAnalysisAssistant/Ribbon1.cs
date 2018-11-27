using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
      
    }
}
