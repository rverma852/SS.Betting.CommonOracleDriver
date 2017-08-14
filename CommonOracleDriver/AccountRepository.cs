using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Configuration;
using System.Data;

namespace CommonOracleDriver
{
    public class AccountRepository
    {
        public void PrintAccountDetails(string accountId)
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            OracleDataReader reader = null;

            try
            {
                connection = new OracleConnection(ConfigurationManager.AppSettings["OracleConnectionString"]);
                command = new OracleCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "accounts_api.get_account_details",
                    Connection = connection,
                    BindByName = true

                };
                command.Parameters.Add(DbUtility.CreateParameter("p_account_identifier", OracleDbType.Varchar2, accountId));
                var cursor = command.Parameters.Add(DbUtility.CreateParameter("po_return_cursor", OracleDbType.RefCursor, null, ParameterDirection.Output));

                command.ExecuteNonQuery();

                reader = ((OracleRefCursor)cursor.Value).GetDataReader();
                while (reader.Read())
                {
                    PrintAccountDetailFromReader(reader);
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

        private void PrintAccountDetailFromReader(OracleDataReader reader)
        {
            var accountId = DbUtility.TryParse<int>(reader["accounts_id"], int.TryParse);
            Console.WriteLine($"AccountId = {accountId}");

            var accountNumber = reader["account_number"].ToString();
            Console.WriteLine($"AccountNumber = {accountNumber}");

            var version = DbUtility.TryParse<int>(reader["version_number"], int.TryParse);
            Console.WriteLine($"Version = {version}");
        }
    }
}
