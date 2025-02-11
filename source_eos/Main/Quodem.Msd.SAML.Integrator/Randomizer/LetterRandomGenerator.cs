using System;
using Quodem.Msd.SAML.Integrator.Enums;

namespace Quodem.Msd.SAML.Integrator.Randomizer
{
    public class LetterRandomGenerator : IRandomGenerator
    {
        #region const

        private const string ElementsAllowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion

        #region Properties

        private Random r;

        #endregion

        #region Constructor

        public LetterRandomGenerator()
        {
            r = new Random();
        }

        #endregion

        #region Methods

        public bool IsMatch(ERandomTypes randomType)
        {
            return randomType == ERandomTypes.Letter;
        }

        public char GenerateRandomElement()
        {
            int randomPosition = r.Next(0, ElementsAllowed.Length);
            return ElementsAllowed[randomPosition];
        }

        #endregion
    }
}
