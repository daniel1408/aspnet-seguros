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
    public class ApiCotacao
    {
        private static string UrlMoedas = "http://api.promasters.net.br/cotacao/v1/valores";
        private static string UrlMoedaEspecifica = "http://api.promasters.net.br/cotacao/v1/valores?moedas={0}&alt=json";
        private static string UrlMoedasDisponiveis = "http://api.promasters.net.br/cotacao/v1/moedas";

        public static RootObject ValorMoeda()
        {
            try
            {
                string responseData = Request();

                RootObject moedas;
                moedas = JsonConvert.DeserializeObject<RootObject>(responseData);

                return moedas;

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
                string url = string.Format(UrlMoedas);
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
