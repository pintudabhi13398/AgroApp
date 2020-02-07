using AgroApp.Data;
using Entities;
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
    public class CommonController : Controller
    {
        AgroAppEntities context = new AgroAppEntities();
        public async Task <ActionResult> GetStateList()
        {
            try
            {
                List<ListModel> model;
                var Para = 1; // For State List
                var ParaParam = new SqlParameter
                {
                    ParameterName = "Para",
                    DbType = DbType.Int32,
                    Value = Para
                };
                model = context.Database.SqlQuery<ListModel>("SP_GetCommonLists @Para",ParaParam).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }

        }
        public async Task<ActionResult> GetAllState()
        {
            try
            {
                List<ListModel> model;
                model = context.Database.SqlQuery<ListModel>("SP_Common_GetAllState").ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }

        }
        public async Task<ActionResult> GetAllDist(int?stateId)
        {
            try
            {
                var StateIdParam = new SqlParameter
                {
                    ParameterName = "StateId",
                    DbType = DbType.Int32,
                    Value = stateId
                };
                List<ListModel> model;
                model = context.Database.SqlQuery<ListModel>("SP_Common_GetAllDistrict @StateId", StateIdParam).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }

        }
        public async Task<ActionResult> GetAllRole()
        {
            try
            {
                
                List<ListModel> model;
                model = context.Database.SqlQuery<ListModel>("SP_Common_GetAllRoleList").ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }

        }
    }
}