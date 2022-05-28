using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EInventory.Models;
using System.Collections;

namespace EInventory.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public static string SESSION_COOKIE = "Session_Cookie";
        public DataSet dsSession = null;
        public string strSessionID { get; set; }
        public int intUserID { get; set; }

        public int intCompanyID { get; set; }
        public string strTerminal { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string actionName = filterContext.ActionDescriptor.ActionName;
            if (filterContext.ActionParameters.Count(X => X.Key == "ID") > 0 &&
                Convert.ToString(filterContext.ActionParameters.FirstOrDefault(X => X.Key == "ID").Value) != string.Empty)
            {
                actionName = actionName + "\\" + filterContext.ActionParameters.FirstOrDefault(X => X.Key == "ID").Value;
            }
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            // If session exists
            if (!(actionName.ToLower() == "login" &&
                 controllerName.ToLower() == "account") &&
                !(actionName.ToLower() == "sessionexpired" &&
                 controllerName.ToLower() == "home"))
            {

                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies[SESSION_COOKIE];

                //if cookie exists and sessionid index is greater than zero
                if (cookie == null)
                {
                    //redirect to desired session 
                    //expiration action and controller
                    filterContext.Result =
                        RedirectToAction("SessionExpired", "Home");

                    return;
                }
                else
                {
                    strSessionID = cookie.Value;
                    LoginModel objLogin = new LoginModel();
                    DataSet dsSession = objLogin.GetSessionData(strSessionID);

                    if (dsSession != null &&
                       dsSession.Tables.Count > 0)
                    {
                        if (Convert.ToInt16(dsSession.Tables[0].Rows[0]["IsSession"]) == 1)
                        {
                            this.dsSession = dsSession;

                            this.ViewBag.UserName = Convert.ToString(dsSession.Tables[0].Rows[0]["UserName"]);
                            this.ViewBag.FullName = Convert.ToString(dsSession.Tables[0].Rows[0]["FullName"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(dsSession.Tables[0].Rows[0]["PhotoPath"])))
                            {
                                this.ViewBag.PhotoPath = "../../Shared/UserImages/" + Convert.ToString(dsSession.Tables[0].Rows[0]["PhotoPath"]);
                            }
                            else
                            {
                                if (Convert.ToString(dsSession.Tables[0].Rows[0]["Gender"]).ToLower() == "M".ToLower())
                                    this.ViewBag.PhotoPath = "../../Shared/UserImages/MaleAvatar.png";
                                else
                                    this.ViewBag.PhotoPath = "../../Shared/UserImages/FemaleAvatar.png";
                            }
                            this.ViewBag.RoleName = Convert.ToString(dsSession.Tables[0].Rows[0]["RoleName"]);

                            DataTable dtNavigation = dsSession.Tables[1];
                            DataTable dtMenuGroup = dsSession.Tables[1].AsDataView().ToTable(true, "MenuGroupID", "MenuGroupName");
                            DataRow[] drBreadCrumb = dtNavigation.Select(string.Format("ControllerName = '{0}' AND ActionName = '{1}'", controllerName, actionName));

                            string strMenuPath = string.Empty;
                            int intMenuGroup = -1;
                            string breadcrumbHTML = string.Empty;

                            intUserID = Convert.ToInt32(dsSession.Tables[0].Rows[0]["UserID"]);
                            intCompanyID = Convert.ToInt32(dsSession.Tables[0].Rows[0]["CompID"]);
                            this.ViewBag.UserID = intUserID;
                            strTerminal = "127.0.0.1";

                            if (drBreadCrumb.Length > 0)
                            {
                                strMenuPath = Convert.ToString(drBreadCrumb[0]["MenuPath"]);
                                intMenuGroup = Convert.ToInt32(drBreadCrumb[0]["MenuGroupID"]);
                                this.ViewBag.PageName = Convert.ToString(drBreadCrumb[0]["MenuName"]);
                            }

                            ArrayList strArr = new ArrayList();
                            bool IsUIStarted = false;

                            foreach (DataRow drGroup in dtMenuGroup.Rows)
                            {
                                if (Convert.ToInt32(drGroup["MenuGroupID"]) == intMenuGroup)
                                {
                                    strArr.Add("<li class=\"treeview active\">");
                                    breadcrumbHTML += "<li><a href=\"#\">" + drGroup["MenuGroupName"] + "</a></li>";
                                }
                                else
                                    strArr.Add("<li class=\"treeview\">");

                                strArr.Add("<a href=\"#\"><i class=\"fa fa-dashboard\"></i> <span>" + drGroup["MenuGroupName"].ToString() + "</span> <i class=\"fa fa-angle-left pull-right\"></i></a>");

                                DataRow[] drSel = dsSession.Tables[1].Select("MenuGroupID = " + drGroup["MenuGroupID"].ToString());

                                strArr.Add("<ul class=\"treeview-menu\">");

                                foreach (DataRow drMenu in drSel)
                                {

                                    if (drMenu["ParentMenuID"] != DBNull.Value
                                        && Convert.ToInt32(drMenu["ParentMenuID"]) == Convert.ToInt32(drMenu["MenuID"]))
                                    {
                                        if (IsUIStarted)
                                        {
                                            strArr.Add("</ul>");
                                            strArr.Add("</li>");
                                            IsUIStarted = false;
                                        }
                                    }

                                    if (drMenu["ParentMenuID"] == DBNull.Value ||
                                       Convert.ToInt32(drMenu["ParentMenuID"]) != Convert.ToInt32(drMenu["MenuID"]))
                                    {
                                        if (strMenuPath.Contains(@"\" + Convert.ToString(drMenu["MenuID"]) + @"\"))
                                        {
                                            breadcrumbHTML += "<li><a href=\"#\">" + drMenu["MenuName"] + "</a></li>";
                                            strArr.Add("<li class=\"active\" id=\"" + Convert.ToInt32(drMenu["MenuID"]).ToString() + "\"><a href=\"" + Url.Action(Convert.ToString(drMenu["ActionName"]), Convert.ToString(drMenu["ControllerName"])) + "\"><i class=\"fa fa-circle-o\"></i>" + drMenu["MenuName"].ToString() + "</a></li>");
                                        }
                                        else
                                        {
                                            strArr.Add("<li id=\"" + Convert.ToInt32(drMenu["MenuID"]).ToString() + "\"><a href=\"" + Url.Action(Convert.ToString(drMenu["ActionName"]), Convert.ToString(drMenu["ControllerName"])) + "\"><i class=\"fa fa-circle-o\"></i>" + drMenu["MenuName"].ToString() + "</a></li>");
                                        }
                                    }
                                    else
                                    {
                                        if (strMenuPath.Contains(@"\" + Convert.ToString(drMenu["MenuID"]) + @"\"))
                                        {
                                            strArr.Add("<li class=\"active\"><a href=\"\"><i class=\"fa fa-circle-o\"></i>" + drMenu["MenuName"].ToString() + "<i class=\"fa fa-angle-left pull-right\"></i></a>");
                                            breadcrumbHTML += "<li><a href=\"#\">" + drMenu["MenuName"] + "</a></li>";
                                        }
                                        else
                                            strArr.Add("<li><a href=\"\"><i class=\"fa fa-circle-o\"></i>" + drMenu["MenuName"].ToString() + "<i class=\"fa fa-angle-left pull-right\"></i></a>");

                                        strArr.Add("<ul class=\"treeview-menu\">");
                                        IsUIStarted = true;
                                    }

                                }

                                this.ViewBag.breadcrumbHTML = breadcrumbHTML;

                                if (IsUIStarted)
                                {
                                    strArr.Add("</ul>");
                                    strArr.Add("</li>");
                                    IsUIStarted = false;
                                }

                                strArr.Add("</ul>");
                                strArr.Add("</li>");
                            }

                            //strArr.Add("<li class=\"treeview\">");
                            //strArr.Add("<a href=\"#\"><i class=\"fa fa-dashboard\"></i> <span>Masters</span> <i class=\"fa fa-angle-left pull-right\"></i></a>");
                            //strArr.Add("<ul class=\"treeview-menu\">");
                            //strArr.Add("<li><a href=\"\"><i class=\"fa fa-circle-o\"></i>Company Setup</a></li>");
                            //strArr.Add("</ul>");
                            //strArr.Add("</li>");

                            string[] strFinal = strArr.ToArray(typeof(string)) as string[];

                            if (strFinal.Length > 0)
                                this.ViewBag.NavHtml = strFinal.Aggregate((acc, next) => acc + " " + next);


                            RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            RedirectToAction("LogOff", "Account");
                        }
                    }
                    else
                    {
                        RedirectToAction("LogOff", "Account");
                    }
                }
            }

            //otherwise continue with action
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            //Logging the Exception
            filterContext.ExceptionHandled = true;

            var Result = this.View("Error", new HandleErrorInfo(exception,
                filterContext.RouteData.Values["controller"].ToString(),
                filterContext.RouteData.Values["action"].ToString()));

            filterContext.Result = Result;

        }

    }
}
