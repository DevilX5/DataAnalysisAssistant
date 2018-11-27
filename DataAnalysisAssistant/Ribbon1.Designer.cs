namespace DataAnalysisAssistant
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.分析 = this.Factory.CreateRibbonGroup();
            this.button1 = this.Factory.CreateRibbonButton();
            this.GroupKhxx = this.Factory.CreateRibbonGroup();
            this.btnkhxx = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.分析.SuspendLayout();
            this.GroupKhxx.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.分析);
            this.tab1.Groups.Add(this.GroupKhxx);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // 分析
            // 
            this.分析.Items.Add(this.button1);
            this.分析.Label = "SQLite";
            this.分析.Name = "分析";
            // 
            // button1
            // 
            this.button1.Label = "创建与导入";
            this.button1.Name = "button1";
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // GroupKhxx
            // 
            this.GroupKhxx.Items.Add(this.btnkhxx);
            this.GroupKhxx.Label = "客户信息";
            this.GroupKhxx.Name = "GroupKhxx";
            // 
            // btnkhxx
            // 
            this.btnkhxx.Label = "获取信息";
            this.btnkhxx.Name = "btnkhxx";
            this.btnkhxx.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnkhxx_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.分析.ResumeLayout(false);
            this.分析.PerformLayout();
            this.GroupKhxx.ResumeLayout(false);
            this.GroupKhxx.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup 分析;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupKhxx;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnkhxx;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
