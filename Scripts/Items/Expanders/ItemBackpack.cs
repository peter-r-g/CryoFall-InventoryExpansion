namespace CryoFall.InventoryExpansion.Items
{
    using CryoFall.InventoryExpansion.Items.Expanders;

    public class ItemBackpack : ProtoItemExpander
    {
        public override string Description => "A simple backpack, useful for carrying more on your person.";

        public override ushort MaxItemsPerStack => 1;

        public override string Name => "Backpack";

        public override byte ContainerSlotIncrease => 20;

        public override ContainerType ContainerType => ContainerType.Inventory;
    }
}