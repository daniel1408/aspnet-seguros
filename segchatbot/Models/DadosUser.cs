using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace segchatbot.Models
{
    [Serializable]
    public class DadosUser
    {
        public string email { get; set; }
        public string cpf { get; set; }
    }
}