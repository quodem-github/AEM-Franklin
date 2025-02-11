using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class Passenger
    {
        public int Idpassengerlist { get; set; }
        public string Nombre { get; set; }
        public string apel1 { get; set; }
        public string apel2 { get; set; }
        public string NIF { get; set; }
        public string DireccionEmail { get; set; }
        public string TelefonoContacto { get; set; }
        public string FechaNacimiento { get; set; }
        public string Pasaporte { get; set; }
        public string FechaCaducidadPasaporte { get; set; }
        public string NombreCentroTrabajo { get; set; }
        public string LocalidadCentroTrabajo { get; set; }
        public string DireccionCentroTrabajo { get; set; }
        public string LugarEmisionPasaporte { get; set; }
        public string NacionalidadPasaporte { get; set; }
        public string OrganizacionVisitaProfesional { get; set; }
        public string DireccionProfesionalOrganizacion { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public string msdid { get; set; }
        public int Locked { get; set; }
        public int Idtratamiento { get; set; }
        public int Iddistrito { get; set; }
        public int Idespecialidad { get; set; }
        public string Descuentoresidente { get; set; }
        public int Idnivelriesgo { get; set; }
        public int Inactivo { get; set; }
        public string FechaUltimaActualizacion { get; set; }
        public string Centercode { get; set; }
        public int Internacional { get; set; }
        public string GoldenId { get; set; }
        public string GenesysCode { get; set; }
        public int Peticionario { get; set; }
        public int? IdpassengerlistOriginGP { get; set; }
        public int? IdpassengerlistOriginAMEX { get; set; }
        public int? IdpassengerlistOriginMT { get; set; }
    }
}
