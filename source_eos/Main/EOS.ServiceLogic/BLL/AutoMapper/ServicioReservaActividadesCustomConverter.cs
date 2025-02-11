using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class ServicioReservaActividadesCustomConverter : ITypeConverter<Serviciosreservasactividades, ServiciosReservasActividadesEditDto>
    {

        ServiciosReservasActividadesEditDto ITypeConverter<Serviciosreservasactividades, ServiciosReservasActividadesEditDto>.Convert(ResolutionContext context)
        {


            Serviciosreservasactividades serEdit = (Serviciosreservasactividades)context.SourceValue;

            ServiciosReservasActividadesEditDto serviciosreservasactividades = new ServiciosReservasActividadesEditDto()
            {


                Idservicioactividad = serEdit.Idservicioactividad,
                Idtarifaactividad = serEdit.Idtarifaactividad,
                Observaciones = serEdit.Observaciones,
                Pvp = serEdit.Pvp,
                Locked = serEdit.Locked,
                Sede = serEdit.Sede,
                Descripcion = serEdit.Descripcion,
                Tipo = serEdit.Tipo,
                Fechainicio = serEdit.Fechainicio != null ? serEdit.Fechainicio.Value.ToString("MM/dd/yyyy") : string.Empty,
                Horainicio = serEdit.Horainicio,
                Minutosinicio = serEdit.Minutosinicio,
                Fechafin = serEdit.Fechafin != null ? serEdit.Fechafin.Value.ToString("MM/dd/yyyy hh:mm:ss") : string.Empty,
                Horafin = serEdit.Horafin,
                Minutosfin = serEdit.Minutosfin,
                Pax = serEdit.Pax
            };

            return serviciosreservasactividades;
        }
    }
}
