using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisAssistant.Models.客户信息
{
    public class Khxx
    {
        public string Jxsbm { get; set; }
        public string Zgsbm { get; set; }
        public string Zgsmc { get; set; }
        public string Qlywzrbm { get; set; }
        public string Qlywzrmc { get; set; }
        public string Gmmc { get; set; }
    }
    public class FYXT_ZFGS
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public string Id { get; set; }
        /// <summary>
        /// 主公司编码
        /// </summary>
        [Description("主公司编码")]
        public string Zjxssapbm { get; set; }
        /// <summary>
        /// 主公司名称
        /// </summary>
        [Description("主公司名称")]
        public string Zjxsmc { get; set; }
        /// <summary>
        /// 副公司编码
        /// </summary>
        [Description("副公司编码")]
        public string Fgssapbm { get; set; }
        /// <summary>
        /// 副公司名称
        /// </summary>
        [Description("副公司名称")]
        public string Fgsmc { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        [Description("生效时间")]
        public string Gjsxsj { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        public string Gjjzsj { get; set; }
        /// <summary>
        /// 上次修改时间
        /// </summary>
        [Description("上次修改时间")]
        public string Scxgsj { get; set; }
    }
    public class SAP_KHZSJ
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public string Id { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        [Description("排序字段")]
        public string Pxzd { get; set; }
        /// <summary>
        /// 检索项1
        /// </summary>
        [Description("检索项1")]
        public string Jsx1 { get; set; }
        /// <summary>
        /// 检索项2
        /// </summary>
        [Description("检索项2")]
        public string Jsx2 { get; set; }
        /// <summary>
        /// 地区描述
        /// </summary>
        [Description("地区描述")]
        public string Doms { get; set; }
        /// <summary>
        /// 销售组织
        /// </summary>
        [Description("销售组织")]
        public string Xszzbm { get; set; }
        /// <summary>
        /// 销售组织
        /// </summary>
        [Description("销售组织")]
        public string Xszz { get; set; }
        /// <summary>
        /// 分销渠道
        /// </summary>
        [Description("分销渠道")]
        public string Fxqdbm { get; set; }
        /// <summary>
        /// 分销渠道
        /// </summary>
        [Description("分销渠道")]
        public string Fxqd { get; set; }
        /// <summary>
        /// 销售组
        /// </summary>
        [Description("销售组")]
        public string Xszbm { get; set; }
        /// <summary>
        /// 销售组
        /// </summary>
        [Description("销售组")]
        public string Xsz { get; set; }
        /// <summary>
        /// 销售部门
        /// </summary>
        [Description("销售部门")]
        public string Xsbmbm { get; set; }
        /// <summary>
        /// 销售部门
        /// </summary>
        [Description("销售部门")]
        public string Xsbm { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        [Description("客户编号")]
        public string Khbh { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [Description("客户名称")]
        public string Khmc { get; set; }
        /// <summary>
        /// 送达方
        /// </summary>
        [Description("送达方")]
        public string Sdf { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Description("联系电话")]
        public string Sdfljdh { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string Ljr { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Description("联系电话")]
        public string Ljdh { get; set; }
        /// <summary>
        /// 卸货地址
        /// </summary>
        [Description("卸货地址")]
        public string Xhdz { get; set; }
    }
    public class FYXT_QLYWZR
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public string Id { get; set; }
        /// <summary>
        /// 转让前客户编码
        /// </summary>
        [Description("转让前客户编码")]
        public string Zrqkhbm { get; set; }
        /// <summary>
        /// 转让前客户名称
        /// </summary>
        [Description("转让前客户名称")]
        public string Zrqkhmc { get; set; }
        /// <summary>
        /// 转让后客户编码
        /// </summary>
        [Description("转让后客户编码")]
        public string Zrhkhbm { get; set; }
        /// <summary>
        /// 转让后客户名称
        /// </summary>
        [Description("转让后客户名称")]
        public string Zrhkhmc { get; set; }
    }
}
