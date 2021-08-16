using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface ISP_Call :  IDisposable
    {
        IEnumerable<T> Returnlist<T>(string procedureName, DynamicParameters param = null);

        void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null);

        T ExcecureReturnScaler<T>(string procedureName, DynamicParameters param = null);
    }
}
