namespace CryoFall.InventoryExpansion.Items.Expanders
{
    using AtomicTorch.CBND.CoreMod.Characters.Player;
    using CryoFall.InventoryExpansion.Items.Usables;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Data.State;

    /// <summary>
    /// Item prototype for expander items (with state).
    /// </summary>
    public abstract class ProtoItemExpander<TPrivateState, TPublicState, TClientState> : ProtoItemUsable<TPrivateState, TPublicState, TClientState>, IProtoItemUsable
        where TPrivateState : BasePrivateState, new()
        where TPublicState : BasePublicState, new()
        where TClientState : BaseClientState, new()
    {
        /// <summary>
        /// The number of slots this item adds to the container
        /// </summary>
        public abstract byte ContainerSlotIncrease { get; }

        /// <summary>
        /// The type of container to add the slots to
        /// </summary>
        public abstract ContainerType ContainerType { get; }

        protected override string GenerateIconPath()
        {
            return "Items/Expanders/" + this.GetType().Name;
        }

        protected sealed override void PrepareProtoItemUsable()
        {
        }

        protected virtual void PrepareProtoItemExpander()
        {
        }

        protected override bool SharedCanUse(ICharacter character)
        {
            if (character.IsNpc || character.IsStatic)
            {
                return false;
            }

            IItemsContainer container = GetItemsContainer(character);

            if ((ContainerType == ContainerType.HotBar && container?.SlotsCount == 10) ||
                (ContainerType == ContainerType.Inventory && container?.SlotsCount == 255))
            {
                return false;
            }

            return true;
        }

        protected override void ServerOnUse(ICharacter character)
        {
            IItemsContainer container = GetItemsContainer(character);

            if (container is null)
            {
                return;
            }

            int newContainerSize = container.SlotsCount + ContainerSlotIncrease;
            if (newContainerSize < 1)
            {
                newContainerSize = 1;
            }
            else if (ContainerType == ContainerType.HotBar && newContainerSize > 10)
            {
                newContainerSize = 10;
            }
            else if (ContainerType == ContainerType.Inventory && newContainerSize > 255)
            {
                newContainerSize = 255;
            }

            Server.Items.SetSlotsCount(container, (byte)newContainerSize);
        }

        private IItemsContainer GetItemsContainer(ICharacter character)
        {
            switch (ContainerType)
            {
                case ContainerType.HotBar:
                    return character.SharedGetPlayerContainerHotbar();
                case ContainerType.Inventory:
                    return character.SharedGetPlayerContainerInventory();
                default:
                    return null;
            }
        }
    }

    public enum ContainerType
    {
        Inventory,
        HotBar
    }

    /// <summary>
    /// Item prototype for expander items (without state).
    /// </summary>
    public abstract class ProtoItemExpander : ProtoItemExpander<EmptyPrivateState, EmptyPublicState, EmptyClientState>
    {
    }
}
