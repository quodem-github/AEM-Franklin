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
using EOS.Web;

namespace EOS
{
    /// <summary>
    /// Descripción breve de MySQLSentence
    /// </summary>
    /// namespace EOS

    public class SQLSentenceAMEC
    {
        //public MySQLSentence()
        //{
        //    //
        //    // TODO: Agregar aquí la lógica del constructor
        //    //
        //}

        private static SQLSentenceAMEC Instancia = null;
        private string sentencia = string.Empty;
        private string sentencia2 = string.Empty;
        private bool hasAsistente = false;

        private WhereClauseTipo tipowhereclause = WhereClauseTipo.Ninguna;
        private enum WhereClauseTipo
        {
            Ninguna = 0,
            Producto = 1,
            Peticionario = 2,
            Distrito = 3,
            Region = 4,
            Area = 5,
            Unidad = 6
        }

        private SQLSentenceAMEC()
        {

        }
        private static void createInstance()
        {
            if (Instancia == null)
            {
                Instancia = new SQLSentenceAMEC();
            }
        }

        public static SQLSentenceAMEC GetInstancia()
        {
            if (Instancia == null)
                createInstance();
            return Instancia;
        }
        //Login

        public string login(string user, string pass)
        {
            sentencia = "SELECT IdPeticionario FROM peticionarios WHERE login='" + user + "' and password='" + pass + "'";

            return sentencia;


        }


        //UNIDADES
        public string getUnidad(string unidad)
        {
            sentencia = "SELECT * FROM unidades WHERE inactivo = 0 and idunidad =" + unidad;
            return sentencia;
        }
        public string getAllUnidades()
        {
            sentencia = "SELECT * FROM unidades where inactivo = 0";
            return sentencia;
        }



        //REGIONES
        public string getRegion(string reg)
        {
            sentencia = "SELECT distinct idregion, region, fkidempresa, locked, codregion FROM regiones WHERE inactivo = 0 and idregion =" + reg;
            return sentencia;
        }
        public string getRegionesDistrito(string distrito)
        {
            sentencia = "SELECT distinct idregion, region, fkidempresa, locked, codregion FROM regiones r LEFT JOIN distritos d ON r.idregion=d.idregion WHERE r.inactivo = 0 and d.inactivo = 0 and d.iddistrito=" + distrito;
            return sentencia;
        }

        public string getRegionesUnidades(string unidad)
        {
            /*sentencia = "SELECT distinct r.idregion, r.region FROM regiones r LEFT JOIN distritos d ON r.idregion=d.idregion"
                + " LEFT JOIN areas_distritos ad  ON ad.iddistrito=d.iddistrito"
                + " LEFT JOIN areas a  ON a.Idarea=ad.Idarea"
                + " LEFT JOIN unidades u  ON u.idunidad=a.idunidad"
                + " WHERE r.inactivo = 0 and u.idunidad=" + unidad;*/

            sentencia = " SELECT " +
                        "   distinct r.idregion, " +
                        "   r.region  " +
                        " FROM REGIONES R  " +
                        " LEFT JOIN UNIDADES U ON " +
                        " 	R.IDUNIDAD = U.IDUNIDAD " +
                        " LEFT JOIN DISTRITOS D ON " +
                        " 	D.IDREGION = R.IDREGION " +
                        " LEFT JOIN areas_distritos ad  ON  " +
                        " 	ad.iddistrito=d.iddistrito      " +
                        " WHERE  " +
                        " 	r.inactivo = 0 and  " +
                        "   ( u.idunidad=" + unidad + " or u.idunidad is null )" +
                        " ORDER BY R.REGION ASC;  ";

            return sentencia;
        }

        public string getAllRegiones()
        {
            sentencia = "SELECT distinct idregion, region FROM regiones where inactivo = 0";
            return sentencia;
        }


        //AREAS
        public string getArea(string area)
        {
            sentencia = "SELECT * FROM areas WHERE inactivo = 0 and idarea=" + area;
            return sentencia;
        }

        public string getAreasUnidad(string unidad)
        {
            sentencia = "SELECT * FROM areas WHERE inactivo = 0 and idunidad=" + unidad;
            return sentencia;
        }

        public string getAreaDistrito(string distrito)
        {
            sentencia = "SELECT * FROM areas a LEFT JOIN areas_distritos ad ON a.Idarea=ad.Idarea"
                + " WHERE a.inactivo = 0 and ad.iddistrito=" + distrito;
            return sentencia;
        }

        public string getAllAreas()
        {
            sentencia = "SELECT * FROM areas where inactivo = 0";
            return sentencia;
        }



        //DISTRITOS
        public string getDistrito(string distrito)
        {
            sentencia = "SELECT * FROM distritos WHERE inactivo = 0 and iddistrito=" + distrito;
            return sentencia;
        }

        public string getDistritoRegion(string region)
        {
            sentencia = "SELECT distinct r.idregion, r.region, d.iddistrito, d.distrito, d.locked, d.coddistrito FROM"
                + " regiones r INNER JOIN distritos d ON r.idregion=d.idregion"
                + " LEFT JOIN areas_distritos ad ON ad.iddistrito=d.iddistrito"
                + " WHERE r.inactivo = 0 and d.inactivo = 0 and d.idregion=" + region;


            return sentencia;
        }

        public string getDistritoRegionAreaNuevo(string region, string area)
        {
            sentencia = "SELECT distinct r.idregion, r.region, d.iddistrito, d.distrito, d.locked, d.coddistrito FROM"
        + " regiones r INNER JOIN distritos d ON r.idregion=d.idregion"
        + " LEFT JOIN areas_distritos ad ON ad.iddistrito=d.iddistrito"
        + " WHERE r.inactivo = 0 and d.inactivo = 0 and d.idregion=" + region + " and (ad.Idarea=" + area + " or d.idarea=" + area + ")";

            return sentencia;
        }

        public string getDistritoRegionArea(string region, string area)
        {
            if (region != "" && region != "0")
            {
                sentencia = "SELECT distinct r.idregion, r.region, d.iddistrito, d.distrito, d.locked, d.coddistrito FROM"
        + " regiones r INNER JOIN distritos d ON r.idregion=d.idregion"
        + " LEFT JOIN areas_distritos ad ON ad.iddistrito=d.iddistrito"
        + " WHERE r.inactivo = 0 and d.inactivo = 0 and d.idregion=" + region;
                if (area != "" && area != "0") sentencia += " and (ad.Idarea=" + area + " or d.idarea=" + area + ")";
            }
            else
            {

                sentencia = "SELECT distinct r.idregion, r.region, d.iddistrito, d.distrito, d.locked, d.coddistrito FROM"
                    + " regiones r INNER JOIN distritos d ON r.idregion=d.idregion"
                    + " LEFT JOIN areas_distritos ad ON ad.iddistrito=d.iddistrito where r.inactivo = 0 and d.inactivo = 0";
                if (area != "" && area != "0") sentencia += " and (ad.Idarea=" + area + " or d.idarea=" + area + ")";
            }

            return sentencia;
        }
        public string getDistritoArea(string area)
        {
            sentencia = "SELECT distinct r.idregion, r.region, d.iddistrito, d.distrito, d.locked, d.coddistrito FROM"
                + " regiones r INNER JOIN distritos d ON r.idregion=d.idregion"
                + " LEFT JOIN areas_distritos ad ON ad.iddistrito=d.iddistrito"
                + " Where d.inactivo = 0 and ad.Idarea=" + area + " or d.idarea=" + area;

            return sentencia;
        }

        public string getDistritoUnidad(string unidad)
        {
            sentencia = "SELECT distinct r.idregion, r.region, d.iddistrito, d.distrito, d.locked, d.coddistrito FROM"
                + " regiones r INNER JOIN distritos d ON r.idregion=d.idregion"
                + " LEFT JOIN areas_distritos ad ON ad.iddistrito=d.iddistrito"
                + " LEFT JOIN areas a ON a.idarea=ad.idarea or a.idarea=d.idarea"
                + " WHERE d.inactivo = 0 and r.inactivo = 0 and a.idunidad=" + unidad;


            return sentencia;
        }


        public string getAllDistritos()
        {
            sentencia = "SELECT * FROM distritos where inactivo = 0";
            return sentencia;
        }

        //PETICIONARIOS
        public string getPeticionarioDistrito(string distrito)
        {
            sentencia = "SELECT idPeticionario, rtrim(ltrim(isnull(Nombre, '') + ' ' + rtrim(ltrim(isnull(Apellido1, '')))) as nombreCompleto, iddistrito FROM"
                + " peticionarios"
                + " WHERE iddistrito=" + distrito;
            return sentencia;
        }


        public string getPeticionarioRegion(string region)
        {
            sentencia = "SELECT idPeticionario, rtrim(ltrim(isnull(Nombre, '') + ' ' + rtrim(ltrim(isnull(Apellido1, '')))) as nombreCompleto FROM"
                + " peticionarios p JOIN distritos d ON p.iddistrito=d.iddistrito"
                + " WHERE d.idregion=" + region;
            return sentencia;
        }

        public string getAllPeticionarios()
        {
            sentencia = "SELECT idPeticionario, rtrim(ltrim(isnull(Apellido1, '') + ' ' + rtrim(ltrim(isnull(Nombre, '')))) as nombreCompleto FROM peticionarios ORDER BY nombreCompleto";
            return sentencia;
        }

        public string getThePeticionario(string idPeti)
        {
            sentencia = "SELECT idPeticionario, rtrim(ltrim(isnull(Apellido1, '') + ' ' + rtrim(ltrim(isnull(Nombre, '')))) as nombreCompleto FROM peticionarios WHERE  IdPeticionario =" + idPeti;
            return sentencia;
        }

        public string getRol(string idPeti)
        {
            sentencia = "SELECT IdPeticionario, Nombre, Apellido1, p.IdCargo, c.Cargo FROM peticionarios p"
            + " LEFT JOIN cargos c ON p.IdCargo=c.IdCargo WHERE p.IdPeticionario=" + idPeti;

            return sentencia;
        }

        public string getPeticionarioGerente(string idPeticionario)
        {
            sentencia = "SELECT p.FkIdEmpresa, p.Nombre, p.Apellido1, p.Movil, p.Email, p.idregion,  p.iddistrito,  a.idunidad,"
            + " p.IdCargo, c.Cargo, p.Idarea, a.area, p.distrito,"
            + " u.codigo, u.unidad, r.region, r.codregion, c.delegado, c.marketing"
            + " FROM peticionarios p "
            + " LEFT JOIN cargos c ON p.IdCargo=c.IdCargo"
            + " LEFT JOIN regiones r ON p.idregion=r.idregion"
            + " LEFT JOIN areas a ON a.Idarea=p.Idarea"
            + " LEFT JOIN unidades u ON u.idunidad=a.idunidad"
            + " WHERE p.idPeticionario = " + idPeticionario;

            return sentencia;
        }

        public string getPeticionario(string idPeticionario)
        {
            sentencia = "SELECT FKIdEmpresa, Nombre, Apellido1, Movil, Email, idregion, iddistrito, idunidad, p.IdCargo, Cargo, IdEmpresa, idArea, delegado, marketing"
            + " FROM peticionarios p left JOIN cargos c ON p.IdCargo=c.IdCargo "
            + " WHERE p.IdPeticionario =" + idPeticionario;




            sentencia2 = "SELECT p.FkIdEmpresa, p.Nombre, p.Apellido1, p.Movil, p.Email, p.idregion,  p.iddistrito,  p.idunidad,"
            + " p.IdCargo, c.Cargo, p.Idarea, a.area, d.distrito,"
            + " u.codigo, u.unidad, r.region, r.codregion, delegado, marketing"
            + " FROM peticionarios p "
            + " LEFT JOIN cargos c ON p.IdCargo=c.IdCargo"
            + " LEFT JOIN distritos d ON p.iddistrito=d.iddistrito"
            + " LEFT JOIN regiones r ON d.idregion=r.idregion"
            + " LEFT JOIN areas_distritos ad ON ad.iddistrito=d.iddistrito"
            + " LEFT JOIN areas a ON a.Idarea=ad.Idarea"
            + " LEFT JOIN unidades u ON u.idunidad=a.idunidad"
            + " WHERE p.idPeticionario = " + idPeticionario;

            return sentencia2;
        }

        public string getPeticionarios(string idDistrito, string idRegion, string idArea, string idUnidad)
        {
            sentencia =
                "SELECT idPeticionario, rtrim(ltrim(isnull(Apellido1, '') + ' ' + isnull(Nombre, '')) as nombreCompleto FROM peticionarios";

            if (!string.IsNullOrEmpty(idDistrito) || !string.IsNullOrEmpty(idRegion) || !string.IsNullOrEmpty(idArea) || !string.IsNullOrEmpty(idUnidad))
            {
                string sentenciaWhere = string.Empty;

                if (!string.IsNullOrEmpty(idDistrito))
                {
                    if (string.IsNullOrEmpty(sentenciaWhere)) sentenciaWhere = " WHERE "; else sentenciaWhere += " AND ";
                    sentenciaWhere += "iddistrito=" + idDistrito;
                }

                if (!string.IsNullOrEmpty(idRegion))
                {
                    if (string.IsNullOrEmpty(sentenciaWhere)) sentenciaWhere = " WHERE "; else sentenciaWhere += " AND ";
                    sentenciaWhere += "idregion=" + idRegion;
                }

                if (!string.IsNullOrEmpty(idArea))
                {
                    if (string.IsNullOrEmpty(sentenciaWhere)) sentenciaWhere = " WHERE "; else sentenciaWhere += " AND ";
                    sentenciaWhere += "idarea=" + idArea;
                }

                if (!string.IsNullOrEmpty(idUnidad))
                {
                    if (string.IsNullOrEmpty(sentenciaWhere)) sentenciaWhere = " WHERE "; else sentenciaWhere += " AND ";
                    sentenciaWhere += "idunidad=" + idUnidad;
                }

                sentencia += sentenciaWhere;
            }

            return sentencia + " ORDER BY nombreCompleto";
        }


        public string getParametrosProducto(Informe informe)
        {
            bool escrito = false;
            if (informe.IdArea != "0" && informe.IdArea != "")
            {
                sentencia = "SELECT pe.idProductoEmpresa,pe.producto, aape.idamec FROM productosempresa pe"
                + " INNER JOIN areas_productosempresa ape on ape.IdProductoempresa=pe.IdProductoempresa"
                + " INNER JOIN amecs_areasproductosempresa aape on aape.IdareaProductoempresa=ape.IdareaProductoempresa"
                + " where pe.idproductoempresa in (select distinct ape.idProductoempresa "
                + " from areas_productosempresa ape "
                + " inner join amecs_areasproductosempresa aape on aape.IdareaProductoempresa=ape.IdareaProductoempresa"
                + " where aape.idAmec in ( select idAmec from expediente e)"
                + " and ape.idArea =" + informe.IdArea + ")";
                escrito = true;
            }

            if (informe.IdUnidad != "0" && informe.IdUnidad != "")
            {
                sentencia = "SELECT pe.idProductoEmpresa,pe.producto, aape.idamec FROM productosempresa pe"
                + " INNER JOIN areas_productosempresa ape on ape.IdProductoempresa=pe.IdProductoempresa"
                + " INNER JOIN amecs_areasproductosempresa aape on aape.IdareaProductoempresa=ape.IdareaProductoempresa"
                + " where pe.idproductoempresa in (select distinct ape.idProductoempresa "
                + " from areas_productosempresa ape "
                + " inner join amecs_areasproductosempresa aape on aape.IdareaProductoempresa"
                + " inner join areas a on a.idarea=ape.idarea";
                //+ " where aape.idAmec in ( select idAmec from expediente e) and a.idunidad=" + informe.IdUnidad + ")";

                if (informe.IdArea != "0" && informe.IdArea != "")
                {
                    sentencia += " where aape.idAmec in ( select idAmec from expediente e) and a.idunidad=" + informe.IdUnidad;
                    sentencia += " and ape.idArea =" + informe.IdArea + ")";
                }
                else
                {
                    sentencia += " where aape.idAmec in ( select idAmec from expediente e) and a.idunidad=" + informe.IdUnidad + ")";
                }
                escrito = true;

            }

            if (informe.IdDistrito != "0" && informe.IdDistrito != "")
            {
                sentencia = "SELECT pe.idProductoEmpresa,pe.producto, aape.idamec FROM productosempresa pe"
                + " INNER JOIN areas_productosempresa ape on ape.IdProductoempresa=pe.IdProductoempresa"
                + " INNER JOIN amecs_areasproductosempresa aape on aape.IdareaProductoempresa=ape.IdareaProductoempresa"
                + " where pe.idproductoempresa in (select distinct ape.idProductoempresa "
                + " from areas_productosempresa ape "
                + " inner join amecs_areasproductosempresa aape on aape.IdareaProductoempresa"
                + " inner join areas_distritos ad ON ad.Idarea=ape.Idarea"
                + " left join areas a ON ad.Idarea=a.Idarea";

                if (informe.IdArea != "0" && informe.IdArea != "" || informe.IdUnidad != "0" && informe.IdUnidad != "")
                {
                    sentencia += " where aape.idAmec in ( select idAmec from expediente e) and ad.iddistrito=" + informe.IdDistrito;
                    if (informe.IdArea != "0" && informe.IdArea != "") sentencia += " and ape.idArea =" + informe.IdArea;
                    if (informe.IdUnidad != "0" && informe.IdUnidad != "") sentencia += " AND a.idunidad=" + informe.IdUnidad;
                    sentencia += ")";
                }
                else
                {
                    sentencia += " where aape.idAmec in ( select idAmec from expediente e) and ad.iddistrito=" + informe.IdDistrito + ")";
                }
                escrito = true;

            }



            if (informe.IdRegion != "0" && informe.IdRegion != "1" && informe.IdRegion != "")
            {

                sentencia = "SELECT pe.idProductoEmpresa,pe.producto, aape.idamec FROM productosempresa pe"
                + " INNER JOIN areas_productosempresa ape on ape.IdProductoempresa=pe.IdProductoempresa"
                + " INNER JOIN amecs_areasproductosempresa aape on aape.IdareaProductoempresa=ape.IdareaProductoempresa"
                + " where pe.idproductoempresa in (select distinct ape.idProductoempresa "
                + " from areas_productosempresa ape "
                + " inner join amecs_areasproductosempresa aape on aape.IdareaProductoempresa"
                + " inner join areas_distritos ad ON ad.Idarea=ape.Idarea"
                + " inner join areas a ON a.Idarea=ad.Idarea"
                + " inner join distritos d ON d.iddistrito=ad.iddistrito";

                if (informe.IdArea != "0" && informe.IdArea != "" || informe.IdUnidad != "0" && informe.IdUnidad != "" || informe.IdDistrito != "0" && informe.IdDistrito != "")
                {
                    sentencia += " where aape.idAmec in ( select idAmec from expediente e) and d.idregion=" + informe.IdRegion;
                    if (informe.IdArea != "0" && informe.IdArea != "") sentencia += " and ape.idArea =" + informe.IdArea;
                    if (informe.IdUnidad != "0" && informe.IdUnidad != "") sentencia += " AND a.idunidad=" + informe.IdUnidad;
                    if (informe.IdDistrito != "0" && informe.IdDistrito != "") sentencia += " AND ad.iddistrito=" + informe.IdDistrito;
                    sentencia += ")";
                }
                else
                {
                    sentencia += " where aape.idAmec in ( select idAmec from expediente e) and d.idregion=" + informe.IdRegion + ")";
                }
                escrito = true;

            }

            if (informe.IdPeticionario != "0")
            {
                sentencia = "SELECT pe.idProductoEmpresa,pe.producto, aape.idamec FROM productosempresa pe"
                + " INNER JOIN areas_productosempresa ape on ape.IdProductoempresa=pe.IdProductoempresa"
                + " INNER JOIN amecs_areasproductosempresa aape on aape.IdareaProductoempresa=ape.IdareaProductoempresa"
                + " where pe.idproductoempresa in (select distinct ape.idProductoempresa "
                + " from areas_productosempresa ape "
                + " inner join amecs_areasproductosempresa aape on aape.IdareaProductoempresa"
                + " where aape.idAmec in ( select idAmec from expediente e where e.idPeticionario = " + informe.IdPeticionario + " ))";

                escrito = true;
            }

            //Todas las Actividades
            if (!escrito)
            {
                sentencia = "SELECT pe.idProductoEmpresa,pe.producto, aape.idamec FROM productosempresa pe"
                + " INNER JOIN areas_productosempresa ape on ape.IdProductoempresa=pe.IdProductoempresa"
                + " INNER JOIN amecs_areasproductosempresa aape on aape.IdareaProductoempresa=ape.IdareaProductoempresa"
                + " where aape.idAmec in ( select idAmec from expediente)";
            }


            return sentencia;
        }


        public string getParametrosProducto2(Informe informe)
        {
            sentencia = "SELECT pe.idProductoEmpresa,pe.producto FROM productosempresa pe"
                + " INNER JOIN areas_productosempresa ape on ape.IdProductoempresa=pe.IdProductoempresa"
                + " where pe.idproductoempresa in (select distinct ape.idProductoempresa "
                + " from areas_productosempresa ape) ";

            if (informe.IdArea != "0" && informe.IdArea != "")
                sentencia += " and ape.idArea =" + informe.IdArea;

            return sentencia;

        }

        public string getProductosJefeProducto(string idjefeProducto)
        {
            sentencia = "SELECT jp.IdPeticionario, jp.idProductoempresa, jp.idregion, p.idregion,  p.idunidad, p.iddistrito, p.idcargo, pe.Producto"
            + " FROM jefes_productoempresa jp JOIN peticionarios p on jp.IdPeticionario=p.IdPeticionario"
            + " JOIN cargos c on p.idcargo=c.idcargo"
            + " JOIN productosempresa pe on jp.idProductoempresa=pe.idProductoempresa"
            + " WHERE jp.IdPeticionario=" + idjefeProducto;

            return sentencia;
        }
        public string getTipoActividad()
        {
            sentencia = "SELECT distinct * FROM tiposcongreso tc"
            + " INNER JOIN congresos c ON c.idtipocongreso=tc.idtipocongreso"
            + " where c.idcongreso in (SELECT idcongreso FROM amec)";

            return sentencia;
        }


        public string getEspecialidadCongresos()
        {
            sentencia = "SELECT * FROM especialidades";

            sentencia2 = "SELECT * FROM especialidades esp"
            + " LEFT JOIN Congresos c ON esp.idespecialidad=c.idespecialidad ";
            sentencia2 += " WHERE c.IdCongreso=";

            return sentencia;
        }



        public string getAsistentes()
        {
            int i = 0;
            sentencia = "SELECT distinct pl.idpassengerlist, rtrim(ltrim(rtrim(ltrim(isnull(pl.nombre, '') + ' ' + isnull(pl.apel1, '')))  + ' ' + isnull(pl.apel2, ''))) as nombreCompleto FROM expediente exp"
            + " INNER JOIN reservas_passengers_list rpl ON exp.idxpediente=rpl.idxpediente"
            + " INNER JOIN passengers_list pl ON pl.idpassengerlist=rpl.idpassengerlist"
            + " WHERE exp.idamec in (SELECT idamec FROM amec)";


            return sentencia;
        }

        public string getEspecialidadesAsistentes()
        {

            sentencia = "SELECT distinct esp.idespecialidad, esp.especialidad FROM"
            + " passengers_list pl INNER JOIN especialidades esp ON pl.idespecialidad=esp.idespecialidad"
            + " INNER JOIN reservas_passengers_list rpl ON pl.idpassengerlist=rpl.idpassengerlist"
            + " INNER JOIN expediente exp ON exp.idxpediente=rpl.idxpediente"
            + " WHERE exp.idamec in (SELECT idamec FROM amec)";

            /*
            int i = 0;
            sentencia = "SELECT esp.idespecialidad, esp.especialidad  FROM"
                + " passengers_list pl INNER JOIN especialidades esp ON pl.idespecialidad=esp.idespecialidad";
            if (listaAsistentes.Length != 0)
            {
                sentencia += " WHERE pl.idpassengerlist=" + listaAsistentes[i];
                i++;
                while (i < listaAsistentes.Length)
                {
                    sentencia += " OR pl.idpassengerlist=" + listaAsistentes[i];
                    i++;
                }
            }
            */
            return sentencia;
        }

        public string getEstadoExpediente()
        {
            sentencia = "SELECT distinct e.idestado, er.estado FROM amec a "
            + " INNER JOIN expediente e on a.idamec=e.idamec"
            + " INNER JOIN estadosreservas er on er.idestado=e.idestado";
            //where aape.idAmec in ( select idAmec from expediente e)
            //No es lo mismo???

            return sentencia;
        }

        public string getEstadoReserva()
        {
            sentencia = "SELECT distinct rv.idestado, er.estado FROM estadosreservas er "
            + " INNER JOIN reservasviajes rv on rv.idestado=er.idestado";

            return sentencia;
        }



        public string getEstadoAmecs(string[] listaAmecs)
        {
            int i = 0;
            sentencia = "SELECT distinct a.idestado, estado FROM estadosreservas er"
            + " INNER JOIN amec a on a.idestado= er.idestado";
            /*
            while (listaAmecs[i] != null && listaAmecs[i] != "" && i < listaAmecs.Length)
            {
                if (i == 0) sentencia += " WHERE a.idamec=" + listaAmecs[i];
                else sentencia += " or a.idamec=" + listaAmecs[i];
                i++;
            }
            */
            return sentencia;
        }

        public string getValoracionFI()
        {
            sentencia = "SELECT idvaloracionfi, descripcion FROM valoracion_fi";

            return sentencia;
        }


        public string getInformeAmec(string idAmec)
        {
            sentencia = "SELECT a.idamec, a.amec, a.idpeticionario,rtrim(ltrim(isnull(Apellido1, '') + ' ' + isnull(Nombre, '')) as nombreCompleto, p.idcargo, c.Cargo, p.wein, a.fecha,"
            + " areas.area, r.region, d.distrito, a.paraguas, a.aprobado, tac.tipoactividadcongreso, a.nombrePrograma, a.codgenesis, a.objetivosprograma,"
            + " a.contenido, a.lugar, a.ambitogeografico, a.duracion, a.numparticipantes, a.fechacomienzop, a.fechafinp, a.gastosdes_aloj,"
            + " cs1.criterioseleccion as cs1, cs2.criterioseleccion as cs2, cs3.criterioseleccion as cs3, a.relponentes, a.numponentes, a.honorarios, "
            + " a.conceptogastos, a.importetotal, a.numpropuestas, a.numcartas, a.numhojasinscripcion, con.Congreso,a.observaciones, a.comunicar_amec, u.unidad,"
            + " ar1.area, ar2.area, ar3.area, exp.idxpediente, ce.codigo_agencia"
            + " FROM amec a"
            //+ " INNER JOIN expediente exp ON a.idamec=exp.idamec"
            + " LEFT JOIN expediente exp ON a.idamec=exp.idamec"
            //+ " INNER JOIN confempresa ce ON ce.IdEmpresa=a.IdEmpresa"
            + " LEFT JOIN confempresa ce ON ce.IdEmpresa=a.IdEmpresa"
            + " LEFT JOIN peticionarios p ON a.idpeticionario=p.idpeticionario"
            + " LEFT JOIN cargos c ON p.idCargo=c.idCargo"
            + " LEFT JOIN areas areas ON areas.Idarea=a.idarea"
            + " LEFT JOIN unidades u ON u.idunidad=a.idunidad"
            + " LEFT JOIN regiones r ON r.idregion=a.idregion"
            + " LEFT JOIN distritos d ON d.iddistrito=a.iddistrito"
            //+ " INNER JOIN congresos con ON con.IdCongreso=a.IdCongreso"
            + " LEFT JOIN congresos con ON con.IdCongreso=a.IdCongreso"
            //Para la valoracion Farma Industria
            + " left join valoracion_fi valfi on valfi.idvaloracionfi=con.idvaloracionfi"
            + " LEFT JOIN tipos_actividad_congreso tac ON tac.idtipoactividadcongreso=a.idtipoactividadcongreso"
            + " LEFT JOIN criteriosseleccion cs1 ON cs1.idcriterio=a.idcriterio"
            + " LEFT JOIN criteriosseleccion cs2 ON cs2.idcriterio=a.idcriterio2"
            + " LEFT JOIN criteriosseleccion cs3 ON cs3.idcriterio=a.idcriterio3"

            + " LEFT JOIN areas ar1 ON ar1.Idarea=a.idarea1"
            + " LEFT JOIN areas ar2 ON ar2.Idarea=a.idarea2"
            + " LEFT JOIN areas ar3 ON ar3.Idarea=a.idarea3"
            + " WHERE a.idamec=" + idAmec;

            return sentencia;
        }

        public string getAsistentesAmec(string idAmec)
        {
            sentencia = "SELECT pl.idpassengerlist, rtrim(ltrim(rtrim(ltrim(isnull(pl.nombre, '') + ' ' + isnull(pl.apel1, '')))  + ' ' + isnull(pl.apel2, ''))) as nombreCompleto, pl.NombreCentroTrabajo, e.especialidad, pl.LocalidadCentroTrabajo, exp.idxpediente"
            + " FROM expediente exp"
            + " INNER JOIN reservas_passengers_list rpl ON exp.idxpediente=rpl.idxpediente"
            + " INNER JOIN passengers_list pl ON pl.idpassengerlist=rpl.idpassengerlist"
            + " INNER JOIN especialidades e ON e.idespecialidad=pl.idespecialidad"
            + " WHERE exp.idamec =" + idAmec;

            return sentencia;
        }

        public string getExpedientesAmec(string idamec)
        {
            sentencia = "SELECT idxpediente FROM expediente WHERE idamec=" + idamec;

            return sentencia;
        }

        public string getInfoExpediente(Informe informe)
        {
            hasAsistente = false;
            string asistentesentencia = getAsistentesSentenciaInformePeticionExpedientes(informe, string.Empty);

            sentencia = "SELECT IdExpediente, min(Unidad) as Unidad, min(Region) as Region, min(Area) as Area, min(Distrito) as Distrito, min([Nombre Peticionario]) as [Nombre Peticionario], min([Apellido Peticionario]) as [Apellido Peticionario], min([Cargo Peticionario]) as [Cargo Peticionario], min(Amec) as Amec, min([Tipo actividad]) as [Tipo actividad], min(Actividad) as Actividad, min(Lugar) as Lugar, min([Fecha inicio]) as [Fecha inicio], min([Fecha fin]) as [Fecha fin], min([Estado Expediente]) as [Estado Expediente], min([NumPedido/Sgap]) as [NumPedido/Sgap], min([Fecha Expediente]) as [Fecha Expediente], min(ImporteExpediente) as ImporteExpediente, min([Importe Inscripciones]) as [Importe Inscripciones], min([Importe Alojamiento]) as [Importe Alojamiento], min([Importe Otros servicios]) as [Importe Otros servicios], min([Importe Desplazamiento]) as [Importe Desplazamiento], min([Num Pax]) as [Num Pax], min(Producto) as Producto, min([Porcentaje Producto]) as [Porcentaje Producto], min([Codigo tipo congreso]) as [Codigo tipo congreso], min(Sede) as Sede, min(IdPeticionario) as IdPeticionario, min(IdDistrito) as IdDistrito, min(IdRegion) as IdRegion, min(IdArea) as IdArea, min(IdUnidad) as IdUnidad, min(FechaComienzo) as FechaComienzo, min(FechaFin) as FechaFin, min(IdTipoActividadCongreso) as IdTipoActividadCongreso, min(IdCongreso) as IdCongreso, min(IdAmec) as IdAmec, min(CodExpediente) as CodExpediente, min(IdEstadoReserva) as IdEstadoReserva, min(IdEstadoExpediente) as IdEstadoExpediente, min(IdValfi) as IdValfi, min(FechaCreacion) as FechaCreacion, min(IdPassengerList) as IdPassengerList, min(IdReserva) as IdReserva, IdAreaProductoEmpresa from cv_informe_expediente ";
            //start producing the where clause
            string whereclause = string.Empty;

            if (informe.IdPeticionario != "0")
            {
                whereclause = " WHERE IdPeticionario=" + informe.IdPeticionario;
                tipowhereclause = WhereClauseTipo.Peticionario;
            }

            if (informe.IdDistrito != "0")
            {
                if (whereclause.Length > 0) whereclause += " AND "; else whereclause = " WHERE ";
                whereclause += "IdDistrito =" + informe.IdDistrito;
                tipowhereclause = WhereClauseTipo.Distrito;
            }
            if (informe.IdRegion != "0" && informe.IdRegion != "1")
            {
                if (whereclause.Length > 0) whereclause += " AND "; else whereclause = " WHERE ";
                whereclause += "IdRegion =" + informe.IdRegion;
                tipowhereclause = WhereClauseTipo.Region;
            }
            if (informe.IdArea != "0")
            {
                if (whereclause.Length > 0) whereclause += " AND "; else whereclause = " WHERE ";
                whereclause += "IdArea =" + informe.IdArea;
                tipowhereclause = WhereClauseTipo.Area;
            }
            if (informe.IdUnidad != "0")
            {
                if (whereclause.Length > 0) whereclause += " AND "; else whereclause = " WHERE ";
                whereclause += "IdUnidad =" + informe.IdUnidad;
                tipowhereclause = WhereClauseTipo.Unidad;
            }

            if (string.IsNullOrEmpty(whereclause))
            {
                whereclause = "WHERE 1=1 "; //No sabemos porque, pero bueno, nos dice que no se ha seleccionado nada y por tanto se ve TODO
            }

            if (!string.IsNullOrEmpty(informe.FechaActDesde)) whereclause += " and FechaComienzo >= '" + informe.FechaActDesde + "'";
            if (!string.IsNullOrEmpty(informe.FechaActHasta)) whereclause += " and FechaFin <= '" + informe.FechaActHasta + "'";

            if (informe.IdTipoActividad != "-1" && informe.IdTipoActividad != "0") whereclause += " and IdTipoActividadCongreso=" + informe.IdTipoActividad;
            if (!string.IsNullOrEmpty(informe.IdCongreso)) whereclause += " and IdCongreso=" + informe.IdCongreso;

            //Asistentes
            if (hasAsistente) whereclause += asistentesentencia;

            if (!string.IsNullOrEmpty(informe.CodigoAmec)) whereclause += " and Amec='" + informe.CodigoAmec + "'";
            if (!string.IsNullOrEmpty(informe.NumPedido)) whereclause += " and CodExpediente='" + informe.NumPedido + "'";
            if (informe.IdEstadoReserva != "0") whereclause += " and IdEstadoReserva='" + informe.IdEstadoReserva + "'";
            if (informe.IdEstadoExpediente != "0") whereclause += " and IdEstadoExpediente='" + informe.IdEstadoExpediente + "'";
            //Ismael Ameller 01-03-2011 Elimino el filtro del estado Amec
            //if (informe.IdEstadoAmec != "0") whereclause += " and ame.idestado='" + informe.IdEstadoAmec + "'";
            //FIN Ismael Ameller 01-03-2011 Elimino el filtro del estado Amec

            //Ismael Ameller 01-03-2011 Añado el filtro de Numero de expediente y numero de reserva
            if (!string.IsNullOrEmpty(informe.NumExpediente)) whereclause += " and IdExpediente='" + informe.NumExpediente + "'";
            if (!string.IsNullOrEmpty(informe.NumReserva)) whereclause += " and IdReserva='" + informe.NumReserva + "'";
            //FIN Ismael Ameller 01-03-2011 Añado el filtro de Numero de expediente y numero de reserva

            //Ismael Ameller 01-03-2011 filtro por Valorado Farmaindustria
            if (informe.IdValoradoFI != "0") whereclause += " and IdValfi='" + informe.IdValoradoFI + "'";
            //Ismael Ameller 01-03-2011 filtro por Valorado Farmaindustria

            //Ismael Ameller 01-03-2011 Elimino el filtro Amec aprobado
            //if (informe.AmecAprobadoSeleccionado) {
            //    if (informe.AmecAprobado) whereclause += " and ame.aprobado = 1";
            //    else whereclause += " and (ame.aprobado is null or ame.aprobado = 0)";
            //}
            //FIN Ismael Ameller 01-03-2011 Elimino el filtro Amec aprobado

            //Ismael Ameller 01-03-2011 Cambio del formato y control de fechas
            bool iguales = false;
            if ((informe.FechaDesdeDt != null) && (informe.FechaHastaDt != null))
            {
                if (informe.FechaDesdeDt == informe.FechaHastaDt)
                {
                    whereclause += " and FechaCreacion >= '" + informe.FechaDesdeDt.Value.ToString("yyyy/MM/dd") + "' and FechaCreacion < '" + informe.FechaDesdeDt.Value.AddDays(1).ToString("yyyy/MM/dd") + "' ";
                    iguales = true;
                }
            }
            if ((informe.FechaDesdeDt != null) && (iguales == false))
            {
                whereclause += " and FechaCreacion >= '" + informe.FechaDesdeDt.Value.ToString("yyyy/MM/dd") + "'";
            }
            if ((informe.FechaHastaDt != null) && (iguales == false))
            {
                whereclause += " and FechaCreacion <= '" + informe.FechaHastaDt.Value.AddDays(1).ToString("yyyy/MM/dd") + "'";
            }
            //FIN Ismael Ameller 01-03-2011 Cambio del formato y control de fechas

            //Ismael Ameller 04-03-2011 Añado el filtro del radiobutton NºPendiente/AMEX SI o NO
            if (informe.PendientePedidoSeleccionado)
            {
                if (informe.PendientePedido)
                {
                    whereclause += " and CodExpediente <> '' and  CodExpediente is not null";
                }
                else
                {
                    whereclause += " and (CodExpediente= '' or CodExpediente is null)";
                }
            }
            //FIN Ismael Ameller 04-03-2011 Añado el filtro del radiobutton NºPendiente/AMEX SI o NO

            if (informe.IdProducto != "0") whereclause += " and IdAreaProductoEmpresa='" + informe.IdProducto + "'";

            whereclause += " group by IdExpediente, idareaproductoempresa ";

            //create the final query by joining the query with the where clause
            sentencia += whereclause;
            return sentencia;
        }

        public string getInformeReserva(Informe informe)
        {
            hasAsistente = false;
            string asistentesentencia = getAsistentesSentenciaInformePeticionExpedientes(informe, string.Empty);
            //Ismael Ameller 03-03-2011 Consulta Nueva de Informe Pendiente
            sentencia = "SELECT * from cv_informe_peticion ";

            //start producing the where clause
            string whereclause = string.Empty;

            if (informe.IdPeticionario != "0")
            {
                whereclause += " WHERE  IdPeticionario=" + informe.IdPeticionario;
                tipowhereclause = WhereClauseTipo.Peticionario;
            }
            else if (informe.IdDistrito != "0")
            {
                whereclause += " WHERE IdDistrito =" + informe.IdDistrito;
                tipowhereclause = WhereClauseTipo.Distrito;
            }
            else if (informe.IdRegion != "0")
            {
                whereclause += " WHERE IdRegion =" + informe.IdRegion;
                tipowhereclause = WhereClauseTipo.Region;
            }
            else if (informe.IdArea != "0")
            {
                whereclause += " WHERE IdArea =" + informe.IdArea;
                tipowhereclause = WhereClauseTipo.Area;
            }
            else if (informe.IdUnidad != "0")
            {
                whereclause += " WHERE IdUnidad =" + informe.IdUnidad;
                tipowhereclause = WhereClauseTipo.Unidad;
            }

            if (string.IsNullOrEmpty(whereclause))
            {
                whereclause = "WHERE 1=1 ";
            }
            /* is this necessary?
                whereclause += getTipoWhereClause(informe);
            */

            if (!string.IsNullOrEmpty(informe.FechaActDesde)) whereclause += " and FechaComienzo >='" + informe.FechaActDesde + "'";
            if (!string.IsNullOrEmpty(informe.FechaActHasta)) whereclause += " and FechaFin <= '" + informe.FechaActHasta + "'";

            if (informe.IdTipoActividad != "-1" && informe.IdTipoActividad != "0") whereclause += " and IdTipoActividadCongreso =" + informe.IdTipoActividad;
            if (!string.IsNullOrEmpty(informe.IdCongreso)) whereclause += " and IdCongreso=" + informe.IdCongreso;
            //if (informe.IdEspecialidadCongreso != "") whereclause += " and con.IdTipoCongreso=" + informe.IdEspecialidadCongreso;

            //Asistentes
            if (hasAsistente) whereclause += asistentesentencia;

            if (!string.IsNullOrEmpty(informe.CodigoAmec)) whereclause += " and Amec='" + informe.CodigoAmec + "'";
            if (!string.IsNullOrEmpty(informe.NumPedido)) whereclause += " and CodExpediente='" + informe.NumPedido + "'";
            if (informe.IdEstadoReserva != "0") whereclause += " and IdEstadoReserva='" + informe.IdEstadoReserva + "'";
            if (informe.IdEstadoExpediente != "0") whereclause += " and IdEstadoExpediente='" + informe.IdEstadoExpediente + "'";

            //Ismael Ameller 01-03-2011 Elimino el filtro del estado Amec
            //if (informe.IdEstadoAmec != "0") whereclause += " and ame.idestado='" + informe.IdEstadoAmec + "'";
            //FIN Ismael Ameller 01-03-2011 Elimino el filtro del estado Amec

            //Ismael Ameller 01-03-2011 Añado el filtro de Numero de expediente y numero de reserva
            if (!string.IsNullOrEmpty(informe.NumExpediente)) whereclause += " and IdExpediente='" + informe.NumExpediente + "'";
            //Jose Laguna 26-04-2012 Eliminamos este filtro ya que el importante es el de IdAmec y lo substiruimos en el excel
            //if (!string.IsNullOrEmpty(informe.NumReserva)) whereclause += " and IdReserva='" + informe.NumReserva + "'";
            //FIN Ismael Ameller 01-03-2011 Añado el filtro de Numero de expediente y numero de reserva

            //Ismael Ameller 01-03-2011 filtro por Valorado Farmaindustria
            if (informe.IdValoradoFI != "0") whereclause += " and IdValfi='" + informe.IdValoradoFI + "'";
            //Ismael Ameller 01-03-2011 filtro por Valorado Farmaindustria

            //Ismael Ameller 01-03-2011 Elimino el filtro Amec aprobado
            //if (informe.AmecAprobadoSeleccionado)
            //{
            //    if (informe.AmecAprobado) whereclause += " and ame.aprobado = 1";
            //    else whereclause += " and (ame.aprobado is null or ame.aprobado = 0)";
            //}
            //FIN Ismael Ameller 01-03-2011 Elimino el filtro Amec aprobado

            //Ismael Ameller 01-03-2011 Cambio del formato y control de fechas
            bool iguales = false;
            if ((informe.FechaDesdeDt != null) && (informe.FechaHastaDt != null))
            {
                if (informe.FechaDesdeDt == informe.FechaHastaDt)
                {
                    whereclause += " and FechaCreacion >= '" + informe.FechaDesdeDt.Value.ToString("yyyy/MM/dd") + "' and FechaCreacion < '" + informe.FechaDesdeDt.Value.AddDays(1).ToString("yyyy/MM/dd") + "' ";
                    iguales = true;
                }
            }
            if ((informe.FechaDesdeDt != null) && (iguales == false))
            {
                whereclause += " and FechaCreacion >= '" + informe.FechaDesdeDt.Value.ToString("yyyy/MM/dd") + "'";
            }
            if ((informe.FechaHastaDt != null) && (iguales == false))
            {
                whereclause += " and FechaCreacion <= '" + informe.FechaHastaDt.Value.AddDays(1).ToString("yyyy/MM/dd") + "'";
            }
            //FIN Ismael Ameller 01-03-2011 Cambio del formato y control de fechas

            //Ismael Ameller 04-03-2011 Añado el filtro del radiobutton NºPendiente/AMEX SI o NO
            if (informe.PendientePedidoSeleccionado)
            {
                if (informe.PendientePedido)
                {
                    whereclause += " and CodExpediente <> '' and  CodExpediente is not null";
                }
                else
                {
                    whereclause += " and (CodExpediente= '' or CodExpediente is null)";
                }
            }
            //FIN Ismael Ameller 04-03-2011 Añado el filtro del radiobutton NºPendiente/AMEX SI o NO

            if (informe.IdProducto != "0") whereclause += " and IdAreaProductoEmpresa='" + informe.IdProducto + "'";
            //whereclause += " group by IdReserva";

            sentencia += whereclause;
            return sentencia;
        }

        private string getEstadoReserva(Informe informe)
        {
            string addReserva = " left join estadosreservas est on exp.idestado = est.idestado";
            int idestadoreserva = 0;
            try
            {
                idestadoreserva = Convert.ToInt32(informe.IdEstadoReserva);
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
            }
            if (idestadoreserva > 0)
                addReserva += " inner join estadosreservas est on exp.idestado = est.idestado";
            return addReserva;
        }

        public string getTipoPatrocinio(string idexp, string idpl)
        {
            sentencia = "SELECT 'Tipo Inscripcion' = "
            + "CASE"
                + "WHEN ser.idservicioinscripcion is null THEN 'Inscripción'"
                + "WHEN ser.idserviciohotel is null THEN 'Alojamiento'"
                + "WHEN ser.idservicioactividad is null THEN 'Actividad'"
                + "WHEN ser.idserviciotransporte is null THEN 'Desplazamiento'"
                + "ELSE ''"
            + "END"
            + " FROM expediente exp"

            + " left join reservasviajes res on exp.idxpediente = res.fkidexpediente"
            + " left join serviciosreservasviajes ser on res.idreserva = ser.idreserva"

            + " left join serviciosreservasinscripciones serins on ser.idservicioinscripcion = serins.idservicioinscripcion"
            + " left join serviciosreservasactividades seract on ser.idservicioactividad = seract.idservicioactividad"
            + " left join serviciosreservashotel serhot on ser.idserviciohotel = serhot.idserviciohotel"
            + " left join serviciosreservastransporte sertra on ser.idserviciotransporte = sertra.idserviciotransporte"

            + " left join actividades_passengers_list passact on seract.idservicioactividad = passact.idservicioactividad"
            + " left join hotel_passengers_list passhot on serhot.idserviciohotel = passhot.idserviciohotel"
            + " left join transportepassengerslist passtran on sertra.idserviciotransporte = passtran.idserviciotransporte"
            + " left join ins_passengers_list passins on serins.idservicioinscripcion = passins.idservicioinscripcion"

            + " left join passengers_list pl1 on passins.idpassengerlist = pl1.idpassengerlist"
            + " left join passengers_list pl2 on passtran.idpassengerlist = pl2.idpassengerlist"
            + " left join passengers_list pl3 on passhot.idpassengerlist = pl3.idpassengerlist"
            + " left join passengers_list pl4 on passact.idpassengerlist = pl4.idpassengerlist"
            + " where exp.idxpediente = " + idexp + " and pl1.idpassengerlist =  " + idpl + "  or pl2.idpassengerlist = " + idpl + " or pl3.idpassengerlist = " + idpl + " or pl4.idpassengerlist = " + idpl;

            return sentencia;
        }

        public string getAsistentes(string cadena)
        {
            sentencia = "SELECT distinct rtrim(ltrim(rtrim(ltrim(isnull(pl.nombre, '') + ' ' + isnull(pl.apel1, '')))  + ' ' + isnull(pl.apel2, ''))) as nombreCompleto FROM expediente exp"
                    + " INNER JOIN reservas_passengers_list rpl ON exp.idxpediente=rpl.idxpediente"
                    + " INNER JOIN passengers_list pl ON pl.idpassengerlist=rpl.idpassengerlist"
                    + " WHERE exp.idamec in (SELECT idamec FROM amec) and pl";

            return sentencia;
        }

        public string buscaActividades(Informe informe)
        {
            /*
                informe.NombreActividad = txtActividad.Text.ToString();
                informe.IdTipoActividad = ddlTipoActividad.SelectedValue.ToString();
                informe.FechaActDesde = txtFechaActDesde.Text.ToString();
                informe.FechaActHasta = txtFechaActHasta.Text.ToString();
            */
            DateTime desde = new DateTime();
            DateTime hasta = new DateTime();

            bool escrito = false;
            bool escritof = false;

            if (informe.FechaActDesde != "")
            {
                informe.FechaActDesde = informe.FechaActDesde.Replace('/', '-');
                desde = DateTime.Parse(informe.FechaActDesde);
                informe.FechaActDesde = desde.Year + "-" + desde.Month + "-" + desde.Day;
            }
            if (informe.FechaActHasta != "")
            {
                informe.FechaActHasta = informe.FechaActHasta.Replace('/', '-');
                hasta = DateTime.Parse(informe.FechaActHasta);
                informe.FechaActHasta = hasta.Year + "-" + hasta.Month + "-" + hasta.Day;
            }

            if (informe.NombreActividad != "")
            {
                sentencia = "SELECT distinct e.idespecialidad, e.especialidad FROM especialidades e LEFT JOIN Congresos c on e.idEspecialidad=c.idespecialidad"
                + " WHERE especialidad like '%" + informe.NombreActividad + "%'";
                escrito = true;
            }
            else if (informe.IdTipoActividad != "0")
            {
                sentencia = "SELECT distinct e.idespecialidad, e.especialidad FROM especialidades e LEFT JOIN Congresos c on e.idEspecialidad=c.idespecialidad"
                + " WHERE idespecialidad=" + informe.IdTipoActividad;
                escrito = true;
            }

            if (informe.NombreActividad == "" && informe.IdTipoActividad != "0")
            {
                sentencia = "SELECT distinct e.idespecialidad, e.especialidad FROM especialidades e LEFT JOIN Congresos c on e.idEspecialidad=c.idespecialidad"
                + " WHERE especialidad like '%" + informe.NombreActividad + "%' and idespecialidad=" + informe.IdTipoActividad;
                escrito = true;
            }//Solo fechas
            else if (informe.NombreActividad == "" && informe.IdTipoActividad == "0")
            {
                if (informe.FechaActDesde != "")
                {
                    sentencia = "SELECT distinct e.idespecialidad, e.especialidad FROM especialidades e LEFT JOIN Congresos c on e.idEspecialidad=c.idespecialidad"
                    + " WHERE Desde>='" + informe.FechaActDesde + "'";
                    escritof = true;
                }
                if (informe.FechaActHasta != "")
                {
                    sentencia = "SELECT distinct e.idespecialidad, e.especialidad FROM especialidades e LEFT JOIN Congresos c on e.idEspecialidad=c.idespecialidad"
                    + " WHERE Hasta<='" + informe.FechaActHasta + "'";
                    escritof = true;
                }
                if (informe.FechaActDesde != "" && informe.FechaActHasta != "")
                {
                    sentencia = "SELECT distinct e.idespecialidad, e.especialidad FROM especialidades e LEFT JOIN Congresos c on e.idEspecialidad=c.idespecialidad"
                    + " WHERE Hasta<='" + informe.FechaActHasta + "' and Desde>='" + informe.FechaActDesde + "'";
                    escritof = true;
                }
            }

            if (escrito && !escritof)
            {
                if (informe.FechaActDesde != "")
                {
                    sentencia += " and Desde>='" + informe.FechaActDesde + "'";
                    escritof = true;
                }
                if (informe.FechaActHasta != "")
                {
                    sentencia += " and Hasta<='" + informe.FechaActHasta + "'";
                    escritof = true;
                }
            }
            if (!escrito && !escritof)
                sentencia = "SELECT distinct e.idespecialidad, e.especialidad FROM especialidades e LEFT JOIN Congresos c on e.idEspecialidad=c.idespecialidad";

            return sentencia;
        }

        public string buscaAsistentes(Informe informe)
        {
            sentencia = "SELECT distinct rtrim(ltrim(rtrim(ltrim(isnull(pl.nombre, '') + ' ' + isnull(pl.apel1, '')))  + ' ' + isnull(pl.apel2, ''))), pl.idpassengerlist FROM expediente exp"
                    + " INNER JOIN reservas_passengers_list rpl ON exp.idxpediente=rpl.idxpediente"
                    + " INNER JOIN passengers_list pl ON pl.idpassengerlist=rpl.idpassengerlist"
                    + " WHERE exp.idamec in (SELECT idamec FROM amec)";

            sentencia += getAsistentesSentencia(informe, sentencia);
            return sentencia;
        }

        public string getAdministratorSentencia(string IdPeticionario)
        {
            sentencia = "SELECT idPeticionario,d.administrador"
             + " FROM peticionarios d left join cargos c on c.idcargo=d.idcargo"
             + " where password is not null and idPeticionario = " + IdPeticionario + ";";
            return sentencia;
        }
        //04-03-2011 Ismael Ameller añadido funcion para el informe de petición
        private string getAsistentesSentenciaInformePeticionExpedientes(Informe informe, string sentencia)
        {
            if (informe.Asistente != null && informe.Asistente.Count > 0)
            {
                string asistentesparaañadir = string.Empty;
                List<string> idAsistentes = new List<string>();

                foreach (Asistente asistente in informe.Asistente)
                {
                    if (!string.IsNullOrEmpty(asistente.IdAsistente) && !idAsistentes.Contains(asistente.IdAsistente))
                    {
                        if (string.IsNullOrEmpty(asistentesparaañadir))
                        {
                            asistentesparaañadir += " and IdPassengerList = " + asistente.IdAsistente;
                        }
                        else
                        {
                            asistentesparaañadir += " or IdPassengerList = " + asistente.IdAsistente;
                        }
                        idAsistentes.Add(asistente.IdAsistente);
                    }
                }
                if (!string.IsNullOrEmpty(asistentesparaañadir))
                {
                    asistentesparaañadir = asistentesparaañadir.Replace(" and IdPassengerList", " and (IdPassengerList");
                    asistentesparaañadir += ")";
                    sentencia += asistentesparaañadir;

                    hasAsistente = true;
                }
            }
            return sentencia;
        }
        //FIN 04-03-2011 Ismael Ameller añadido funcion para el informe de petición
        private string getAsistentesSentencia(Informe informe, string sentencia)
        {
            if (informe.Asistente != null && informe.Asistente.Count > 0)
            {
                string asistentesparaañadir = string.Empty;
                List<string> idAsistentes = new List<string>();

                foreach (Asistente asistente in informe.Asistente)
                {
                    if (!string.IsNullOrEmpty(asistente.IdAsistente) && !idAsistentes.Contains(asistente.IdAsistente))
                    {
                        if (string.IsNullOrEmpty(asistentesparaañadir))
                        {
                            asistentesparaañadir += " and respass.idPassengerlist = " + asistente.IdAsistente;
                        }
                        else
                        {
                            asistentesparaañadir += " or respass.idPassengerlist = " + asistente.IdAsistente;
                        }
                        idAsistentes.Add(asistente.IdAsistente);
                    }
                }
                if (!string.IsNullOrEmpty(asistentesparaañadir))
                {
                    asistentesparaañadir = asistentesparaañadir.Replace(" and respass.idPassengerlist", " and (respass.idPassengerlist");
                    asistentesparaañadir += ")";
                    sentencia += asistentesparaañadir;

                    hasAsistente = true;
                }
            }
            return sentencia;
        }

        private string getTipoWhereClause(Informe informe)
        {
            string whereclause = string.Empty;
            if (tipowhereclause == WhereClauseTipo.Producto)
            {
                if (informe.IdPeticionario != "0")
                    whereclause += " and (exp.idpeticionario = " + informe.IdPeticionario + " or exp.idpeticionario is null)";
                tipowhereclause = WhereClauseTipo.Peticionario;
            }
            if (tipowhereclause == WhereClauseTipo.Peticionario)
            {
                if (informe.IdDistrito != "0")
                    whereclause += " and (exp.iddistrito = " + informe.IdDistrito + " or exp.iddistrito is null)";
                tipowhereclause = WhereClauseTipo.Distrito;
            }
            if (tipowhereclause == WhereClauseTipo.Distrito)
            {
                if (informe.IdRegion != "0")
                    whereclause += " and (exp.idregion = " + informe.IdRegion + " or exp.idregion is null)";
                tipowhereclause = WhereClauseTipo.Region;
            }
            if (tipowhereclause == WhereClauseTipo.Region)
            {
                if (informe.IdArea != "0")
                    whereclause += " and (exp.idarea = " + informe.IdArea + " or exp.idarea is null)";
                tipowhereclause = WhereClauseTipo.Area;
            }
            if (tipowhereclause == WhereClauseTipo.Area)
            {
                if (informe.IdUnidad != "0")
                    whereclause += " and (exp.idunidad = " + informe.IdUnidad + " or exp.idunidad is null)";
                tipowhereclause = WhereClauseTipo.Unidad;
            }
            return whereclause;
        }

        public static int ID_UNIDAD = 0;
        public static int ID_EMPRESA_UNIDAD = 1;
        public static int CODIGO_UNIDAD = 2;
        public static int NOMBRE_UNIDAD = 3;
        public static int LOCKED_UNIDAD = 4;

        //DataSet Region
        public static int ID_REGION = 0;
        public static int NOMBRE_REGION = 1;
        public static int ID_EMPRESA_REGION = 2;
        public static int LOCKED_REGION = 3;
        public static int CODIGO_REGION = 4;

        //DataSet Area
        public static int ID_AREA = 0;
        public static int ID_EMPRESA_AREA = 1;
        public static int ID_UNIDAD_AREA = 2;
        public static int COD_AREA = 3;
        public static int NOMBRE_AREA = 4;
        public static int INACTIVO_AREA = 5;
        public static int LOCKED_AREA = 6;

        //DataSet Distrito
        public static int ID_DISTRITO = 0;
        public static int ID_EMPRESA_DISTRITO = 1;
        public static int ID_REGION_DISTRITO = 2;
        public static int NOMBRE_DISTRITO = 3;

        //DataSet Peticionario
        public static int FK_ID_EMPRESA = 0;
        public static int NOMBRE_PETICIONARIO = 1;
        public static int APELLIDO_PETICIONARIO = 2;
        public static int MOVIL_PETICIONARIO = 3;
        public static int EMAIL_PETICIONARIO = 4;
        public static int ID_REGION_PETICIONARIO = 5;
        public static int ID_DISTRITO_PETICIONARIO = 6;
        public static int ID_UNIDAD_PETICIONARIO = 7;
        public static int ID_CARGO_PETICIONARIO = 8;
        public static int NOMBRE_CARGO_PETICIONARIO = 9;
        public static int ID_AREA_PETICIONARIO = 10;
        public static int NOMBRE_AREA_PETICIONARIO = 11;
        public static int NOMBRE_DISTRITO_PETICIONARIO = 12;
        public static int CODIGO_UNIDAD_PETICIONARIO = 13;
        public static int NOMBRE_REGION_PETICIONARIO = 14;
        public static int CODIGO_REGION_PETICIONARIO = 15;
        public static int ES_DELEGADO = 17;
        public static int ES_MARKETING = 18;

        //Informe AMEC
        public static int INFORME_ID_AMEC = 0;
        public static int INFORME_AMEC = 1;
        public static int INFORME_ID_PETICIONARIO = 2;
        public static int INFORME_NOMBRE_Y_APELLIDO = 3;
        public static int INFORME_ID_CARGO = 4;
        public static int INFORME_CARGO = 5;
        public static int INFORME_WEIN = 6;
        public static int INFORME_FECHA = 7;
        public static int INFORME_AREA = 8;
        public static int INFORME_REGION = 9;
        public static int INFORME_DISTRITO = 10;
        public static int INFORME_PARAGUAS = 11;
        public static int INFORME_APROBADO = 12;
        public static int INFORME_TIPO_ACT_CONGRESO = 13;
        public static int INFORME_NOMBRE_PROGRAMA = 14;
        public static int INFORME_COD_GENESIS = 15;
        public static int INFORME_OBJETIVOS_PROGRAMA = 16;
        public static int INFORME_CONTENIDO = 17;
        public static int INFORME_LUGAR = 18;
        public static int INFORME_AMBITO_GEO = 19;
        public static int INFORME_DURACION = 20;
        public static int INFORME_NUM_PARTICIPANES = 21;
        public static int INFORME_FECHA_COMIENZO = 22;
        public static int INFORME_FECHA_FIN = 23;
        public static int INFORME_GASTOS = 24;
        public static int INFORME_CRITERIO_SELECCION1 = 25;
        public static int INFORME_CRITERIO_SELECCION2 = 26;
        public static int INFORME_CRITERIO_SELECCION3 = 27;
        public static int INFORME_RELACION_PONENTES = 28;
        public static int INFORME_NUM_PONENTES = 29;
        public static int INFORME_HONORARIOS = 30;
        public static int INFORME_CONCEPTO_GASTOS = 31;
        public static int INFORME_IMPORTE_TOTAL = 32;
        public static int INFORME_NUM_PROPUESTAS = 33;
        public static int INFORME_NUM_CARTAS = 34;
        public static int INFORME_NUM_HOJAS_INSCR = 35;
        public static int INFORME_NOMBRE_CONGRESO = 36;
        public static int INFORME_OBSERVACION = 37;
        public static int INFORME_VALORACION_FARMA_IND = 38;
        public static int INFORME_UNIDAD = 39;
        public static int INFORME_AREA1 = 40;
        public static int INFORME_AREA2 = 41;
        public static int INFORME_AREA3 = 42;
        public static int INFORME_ID_EXPEDIENTE = 43;
        public static int INFORME_NUM_AMEC_DOC = 44;
        //public static int INFORME_ = 00;

        //DataSet InfoExpediente
        public static int EXP_COD_UNIDAD = 0;
        public static int EXP_REGION = 1;
        public static int EXP_AREA = 2;
        public static int EXP_DISTRITO = 3;
        public static int EXP_NOMBRE_PETICIONARIO = 4;
        public static int EXP_APELLIDO_PETICIONARIO = 5;
        public static int EXP_CARGO_PETICIONARIO = 6;
        public static int EXP_AMEC = 7;
        public static int EXP_ESTADO_AMEC = 8;
        public static int EXP_APROBADO = 9;
        public static int EXP_NUM_ASISTENTES = 10;
        public static int EXP_IMPORTE_AMEC = 11;
        public static int EXP_IMPORTE_RESTANTE_AMEC = 12;
        public static int EXP_COMUNICADO = 13;
        public static int EXP_TIPO_ACTIVIDAD = 14;
        public static int EXP_ACTIVIDAD = 15;
        public static int EXP_SEDE = 16;
        public static int EXP_CATEGORIA_SEDE = 17;
        public static int EXP_LUGAR = 18;
        public static int EXP_FECHA_INICIO = 19;
        public static int EXP_FECHA_FIN = 20;
        public static int EXP_ESTADO = 21;
        public static int EXP_ID_EXPEDIENTE = 22;
        public static int EXP_NUM_PEDIDO = 23;
        public static int EXP_FECHA_EXPEDIENTE = 24;
        public static int EXP_IMPORTE_EXPEDIENTE = 25;
        public static int EXP_IMPORTE_INSCRIPCIONES = 26;
        public static int EXP_IMPORTE_ALOJAMIENTO = 27;
        public static int EXP_IMPORTE_OTROS = 28;
        public static int EXP_IMPORTE_DESPLAZAMIENTO = 29;
        public static int EXP_NUM_PAX = 30;
    }
}