using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class ReservasViajesCustomConverter : ITypeConverter<Reservasviajes, ReservasViajesEditDto>
    {

        ReservasViajesEditDto ITypeConverter<Reservasviajes, ReservasViajesEditDto>.Convert(ResolutionContext context)
        {


            Reservasviajes resEdit = (Reservasviajes)context.SourceValue;

            ReservasViajesEditDto peticionesActividad = new ReservasViajesEditDto()
            {
                Idreserva = resEdit.Idreserva,
                Reserva = resEdit.Reserva,
                Fechapeticion = resEdit.Fechapeticion.ToString(Variables.DATE_TIME_FORMAT),
                Idestado = resEdit.Idestado,
                Lastupd = resEdit.Lastupd != null ? resEdit.Lastupd.Value.ToString(Variables.DATE_TIME_FORMAT) : string.Empty,
                Lastlog = resEdit.Lastlog,
                Idpeticionario = resEdit.Idpeticionario,
                Observaciones = resEdit.Observaciones,
                ObservAgencia = resEdit.ObservAgencia,
                Mainreserva = resEdit.Mainreserva,
                Fkidexpediente = resEdit.Fkidexpediente,
                Locked = resEdit.Locked






            };

            return peticionesActividad;
        }
    }
}
