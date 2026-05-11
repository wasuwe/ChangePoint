using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Change_Point.Models;



namespace Change_Point.Controllers
{
    public class PdfController : Controller
    {
        Home ARR = new Home();

        public ActionResult Index()
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;
            ARR.ROLE = Session["ROLE"];

            if (Session["EMP_NO"] == null)
            {
                Session["STATUS"] = "V";
                Session["ROLE"] = "V";
                Session["WC_CODE"] = "";
                Session["GROUP_ID"] = "";

                Response.Redirect(Url.Action("index", "Login"));
            }


            //var renderer = new ChromePdfRenderer();
            //var pdf = renderer.RenderHtmlAsPdf("<h1> Hello IronPdf </h1>");
            //pdf.SaveAs(Server.MapPath("~/src/format/pixel-perfect.pdf"));

            


            ViewBag.Data = ARR;

            return View();
        }

    }


}







