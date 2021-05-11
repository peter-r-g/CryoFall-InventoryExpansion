namespace CryoFall.InventoryExpansion.Items
{
    using CryoFall.InventoryExpansion.Items.ExpanderDevices;

    public class ItemBackpackDevice : ProtoItemExpanderDevice
    {
        public override string Description => "A backpack you can store some items in.";

        public override string Name => "Backpack Device";

        public override bool OnlySingleDeviceOfThisProtoAppliesEffect => true;

        public override uint DurabilityMax => 100;

        public override byte ContainerSlotIncrease => 10;

        public override ContainerType ContainerType => ContainerType.Inventory;
    }
}
