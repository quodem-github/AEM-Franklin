using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class GestorDocumentalDocumentoVersionCustomConverter : ITypeConverter<GestordocumentalDocumentoversion, GestorDocumentalDocumentoVersionDto>
    {
        GestorDocumentalDocumentoVersionDto ITypeConverter<GestordocumentalDocumentoversion, GestorDocumentalDocumentoVersionDto>.Convert(ResolutionContext context)
        {

            GestordocumentalDocumentoversion versionEdit = (GestordocumentalDocumentoversion)context.SourceValue;

            GestorDocumentalDocumentoVersionDto version = new GestorDocumentalDocumentoVersionDto()
            {
                Id = versionEdit.Id,
                DocumentoNoValido = versionEdit.Documentonovalido,
                IdDocumento = versionEdit.Iddocumento,
                FechaCreacion = versionEdit.Fecha != null? versionEdit.Fecha.ToString(Variables.DATE_TIME_FORMAT) : string.Empty,
                NombreOriginal = versionEdit.Nombreoriginal,
                version = versionEdit.Version,
                FechaModificacion = versionEdit.Fechamodificacion != null ? versionEdit.Fechamodificacion.ToString(Variables.DATE_TIME_FORMAT) : string.Empty,

            };

            return version;
        }
    }
}
