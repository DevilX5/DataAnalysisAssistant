using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisAssistant
{
    public class DapperHelper
    {
        private static readonly string connString = "server=192.168.120.2;user id=sa;password= Xgb123;initial catalog=XGB";
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetDbConnection()
        {
            return new SqlConnection(connString);
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <typeparam name="T">返回集合的类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化值</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null)
        {
            IEnumerable<T> _list = default(IEnumerable<T>);
            if (!string.IsNullOrEmpty(sql))
            {
                using (var conn = GetDbConnection())
                {
                    _list = conn.Query<T>(sql, param);
                }
            }
            return _list;
        }
        /// <summary>
        /// 和Query<T>的区别是 T之后返回能转换为T类型的集合，这里能返回所有。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Query(string sql, object param = null)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Query(sql, param);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            IEnumerable<T> _list = default(IEnumerable<T>);
            if (!string.IsNullOrEmpty(sql))
            {
                using (var conn = GetDbConnection())
                {
                    _list = await conn.QueryAsync<T>(sql, param);
                }
            }
            return _list;
        }
    }
}
