using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database
{
    internal class DBCreateParameter
    {
        public static void AddParameterWithValue(IDbCommand command, string parameterName, DbType dbType, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}
