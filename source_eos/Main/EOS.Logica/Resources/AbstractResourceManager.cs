using System;
using System.Resources;
using System.Globalization;

namespace EOS.Logica.Resources
{
    public class AbstractResourceManager
    {
        protected string _resourceFile;
        protected System.Resources.ResourceManager _resourceManager;

        protected AbstractResourceManager(string resourceFile)
        {
            _resourceFile = resourceFile;
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().ToString();
            assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(",")).Trim();
            _resourceManager = new ResourceManager(assemblyName + "." + _resourceFile, System.Reflection.Assembly.GetExecutingAssembly());
        }

        public virtual string Get(string propertyName)
        {
            return _resourceManager.GetString(propertyName);
        }

        public virtual string Get(string propertyName, CultureInfo culture)
        {
            return _resourceManager.GetString(propertyName, culture);
        }
    }
}
