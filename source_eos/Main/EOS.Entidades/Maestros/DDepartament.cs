using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Maestros
{
    public class DDepartament
    {
        public int IdDepartament { get; set; }
        public int IdEmpresa { get; set; }
        public string Departament { get; set; }
        public int? Inactivo { get; set; }

        public static List<DDepartament> ConvertToDto(DataTable dtTable)
        {
            List<DDepartament> collection = new List<DDepartament>();

            foreach (DataRow row in dtTable.Rows)
            {
                DDepartament dept = new DDepartament()
                {
                    IdDepartament = int.Parse(row["iddepartament"].ToString()),
                    IdEmpresa = int.Parse(row["idEmpresa"].ToString()),
                    Departament = row["departament"].ToString(),
                    Inactivo = row["inactivo"] != null ? int.Parse(row["inactivo"].ToString()) : 0,
                };
                collection.Add(dept);
            }

            return collection;
        }
    }
}
