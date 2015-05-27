using FisionSSOExample.Models;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace FisionSSOExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // TODO: Contact Fision Client Service and request your organization's SSO login info to be filled in below
            return View(new FisionSSOModel() { FisionUserName = "", FisionUserPassword = "", UserToken = "", AdminToken = "", FisionLandingPage = FisionLandingPages.Homepage });
        }

        [HttpPost]
        public ActionResult Index(FisionSSOModel model)
        {
            if (ModelState.IsValid)
            {
                string intranetLoginUrl = "http://fisionrest.fisionsystem.com/api/RemoteLoginDirect";

                // Create a standard Basic Auth HTTP header with the valid Fision username and password
                string authenticateUserValue = string.Format("Basic {0}", EncodeTo64(string.Format("{0}:{1}", model.FisionUserName, model.FisionUserPassword)));

                // Create the HTTP POST request with the required headers
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(intranetLoginUrl);
                webRequest.Method = "POST";
                webRequest.Headers.Add("Authorization", authenticateUserValue);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.AllowAutoRedirect = false;


                // Add the data to the body of the header. This can also be replaced with JSON instead. If you elect to use JSON don't forget to update the ContentType header
                using (var writer = new StreamWriter(webRequest.GetRequestStream()))
                {
                    // This is formatted as ContentType = "application/x-www-form-urlencoded";
                    writer.Write(string.Format("LoginUserId={0}&", model.FisionUserName));
                    writer.Write(string.Format("SecurityToken={0}&", model.UserToken));
                    writer.Write(string.Format("SecurityAdmin={0}&", model.AdminToken));
                    writer.Write(string.Format("LandingPageint={0}", (int)model.FisionLandingPage));
                }

                string redirectUrl = string.Empty;

                // Get the Fision endpoint response so you can get at the Location header in the response
                try
                {
                    using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
                    {
                        redirectUrl = response.Headers[HttpResponseHeader.Location];

                        response.Close();
                    }
                }
                catch (Exception ex)
                {
                    model.ErrorMessage = string.Format("Unable to login due to the following error: {0}", ex.Message);

                    return View(model);
                }

                // Send the web client to the endpoint specified in the response header you retrieved above
                return Redirect(redirectUrl);
            }
            else
            {
                return View(model);
            }
        }
        static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}