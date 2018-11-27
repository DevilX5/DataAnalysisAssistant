using DataAnalysisAssistant.Models.客户信息;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisAssistant.Services.客户信息
{
    public class KhxxServices
    {
        private DapperHelper conn;
        public KhxxServices()
        {
            conn = new DapperHelper();
        }
        public async Task<List<Khxx>> GetKhxxes(List<Khxx> jxs)
        {
            //分批获取经销商权利义务转让信息
            var jxsbase = jxs.Where(w => !string.IsNullOrEmpty(w.Jxsbm)).GroupBy(g => new { g.Jxsbm }).Select(s => s.Key.Jxsbm);
            var jxsbms = string.Join("','", jxsbase);
            var qlsql = $"select * from FYXT_QLYWZR where Zrqkhbm in ('{jxsbms}')";
            var qllst = await conn.QueryAsync<FYXT_QLYWZR>(qlsql);
            foreach (var n in jxs)
            {
                var ql = qllst.FirstOrDefault(w => w.Zrqkhbm == n.Jxsbm);
                n.Qlywzrbm = ql == null ? n.Jxsbm : ql.Zrhkhbm;
                n.Qlywzrmc = ql == null ? "" : ql.Zrhkhmc;
            }
            //分批获取经销商主副信息
            var qlbase = jxs.Where(w => !string.IsNullOrEmpty(w.Qlywzrbm)).GroupBy(g => new { g.Qlywzrbm }).Select(s => s.Key.Qlywzrbm);
            var qls = string.Join("','", qlbase);
            var zfsql = $"select * from FYXT_ZFGS where Zjxssapbm in ('{qls}') or Fgssapbm in ('{qls}')";
            var zflst = await conn.QueryAsync<FYXT_ZFGS>(zfsql);
            foreach (var n in jxs)
            {
                var zf = zflst.OrderByDescending(o => o.Gjjzsj).FirstOrDefault(f => f.Zjxssapbm == n.Qlywzrbm || f.Fgssapbm == n.Qlywzrbm);
                n.Zgsbm = zf == null ? n.Qlywzrbm : zf.Zjxssapbm;
                n.Zgsmc = zf == null ? n.Qlywzrmc : zf.Zjxsmc;
            }
            //分批获取经销商名称信息
            var zsbase = jxs.Where(w => string.IsNullOrEmpty(w.Zgsmc)).GroupBy(g => new { g.Zgsbm }).Select(s => s.Key.Zgsbm);
            var zss = string.Join("','", zsbase);
            var zsjsql = $"select khbh,khmc from sap_khzsj where khbh in ('{zss}') group by khbh,khmc";
            var zslst = await conn.QueryAsync<SAP_KHZSJ>(zsjsql);
            foreach (var n in jxs)
            {
                var zs = zslst.FirstOrDefault(f => f.Khbh == n.Zgsbm);
                n.Gmmc = zs == null ? n.Zgsmc : zs.Khmc;
            }
            return jxs;
        }
    }
}
