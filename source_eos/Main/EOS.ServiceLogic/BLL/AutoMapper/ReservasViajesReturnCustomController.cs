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
    public class ReservasViajesReturnCustomConverter : ITypeConverter<ReservasViajesEditDto, Reservasviajes>
    {

        Reservasviajes ITypeConverter<ReservasViajesEditDto, Reservasviajes>.Convert(ResolutionContext context)
        {


            ReservasViajesEditDto resrEdit = (ReservasViajesEditDto) context.SourceValue;

            Reservasviajes poblaciones = new Reservasviajes()
            {
                Idreserva = resrEdit.Idreserva,
                Reserva = resrEdit.Reserva,
                Fechapeticion = resrEdit.Fechapeticion == null ? DateTime.Parse(resrEdit.Fechapeticion) : DateTime.Parse(resrEdit.Fechapeticion.ToString()),
                Idestado = resrEdit.Idestado,
                Lastupd = resrEdit.Lastupd == null ? DateTime.Parse(resrEdit.Lastupd) : DateTime.Parse(resrEdit.Lastupd.ToString()),
                Lastlog = resrEdit.Lastlog,
                Idpeticionario = resrEdit.Idpeticionario,
                Observaciones = resrEdit.Observaciones,
                ObservAgencia = resrEdit.ObservAgencia,
                Mainreserva = resrEdit.Mainreserva,
                Fkidexpediente = resrEdit.Fkidexpediente,
                Locked = resrEdit.Locked
            };

            return poblaciones;
        }
    }
}
