using System;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Entities.Characters;

namespace MetroDigger.Gameplay.Entities.Others
{
    public abstract class Item : StaticEntity
    {

        public virtual void GetCollected(ICollector collector)
        {
            RaiseCollected(this, collector);
        }

        public event Action<Item, ICollector> Collected;

        protected void RaiseCollected(Item item, ICollector collector)
        {
            if(Collected!=null)
                Collected(item, collector);
        }
    }
}
