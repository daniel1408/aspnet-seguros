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
    public class PersonalPackage
    {
        public static Package GetPackage(DadosSeguro dados)
        {
            Package pacote = new Package();
            float valor = 0;

            if(dados.EsporteRadical == true)
            {
                valor = valor + 60;
            }

            switch (dados.EstadoSaida)
            {
                case "São Paulo":
                    valor = valor + 12;
                    break;
                case "Rio de Janeiro":
                    valor = valor + 15;
                    break;
                case "Minas Gerais":
                    valor = valor + 13;
                    break;
                case "Espirito Santo":
                    valor = valor + 14;
                    break;
                case "Outro":
                    valor = valor + 16;
                    break;
            }

            switch (dados.Destino)
            {
                case "Oriente Médio":
                    valor = valor + 67;
                    break;
                case "Ásia":
                    valor = valor + 52;
                    break;
                case "América do Norte":
                    valor = valor + 35;
                    break;
                case "América do Sul":
                    valor = valor + 42;
                    break;
                case "África":
                    valor = valor + 82;
                    break;
                case "Europa":
                    valor = valor + 36;
                    break;
                case "Oceania":
                    valor = valor + 31;
                    break;
            }

            TimeSpan date = dados.DataDesembarque - dados.DataSaida;
            if (date.Days < 5)
            {
                valor = valor + 60;
            }
            else
                if (date.Days > 5 && date.Days < 10)
            {
                valor = valor + 97;
            }

            else
                if (date.Days > 5 && date.Days > 10)
            {
                valor = valor + 135;
            }

            valor = valor * dados.QuantidadePessoas;
            pacote.Details = $"Estimamos seu pacote em RS {valor},00 levando em consideração os dados informados.";
            pacote.Title = "Pacote Personalizado";
            pacote.Cost = $"R$ {valor}, 00";

            return pacote;
        }
    }
}