using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Maestros
{
    public class DDistrict
    {
        public int IdDistrict { get; set; }
        public int IdSaleForce { get; set; }
        public int IdEmpresa { get; set; }
        public string District { get; set; }
        public int? Inactivo { get; set; }

        public static List<DDistrict> ConvertToDto(DataTable dtTable)
        {
            List<DDistrict> collection = new List<DDistrict>();

            foreach (DataRow row in dtTable.Rows)
            {
                DDistrict item = new DDistrict()
                {
                    IdDistrict = int.Parse(row["iddistrict"].ToString()),
                    IdSaleForce = int.Parse(row["idsaleforce"].ToString()),
                    IdEmpresa = int.Parse(row["idEmpresa"].ToString()),
                    District = row["district"].ToString(),
                    Inactivo = row["inactivo"] != null ? int.Parse(row["inactivo"].ToString()) : 0,
                };
                collection.Add(item);
            }

            return collection;
        }
    }
}
