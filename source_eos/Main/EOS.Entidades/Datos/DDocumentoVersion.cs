using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDocumentoVersion
    {
        public int IdDocumento { get; set; }
        public string IdAmec { get; set; }
        public int? IdExpediente { get; set; }
        public int IdTipoDoc { get; set; }
        public int? IdSubTipoDoc { get; set; }
        public int IdPeticionario { get; set; }
        public int? IdPassengerList { get; set; }
        public DateTime FechaDocOriginal { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaFichero { get; set; }
        public int Version { get; set; }
        public DateTime FechaDocVersion { get; set; }
        public string Metadata { get; set; }
        public bool DocumentoNoValido { get; set; }

        public static List<DDocumentoVersion> ConvertTo(DataTable dt)
        {
            List<DDocumentoVersion> list = new List<DDocumentoVersion>();

            foreach (DataRow row in dt.Rows)
            {
                string expediente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdExpediente");
                string subtipo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSubtipodoc");
                string passenger = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPassengerList");

                list.Add(new DDocumentoVersion()
                {
                    IdDocumento = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idDocumento"),
                    IdAmec = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdAmec"),
                    IdExpediente = !string.IsNullOrEmpty(expediente) ? int.Parse(expediente) : new int?(),
                    IdTipoDoc = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdTipodoc"),
                    IdSubTipoDoc = !string.IsNullOrEmpty(subtipo) ? int.Parse(subtipo) : new int?(),
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    IdPassengerList = !string.IsNullOrEmpty(passenger) ? int.Parse(passenger) : new int?(),
                    Version = Quodem.Utility.DataLayerUtil.GetIntValue(row, "version"),
                    FechaDocOriginal = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaDoc"),
                    FechaDocVersion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaVer"),
                    NombreOriginal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreOriginal"),
                    RutaFichero = Quodem.Utility.DataLayerUtil.GetStringValue(row, "RutaFichero"),
                    Metadata = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Metadata"),
                    DocumentoNoValido = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "DocumentoNoValido"),

                });
            }

            return list;
        }
    }
}
