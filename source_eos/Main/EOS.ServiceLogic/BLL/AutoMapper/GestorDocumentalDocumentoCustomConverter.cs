using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class GestorDocumentalDocumentoCustomConverter : ITypeConverter<GestordocumentalDocumento, GestorDocumentalDocumentoDto>
    {
        GestorDocumentalDocumentoDto ITypeConverter<GestordocumentalDocumento, GestorDocumentalDocumentoDto>.Convert(ResolutionContext context)
        {
            GestordocumentalDocumento docEdit = (GestordocumentalDocumento)context.SourceValue;

            GestorDocumentalDocumentoDto doc = new GestorDocumentalDocumentoDto()
            {
                Id = docEdit.Id,
                FechaCreacion = docEdit.Fecha != null ? docEdit.Fecha.ToString(Variables.DATE_TIME_FORMAT) : string.Empty,
                FechaModificacion = docEdit.Fechamodificacion != null ? docEdit.Fechamodificacion.ToString(Variables.DATE_TIME_FORMAT) : string.Empty,
                IdAmec = docEdit.Idamec,
                IdExpediente = docEdit.Idexpediente,
                IdTipodoc = docEdit.Idtipodoc,
                IdSubTipodoc = docEdit.Idsubtipodoc,
                IdPassengerList = docEdit.Idpassengerlist,
                CampoLibre = docEdit.Campolibre
            };

            return doc;
        }
    }
}
