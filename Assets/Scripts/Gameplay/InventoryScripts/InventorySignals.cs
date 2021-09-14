using Scripts.Utils;

namespace Scripts.Gameplay.InventoryScripts
{
    public class OpenInventorySignal : ASignal<Inventory> { }
    public class CloseInventorySignal : ASignal<Inventory> { }
    public class UpdateInventorySignal : ASignal<Inventory> { }

    public class HandSlotUpdateSignal : ASignal<ItemSlot> { }
}