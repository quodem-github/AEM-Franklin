using System.Collections.Generic;

namespace EOS.Web.BLL.GestorFlujo
{
    public interface IFlujo
    {
        bool EsDifSolicitante { get; }
        bool ConParaguas { get; }
        bool EsAsistente { get; }
        bool EsDelegado { get; }
        bool EsDirector { get; }
        bool BuscarFlujo(CondicionesFlujo condiciones);
        bool SometerAmec(CondicionesFlujo condiciones, string idamec, int idPeticionario, int idCreador);
        List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario);

    }
}
