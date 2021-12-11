using System;
using System.Collections.Generic;
using System.Linq;
using ATM.Model.DbModel.Entities;
using ATM.Model.Interfaces;

namespace ATM.Model.DbModel
{
    public class DbSerializer : ISerializer
    {
        private readonly AtmDbContext _context;

        public DbSerializer(AtmDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Címletlista betöltése.
        /// </summary>
        /// <returns>
        ///     Az adatbázisban tárolt címletek listája, ahol a címlet a kulcs és a darabszám az érték.
        /// </returns>
        public IDictionary<int, int> LoadContent()
        {
            var dbValues = _context.Denominations.ToList();                                 // az adatbázis sorai
            var result = new Dictionary<int, int>();                                        // az eredménylista, amibe pakolni fogunk   
            foreach (var dbValue in dbValues)                                               // végigtraverzálva a sorokon
            {
                if (AcceptedValues.AcceptedDenominations.Contains(dbValue.Denomination))    // az elfogadhatatlan címleteket figyelmen kívül hagyjuk
                {
                    result[dbValue.Denomination] = dbValue.Quantity;                        // az elfogadhatókat berakjuk az eredménylistába, a Denomination egyedi kulcs, nem lehetnek duplikációk
                }
            }
            return result;
        }


        /// <summary>
        ///     Címletlista tartalmának mentése.
        /// </summary>
        /// <param name="content">
        ///     A mentendő címletek listája, ahol a címlet a kulcs és a darabszám az érték.
        /// </param>
        public void SaveContent(IDictionary<int, int> content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            foreach (var item in content)                               // végigmegyünk a listán
            {
                Denominations dbValue = _context.Denominations
                    .FirstOrDefault(e => e.Denomination == item.Key);   // megnézzük, hogy az adatbázisban van-e már ilyen címlet
                if (dbValue == null)                                    // ha nincs  
                {
                    _context.Denominations.Add(new Denominations        // akkor létrehozzuk ezt a címletet
                    {
                        Denomination = item.Key,                        // címlet
                        Quantity = item.Value                           // mennyiség
                    });
                }
                else                                                    // ha már volt
                {
                    dbValue.Quantity = item.Value;                      // akkor csak a mennyiséget aktualizáljuk  
                }
            }

            _context.SaveChanges(true);                                 // végül elmentjük  
        }

    }
}
