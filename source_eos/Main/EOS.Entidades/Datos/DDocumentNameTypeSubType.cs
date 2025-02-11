using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDocumentNameTypeSubType
    {
        public string NombreOriginal { get; set; }
        public string NombreTipo { get; set; }
        public string NombreSubTipo { get; set; }
        public int IdTipo { get; set; }
        public int IdSubTipo { get; set; }

        public static List<DDocumentNameTypeSubType> ConvertToDto(DataTable dt)
        {
            List<DDocumentNameTypeSubType> list = new List<DDocumentNameTypeSubType>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DDocumentNameTypeSubType()
                {
                    NombreOriginal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreOriginal"),
                    NombreTipo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nTipo"),
                    NombreSubTipo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nSubTipo"),
                    IdTipo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idTipo"),
                    IdSubTipo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idSubTipo"),
                });
            }
            return list;
        }

    }
}
