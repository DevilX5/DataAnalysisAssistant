using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisAssistant
{
    public static class ConvertExtend
    {
        public static DataTable ListToDt<T>(this IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            //dt.Columns.AddRange(props.Where(w => Attribute.GetCustomAttribute(w, typeof(DescriptionAttribute)) != null).Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            //foreach (var p in props)
            //{
            //    var ms = (DescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute));
            //    dt.Columns[p.Name].ColumnName = ms == null ? p.Name : ms.Description;
            //}
            return dt;
        }
        public static Task<DataTable> ListToDtAsync<T>(this IEnumerable<T> collection) => Task.Run(() => collection.ListToDt());
        public static IEnumerable<T> DtToList<T>(this DataTable dt) where T : class
        {
            var lst = new List<T>();
            var t = typeof(T);
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (DataRow dr in dt.Rows)
            {
                var o = Activator.CreateInstance(t);
                foreach (DataColumn dc in dt.Columns)
                {
                    var colname = dc.ColumnName;
                    foreach (var p in props)
                    {
                        if (colname.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            p.SetValue(o, Convert.ChangeType(dr[colname], p.PropertyType));
                        }
                    }
                }
                lst.Add(o as T);
            }
            return lst;
        }
        public static Task<IEnumerable<T>> DtToListAsync<T>(this DataTable dt) where T : class => Task.Run(() => dt.DtToList<T>());

    }
}
