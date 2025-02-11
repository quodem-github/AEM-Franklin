using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class PassengersListDTO
    {
        public List<Passenger> PassengerList { get; set; }
        public int CurrentPageIndex { get; set; }
        public string LastUpdateDateFrom { get; set; }
        public string LastUpdateDateTo { get; set; }
        public int? IdPassengerList { get; set; }
        public string Msdid { get; set; }
        public int? Internacional { get; set; }
        public string GoldenId { get; set; }
        public string GenesysCode { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }

        public PassengersListDTO()
        {
            PassengerList = new List<Passenger>();
        }
    }
}
