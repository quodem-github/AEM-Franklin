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
    public class PeticionActividadCustomConverter : ITypeConverter<PeticionActividadEditDto, PeticionesActividad>
    {
       
               PeticionesActividad ITypeConverter<PeticionActividadEditDto, PeticionesActividad>.Convert(ResolutionContext context)
               {


                   PeticionActividadEditDto petEdit = (PeticionActividadEditDto) context.SourceValue;

                  PeticionesActividad peticionesActividad = new PeticionesActividad()
                               { 
                                   Idpeticionactividad = petEdit.Idpeticionactividad,
                                   Nombre = petEdit.Nombre,
                                   Desde = petEdit.Desde == null ? DateTime.Parse(petEdit.Desde) : DateTime.Parse(petEdit.Desde.ToString()),
                                   Hasta = petEdit.Hasta == null ? DateTime.Parse(petEdit.Hasta) : DateTime.Parse(petEdit.Hasta.ToString()),
                                   Idpoblacion = petEdit.Idpoblacion,
                                   Idcongreso = petEdit.Idcongreso,
                                   Idespecialidad = petEdit.Idespecialidad,
                                   Idtipocongreso = petEdit.Idtipocongreso,
                                   Locked = petEdit.Locked,
                                   Internacional = petEdit.Internacional == null ? short.Parse("1") : short.Parse(petEdit.Internacional.ToString()),
                                   UrlWeb = petEdit.UrlWeb,
                                   EmailSecretaria = petEdit.EmailSecretaria,
                                   Especialidad = petEdit.Especialidad,
                                   Fechacreacion = petEdit.Fechacreacion == null ? DateTime.Parse(petEdit.Fechacreacion) : DateTime.Parse(petEdit.Fechacreacion.ToString()),
                                   Idpeticionario = petEdit.Idpeticionario,
                                   Comunicar = petEdit.Comunicar == null ? short.Parse("1") : short.Parse(petEdit.Comunicar.ToString()),
                                   ValoracionFarmaindustria = petEdit.ValoracionFarmaindustria

                      




                               }; 

                   return peticionesActividad; 
    }  
    }
}
