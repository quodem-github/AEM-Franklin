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
    public class ProductoCustomConverter : ITypeConverter<ProductoEditDto, Productos>
    {

        Productos ITypeConverter<ProductoEditDto, Productos>.Convert(ResolutionContext context)
        {


            ProductoEditDto pobEdit = (ProductoEditDto) context.SourceValue;

            Productos poblaciones = new Productos()
            {
                Idproducto = pobEdit.Idproducto,
                Producto = pobEdit.Producto,
                Inactivo = pobEdit.Inactivo == null ? short.Parse("1") : short.Parse(pobEdit.Inactivo.ToString()),
                Locked = pobEdit.Locked
            };

            return poblaciones;
        }
    }
}
