using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonOracleDriver
{
    public class AccountRepository
    {
        public void PrintAccountDetails(string accountId)
        {
            OracleCommand command = null;
            OracleDataReader reader = null;

            try
            {
                command = new OracleCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = ACCOUNT_GET_DETAILS,
                    Connection = (OracleConnection)_dbContext.Connection,
                    BindByName = true

                };
                command.Parameters.Add(DbUtility.CreateParameter("p_account_identifier", OracleDbType.Varchar2, accountId));
                var cursor = command.Parameters.Add(DbUtility.CreateParameter("po_return_cursor", OracleDbType.RefCursor, null, ParameterDirection.Output));

                command.ExecuteNonQuery();

                reader = ((OracleRefCursor)cursor.Value).GetDataReader();

                while (reader.Read())
                {
                    account = new AccountDetailResponse();
                    ExtractAccountDetailFromReader(reader, account);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed) reader.Close();
                    reader.Dispose();
                }
                if (command != null) command.Dispose();
            }
        }
    }
}
