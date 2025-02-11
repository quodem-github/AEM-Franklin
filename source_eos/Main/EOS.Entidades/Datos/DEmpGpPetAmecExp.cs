using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DEmpGpPetAmecExp
    {
        public int Id { get; set; }
        public int IdConfEmpresa { get; set; }
        public string IdEmpleadoGp { get; set; }
        public string NombreEmpleadoGp { get; set; }
        public string ApellidoEmpleadoGp { get; set; }
        public string EmailEmpleadoGp { get; set; }
        public bool Locked { get; set; }
        public int IdPeticionario { get; set; }
        public string IdAmec { get; set; }
        public int IdExpediente { get; set; }

        public static List<DEmpGpPetAmecExp> ConvertToDto(DataTable dtTable)
        {
            List<DEmpGpPetAmecExp> collection = new List<DEmpGpPetAmecExp>();
            foreach (DataRow row in dtTable.Rows)
            {
                DEmpGpPetAmecExp data = new DEmpGpPetAmecExp()
                {
                    Id = Quodem.Utility.DataLayerUtil.GetIntValue(row, "id"),
                    IdConfEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idconfempresa"),
                    IdEmpleadoGp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idempleadogp"),
                    NombreEmpleadoGp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombre"),
                    ApellidoEmpleadoGp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "apellido"),
                    EmailEmpleadoGp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "email"),
                    Locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked") == 1,
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    IdAmec = Quodem.Utility.DataLayerUtil.GetStringValue(row, "amec"),
                    IdExpediente = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idxpediente")

                };
                collection.Add(data);
            }
            return collection;
        }

    }
}
