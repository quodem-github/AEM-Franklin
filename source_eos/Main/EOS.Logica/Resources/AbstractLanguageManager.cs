using System;
using System.Resources;
using System.Globalization;

namespace EOS.Logica.Resources
{
    /// <summary>
    /// Descripción breve de RecursoIdioma.
    /// </summary>
    public abstract class AbstractLanguageManager : AbstractResourceManager
    {
        protected AbstractLanguageManager(string resourceFile)
            : base(resourceFile)
        {
        }

        public override string Get(string propertyName, CultureInfo culture)
        {
            return _resourceManager.GetString(propertyName, culture);
        }

        public override string Get(string propertyName)
        {
            try
            {
                return _resourceManager.GetString(propertyName);
            }
            catch (System.Resources.MissingManifestResourceException ex)
            {
                Exception newExcpt = new Exception("No se ha especificado el archivo '" + _resourceFile + "' para el idioma '" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + "'" + "\r\n" + ex.Message);
                throw newExcpt;
            }
        }

    }
}
