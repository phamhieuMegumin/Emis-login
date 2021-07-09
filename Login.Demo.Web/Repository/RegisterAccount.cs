using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using Login.Demo.Web.Entities;

namespace Login.Demo.Web.Repository
{
    public class RegisterAccount
    {
        IDbConnection _dbConnection;
        IConfiguration _configuration;
        string _connectString = "Host = 47.241.69.179; Port = 3306; Database = MF858_PQHIEU_EMIS; UserId = nvmanh; Password = 12345678";
        public RegisterAccount(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public RegisterAccount()
        {

        }
        public int InsertAccount(Account account)
        {
            using (_dbConnection = new MySqlConnection(_connectString))
            {
                var sqlCommand = "Proc_AddAccount";
                return _dbConnection.Execute(sqlCommand, account, commandType: CommandType.StoredProcedure);
            };
        }
        public Account GetAccountdByUserName(string userName)
        {
            using (_dbConnection = new MySqlConnection(_connectString))
            {
                var sqlCommand = "Proc_GetAccountByUserName";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(@"m_userName", userName);
                return _dbConnection.Query<Account>(sqlCommand, dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Account GetAccountById(Guid accountId)
        {
            using (_dbConnection = new MySqlConnection(_connectString))
            {
                var sqlCommand = "Proc_GetAccountById";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(@"m_accountId", accountId);
                return _dbConnection.Query<Account>(sqlCommand, dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
        }
    }
}
