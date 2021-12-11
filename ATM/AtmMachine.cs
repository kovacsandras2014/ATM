using System;
using System.Collections.Generic;
using System.Linq;
using ATM.Model;
using ATM.Model.Exceptions;
using ATM.Model.Interfaces;


namespace ATM
{
    /// <summary>
    ///     Egy lehetséges ATM implementáció.
    /// </summary>
    public class AtmMachine : IAtm
    {

        private readonly ISerializer _serializer;                       // az ATM tartalmát szerializáló entitás (konkrétan SQLite DB lesz)
        private IDictionary<int, int> _lazyContent;                     // az ATM pillanatnyi tartalma, ahol a kulcs a címlet, az érték a darabszám

        private IDictionary<int, int> Content                           // ezt a mezőt fogjuk használni a lazy load elfedésére
        {
            get { return _lazyContent ??= _serializer.LoadContent(); }  // ha még nem ismert a tartalom, akkor betöltjük a szerializálóval
        }                                                               // éppenséggel megtehetnénk a konstruktorban is, de nekem azt tanították, hogy nem illik

        public AtmMachine(ISerializer serializer)
        {
            _serializer = serializer;
        }


        /// <summary>
        ///     Összeg feltöltése az ATM-be.
        /// </summary>
        /// <param name="charge">
        ///     A feltöltendő összegek listája, ahol a címlet a kulcs és a darabszám az érték.
        /// </param>
        /// <returns>
        ///     Az ATM pillanatnyi össz pénzkészlete.
        /// </returns>
        public int Charge(IDictionary<int, int> charge)
        {
            if (charge == null) throw new ArgumentNullException(nameof(charge));

            // elfogadhatatlan címlet
            if (charge.Any(v => !AcceptedValues.AcceptedDenominations.Contains(v.Key)))
                throw new AtmUnacceptedValueException("Charge value contains unaccepted denomination!");

            int total = 0;                                              // ide gyűjtjük az összeget
            foreach (var item in charge)                                // végiglépdelünk a paraméterlistán
            {
                Content.TryGetValue(item.Key, out int currentValue);    // ha van már ilyen címlet, akkor kivesszük a darabszámát
                currentValue += item.Value;                             // hozzáadjuk a darabszámhoz a paraméterlistában talált darabszámot
                Content[item.Key] = currentValue;                       // az így növelt értéket visszatesszük az ATM listájába
                total += item.Key * currentValue;                       // mindösszesen
            }
            _serializer.SaveContent(Content);                           // mentjük az update-elt tartalmat

            return total;
        }


        /// <summary>
        ///     Összeg kivétele az ATM-ből.
        /// </summary>
        /// <param name="money">
        ///     A kiadandó összeg.
        /// </param>
        /// <returns>
        ///     A kiadott összeg címlet-darabszám lista bontásban, ahol a címlet a kulcs, a darabszám az érték.
        /// </returns>
        public IDictionary<int, int> Pay(int money)
        {
            if (money <= 0) throw new ArgumentOutOfRangeException(nameof(money));                                       // csak pozitív összegeket tudunk kiadni
            if (money % 1000 != 0) throw new AtmUnacceptedValueException($"{money} is not divisible by thousands");     // és csak ezerrel oszthatókat  

            var result = new Dictionary<int, int>();
            int desiredMoney = money;
            foreach (int denomination in AcceptedValues.AcceptedDenominations)                  // végignézzük a címleteket a legnagyobbtól lefelé
            {
                if (denomination > desiredMoney) continue;                                      // eleve nem kell olyan címlettel foglalkozni, ami nagyobb, mint a kívánt összeg
                if (Content.TryGetValue(denomination, out int givenAmount) && givenAmount > 0)  // ha van ilyen címlet az ATM-ben, akkor...    
                {
                    int paidAmount = Math.Min(desiredMoney / denomination, givenAmount);        // kiigénylünk annyit amennyi fedezi a kívánt összeget, vagy ha nincs annyi az ATM-ben, akkor a létező mennyiséget
                    result[denomination] = paidAmount;                                          // ennyit kell/lehet kiigényelni a gépből, de még nem vesszük ki, mert lehet hogy a későbbiekben hiba lesz!
                    desiredMoney -= denomination * paidAmount;                                  // ennyi pénzt kell még kiigényelni
                    if (desiredMoney == 0) break;                                               // ha kiigényeltük az egész kívánt mennyiséget, nincs tovább
                }
            }

            if (desiredMoney > 0)                                               // hoppá, nem volt fedezet a teljes összegre!                                      
                throw new AtmPaymentException($"{money} cannot be paid");

            // Most viszont le kell vonni a kigényelt összegeket az ATM-ből
            foreach (var item in result)
            {
                Content[item.Key] -= item.Value;                                // Nem kell TryGetValue-val kísérletezgetni, hiszen az eredmény összeállításánál már láttuk, hogy van benne annyi                            
            }
            _serializer.SaveContent(Content);                                   // mentjük az update-elt tartalmat

            return result;
        }

    }
}
