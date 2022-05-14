using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerUserLoginService.Common
{
    public interface IUtility
    {
        public IList<T> ConvertToData<T>(DataTable dataTable);
        public T GetItem<T>(DataRow row);

    }
}
