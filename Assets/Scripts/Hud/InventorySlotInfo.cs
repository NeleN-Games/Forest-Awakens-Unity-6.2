using Interfaces;

namespace Hud
{
    public class InventorySlotInfo
    {
        public IEquippable Equippable;
        public int Count;

        public override bool Equals(object obj)
        {
            return obj is InventorySlotInfo other &&
                   Equippable.Equals(other.Equippable);
        }

        public override int GetHashCode()
        {
            return Equippable.GetHashCode();
        }
    }
}
