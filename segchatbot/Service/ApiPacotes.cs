using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using segchatbot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace segchatbot.Service
{
    public class ApiPacotes
    {
        private static string UrlPackage = "http://localhost:5000/api/Package";

        public static List<Package> GetAllPackages()
        {
            try
            {
                string responseData = Request();
                List<Package> packages;
                packages = JsonConvert.DeserializeObject<List<Package>>(responseData);
                return packages;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Request()
        {
            try
            {
                string url = string.Format(UrlPackage);
                HttpWebRequest quoteRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse quoteResponse = (HttpWebResponse)quoteRequest.GetResponse();

                StreamReader quoteResponseReader = new StreamReader(quoteResponse.GetResponseStream());

                //Objeto Json
                string responseData = quoteResponseReader.ReadToEnd();

                quoteResponseReader.Close();
                quoteRequest.GetResponse().Close();

                return responseData;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}