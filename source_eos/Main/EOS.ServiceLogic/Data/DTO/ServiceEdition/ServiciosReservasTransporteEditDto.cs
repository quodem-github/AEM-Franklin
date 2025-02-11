
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ServiciosReservasTransporteEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idserviciotransporte no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idserviciotransporte es obligatorio")]
        [DataMember]
        public int Idserviciotransporte { get; set; }
        [Required(ErrorMessage = "IdTipoBono_ida1 es obligatorio")]
        [DataMember(Name = "IdTipoBono_ida1")]
        public string IdtipobonoIda1 { get; set; }
        [Required(ErrorMessage = "Ida1Fechasalida es obligatorio")]
        [DataMember(Name = "ida1_fechasalida")]
        public string Ida1Fechasalida { get; set; }
        [DataMember(Name = "ida1_origen")]
        public string Ida1Origen { get; set; }
        [DataMember(Name = "ida1_destino")]
        public string Ida1Destino { get; set; }
        [DataMember(Name = "ida1_numvuelo_tren")]
        public string Ida1NumvueloTren { get; set; }
        [DataMember(Name = "ida1_horasalida")]
        public string Ida1Horasalida { get; set; }
        [DataMember(Name = "ida1_horallegada")]
        public string Ida1Horallegada { get; set; }
        [DataMember(Name = "IdTipoBono_ida2")]
        public string IdtipobonoIda2 { get; set; }
        [DataMember(Name = "ida2_fechasalida")]
        public string Ida2Fechasalida { get; set; }
        [DataMember(Name = "ida2_origen")]
        public string Ida2Origen { get; set; }
        [DataMember(Name = "ida2_destino")]
        public string Ida2Destino { get; set; }
        [DataMember(Name = "ida2_numvuelo_tren")]
        public string Ida2NumvueloTren { get; set; }
        [DataMember(Name = "ida2_horasalida")]
        public string Ida2Horasalida { get; set; }
        [DataMember(Name = "ida2_horallegada")]
        public string Ida2Horallegada { get; set; }
        [DataMember(Name = "IdTipoBono_reg1")]
        public string IdtipobonoReg1 { get; set; }
        [DataMember(Name = "reg1_fechasalida")]
        public string Reg1Fechasalida { get; set; }
        [DataMember(Name = "reg1_origen")]
        public string Reg1Origen { get; set; }
        [DataMember(Name = "reg1_destino")]
        public string Reg1Destino { get; set; }
        [DataMember(Name = "reg1_numvuelo_tren")]
        public string Reg1NumvueloTren { get; set; }
        [DataMember(Name = "reg1_horasalida")]
        public string Reg1Horasalida { get; set; }
        [DataMember(Name = "reg1_horallegada")]
        public string Reg1Horallegada { get; set; }
        [DataMember(Name = "IdTipoBono_reg2")]
        public string IdtipobonoReg2 { get; set; }
        [DataMember(Name = "reg2_fechasalida")]
        public string Reg2Fechasalida { get; set; }
        [DataMember(Name = "reg2_origen")]
        public string Reg2Origen { get; set; }
        [DataMember(Name = "reg2_destino")]
        public string Reg2Destino { get; set; }
        [DataMember(Name = "reg2_numvuelo_tren")]
        public string Reg2NumvueloTren { get; set; }
        [DataMember(Name = "reg2_horasalida")]
        public string Reg2Horasalida { get; set; }
        [DataMember(Name = "reg2_horallegada")]
        public string Reg2Horallegada { get; set; }
        [DataMember(Name = "importe_max")]
        public double? ImporteMax { get; set; }
        [DataMember(Name = "observaciones_ida")]
        public string ObservacionesIda { get; set; }
        [DataMember(Name = "observaciones_reg")]
        public string ObservacionesReg { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember(Name = "gastos_cancelacion")]
        public string GastosCancelacion { get; set; }
        [DataMember(Name = "Ida")]
        public string Ida { get; set; }
        [DataMember(Name = "Regreso")]
        public string Regreso { get; set; }
        [Range(0, 1, ErrorMessage = "locked tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        public int? Idconfempresa { get; set; }

        public  List<Serviciosreservasviajes> Serviciosreservasviajes
        {
            get;
            set;
        }

        public  List<Transportepassengerslist> Transportepassengerslists
        {
            get;
            set;
        }

        public  Confempresa Confempresa
        {
            get;
            set;
        }
    }
}