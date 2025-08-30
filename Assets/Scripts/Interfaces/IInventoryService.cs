using Enums;

namespace Interfaces
{
    public interface IInventoryService
    {
        int GetObjectAmount(IEquippable sourceType);
        int GetSourceAmount(SourceType sourceType);
    }
}