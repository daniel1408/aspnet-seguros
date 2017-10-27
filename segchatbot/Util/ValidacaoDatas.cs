using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace segchatbot.Util
{
    public class ValidacaoDatas
    {
        public bool DataValida()
        {
            DateTime resultado = DateTime.MinValue;

            if (DateTime.TryParse("01/01/2015", out resultado)){
                return true; 
            }

            return false; 
        }
    }
}