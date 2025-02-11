using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDocType
    {
        public int Id { get; set; }
        public int IdTipo { get; set; }
        public string Nombre { get; set; }
        public int Inactivo { get; set; }
        public List<DDocSubType> SubTypes { get; set; }


        public static List<DDocType> ConvertToDto(DataTable dt)
        {
            List<DDocType> list = new List<DDocType>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DDocType()
                {
                    Id = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Id"),
                    IdTipo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdTipo"),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    Inactivo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Inactivo")
                });
            }

            return list;
        }
    }
}
