using AgroApp.Common;
using AgroApp.Data;
using Common;
using Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AgroApp.Controllers
{
    public class StateMasterController : Controller
    {
        #region Context Object
        // Entity Context
        AgroAppEntities context = new AgroAppEntities();
        #endregion


        // GET: StateMaster
        public async Task<ActionResult> Index()
        {
            return View();
        }
      

        #region State
        public async Task<ActionResult> StateRead([DataSourceRequest]DataSourceRequest request,string str)
        {
            try
            {
                var stateId=0;
                if(str==null)
                {
                    str = "";
                }
                var StateIdParam = new SqlParameter
                {
                    ParameterName = "StateId",
                    DbType = DbType.Int32,
                    Value = stateId
                };
                var StringParam = new SqlParameter
                {
                    ParameterName = "String",
                    DbType = DbType.String,
                    Value = str
                };
                List<StateMasterModel.State> model = new List<StateMasterModel.State>();
                
                model = context.Database.SqlQuery<StateMasterModel.State>("Sp_StateMaster_GetAllState @StateId,@String", StateIdParam,StringParam).ToList();


                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Message = CommonHelper.GetErrorMessage(ex), IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public async Task<ActionResult> StateSave([DataSourceRequest] DataSourceRequest request, StateMasterModel.State model)
        {
            string message = string.Empty;
            int result = 0;
            try
            {
                var StateIdParam = new SqlParameter
                {
                    ParameterName = "StateId",
                    DbType = DbType.Int32,
                    Value = model.StateId
                };
                var StateNameParam = new SqlParameter
                {
                    ParameterName = "StateName",
                    DbType = DbType.String,
                    Value = model.StateName
                };

                var IsActiveParam = new SqlParameter
                {
                    ParameterName = "IsActive",
                    DbType = DbType.Boolean,
                    Value = model.IsActive
                };

                 result = context.Database.SqlQuery<int>("SP_StateMaster_StateInsertUpdate @StateId,@StateName,@IsActive",StateIdParam,StateNameParam,IsActiveParam).FirstOrDefault();

               
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }
           
            return Json(new { id = result}, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> StateDestroy([DataSourceRequest] DataSourceRequest request, int? stateId)
        {
            string message = string.Empty;
            try
            {
                var StateIdParam = new SqlParameter
                {
                    ParameterName = "StateId",
                    DbType = DbType.Int32,
                    Value = (object)stateId ?? DBNull.Value
                };

                int result = context.Database.SqlQuery<int>("SP_StateMaster_DeleteState @StateId", StateIdParam).FirstOrDefault();
                if (result == 1)
                {
                    message = "State Deleted SucessFully !!! ";
                }
                else if (result == 0)
                {
                    message = "State Deleted Failed !  Try After Some Time!!! ";
                }
                return (result == 0)
                 ? Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet)
                : Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Success) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult StateWindowOpen(int? stateId)
        {
            StateMasterModel.State model = new StateMasterModel.State();
            if (stateId>0)
            {
                var StateIdParam = new SqlParameter
                {
                    ParameterName = "StateId",
                    DbType = DbType.Int32,
                    Value = stateId
                };
                model = context.Database.SqlQuery<StateMasterModel.State>("Sp_StateMaster_GetAllState @StateId", StateIdParam).FirstOrDefault();
            }
            
            return PartialView("_StateAddEdit",model);
        }
        #endregion

        #region District
        public async Task<ActionResult> DistRead([DataSourceRequest]DataSourceRequest request, string strDist)
        {
            try
            {
                var distId = 0;
                List<StateMasterModel.District> model = new List<StateMasterModel.District>();
                var DistIdParam = new SqlParameter
                {
                    ParameterName = "DistId",
                    DbType = DbType.Int32,
                    Value = distId
                };
                var StringDistParam = new SqlParameter
                {
                    ParameterName = "StringDist",
                    DbType = DbType.String,
                    Value = strDist
                };
                model = context.Database.SqlQuery<StateMasterModel.District>("Sp_DistMaster_GetAllDist @DistId,@StringDist", DistIdParam,StringDistParam).ToList();


                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Message = CommonHelper.GetErrorMessage(ex), IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> DistSave([DataSourceRequest] DataSourceRequest request, StateMasterModel.District model)
        {
            string message = string.Empty;
            int result = 0;
            try
            {
                var DistIdParam = new SqlParameter
                {
                    ParameterName = "DistId",
                    DbType = DbType.Int32,
                    Value = model.DistId
                };
                var StateIdParam = new SqlParameter
                {
                    ParameterName = "StateId",
                    DbType = DbType.Int32,
                    Value = model.StateId
                };
                var DistNameParam = new SqlParameter
                {
                    ParameterName = "DistName",
                    DbType = DbType.String,
                    Value = model.DistName
                };

                var IsActiveParam = new SqlParameter
                {
                    ParameterName = "IsActive",
                    DbType = DbType.Boolean,
                    Value = model.DistIsActive
                };

                result = context.Database.SqlQuery<int>("SP_DistrictMaster_DistrictInsertUpdate @DistId,@StateId,@DistName,@IsActive", DistIdParam, StateIdParam, DistNameParam, IsActiveParam).FirstOrDefault();


            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }
            //return Json(new[] { model }.ToDataSourceResult(request));
            return Json(new { id = result }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DistDestroy([DataSourceRequest] DataSourceRequest request, int? distId)
        {
            string message = string.Empty;
            try
            {
                var DistIdParam = new SqlParameter
                {
                    ParameterName = "DistId",
                    DbType = DbType.Int32,
                    Value = (object)distId ?? DBNull.Value
                };

                int result = context.Database.SqlQuery<int>("SP_DistMaster_DleteDistrict @DistId", DistIdParam).FirstOrDefault();
                if (result == 1)
                {
                    message = "District Deleted SucessFully !!! ";
                }
                else if (result == 0)
                {
                    message = "District Deleted Failed !  Try After Some Time!!! ";
                }
                return (result == 0)
                 ? Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet)
                : Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Success) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = message, IsError = Convert.ToString((int)Enums.NotifyType.Error) }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task< ActionResult> DistWindowOpen(int? distId)
        {
            StateMasterModel.District model = new StateMasterModel.District();
            if (distId > 0)
            {
                var str1 = "";
                var DistIdParam = new SqlParameter
                {
                    ParameterName = "DistId",
                    DbType = DbType.Int32,
                    Value = distId
                };
                var StringDistParam = new SqlParameter
                {
                    ParameterName = "StringDist",
                    DbType = DbType.String,
                    Value = str1
                };
                model = context.Database.SqlQuery<StateMasterModel.District>("Sp_DistMaster_GetAllDist @DistId,@StringDist", DistIdParam,StringDistParam).FirstOrDefault();
            }

            return PartialView("_DistrictAddEdit", model);
        }

        #endregion
    }
}