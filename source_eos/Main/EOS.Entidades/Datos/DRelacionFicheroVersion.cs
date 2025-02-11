using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DRelacionFicheroVersion
    {
        public string NombreOriginal { get; set; }
        public string NombreFicheroVersionado { get; set; }

        public static List<DRelacionFicheroVersion> ConvertTo(DataTable dt)
        {
            List<DRelacionFicheroVersion> list = new List<DRelacionFicheroVersion>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DRelacionFicheroVersion()
                {
                    NombreOriginal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreOriginal"),
                    NombreFicheroVersionado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "RutaFichero")
                });
            }

            return list;
        }
    }

    


}
