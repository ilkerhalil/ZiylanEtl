using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ZiylanEtl.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly SqlConnection _sqlConnection;

        public DataAccess()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Peraport"].ConnectionString;
            _sqlConnection = new SqlConnection(connectionString);
        }
        public DataAccess(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }
       

        public IEnumerable<T> Query<T>(string sql, CommandType commandType, params object[] parameter)
        {
            return _sqlConnection.Query<T>(sql, parameter,commandType:commandType);
        }

        public int ExecuteQuery(string sql, CommandType commandType, params object[] parameter)
        {
            return _sqlConnection.Execute(sql, parameter, commandType: commandType);
        }


        #region IDisposeble

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool v)
        {
            if (v)
            {
                _sqlConnection.Dispose();
            }
        }

        #endregion

    }
}