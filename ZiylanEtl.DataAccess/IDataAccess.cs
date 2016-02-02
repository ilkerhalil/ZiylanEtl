using System;
using System.Collections.Generic;
using System.Data;

namespace ZiylanEtl.DataAccess
{
    public interface IDataAccess : IDisposable
    {
        IEnumerable<T> Query<T>(string sql, CommandType commandType,object parameter);

        int ExecuteQuery(string sql, CommandType commandType, object parameter);

    }
}
