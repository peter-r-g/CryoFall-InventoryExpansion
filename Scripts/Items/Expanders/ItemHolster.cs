namespace CryoFall.InventoryExpansion.Items
{
    using CryoFall.InventoryExpansion.Items.Expanders;

    public class ItemHolster : ProtoItemExpander
    {
        public override string Description => "A simple holster, useful for carrying an extra item on your hotbar.";

        public override ushort MaxItemsPerStack => 1;

        public override string Name => "Holster";

        public override byte ContainerSlotIncrease => 1;

        public override ContainerType ContainerType => ContainerType.HotBar;
    }
}