using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace segchatbot
{
    public class Chave
    {
        public int mesId { get; set; }
        public string nomeMes { get; set; }
    }

    public class UnidadeRentabilidade
    {
        public int indiceId { get; set; }
        public double acumuladoMes { get; set; }
        public double acumuladoAno { get; set; }
    }

    public class Result
    {
        public IList<Chave> chaves { get; set; }

        //public IList<UnidadeRentabilidade> valores { get; set; }

        //public IList<IList<>> valores { get; set; }

        public List<List<UnidadeRentabilidade>> valores { get; set; }

        //public List<List<>> valores { get; set; }
    }

    public class Rentabilidade
    {
        public Result result { get; set; }
    }


}