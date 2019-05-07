using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtToken.EF;
using JwtToken.Models;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace JwtToken.Services
{
    public class UserService : IUserService
    {
       private DataContext _datacontext;

        public UserService(DataContext dataContext)
        {
            _datacontext = dataContext;
        }
    
        public DataTable Authenticate(string username, string password)
        {
            DataTable dsUserDetails = new DataTable();
            try
            {
                var commands = _datacontext.Database.GetDbConnection().CreateCommand();
                commands.CommandText = "[dbo].[getUserdetails_Test]";
                commands.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@Username", username);
                SqlParameter param1 = new SqlParameter("@password", password);
                commands.Parameters.Add(param);
                commands.Parameters.Add(param1);
                DbDataAdapter adapters = DbProviderFactories.GetFactory(_datacontext.Database.GetDbConnection()).CreateDataAdapter();
                adapters.SelectCommand = commands;
                adapters.Fill(dsUserDetails);
                if (dsUserDetails.Rows.Count> 0)
                {
                    dsUserDetails.TableName = "UserDetails";
                }
            }
               
            catch(Exception ex)
            {
                throw ex;
            }
            return dsUserDetails;
        }
    }
}
