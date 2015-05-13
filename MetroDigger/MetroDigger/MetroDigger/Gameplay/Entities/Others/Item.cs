using System;
using MetroDigger.Gameplay.Abstract;

namespace MetroDigger.Gameplay.Entities.Others
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla przedmiotów, które mogą zostać podniesione przez ICollectora
    /// </summary>
    public abstract class Item : StaticEntity
    {
        /// <summary>
        /// Metoda wywoływana, gdy obiekt zostaje podniesiony
        /// </summary>
        /// <param name="collector">ICollectory podnoszący obiekt</param>
        public virtual void GetCollected(ICollector collector)
        {
            RaiseCollected(this, collector);
        }
        /// <summary>
        /// Zdarzenie wywoływane w momencie, gdy obiekt zostaje zebrany
        /// </summary>
        public event Action<Item, ICollector> Collected;

        private void RaiseCollected(Item item, ICollector collector)
        {
            if(Collected!=null)
                Collected(item, collector);
        }
    }
}
