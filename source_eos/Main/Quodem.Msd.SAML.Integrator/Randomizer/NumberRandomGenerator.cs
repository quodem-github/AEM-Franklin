using System;
using Quodem.Msd.SAML.Integrator.Enums;

namespace Quodem.Msd.SAML.Integrator.Randomizer
{
    public class NumberRandomGenerator : IRandomGenerator
    {
        #region const

        private const string ElementsAllowed = "0123456789";

        #endregion

        #region Properties

        private Random r;

        #endregion

        #region Constructor

        public NumberRandomGenerator()
        {
            r = new Random();
        }

        #endregion

        #region Methods

        public bool IsMatch(ERandomTypes randomType)
        {
            return randomType == ERandomTypes.Number;
        }

        public char GenerateRandomElement()
        {
            int randomPosition = r.Next(0, ElementsAllowed.Length);
            return ElementsAllowed[randomPosition];
        }

        #endregion
    }
}
