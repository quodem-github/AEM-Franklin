using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDocSubType
    {
        public int Id { get; set; }
        public int IdTipodoc { get; set; }
        public string Nombre { get; set; }
        public int Inactivo { get; set; }
        public int? IdCategoriaDocumento { get; set; }

        public static List<DDocSubType> ConvertToDto(DataTable dt)
        {
            List<DDocSubType> list = new List<DDocSubType>();
            
            foreach (DataRow row in dt.Rows)
            {
                string catDoc = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Id");
                list.Add(new DDocSubType()
                {
                    Id = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Id"),
                    IdTipodoc = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdTipodoc"),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    Inactivo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Inactivo"),
                    IdCategoriaDocumento = !string.IsNullOrEmpty(catDoc)? int.Parse(catDoc) : new int?(),
                });
            }

            return list;
        }
    }
}
