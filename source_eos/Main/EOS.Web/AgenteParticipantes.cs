using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;
using System.Collections;

namespace EOS.Web
{
    public class AgenteParticipantes : AgenteBase
    {

        public LogicaParticipantes MiLogicaParticipantes { get; set; }

        public AgenteParticipantes()
        {
            MiLogicaParticipantes = new LogicaParticipantes();
        }


        #region "Participantes"

        public ICollection<DVParticipante> ObtenerParticipantes1(string filtroID, string filtroNombre1, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            return ObtenerParticipantes(filtroID, filtroNombre1, filtroApel1, filtroApel2, filtroHospital, filtroMSDID, filtroIDDistrito, filtroIDRegion, filtroIDEmpresa, filtroIDExpediente, sortParameter, startRowIndex, maximumRows);
        }

        public ICollection<DVParticipante> ObtenerParticipantes2(string filtroID, string filtroNombre2, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            return ObtenerParticipantes(filtroID, filtroNombre2, filtroApel1, filtroApel2, filtroHospital, filtroMSDID, filtroIDDistrito, filtroIDRegion, filtroIDEmpresa, filtroIDExpediente, sortParameter, startRowIndex, maximumRows);
        }

        public ICollection<DVParticipante> ObtenerParticipantes(string filtroID, string filtroNombre, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            FiltroParticipantes filtro =
                new FiltroParticipantes()
                {
                    Nombre = filtroNombre.CambiarAsterisco(),
                    Apel1 = filtroApel1.CambiarAsterisco(),
                    Apel2 = filtroApel1.CambiarAsterisco(),
                    Hospital = filtroHospital.CambiarAsterisco(),
                    MSDID = filtroMSDID.CambiarAsterisco(),
                    IdDistrito = (string.IsNullOrEmpty(filtroIDDistrito) || filtroIDDistrito == "-1") ? new Nullable<int>() : int.Parse(filtroIDDistrito),
                    IdRegion = (string.IsNullOrEmpty(filtroIDRegion) || filtroIDRegion == "-1") ? new Nullable<int>() : int.Parse(filtroIDRegion),
                    IdEmpresa = (string.IsNullOrEmpty(filtroIDEmpresa) || filtroIDEmpresa == "-1") ? new Nullable<int>() : int.Parse(filtroIDEmpresa),
                    IdExpediente = (string.IsNullOrEmpty(filtroIDExpediente) || filtroIDExpediente == "-1") ? new Nullable<int>() : int.Parse(filtroIDExpediente),
                    Existe = "Not Exists",
                    SortParameter = sortParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows
                };

            if (Session["NuevoExpediente"] != null)
            {
                EOS.Entidades.DetalleExpediente dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];
                filtro.IdExpedienteConjunto = dExpediente.GetConjuntoIDs();
            }

            ICollection<DVParticipante> myEnumPar = MiLogicaParticipantes.ObtenerParticipantes(filtro);

            return myEnumPar;
        }

        public ICollection<DVParticipanteVeeva> ObtenerParticipantesVeeva(string filtroID, string filtroNombre, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente, string filtroIDAmecsString,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            FiltroParticipantesVeeva filtro =
                new FiltroParticipantesVeeva()
                {
                    Nombre = filtroNombre.CambiarAsterisco(),
                    Apel1 = filtroApel1.CambiarAsterisco(),
                    Apel2 = filtroApel1.CambiarAsterisco(),
                    Hospital = filtroHospital.CambiarAsterisco(),
                    MSDID = filtroMSDID.CambiarAsterisco(),
                    IdDistrito = (string.IsNullOrEmpty(filtroIDDistrito) || filtroIDDistrito == "-1") ? new Nullable<int>() : int.Parse(filtroIDDistrito),
                    IdRegion = (string.IsNullOrEmpty(filtroIDRegion) || filtroIDRegion == "-1") ? new Nullable<int>() : int.Parse(filtroIDRegion),
                    IdEmpresa = (string.IsNullOrEmpty(filtroIDEmpresa) || filtroIDEmpresa == "-1") ? new Nullable<int>() : int.Parse(filtroIDEmpresa),
                    IdExpediente = (string.IsNullOrEmpty(filtroIDExpediente) || filtroIDExpediente == "-1") ? new Nullable<int>() : int.Parse(filtroIDExpediente),
                    IdAmecString = filtroIDAmecsString,
                    Existe = "Not Exists",
                    SortParameter = sortParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows
                };

            if (Session["NuevoExpediente"] != null)
            {
                EOS.Entidades.DetalleExpediente dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];
                filtro.IdExpedienteConjunto = dExpediente.GetConjuntoIDs();
            }

            ICollection<DVParticipanteVeeva> myEnumPar = MiLogicaParticipantes.ObtenerParticipantesVeeva(filtro);

            return myEnumPar;
        }

        public int ObtenerNumeroParticipantes1(string filtroID, string filtroNombre1, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente)
        {
            return ObtenerNumeroParticipantes(filtroID, filtroNombre1, filtroApel1, filtroApel2, filtroHospital, filtroMSDID, filtroIDDistrito, filtroIDRegion, filtroIDEmpresa, filtroIDExpediente);
        }

        public int ObtenerNumeroParticipantes2(string filtroID, string filtroNombre2, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente)
        {
            return ObtenerNumeroParticipantes(filtroID, filtroNombre2, filtroApel1, filtroApel2, filtroHospital, filtroMSDID, filtroIDDistrito, filtroIDRegion, filtroIDEmpresa, filtroIDExpediente);
        }

        public int ObtenerNumeroParticipantes(string filtroID, string filtroNombre, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente)
        {
            FiltroParticipantes filtro =
                new FiltroParticipantes()
                {
                    Nombre = filtroNombre.CambiarAsterisco(),
                    Apel1 = filtroApel1.CambiarAsterisco(),
                    Apel2 = filtroApel2.CambiarAsterisco(),
                    Hospital = filtroHospital.CambiarAsterisco(),
                    MSDID = filtroMSDID.CambiarAsterisco(),
                    IdDistrito = (string.IsNullOrEmpty(filtroIDDistrito) || filtroIDDistrito == "-1") ? new Nullable<int>() : int.Parse(filtroIDDistrito),
                    IdRegion = (string.IsNullOrEmpty(filtroIDRegion) || filtroIDRegion == "-1") ? new Nullable<int>() : int.Parse(filtroIDRegion),
                    IdEmpresa = (string.IsNullOrEmpty(filtroIDEmpresa) || filtroIDEmpresa == "-1") ? new Nullable<int>() : int.Parse(filtroIDEmpresa),
                    IdExpediente = (string.IsNullOrEmpty(filtroIDExpediente) || filtroIDExpediente == "-1") ? new Nullable<int>() : int.Parse(filtroIDExpediente),
                    Existe = "Not Exists"
                };

            if (Session["NuevoExpediente"] != null)
            {
                EOS.Entidades.DetalleExpediente dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];
                filtro.IdExpedienteConjunto = dExpediente.GetConjuntoIDs();
            }

            long lNumeroParticipantes = MiLogicaParticipantes.ObtenerNumeroParticipantes(filtro);

            return Convert.ToInt32(lNumeroParticipantes);
        }

        public int ObtenerNumeroParticipantesVeeva(string filtroID, string filtroNombre, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDExpediente, string filtroIDAmecsString)
        {
            FiltroParticipantesVeeva filtro =
                new FiltroParticipantesVeeva()
                {
                    Nombre = filtroNombre.CambiarAsterisco(),
                    Apel1 = filtroApel1.CambiarAsterisco(),
                    Apel2 = filtroApel2.CambiarAsterisco(),
                    Hospital = filtroHospital.CambiarAsterisco(),
                    MSDID = filtroMSDID.CambiarAsterisco(),
                    IdDistrito = (string.IsNullOrEmpty(filtroIDDistrito) || filtroIDDistrito == "-1") ? new Nullable<int>() : int.Parse(filtroIDDistrito),
                    IdRegion = (string.IsNullOrEmpty(filtroIDRegion) || filtroIDRegion == "-1") ? new Nullable<int>() : int.Parse(filtroIDRegion),
                    IdEmpresa = (string.IsNullOrEmpty(filtroIDEmpresa) || filtroIDEmpresa == "-1") ? new Nullable<int>() : int.Parse(filtroIDEmpresa),
                    IdExpediente = (string.IsNullOrEmpty(filtroIDExpediente) || filtroIDExpediente == "-1") ? new Nullable<int>() : int.Parse(filtroIDExpediente),
                    IdAmecString = filtroIDAmecsString,
                    Existe = "Not Exists"
                };

            if (Session["NuevoExpediente"] != null)
            {
                EOS.Entidades.DetalleExpediente dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];
                filtro.IdExpedienteConjunto = dExpediente.GetConjuntoIDs();
            }

            long lNumeroParticipantes = MiLogicaParticipantes.ObtenerNumeroParticipantesVeeva(filtro);

            return Convert.ToInt32(lNumeroParticipantes);
        }

        public DVParticipante ObtenerParticipantePorID(string filtroIdPar)
        {
            DVParticipante miParticipante = null;
            if (!string.IsNullOrEmpty(filtroIdPar))
            {
                FiltroParticipantes flt = new FiltroParticipantes();
                flt.IdParticipante = (string.IsNullOrEmpty(filtroIdPar)) ? -1 : int.Parse(filtroIdPar);
                miParticipante = MiLogicaParticipantes.ObtenerParticipantes(flt).FirstOrDefault();
            }

            return miParticipante;
        }

        #endregion



        #region "Participantes ya Incluidos en el Expediente"

        public ICollection<DVParticipante> ObtenerParticipantesExpediente(string filtroIDExpediente,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            ICollection<DVParticipante> myEnumPar = null;

            if (Session["NuevoExpediente"] != null)
            {
                EOS.Entidades.DetalleExpediente dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];

                if (dExpediente.dParticipantes == null)
                {
                    FiltroParticipantes filtro =
                        new FiltroParticipantes()
                        {
                            IdExpediente = int.Parse(filtroIDExpediente),
                            IdAmec = dExpediente.nIDAMEC == null ? new int?() : int.Parse(dExpediente.nIDAMEC),
                            Existe = "Exists",
                            SortParameter = sortParameter,
                            StartRowIndex = startRowIndex,
                            MaximumRows = maximumRows
                        };

                    dExpediente.dParticipantes = MiLogicaParticipantes.ObtenerParticipantesExpediente(filtro);

                }
                myEnumPar = new Collection<DVParticipante>();
                maximumRows = 80;
                for (int i = startRowIndex; i < startRowIndex + maximumRows; i++)
                {
                    if (i < dExpediente.dParticipantes.Count)
                        myEnumPar.Add(dExpediente.dParticipantes.ElementAt(i));
                }

            }

            return myEnumPar;
        }

        public ICollection<DPassengerList> getInscripcionList(int idexp)
        {
            return MiLogicaParticipantes.getInscripcionList(idexp);
        }

        public ICollection<DPassengerList> getAlojamientoList(int idexp)
        {
            return MiLogicaParticipantes.getInscripcionList(idexp);
        }

        public ICollection<DPassengerList> getTransporteList(int idexp)
        {
            return MiLogicaParticipantes.getInscripcionList(idexp);
        }

        public ICollection<DPassengerList> getActividadesList(int idexp)
        {
            return MiLogicaParticipantes.getInscripcionList(idexp);
        }


        public int ObtenerNumeroParticipantesExpediente(string filtroIDExpediente)
        {
            int nPar = 0;

            if (Session["NuevoExpediente"] != null)
            {
                EOS.Entidades.DetalleExpediente dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];

                if (dExpediente.dParticipantes != null)
                {
                    nPar = dExpediente.dParticipantes.Count;
                }
            }
            return nPar;
        }

        public int EliminarReservasExpediente(int nIDExpediente)
        {
            return MiLogicaParticipantes.EliminarReservasExpediente(nIDExpediente);
        }

        public int EliminarReservasExpediente(int participantesEliminado, int nIDExpediente)
        {
            return MiLogicaParticipantes.EliminarReservasExpediente(participantesEliminado, nIDExpediente);
        }

        public int EliminarReservasServiciosParticipante(int nIDExpediente)
        {
            return MiLogicaParticipantes.EliminarReservasServiciosParticipante(nIDExpediente);
        }
        //Ismael Ameller 22/02/2011 Control de duplicados a la hora de dar de alta un nuevo participante
        public bool Existe_Duplicado_DNI(string dni)
        {
            return MiLogicaParticipantes.Existe_Duplicado_DNI(dni);
        }

        public bool Existe_Duplicado_Msdid(string msdid)
        {
            return MiLogicaParticipantes.Existe_Duplicado_Msdid(msdid);
        }
        //FIN Ismael Ameller 22/02/2011 Control de duplicados a la hora de dar de alta un nuevo participante
        #endregion



        #region "Participantes ya Incluidos en el AMEC"

        public ICollection<DVParticipanteAmec> ObtenerParticipantesAMEC(string filtroIDAMEC,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            ICollection<DVParticipanteAmec> myEnumPar = null;

            if (Session["NuevoDetalleAMEC"] != null)
            {
                EOS.Entidades.DetalleAMEC dAmec = (EOS.Entidades.DetalleAMEC)Session["NuevoDetalleAMEC"];

                if (dAmec.dParticipantesAmec == null)
                {
                    FiltroParticipantes filtro =
                        new FiltroParticipantes()
                        {
                            IdAmec = int.Parse(filtroIDAMEC),
                            Existe = "Exists",
                            SortParameter = sortParameter,
                            StartRowIndex = startRowIndex,
                            MaximumRows = maximumRows
                        };

                    dAmec.dParticipantesAmec = MiLogicaParticipantes.ObtenerParticipantesAMEC(filtro);
                }

                myEnumPar = dAmec.dParticipantesAmec;
            }

            return myEnumPar;
        }

        public int ObtenerNumeroParticipantesAMEC(string filtroIDAMEC)
        {
            int nPar = 0;

            if (Session["NuevoDetalleAMEC"] != null)
            {
                EOS.Entidades.DetalleAMEC dAmec = (EOS.Entidades.DetalleAMEC)Session["NuevoDetalleAMEC"];

                if (dAmec.dParticipantesAmec != null)
                {
                    nPar = dAmec.dParticipantesAmec.Count;
                }
            }
            return nPar;
        }

        public int NuevaReservaParticipante(int nIDPassengerList, int nIDExpediente)
        {
            DReservasPassengersList reserva = new DReservasPassengersList();

            reserva.idpassengerlist = nIDPassengerList;
            reserva.idxpediente = nIDExpediente;

            return MiLogicaParticipantes.NuevaReservaPassengerList(reserva);
        }

        public DVParticipanteAmec ObtenerParticipanteAMECPorID(string filtroIdPar)
        {
            DVParticipanteAmec miParticipante = null;
            if (!string.IsNullOrEmpty(filtroIdPar))
            {
                FiltroParticipantes flt = new FiltroParticipantes();
                flt.IdParticipante = (string.IsNullOrEmpty(filtroIdPar)) ? -1 : int.Parse(filtroIdPar);
                miParticipante = MiLogicaParticipantes.ObtenerParticipantesAMEC(flt).FirstOrDefault();
            }

            return miParticipante;
        }

        public int EliminarReservasAMEC(int nIDAMEC)
        {
            return MiLogicaParticipantes.EliminarReservasAMEC(nIDAMEC);
        }

        #endregion

        #region "Participantes Filtro de Búsqueda para AMEC"


        public ICollection<DVParticipante> ObtenerAMECParticipantes(string filtroID, string filtroNombre, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDAMEC,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            FiltroParticipantes filtro =
                new FiltroParticipantes()
                {
                    Nombre = filtroNombre.CambiarAsterisco(),
                    Apel1 = filtroApel1.CambiarAsterisco(),
                    Apel2 = filtroApel2.CambiarAsterisco(),
                    Hospital = filtroHospital.CambiarAsterisco(),
                    MSDID = filtroMSDID.CambiarAsterisco(),
                    IdDistrito = (string.IsNullOrEmpty(filtroIDDistrito) || filtroIDDistrito == "-1") ? new Nullable<int>() : int.Parse(filtroIDDistrito),
                    IdRegion = (string.IsNullOrEmpty(filtroIDRegion) || filtroIDRegion == "-1") ? new Nullable<int>() : int.Parse(filtroIDRegion),
                    IdEmpresa = (string.IsNullOrEmpty(filtroIDEmpresa) || filtroIDEmpresa == "-1") ? new Nullable<int>() : int.Parse(filtroIDEmpresa),
                    IdAmec = (string.IsNullOrEmpty(filtroIDAMEC) || filtroIDAMEC == "-1") ? new Nullable<int>() : int.Parse(filtroIDAMEC),
                    Existe = "Not Exists",
                    SortParameter = sortParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows
                };

            if (Session["NuevoDetalleAMEC"] != null)
            {
                EOS.Entidades.DetalleAMEC dAmec = (EOS.Entidades.DetalleAMEC)Session["NuevoDetalleAMEC"];
                filtro.IdExpedienteConjunto = dAmec.GetConjuntoIDs();
            }

            ICollection<DVParticipante> myEnumPar = MiLogicaParticipantes.ObtenerParticipantes(filtro);

            return myEnumPar;
        }

        public int ObtenerNumeroAMECParticipantes(string filtroID, string filtroNombre, string filtroApel1, string filtroApel2, string filtroHospital, string filtroMSDID,
            string filtroIDDistrito, string filtroIDRegion, string filtroIDEmpresa, string filtroIDAMEC)
        {
            FiltroParticipantes filtro =
                new FiltroParticipantes()
                {
                    Nombre = filtroNombre.CambiarAsterisco(),
                    Apel1 = filtroApel1.CambiarAsterisco(),
                    Apel2 = filtroApel2.CambiarAsterisco(),
                    Hospital = filtroHospital.CambiarAsterisco(),
                    MSDID = filtroMSDID.CambiarAsterisco(),
                    IdDistrito = (string.IsNullOrEmpty(filtroIDDistrito) || filtroIDDistrito == "-1") ? new Nullable<int>() : int.Parse(filtroIDDistrito),
                    IdRegion = (string.IsNullOrEmpty(filtroIDRegion) || filtroIDRegion == "-1") ? new Nullable<int>() : int.Parse(filtroIDRegion),
                    IdEmpresa = (string.IsNullOrEmpty(filtroIDEmpresa) || filtroIDEmpresa == "-1") ? new Nullable<int>() : int.Parse(filtroIDEmpresa),
                    IdAmec = (string.IsNullOrEmpty(filtroIDAMEC) || filtroIDAMEC == "-1") ? new Nullable<int>() : int.Parse(filtroIDAMEC),
                    Existe = "Not Exists"
                };

            if (Session["NuevoDetalleAMEC"] != null)
            {
                EOS.Entidades.DetalleAMEC dAmec = (EOS.Entidades.DetalleAMEC)Session["NuevoDetalleAMEC"];
                filtro.IdExpedienteConjunto = dAmec.GetConjuntoIDs();
            }

            long lNumeroParticipantes = MiLogicaParticipantes.ObtenerNumeroParticipantes(filtro);

            return Convert.ToInt32(lNumeroParticipantes);
        }

        public DVParticipanteAmec ObtenerAMECParticipantePorID(string filtroIdPar)
        {
            DVParticipanteAmec miParAmec = null;
            if (!string.IsNullOrEmpty(filtroIdPar))
            {
                DVParticipante miParticipante = null;

                FiltroParticipantes flt = new FiltroParticipantes();
                flt.IdParticipante = (string.IsNullOrEmpty(filtroIdPar)) ? -1 : int.Parse(filtroIdPar);
                miParticipante = MiLogicaParticipantes.ObtenerParticipantes(flt).FirstOrDefault();

                miParAmec = new DVParticipanteAmec();
                miParAmec.Apel1 = miParticipante.Apel1;
                miParAmec.Apel2 = miParticipante.Apel2;
                miParAmec.Nombre = miParticipante.Nombre;
                miParAmec.NombreCompleto = string.Format("{0} {1} {2}", miParAmec.Nombre, miParAmec.Apel1, miParAmec.Apel2);
                miParAmec.Especialidad = miParticipante.Especialidad;
                miParAmec.Localidad = miParticipante.Localidad;
                miParAmec.Hospital = miParticipante.Hospital;
                miParAmec.IdPassengerList = miParticipante.IdPassengerlist;
            }

            return miParAmec;
        }

        #endregion

        #region Participantes por Servicios (PAX)

        public string ObtenerParticipantesPorActividad(int nIdExpediente, int? nIdServicioActividad)
        {
            return ObtenerParticipantesPorServicio(nIdExpediente, nIdServicioActividad, 0);
        }
        public string ObtenerParticipantesPorDesplazamiento(int nIdExpediente, int? nIdServicioDesplazamiento)
        {
            return ObtenerParticipantesPorServicio(nIdExpediente, nIdServicioDesplazamiento, 1);
        }
        public string ObtenerParticipantesPorHotel(int nIdExpediente, int? nIdServicioHotel)
        {
            return ObtenerParticipantesPorServicio(nIdExpediente, nIdServicioHotel, 2);
        }
        public string ObtenerParticipantesPorInscripcion(int nIdExpediente, int? nIdServicioInscripcion)
        {
            return ObtenerParticipantesPorServicio(nIdExpediente, nIdServicioInscripcion, 3);
        }

        public string ObtenerParticipantesPorServicio(int nIdExpediente, int? nIdServicio, int nTipoSer)
        {
            string sListado = "";
            ICollection<DVServicioPassengerResumen> miLista = MiLogicaParticipantes.ObtenerParticipantesServicio(nIdExpediente, nIdServicio, nTipoSer);
            ICollection<DVServicioPassengerResumen> lista = new List<DVServicioPassengerResumen>();
            ArrayList uniques = new ArrayList();

            if (miLista != null)
            {
                foreach (DVServicioPassengerResumen pass in miLista)
                {
                    if (nIdServicio == null)
                    {
                        if ((
                            (pass.IdServicioINS != null && pass.IdEstadoINS != "CN" && pass.IdEstadoINS != "AN" &&
                             pass.IdEstadoINS != "CNTR") ||
                            (pass.IdServicioACT != null && pass.IdEstadoACT != "CN" && pass.IdEstadoACT != "AN" &&
                             pass.IdEstadoACT != "CNTR") ||
                            (pass.IdServicioHOT != null && pass.IdEstadoHOT != "CN" && pass.IdEstadoHOT != "AN" &&
                             pass.IdEstadoHOT != "CNTR") ||
                            (pass.IdServicioDSP != null && pass.IdEstadoDSP != "CN" && pass.IdEstadoDSP != "AN" &&
                             pass.IdEstadoDSP != "CNTR")
                            ))
                        {
                            if (!uniques.Contains(pass.IdPassengerList))
                            {
                                lista.Add(pass);
                                uniques.Add(pass.IdPassengerList);
                            }
                        }
                    }
                    else
                    {
                        if (!uniques.Contains(pass.IdPassengerList))
                        {
                            lista.Add(pass);
                            uniques.Add(pass.IdPassengerList);
                        }
                    }
                }

                foreach (DVServicioPassengerResumen pass in lista)
                {
                    sListado += string.Format("<li>{0} {1} {2}</li>", pass.Nombre, pass.Apel1, pass.Apel2);
                }
            }

            if (!string.IsNullOrEmpty(sListado))
            {
                sListado = string.Format("<ul>{0}</ul>", sListado);
            }
            else
            {
                sListado = string.Format("<ul><li>No se han encontrado participantes para esta reserva</li></ul>", sListado);
            }

            return sListado;
        }

        public long ObtenerNumeroServiciosParticipante(int nIdExpediente, int nIdPassengerList)
        {
            try
            {
                return MiLogicaParticipantes.ObtenerNumeroServiciosParticipante(nIdExpediente, nIdPassengerList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }

}
