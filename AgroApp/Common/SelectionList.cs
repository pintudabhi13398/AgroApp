using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AgroApp.Data;
using Entities;


namespace Common
{
    public class SelectionList
    {
        public static List<RoleReadModel> RoleList()
        {
            using (AgroAppEntities Context = new AgroAppEntities())
            {
                return Context.Database.SqlQuery<RoleReadModel>("SP_RoleList").ToList();
            }
        }

        //public static List<tblAccount> AccountList()
        //{
        //    using (TransportMSEntities _dbContext = BaseContext.GetDbContext())
        //    {
        //        return _dbContext.tblAccount.OrderBy(x => x.AccountName).ToList();
        //    }
        //}
    }
}