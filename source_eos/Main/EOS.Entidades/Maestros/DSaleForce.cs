using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Maestros
{
    public class DSaleForce
    {
        public int IdSaleForce { get; set; }
        public int IdDepartament { get; set; }
        public int IdEmpresa { get; set; }
        public string SaleForce { get; set; }
        public int? Inactivo { get; set; }

        public static List<DSaleForce> ConvertToDto(DataTable dtTable)
        {
            List<DSaleForce> collection = new List<DSaleForce>();

            foreach (DataRow row in dtTable.Rows)
            {
                DSaleForce item = new DSaleForce()
                {
                    IdSaleForce = int.Parse(row["idsaleforce"].ToString()),
                    IdEmpresa = int.Parse(row["idEmpresa"].ToString()),
                    IdDepartament = int.Parse(row["iddepartament"].ToString()),
                    SaleForce = row["saleforce"].ToString(),
                    Inactivo = row["inactivo"] != null ? int.Parse(row["inactivo"].ToString()) : 0,
                };
                collection.Add(item);
            }

            return collection;
        }
    }

   
}
