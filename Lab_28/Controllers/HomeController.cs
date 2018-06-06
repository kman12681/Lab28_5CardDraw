using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lab_28.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Deal()
        {
            HttpWebRequest Shuffle = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1&FcstType=json");
            Shuffle.UserAgent = ".NET Framework Test Client";

            HttpWebResponse Response;

            try
            {
                Response = (HttpWebResponse)Shuffle.GetResponse();
            }
            catch (WebException e)
            {
                ViewBag.Error = "Exception";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Error = Response.StatusCode;
                ViewBag.ErrorDescription = Response.StatusDescription;
                return View();
            }

            StreamReader reader = new StreamReader(Response.GetResponseStream());
            string DeckData = reader.ReadToEnd();

            try
            {
                JObject JsonData = JObject.Parse(DeckData);
                Session["DeckID"] = JsonData["deck_id"];

            }
            catch (Exception e)
            {
                ViewBag.Error = "JSON Issue";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            return RedirectToAction("DealFive");

        }

        public ActionResult DealFive()
        {

            object ID = Session["DeckID"];
            HttpWebRequest DealFive = WebRequest.CreateHttp($"https://deckofcardsapi.com/api/deck/{ID}/draw/?count=5&FcstType=json");
            DealFive.UserAgent = ".NET Framework Test Client";

            HttpWebResponse Response;

            try
            {
                Response = (HttpWebResponse)DealFive.GetResponse();
            }
            catch (WebException e)
            {
                ViewBag.Error = "Exception";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Error = Response.StatusCode;
                ViewBag.ErrorDescription = Response.StatusDescription;
                return View();
            }

            StreamReader reader = new StreamReader(Response.GetResponseStream());
            string DeckData = reader.ReadToEnd();

            try
            {
                JObject JsonData = JObject.Parse(DeckData);
                ViewBag.Cards = JsonData["cards"];
                ViewBag.Remaining = JsonData["remaining"];


            }
            catch (Exception e)
            {
                ViewBag.Error = "JSON Issue";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }
            if (ViewBag.Remaining == 0)
            {
                return RedirectToAction("Reshuffle");
            }
            else
            {

                return View();
            }

        }

        public ActionResult Reshuffle()
        {
            object ID = Session["DeckID"];
            HttpWebRequest Shuffle = WebRequest.CreateHttp($"https://deckofcardsapi.com/api/deck/{ID}/shuffle/");
            Shuffle.UserAgent = ".NET Framework Test Client";

            HttpWebResponse Response;

            try
            {
                Response = (HttpWebResponse)Shuffle.GetResponse();
            }
            catch (WebException e)
            {
                ViewBag.Error = "Exception";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Error = Response.StatusCode;
                ViewBag.ErrorDescription = Response.StatusDescription;
                return View();
            }

            StreamReader reader = new StreamReader(Response.GetResponseStream());
            string DeckData = reader.ReadToEnd();

            try
            {
                JObject JsonData = JObject.Parse(DeckData);

            }
            catch (Exception e)
            {
                ViewBag.Error = "JSON Issue";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            return RedirectToAction("DealFive");

        }

    }



}
