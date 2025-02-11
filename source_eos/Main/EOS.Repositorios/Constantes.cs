using System.Collections.Generic;
using EOS.Entidades.Datos;

namespace EOS.Repositorios
{
    public static class Constantes
    {
        public static string ConsultaTotalExpedientes { get; set; }

        public static ICollection<DCabeceraExpedienteAmpliado> TotalExpedientes { get; set; }

        public static string ConsultaNumExpedientes { get; set; }

        public static long NumExpedientes { get; set; }
    }
}