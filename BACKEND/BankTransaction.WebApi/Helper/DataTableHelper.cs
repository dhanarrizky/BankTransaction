using System.Data;

namespace BankTransaction.WebApi.Helper;
public static class DataTableHelper {
    public static List<T> ConvertDataTableToList<T>(DataTable dataTable) where T : new() {
        var result = new List<T>();

        foreach (DataRow row in dataTable.Rows)
        {
            T obj = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.PropertyType.IsEnum) {
                    prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                } else {
                    prop.SetValue(obj, Enum.Parse(prop.PropertyType, row[prop.Name].ToString()));
                }
            }
            result.Add(obj);
        }

        return result;
    }

}