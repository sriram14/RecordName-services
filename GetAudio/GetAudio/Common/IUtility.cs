using System.Collections.Generic;
using System.Data;

namespace GetAudio.Common
{
    public interface IUtility
    {
        public IList<T> ConvertToData<T>(DataTable dataTable);
        public T GetItem<T>(DataRow row);

    }
}
