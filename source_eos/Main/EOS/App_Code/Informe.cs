using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace EOS
{
    /// <summary>
    /// Summary description for Informe
    /// </summary>
    public class Informe
    {
        public Informe()
        {
            idUnidad = "0";
            idArea = "0";
            idRegion = "0";
            idDistrito = "0";
            idPeticionario = "0";
            idProducto = "0";
            idDepartament = "0";
            idSaleForce = "0";
            idDistrict = "0";
        }

        private string idDepartament;
        public string IdDepartament
        {
            get { return idDepartament; }
            set { idDepartament = value; }
        }

        private string idSaleForce;
        public string IdSaleForce
        {
            get { return idSaleForce; }
            set { idSaleForce = value; }
        }

        private string idDistrict;
        public string IdDistrict
        {
            get { return idDistrict; }
            set { idDistrict = value; }
        }

        private string departament;
        public string Departament
        {
            get { return departament; }
            set { departament = value; }
        }
        private string saleForce;
        public string SaleForce
        {
            get { return saleForce; }
            set { saleForce = value; }
        }
        private string district;
        public string District
        {
            get { return district; }
            set { district = value; }
        }

        private string idUnidad;
        public string IdUnidad
        {
            get { return idUnidad; }
            set { idUnidad = value; }
        }

        private string nomUnidad;
        public string NomUnidad
        {
            get { return nomUnidad; }
            set { nomUnidad = value; }
        }

        private string idArea;
        public string IdArea
        {
            get { return idArea; }
            set { idArea = value; }
        }

        private string codArea;
        public string CodArea
        {
            get { return codArea; }
            set { codArea = value; }
        }

        private string nomArea;
        public string NomArea
        {
            get { return nomArea; }
            set { nomArea = value; }
        }
        private string idDistrito;
        public string IdDistrito
        {
            get { return idDistrito; }
            set { idDistrito = value; }
        }

        private string idRegion;
        public string IdRegion
        {
            get { return idRegion; }
            set { idRegion = value; }
        }

        private string nomRegion;
        public string NomRegion
        {
            get { return nomRegion; }
            set { nomRegion = value; }
        }

        private string nomDistrito;
        public string NomDistrito
        {
            get { return nomDistrito; }
            set { nomDistrito = value; }
        }

        private string idPeticionario;
        public string IdPeticionario
        {
            get { return idPeticionario; }
            set { idPeticionario = value; }
        }

        private string nomPeticionario;
        public string NomPeticionario
        {
            get { return nomPeticionario; }
            set { nomPeticionario = value; }
        }

        private string idProducto;
        public string IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        private string nomProducto;
        public string NomProducto
        {
            get { return nomProducto; }
            set { nomProducto = value; }
        }

        private string idCongreso;
        public string IdCongreso
        {
            get { return idCongreso; }
            set { idCongreso = value; }
        }

        private string nombreActividad;
        public string NombreActividad
        {
            get { return nombreActividad; }
            set { nombreActividad = value; }
        }

        private string idTipoActividad;
        public string IdTipoActividad
        {
            get { return idTipoActividad; }
            set { idTipoActividad = value; }
        }

        private string tipoActividad;
        public string TipoActividad
        {
            get { return tipoActividad; }
            set { tipoActividad = value; }
        }

        private string idEspecialidadCongreso;
        public string IdEspecialidadCongreso
        {
            get { return idEspecialidadCongreso; }
            set { idEspecialidadCongreso = value; }
        }

        private string especialidadCongreso;
        public string EspecialidadCongreso
        {
            get { return especialidadCongreso; }
            set { especialidadCongreso = value; }
        }

        private List<Asistente> asistente;
        public List<Asistente> Asistente
        {
            get { return asistente; }
            set { asistente = value; }
        }

        private string codigoAmec;
        public string CodigoAmec
        {
            get { return codigoAmec; }
            set { codigoAmec = value; }
        }

        private string numPedido;
        public string NumPedido
        {
            get { return numPedido; }
            set { numPedido = value; }
        }
        //Ismael Ameller 01-02-2011 Añado Numero de expediente y numero de reserva
        private string numExpediente;
        public string NumExpediente
        {
            get { return numExpediente; }
            set { numExpediente = value; }
        }

        private string numReserva;
        public string NumReserva
        {
            get { return numReserva; }
            set { numReserva = value; }
        }
        //FIN Ismael Ameller 01-02-2011 Añado Numero de expediente y numero de reserva
        private bool amecAprobado;
        public bool AmecAprobado
        {
            get { return amecAprobado; }
            set { amecAprobado = value; }
        }

        private bool amecAprobadoSeleccionado;
        public bool AmecAprobadoSeleccionado
        {
            get { return amecAprobadoSeleccionado; }
            set { amecAprobadoSeleccionado = value; }
        }

        private string idEstadoReserva;
        public string IdEstadoReserva
        {
            get { return idEstadoReserva; }
            set { idEstadoReserva = value; }
        }

        private string estadoReserva;
        public string EstadoReserva
        {
            get { return estadoReserva; }
            set { estadoReserva = value; }
        }

        private string idEstadoExpediente;
        public string IdEstadoExpediente
        {
            get { return idEstadoExpediente; }
            set { idEstadoExpediente = value; }
        }

        private string estadoExpediente;
        public string EstadoExpediente
        {
            get { return estadoExpediente; }
            set { estadoExpediente = value; }
        }

        private bool comunicadoFI;
        public bool ComunicadoFI
        {
            get { return comunicadoFI; }
            set { comunicadoFI = value; }
        }

        private bool comunicadoFISeleccionado;
        public bool ComunicadoFISeleccionado
        {
            get { return comunicadoFISeleccionado; }
            set { comunicadoFISeleccionado = value; }
        }

        private string idValoradoFI;
        public string IdValoradoFI
        {
            get { return idValoradoFI; }
            set { idValoradoFI = value; }
        }

        private string valoradoFI;
        public string ValoradoFI
        {
            get { return valoradoFI; }
            set { valoradoFI = value; }
        }

        private string idEstadoAmec;
        public string IdEstadoAmec
        {
            get { return idEstadoAmec; }
            set { idEstadoAmec = value; }
        }

        private string estadoAmec;
        public string EstadoAmec
        {
            get { return estadoAmec; }
            set { estadoAmec = value; }
        }

        private bool pendientePedido;
        public bool PendientePedido
        {
            get { return pendientePedido; }
            set { pendientePedido = value; }
        }

        private bool pendientePedidoSeleccionado;
        public bool PendientePedidoSeleccionado
        {
            get { return pendientePedidoSeleccionado; }
            set { pendientePedidoSeleccionado = value; }
        }

        private string fechaActDesde;
        public string FechaActDesde
        {
            get { return fechaActDesde; }
            set { fechaActDesde = value; }
        }

        private string fechaActHasta;
        public string FechaActHasta
        {
            get { return fechaActHasta; }
            set { fechaActHasta = value; }
        }

        private string fechaDesde;
        public string FechaDesde
        {
            get { return fechaDesde; }
            set { fechaDesde = value; }
        }

        private string fechaHasta;
        public string FechaHasta
        {
            get { return fechaHasta; }
            set { fechaHasta = value; }
        }
        //Ismael Ameller 01-03-2011 Nueva propiedad en formato Datetime
        private DateTime? fechaDesdeDt;
        public DateTime? FechaDesdeDt
        {
            get { return fechaDesdeDt; }
            set { fechaDesdeDt = value; }
        }

        private DateTime? fechaHastaDt;
        public DateTime? FechaHastaDt
        {
            get { return fechaHastaDt; }
            set { fechaHastaDt = value; }
        }
        //FIN Ismael Ameller 01-03-2011 Nueva propiedad en formato Datetime

        private bool hojasDato;
        public bool HojasDato
        {
            get { return hojasDato; }
            set { hojasDato = value; }
        }
    }

    public class Asistente
    {
        public Asistente()
        {
        }
        //Ismael Ameller 28-02-2011 Agregamos el apellido del asistente a la información del asistente
        public Asistente(string idAssistente, string nombreAssistente, string idEspecialidadAssistente, string especialidadAssistente,string apellidoAssistente)
        {
            idAsistente = idAssistente;
            nombreAsistente = nombreAssistente;
            idEspecialidadAsistente = idEspecialidadAssistente;
            especialidadAsistente = especialidadAssistente;
            apellidoAsistente = apellidoAssistente;
        }
        //FIN Ismael Ameller 28-02-2011 Agregamos el apellido del asistente a la información del asistente
        private string idAsistente;
        public string IdAsistente
        {
            get { return idAsistente; }
            set { idAsistente = value; }
        }

        private string nombreAsistente;
        public string NombreAsistente
        {
            get { return nombreAsistente; }
            set { nombreAsistente = value; }
        }

        //Ismael Ameller 28-02-2011 Agregamos el apellido del asistente a la información del asistente
        private string apellidoAsistente;
        public string ApellidoAsistente
        {
            get { return apellidoAsistente; }
            set { apellidoAsistente = value; }
        }
        //FIN Ismael Ameller 28-02-2011 Agregamos el apellido del asistente a la información del asistente

        private string idEspecialidadAsistente;
        public string IdEspecialidadAsistente
        {
            get { return idEspecialidadAsistente; }
            set { idEspecialidadAsistente = value; }
        }

        private string especialidadAsistente;
        public string EspecialidadAsistente
        {
            get { return especialidadAsistente; }
            set { especialidadAsistente = value; }
        }
    }
}
