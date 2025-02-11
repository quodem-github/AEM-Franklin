using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class PeticionariosListDTO
    {
        public List<Peticionario> PeticionarioList { get; set; }
        public int CurrentPageIndex { get; set; }
        public string LastUpdateDateFrom { get; set; }
        public string LastUpdateDateTo { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int? IdPeticionario { get; set; }
        public string Login { get; set; }
        public string Wein { get; set; }
        public string WeinManager { get; set; }

        public PeticionariosListDTO()
        {
            PeticionarioList = new List<Peticionario>();
        }
    }
}
