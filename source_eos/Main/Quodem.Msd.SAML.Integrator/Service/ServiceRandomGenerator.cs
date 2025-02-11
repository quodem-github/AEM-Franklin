using System;
using System.Collections.Generic;
using Quodem.Msd.SAML.Integrator.Randomizer;

namespace Quodem.Msd.SAML.Integrator.Service
{
    public class ServiceRandomGenerator
    {
        #region Properties

        private readonly List<IRandomGenerator> _services;
        private Random r;

        #endregion

        #region Constructor

        public ServiceRandomGenerator()
        {
            _services = new List<IRandomGenerator>()
            {
                new LetterRandomGenerator(),
                new NumberRandomGenerator()
            };
            r = new Random();
        }

        #endregion

        #region Methods

        public string GetRandomWord(int lengthWord)
        {
            string result = string.Empty;
            for (int i = 0; i < lengthWord; i++)
            {
                int randomPosition = r.Next(0, _services.Count);
                result += _services[randomPosition].GenerateRandomElement();
            }
            return result;
        }

        #endregion
    }
}
