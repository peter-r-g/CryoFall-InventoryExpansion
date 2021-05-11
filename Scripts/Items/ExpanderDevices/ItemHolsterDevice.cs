namespace CryoFall.InventoryExpansion.Items
{
    using CryoFall.InventoryExpansion.Items.ExpanderDevices;

    public class ItemHolsterDevice : ProtoItemExpanderDevice
    {
        public override string Description => "An extra holster never hurts!";

        public override string Name => "Holster Device";

        public override bool OnlySingleDeviceOfThisProtoAppliesEffect => true;

        public override uint DurabilityMax => 100;

        public override byte ContainerSlotIncrease => 1;

        public override ContainerType ContainerType => ContainerType.HotBar;
    }
}
