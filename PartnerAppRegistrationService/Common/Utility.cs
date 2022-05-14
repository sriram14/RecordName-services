using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PartnerAppRegistrationService.Common
{
    public class Utility:IUtility
    {
        public IList<T> ConvertToData<T>(DataTable dataTable)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public T GetItem<T>(DataRow row)
        {
            Type temp = typeof(T);
            T instCheck = Activator.CreateInstance<T>();
            foreach(DataColumn col in row.Table.Columns)
            {
                foreach(PropertyInfo pro in temp.GetProperties())
                {
                    if(pro.Name==col.ColumnName)
                    {
                        pro.SetValue(instCheck, System.Convert.IsDBNull(row[col.ColumnName]) ? default : row[col.ColumnName], null);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return instCheck;
        }
    }
}
