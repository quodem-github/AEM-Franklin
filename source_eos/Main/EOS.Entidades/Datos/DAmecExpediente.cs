using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DAmecExpediente
    {
        public string IdAmecs { get; set; }
        public int IdExpediente { get; set; }

        public DAmecExpediente ConvertTo(DataTable dt)
        {
            return new DAmecExpediente()
            {
                IdExpediente = Quodem.Utility.DataLayerUtil.GetIntValue(dt.Rows[0], "idexpediente"),
                IdAmecs = Quodem.Utility.DataLayerUtil.GetStringValue(dt.Rows[0], "idamecs"),
            };
        }
    }
}
