using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class ServicioReservaActividadesReturnCustomConverter : ITypeConverter<ServiciosReservasActividadesEditDto, Serviciosreservasactividades>
    {

        Serviciosreservasactividades ITypeConverter<ServiciosReservasActividadesEditDto, Serviciosreservasactividades>.Convert(ResolutionContext context)
        {


            ServiciosReservasActividadesEditDto serEdit = (ServiciosReservasActividadesEditDto)context.SourceValue;

            Serviciosreservasactividades serviciosreservasactividades = new Serviciosreservasactividades()
            {
                Idservicioactividad = serEdit.Idservicioactividad,
                Idtarifaactividad = serEdit.Idtarifaactividad,
                Observaciones = serEdit.Observaciones,
                Pvp = serEdit.Pvp,
                Locked = serEdit.Locked,
                Sede = serEdit.Sede,
                Descripcion = serEdit.Descripcion,
                Tipo = serEdit.Tipo,
                Fechainicio = serEdit.Fechainicio == null ? DateTime.ParseExact(serEdit.Fechainicio, "yyyy-MM-dd", null) : DateTime.Parse(serEdit.Fechainicio.ToString()),
                Horainicio = serEdit.Horainicio,
                Minutosinicio = serEdit.Minutosinicio,
                Fechafin = serEdit.Fechafin == null ? DateTime.Parse(serEdit.Fechafin) : DateTime.Parse(serEdit.Fechafin.ToString()),
                Horafin = serEdit.Horafin,
                Minutosfin = serEdit.Minutosfin,
                Pax = serEdit.Pax
            };

            return serviciosreservasactividades;
        }
    }
}
