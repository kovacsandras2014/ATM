using System.Collections.Generic;

namespace ATM.Model.Interfaces
{
        /// <summary>
        ///     Egy ATM implementációtól elvárt működés
        /// </summary>
        public interface IAtm
        {
            /// <summary>
            ///     Összeg feltöltése az ATM-be.
            /// </summary>
            /// <param name="charge">
            ///     A feltöltendő összegek listája, ahol a címlet a kulcs és a darabszám az érték.
            /// </param>
            /// <returns>
            ///     Az ATM pillanatnyi össz pénzkészlete.
            /// </returns>
            int Charge(IDictionary<int, int> charge);

            /// <summary>
            ///     Összeg kivétele az ATM-ből.
            /// </summary>
            /// <param name="money">
            ///     A kiadandó összeg.
            /// </param>
            /// <returns>
            ///     A kiadott összeg címlet-darabszám lista bontásban, ahol a címlet a kulcs, a darabszám az érték.
            /// </returns>
            IDictionary<int, int> Pay(int money);
        }
}
