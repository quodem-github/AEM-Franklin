using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroAmecInfo
    {
        public string IdAMEC { get; set; }
        public int? IdSolicitante { get; set; }
        public int? IdCargo { get; set; }
        public int? IdCreadoPor { get; set; }
        public int? IdEstado { get; set; }
        public DateTime FechaAmecs { get; set; }
        public string Nwein { get; set; }
        public string Solicitante { get; set; }
        public int? IdTipoactividad { get; set; }
        public bool PreaprobadaMed { get; set; }
        public bool PreaprobadaNeg { get; set; }
        public bool PreaprobadaLeg { get; set; }
        public bool Farmaindustria { get; set; }
        public int? CasosClinicos { get; set; }
        public int? ParticipantesMsd { get; set; }
        public string Detallecriterios { get; set; }
        public int? IdCriterioSeleccion { get; set; }
        public bool MedicosFichero { get; set; }
        public bool Paraguas { get; set; }
        public int? DuracionHoras { get; set; }
        public int? PonentesPatrocinados { get; set; }
        public string ConceptoGastos { get; set; }
        public string CargoAdaxas { get; set; }
        public decimal ImporteGasto { get; set; }
        public int? CartasContrato { get; set; }
        public string ProgramaAmec { get; set; }
        public string UrlPrograma { get; set; }
        public string DescripcionObjetivo { get; set; }
        public string Descripcion { get; set; }
        public bool PoliticaN20 { get; set; }
        public DateTime FechaComienzo { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public string LugarSede { get; set; }
        public string CriterioEspecificado { get; set; }
        
       
        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}
