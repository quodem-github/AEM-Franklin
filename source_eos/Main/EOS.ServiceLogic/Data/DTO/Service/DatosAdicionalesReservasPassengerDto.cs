using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class DatosAdicionalesReservasPassengerDto
    {
        public int Iddatosadicionales { get; set; }

        public int? Pagodirecto { get; set; }

        public int? Ficherogenesis { get; set; }

        public int? Idtipoactividadpax { get; set; }

        public int? Idjustificaciones { get; set; }

        public int? Idpassengerlist { get; set; }

        public int? Idexpediente { get; set; }

        public double? Honorarios { get; set; }

        public string Datacreacion { get; set; }

        public string Pagosociedad { get; set; }

        public string Descripcionobjetivos { get; set; }

        public int Idtipoasistente { get; set; }

        public int Idnivelriesgo { get; set; }

    }
}
