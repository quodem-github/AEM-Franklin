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
    public class PoblacionCustomConverter : ITypeConverter<PoblacionEditDto, Poblaciones>
    {

        Poblaciones ITypeConverter<PoblacionEditDto, Poblaciones>.Convert(ResolutionContext context)
        {
            

            PoblacionEditDto pobEdit = (PoblacionEditDto) context.SourceValue;

            Poblaciones poblaciones = new Poblaciones()
            {
                Idpoblacion = pobEdit.Idpoblacion,
                Poblacion = pobEdit.Poblacion,
                Idprovincia = pobEdit.Idprovincia,
                Idpais = pobEdit.Idpais,
                Codpostal = pobEdit.Codpostal,
                Locked = pobEdit.Locked,
                Idpaisabc = pobEdit.Idpaisabc == null ? long.Parse("1") : long.Parse(pobEdit.Idpaisabc.ToString()),
                Inactivo = pobEdit.Inactivo == null ? byte.Parse("0") : byte.Parse(pobEdit.Inactivo.ToString())
            };

            return poblaciones;
        }
    }
}
