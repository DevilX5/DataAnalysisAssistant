using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace DataAnalysisAssistant
{
    public class SQLiteHelper
    {
        //private static string _connString = ConstHelper.ConnString;//Properties.Resources.ConnString;
        public static bool CreateDb()
        {
            try
            {
                var filename = ConstHelper.ConnString.Split('=')[1].Replace(";", "");
                var fi = new FileInfo(filename);
                if (!fi.Exists)
                {
                    SQLiteConnection.CreateFile(filename);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string[] GetAllTableName()
        {
            var sql = "select name from sqlite_master where type='table' and name<>'sqlite_sequence'";
            var dt = FillDataTable(sql);
            return dt.AsEnumerable().Select(n => n[0].ToString()).ToArray();
        }
        public static IEnumerable<string> GetColumnNames(string tablename)
        {
            var sql = $"pragma table_info([{tablename}])";
            var dt = FillDataTable(sql);
            return dt.AsEnumerable().Select(s => s["name"].ToString());
        }
        public static DataTable FillDataTable(string sql)
        {
            var dt = new DataTable();
            using (var conn = new SQLiteConnection(ConstHelper.ConnString))
            {
                var cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                SQLiteDataAdapter dao = new SQLiteDataAdapter(cmd);
                dao.Fill(dt);
                cmd.Dispose();
            }
            return dt;
        }
        public static int DropTable(string tablename)
        {
            var sqllst = new List<string>();
            sqllst.Add($"drop table {tablename}");
            sqllst.Add($"drop index if exists {tablename}_idx");
            return ExecuteList(sqllst);
        }
        /// <summary>
        /// 清除表数据
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="flag">索引清零</param>
        /// <returns></returns>
        public static int ClearTable(string tablename,bool flag)
        {
            var sqllst = new List<string>();
            sqllst.Add($"delete from {tablename}");
            if(flag)
            sqllst.Add($"DELETE FROM sqlite_sequence WHERE name = '{tablename}'");
            return ExecuteList(sqllst);
        }
        public static bool IsExists(string tablename)
        {
            var sql = $"SELECT COUNT(*) FROM sqlite_master where type='table' and name='{tablename}'";
            return Int32.Parse(ExecuteScalar(sql).ToString())==1;
        }
        public static object ExecuteScalar(string sql)
        {
            using (var conn = new SQLiteConnection(ConstHelper.ConnString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                return cmd.ExecuteScalar();
            }
        }

        public static int ExecuteList(List<string> sqlist)
        {
            using (var conn = new SQLiteConnection(ConstHelper.ConnString))
            {
                var count = 0;
                conn.Open();
                var cmd = conn.CreateCommand();
                var trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                foreach (var sql in sqlist)
                {
                    cmd.CommandText = sql;
                    count += cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return count;
            }
        }

        public static int Execute(string sql)
        {
            using (var conn = new SQLiteConnection(ConstHelper.ConnString))
            {
                var count = 0;
                conn.Open();
                var cmd = conn.CreateCommand();
                var trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                cmd.CommandText = sql;
                count += cmd.ExecuteNonQuery();
                trans.Commit();
                return count;
            }
        }
        public static int CreateTable(DataTable dt,string tablename)
        {
            var row = 0;
            var sql = $"CREATE TABLE {tablename}(id INTEGER PRIMARY KEY AUTOINCREMENT,";
            foreach (DataColumn dc in dt.Columns)
            {
                sql += $"{dc.ColumnName} TEXT,";
            }
            sql = sql.TrimEnd(',') + ")";
            row+=Execute(sql);
            var sqlidx = $"create index {tablename}_idx on {tablename}(id)";
            row += Execute(sqlidx);
            return row;
        }
        static string CreateInsertColumns(DataTable dt)
        {
            var result = "";
            foreach (DataColumn dc in dt.Columns)
            {
                var cname = dc.ColumnName;
                if (cname.ToUpper() != "ID")
                {
                    result += $"{cname},";
                }
            }
            return result.TrimEnd(',');
        }
        static string CreateInsertValue(DataRow dr,DataColumnCollection dcc)
        {
            var result = "(";
            foreach (DataColumn dc in dcc)
            {
                var cname = dc.ColumnName;
                if (cname.ToUpper() != "ID")
                {
                    result += $"'{dr[cname].ToString().Replace("'","")}',";
                }
            }
            return result.TrimEnd(',')+"),";
        }
        static string CreateInsertColumns<T>(T obj)
        {
            var props = obj.GetType().GetProperties();
            return string.Join(",",props.Where(w => w.Name.ToUpper() != "ID").Select(s => s.Name));
        }
        static string CreateInsertValue<T>(T obj)
        {
            var sb = "(";
            var props = obj.GetType().GetProperties().Where(w=>w.Name.ToUpper() !="ID");
            foreach (var p in props)
            {
                sb += $"'{p.GetValue(obj)}',";
            }
            sb+=sb.TrimEnd(',')+"),";
            return sb;
        }
        public static int BulkInsert(DataTable datas, string tablename)
        {
            using (var conn = new SQLiteConnection(ConstHelper.ConnString))
            {
                var count = 0;
                conn.Open();
                var cmd = conn.CreateCommand();
                var trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                string sqlhead = $"insert into {tablename}({CreateInsertColumns(datas)}) values ";
                var sqlvalue = "";
                try
                {
                    for (int i = 0; i < datas.Rows.Count; i++)
                    {
                        var obj = datas.Rows[i];
                        sqlvalue += CreateInsertValue(obj, datas.Columns);
                        if (((i + 1) % 1000) == 0 || i == (datas.Rows.Count - 1))
                        {
                            cmd.CommandText = $"{sqlhead}{sqlvalue.TrimEnd(',')}";
                            count += cmd.ExecuteNonQuery();
                            sqlvalue = "";
                        }
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    count = 0;
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
                return count;
            }
        }
        public static int BulkInsert<T>(List<T> datas,string tablename)
        {
            using (var conn = new SQLiteConnection(ConstHelper.ConnString))
            {
                var count = 0;
                conn.Open();
                var cmd = conn.CreateCommand();
                var trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                var sqlhead = $"insert into {tablename}({CreateInsertColumns(datas)}) values ";
                var sqlvalue = "";
                for (int i = 0; i < datas.Count; i++)
                {
                    var obj = datas[i];
                    sqlvalue += CreateInsertValue(obj);
                    if (((i + 1) % 1000) == 0 || i == (datas.Count - 1))
                    {
                        cmd.CommandText = $"{sqlhead}{sqlvalue.TrimEnd(',')}";
                        //cmd.CommandText = $"insert into blogs (Url,Name) values {sb.ToString().TrimEnd(',')}";
                        count+=cmd.ExecuteNonQuery();
                        sqlvalue = "";
                    }
                }
                trans.Commit();
                return count;
            }
        }
    }
}
