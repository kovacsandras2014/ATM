using System.Collections.Generic;

namespace ATM.Model.Interfaces
{
    /// <summary>
    ///     Egy ATM implementáció perzisztálásától elvárt működés
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        ///     Az ATM tárolt értékeinek betöltése.
        /// </summary>
        /// <returns>
        ///     Az ATM-ben tárolt címletek listája, ahol címlet a kulcs és darabszám az érték.
        /// </returns>
        IDictionary<int, int> LoadContent();


        /// <summary>
        ///     Az ATM pillanatnyi tartalmának mentése.
        /// </summary>
        /// <param name="content">
        ///     A mentendő címletek listája, ahol a címlet a kulcs és a darabszám az érték.
        /// </param>
        void SaveContent(IDictionary<int, int> content);
    }
}
