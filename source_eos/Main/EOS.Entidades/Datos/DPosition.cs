using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DPosition
    {
        public int idposition { get; set; }
        public string position { get; set; }
        public bool executive { get; set; }
        public bool director { get; set; }
        public bool assistant { get; set; }
        public int inactivo { get; set; }

        public static ICollection<DPosition> ConvertToDto(DataTable dtTable)
        {
            ICollection<DPosition> collection = new Collection<DPosition>();
            foreach (DataRow row in dtTable.Rows)
            {
                DPosition data = new DPosition()
                {
                    idposition = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idposition"),
                    position = Quodem.Utility.DataLayerUtil.GetStringValue(row, "position"),
                    executive = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "executive"),
                    director = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "director"),
                    assistant = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "assistant"),
                    inactivo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "inactivo"),

                };
                collection.Add(data);
            }
            return collection;
        }

    }

    
}
