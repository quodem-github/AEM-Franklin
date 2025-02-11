using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


/*Pacifico 07012011*/
namespace EOS.Entidades.Datos
{
    public class DServiciosPassengers
    {

        #region Constructores

        public DServiciosPassengers()
        {
        }


        #endregion

        #region Propiedades
        private int _idtransportepassengerlist;
        private int _idserviciotransporte;
        private int _idpassengerlist;
        private int _locked;

        public int Locked
        {
            get { return _locked; }
            set { _locked = value; }
        }

        public int Idpassengerlist
        {
            get { return _idpassengerlist; }
            set { _idpassengerlist = value; }
        }

        public int Idserviciotransporte
        {
            get { return _idserviciotransporte; }
            set { _idserviciotransporte = value; }
        }

        public int Idtransportepassengerlist
        {
            get { return _idtransportepassengerlist; }
            set { _idtransportepassengerlist = value; }
        }


        #endregion


        #region Converter

        public static ICollection<DServiciosPassengers> ConvertToDto(DataTable dtTable)
        {
            ICollection<DServiciosPassengers> collection = new Collection<DServiciosPassengers>();
            foreach (DataRow row in dtTable.Rows)
            {
                DServiciosPassengers data = new DServiciosPassengers()
                {
                    Idpassengerlist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idpassengerlist"),
                    Idserviciotransporte = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idserviciotransporte"),
                    Idtransportepassengerlist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idtransportepassengerlist"),
                    Locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Locked"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
