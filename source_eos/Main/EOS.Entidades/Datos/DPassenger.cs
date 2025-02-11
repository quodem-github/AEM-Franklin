using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DPassenger
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }

        public static List<DPassenger> ConvertToDto(DataTable dt)
        {
            List<DPassenger> list = new List<DPassenger>();
            foreach (DataRow row in dt.Rows)
            {
                var nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombre") + " " +
                             Quodem.Utility.DataLayerUtil.GetStringValue(row, "apel1");
                list.Add(new DPassenger()
                {
                    Id = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Id"),
                    NombreCompleto = nombre
                });
            }
            return list;
        }
    }
}
