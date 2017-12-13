using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace segchatbot.Models
{
    [Serializable]
    public class DadosSeguro
    {
        public bool SaidaBrasil { get; set; }
        public string EstadoSaida { get; set; }
        public string Destino { get; set; }
        public bool EsporteRadical { get; set; }
        public DateTime DataSaida { get; set; }
        public DateTime DataDesembarque { get; set; }
        public long QuantidadePessoas { get; set; }

    }
}