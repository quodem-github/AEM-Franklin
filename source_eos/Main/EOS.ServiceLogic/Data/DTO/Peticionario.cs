using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class Peticionario
    {
        public int IdPeticionario { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Wein { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessCellPhone { get; set; }
        public string Email { get; set; }
        public int? IdDepartament { get; set; }
        public int? IdSalesForce { get; set; }
        public int? IdDistrict { get; set; }
        public string IdManager { get; set; }
        public int? IdPosition { get; set; }
        public int? Medico { get; set; }
        public int? Legal { get; set; }
        public int? Executive { get; set; }
        public int? Director { get; set; }
        public int? Gerente { get; set; }
        public int? Assistant { get; set; }
        public int? AltoCargo { get; set; }
        public string LevelCode { get; set; }
        public string FechaUltimaActualizacion { get; set; }
        public int Inactivo { get; set; }
        public string Company { get; set; }

    }
}
