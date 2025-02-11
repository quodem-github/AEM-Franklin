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
    public class PeticionGruposCustomConverter : ITypeConverter<PeticionGruposEditDto, PeticionGrupos>
    {

        PeticionGrupos ITypeConverter<PeticionGruposEditDto, PeticionGrupos>.Convert(ResolutionContext context)
        {


            PeticionGruposEditDto petgEdit = (PeticionGruposEditDto)context.SourceValue;

            PeticionGrupos peticionesGrupos = new PeticionGrupos()
            {
                Idpeticiongrupo = petgEdit.Idpeticiongrupo,
                UltimaActualizacion = petgEdit.UltimaActualizacion == null ? new DateTime?() : DateTime.Parse(petgEdit.UltimaActualizacion.ToString()),
                FechaPeticion = petgEdit.FechaPeticion == null ? new DateTime?() : DateTime.Parse(petgEdit.FechaPeticion.ToString()),
                FechaInicioEvento = petgEdit.FechaInicioEvento == null ? new DateTime?() : DateTime.Parse(petgEdit.FechaInicioEvento.ToString()),
                FechaFinEvento = petgEdit.FechaFinEvento == null ? new DateTime?() : DateTime.Parse(petgEdit.FechaFinEvento.ToString()),
                FechaExpediente = petgEdit.FechaExpediente == null ? new DateTime?() : DateTime.Parse(petgEdit.FechaExpediente.ToString()),
                Idvalfi = petgEdit.Idvalfi,
                Idunidad = petgEdit.Idunidad,
                IdtipoasistenteDatosadicionales = petgEdit.IdtipoasistenteDatosadicionales,
                IdtipoactividadDatosadicionales = petgEdit.IdtipoactividadDatosadicionales,
                Idtipoactividadcongreso = petgEdit.Idtipoactividadcongreso,
                Idregion = petgEdit.Idregion,
                IdnivelriesgoDatosadicionales = petgEdit.IdnivelriesgoDatosadicionales,
                Idevento = petgEdit.Idevento,
                Iddistrito = petgEdit.Iddistrito,
                Idcargo = petgEdit.Idcargo,
                Idarea = petgEdit.Idarea,
                Fkidcongreso = petgEdit.Fkidcongreso,
                Idsaleforce = petgEdit.Idsaleforce,
                Idposition = petgEdit.Idposition,
                Idpeticionario = petgEdit.Idpeticionario,
                Idexpediente = petgEdit.Idexpediente,
                Iddistrict = petgEdit.Iddistrict,
                Iddepartament = petgEdit.Iddepartament,
                Idasistente = petgEdit.Idasistente,
                ImportePeticion = petgEdit.ImportePeticion,
                Unidad = petgEdit.Unidad,
                TipoGasto = petgEdit.TipoGasto,
                TipoasistenteDatosadicionales = petgEdit.TipoasistenteDatosadicionales,
                TipoactividadDatosadicionales = petgEdit.TipoactividadDatosadicionales,
                Region = petgEdit.Region,
                Producto = petgEdit.Producto,
                PorcentajeProdcuto = petgEdit.PorcentajeProdcuto,
                Numpedido = petgEdit.Numpedido,
                NombrePeticionario = petgEdit.NombrePeticionario,
                Nombre = petgEdit.Nombre,
                NivelriesgoDatosadicionales = petgEdit.NivelriesgoDatosadicionales,
                Msdid = petgEdit.Msdid,
                JustificacionesDatosadicionales = petgEdit.JustificacionesDatosadicionales,
                Idestadoreserva = petgEdit.Idestadoreserva,
                Idamec = petgEdit.Idamec,
                FicherogenesisDatosadicionales = petgEdit.FicherogenesisDatosadicionales,
                Evento = petgEdit.Evento,
                EstadoPeticion = petgEdit.EstadoPeticion,
                EstadoExpediente = petgEdit.EstadoExpediente,
                Distrito = petgEdit.Distrito,
                Codespecialidad = petgEdit.Codespecialidad,
                CargoPeticionario = petgEdit.CargoPeticionario,
                Area = petgEdit.Area,
                ApellidoPeticionario = petgEdit.ApellidoPeticionario,
                Apellido2 = petgEdit.Apellido2,
                Apellido1 = petgEdit.Apellido1
            };

            return peticionesGrupos;
        }
    }
}
