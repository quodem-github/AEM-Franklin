using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;


namespace EOS.Web
{
    public class AgenteBase
    {
        public delegate IList<T> FuncionAccesoARepositorioSinParametros<T>();
        public delegate IList<T> FuncionAccesoARepositorioConParametros<T, P>(P parametro);

        protected HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;

            }
        }  

        public IList<Tipo> ObtenerDesdeCacheORepositorio<Tipo>(string claveCache, FuncionAccesoARepositorioSinParametros<Tipo> facceso, bool bForceReload = false)
        {
            IList<Tipo> provincias = null;
            if (claveCache == "ColeccionPoblacines" || claveCache == "ColeccionPoblacinesTodas")
                HttpContext.Current.Cache.Remove(claveCache);

            IList<Tipo> cacheado = (IList<Tipo>)HttpContext.Current.Cache[claveCache];
            if (cacheado != null && !bForceReload)
            {
                provincias = (IList<Tipo>)HttpContext.Current.Cache.Get(claveCache);
            }
            else
            {
                provincias = facceso();
                if (provincias != null)
                {
                    HttpContext.Current.Cache[claveCache] = provincias;
                }
            }
            return provincias;
        }

        public IList<Tipo> ObtenerDesdeCacheORepositorio<Tipo, TipoP>(string claveCache, FuncionAccesoARepositorioConParametros<Tipo, TipoP> facceso, TipoP parametro)
        {
            IList<Tipo> provincias = null;
            provincias = (IList<Tipo>)HttpContext.Current.Cache[claveCache];
            if (provincias == null)
            {
                provincias = facceso(parametro);
                if (provincias != null)
                {
                    HttpContext.Current.Cache[claveCache] = provincias;
                }
            }
            return provincias;
        }

        public IList<Tipo> ObtenerDesdeCacheORepositorio<Tipo>(string claveCache, FuncionAccesoARepositorioSinParametros<Tipo> facceso, Tipo vacio)
        {
            IList<Tipo> provincias = null;
            provincias = (IList<Tipo>)HttpContext.Current.Cache[claveCache];
            if (provincias == null)
            {
                provincias = facceso();
                provincias.Insert(0, vacio);

                if (provincias != null)
                {
                    HttpContext.Current.Cache[claveCache] = provincias;
                }
            }
            return provincias;
        }

    }
}
