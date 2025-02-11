using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroExpedientesAvanzado : FiltroExpedientes
    {
        /*
        public enum TipoComparacionImporte{
            Igual,
            Menor,
            Mayor
        }
         * */

        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public string Unidad { get; set; }
        public string Producto { get; set; }
        public string AreaNegocio {get; set;}
        public string Region { get; set; }
        public string Distrito { get; set; }
        public string Peticionario { get; set; }
        public string ProveedorServicio { get; set; }
        //public TipoComparacionImporte TipoFiltroImporte { get; set; }
        public decimal? Importe { get; set; }
        public string TipoFiltroImporte { get; set; }

        public string Roles { get; set; }
        public string IdDistrict { get; set; }
        public string IdSaleForce { get; set; }
        public string IdDepartament { get; set; }
        public int IdPeticionarioSession { get; set; }
        public string TipoActividad { get; set; }
    }
}
