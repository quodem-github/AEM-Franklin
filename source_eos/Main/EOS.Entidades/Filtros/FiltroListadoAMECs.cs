using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroListadoAMECs
    {
        public int? year { get; set; }
        public int? mes { get; set; }
        public int? idestadoamec { get; set; }
        public int? paraguas { get; set; }
        public int? estadoAprobacion { get; set; }
        public int? IdPeticionarioUsuarioConectado { get; set; }
        public int? IdDistritoUsuarioConectado { get; set; }
        public int? IdRegionUsuarioConectado { get; set; }
        public int? IdAreaUsuarioConectado { get; set; }
        public int? IdUnidadUsuarioConectado { get; set; }
        public int? IdDistritoFiltro { get; set; }
        public int? IdRegionFiltro { get; set; }
        public int? IdAreaFiltro { get; set; }
        public int? IdUnidadFiltro { get; set; }
        public int? IdCargoUsuarioConectado { get; set; }
        public string AMECLike { get; set; }
        public string ProductoLike { get; set; }
        public string NombrePrograma { get; set; }
        public int? idsolicitante { get; set; }
        public decimal? Importe { get; set; }
        public string TipoFiltroImporte { get; set; }
        public bool Paraguas { get; set; }
        public bool? XecUnidades { get; set; }
        public string Roles { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string IdDistrict { get; set; }
        public string IdSaleForce { get; set; }
        public string IdDepartament { get; set; }
        public int IdPeticionarioSession { get; set; }

        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
        
    }
}
