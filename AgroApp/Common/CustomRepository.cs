using AgroApp.Data;
using Data.Repository;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AgroApp.Common
{
    public class CustomRepository
    {
        public static List<UserAccessPermissionModel> UserAccessPermissions(int? roleId = null, bool isSuperAdmin = false)
        {
            List<UserAccessPermissionModel> dataList;

            using (AgroAppEntities context = new AgroAppEntities())
            {
                var UserIdParam = new SqlParameter
                {
                    ParameterName = "RoleId",
                    DbType = DbType.Int32,
                    Value = roleId
                };
                var IsSuperAdminParam = new SqlParameter
                {
                    ParameterName = "IsSuperAdmin",
                    DbType = DbType.Boolean,
                    Value = (object)isSuperAdmin ?? DBNull.Value
                };

                dataList = context.Database.SqlQuery<UserAccessPermissionModel>("sp_UserAccessPermissions @RoleId,@IsSuperAdmin", UserIdParam, IsSuperAdminParam).ToList();
            }

            return dataList.ToList();
        }
        
    }
}