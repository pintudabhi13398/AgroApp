using AgroApp.Common;
using AgroApp.Data;
using Common;
using Entities;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AgroApp.Controllers
{
    public class RoleController : BaseController
    {
        #region Context Object

        // Define Context Object To Access Database
        AgroAppEntities context = new AgroAppEntities();
        #endregion


        public ActionResult Index()
        {
            //SuccessNotification("Welcome");
            return View();
        }
        public async Task<ActionResult> KendoRead([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                if (!request.Sorts.Any())
                {
                    request.Sorts.Add(new SortDescriptor("RoleName", ListSortDirection.Ascending));
                }
                List<RoleReadModel> model = new List<RoleReadModel>();
                using (AgroAppEntities obj = new AgroAppEntities())
                {
                    model = obj.Database.SqlQuery<RoleReadModel>("SP_GetRoleRightsList").ToList();
                }

                    return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(CommonHelper.GetErrorMessage(ex));
            }
        }
        public async Task<ActionResult> RoleMenusView()
        {
            return PartialView("_RoleMenuAssignment");
        }
        public async Task<ActionResult> GetUserRightsList(DataSourceRequest request, int? RoleId=null)
        {
            try
            {
                if (RoleId != 0)
                {
                    List<AssignRoles_Result> list ;
                    //using (AgroAppEntities context = new AgroAppEntities())
                    //{
                        var RoleIdParam = new SqlParameter
                        {
                            ParameterName = "RoleId",
                            DbType = DbType.Int32,
                            Value = RoleId
                        };
                        list = context.Database.SqlQuery<AssignRoles_Result>("SP_AssignRoles @RoleId", RoleIdParam).ToList();
                    //}
                    return Json(list.ToDataSourceResult(request));
                }
                return Json(new List<AssignRoles_Result>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(CommonHelper.GetErrorMessage(ex));
            }
        }
        public async Task<ActionResult> KendoSave([DataSourceRequest] DataSourceRequest request, RoleReadModel model)
        {
            string message = string.Empty;
            int result=0;
           
            try
            {
                //AgroAppEntities context = new AgroAppEntities();

                var RoleIdParam = new SqlParameter
                {
                    ParameterName = "RoleId",
                    DbType = DbType.Int32,
                    Value = (object)model.RoleId ?? DBNull.Value
                };
                var RoleNameParam = new SqlParameter
                {
                    ParameterName = "RoleName",
                    DbType = DbType.String,
                    Value = (object)model.RoleName ?? DBNull.Value
                };
                var IsActiveParam = new SqlParameter
                {
                    ParameterName = "IsActive",
                    DbType = DbType.Boolean,
                    Value = (object)model.IsActive??DBNull.Value
                };
                               
                    result = context.Database.SqlQuery<int>("SP_RoleInsertAndUpdate @RoleId,@RoleName,@IsActive"
                        ,RoleIdParam,RoleNameParam,IsActiveParam).FirstOrDefault();
                if (result == 1)
                {
                    message = "Role Inserted SucessFully !!! ";
                    
                }
                else if(result ==2)
                {
                    message = "Role Updated SucessFully !!! ";
                    
                }
                else if(result == 3)
                {
                    message = "Role Already Exist !!! ";
                    
                }
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }
            //return Json(new[] { model }.ToDataSourceResult(request));
            return (result==3)
                ? Json(new { Message =message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet)
               : Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Success) }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> KendoDestroy([DataSourceRequest] DataSourceRequest request, RoleReadModel model)
        {
            string deleteMessage = string.Empty;
            int result = 0;
            try
            {
                //AgroAppEntities context = new AgroAppEntities();
                var RoleIdParam = new SqlParameter
                {
                    ParameterName = "RoleId",
                    DbType = DbType.Int32,
                    Value = model.RoleId
                };

                result = context.Database.SqlQuery<int>("SP_RoleMaster_DeleteRole @RoleId", RoleIdParam).FirstOrDefault();

            }
            catch (Exception ex)
            {
                deleteMessage = CommonHelper.GetErrorMessage(ex);
            }

            return Json(new[] { model }.ToDataSourceResult(request));
        }
        [HttpPost]
        public  async Task< string> SaveUserRights(string model)
        {
            try
            {
                var roleActions = JsonConvert.DeserializeObject<IEnumerable<AssignRoles_Result>>(model);

                foreach (AssignRoles_Result roleAction in roleActions)
                {
                    var IsInsertParam = new SqlParameter
                    {
                        ParameterName = "IsInsert",
                        DbType = DbType.Boolean,
                        Value = roleAction.IsInsert 
                    };

                    var IsViewParam = new SqlParameter
                    {
                        ParameterName = "IsView",
                        DbType = DbType.Boolean,
                        Value = roleAction.IsView 
                    };

                    var IsEditParam = new SqlParameter
                    {
                        ParameterName = "IsEdit",
                        DbType = DbType.Boolean,
                        Value = roleAction.IsEdit 
                    };

                    var IsDeleteParam = new SqlParameter
                    {
                        ParameterName = "IsDelete",
                        DbType = DbType.Boolean,
                        Value = roleAction.IsDelete 
                    };

                    var IsChangeStatusParam = new SqlParameter
                    {
                        ParameterName = "IsChangeStatus",
                        DbType = DbType.Boolean,
                        Value = roleAction.IsChangeStatus
                    };

                    var RoleMenuMapIdParam = new SqlParameter
                    {
                        ParameterName = "RoleMenuMapId",
                        DbType = DbType.Int32,
                        Value = roleAction.RoleMenuMapId
                    };
                    
                    //using (AgroAppEntities context = new AgroAppEntities())
                    //{
                        int message = context.Database.SqlQuery<int>
                            ("SP_SaveRoleRights @IsInsert,@IsView,@IsEdit,@IsDelete,@IsChangeStatus,@RoleMenuMapId"
                            , IsInsertParam,IsViewParam,IsEditParam,IsDeleteParam,IsChangeStatusParam,RoleMenuMapIdParam).FirstOrDefault();
                    //}
                    
                }
                //SuccessNotification("Role And Rights Updates SucessFully !!!");
                return string.Empty;
            }
            catch (Exception ex)
            {
                return CommonHelper.GetErrorMessage(ex, false);
            }
        }



    }
}