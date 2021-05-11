namespace CryoFall.InventoryExpansion.Items.ExpanderDevices
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.Characters.Player;
    using AtomicTorch.CBND.CoreMod.ItemContainers;
    using AtomicTorch.CBND.CoreMod.Items;
    using AtomicTorch.CBND.CoreMod.Items.Equipment;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Data.State;
    using AtomicTorch.CBND.GameApi.Scripting.Network;

    /// <summary>
    /// Item prototype for expander devices (with state).
    /// </summary>
    public abstract class ProtoItemExpanderDevice<TPrivateState, TPublicState, TClientState> : ProtoItemEquipmentDevice, IProtoItemExpanderDevice
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

        public override void ClientOnItemDrop(IItem item, IItemsContainer itemsContainer = null)
        {
            base.ClientOnItemDrop(item, itemsContainer);
            if (itemsContainer is null)
            {
                itemsContainer = item.Container;
                if (itemsContainer is null)
                {
                    return;
                }
            }

            if (itemsContainer.ProtoItemsContainer is ItemsContainerCharacterEquipment)
            {
                this.CallServer(_ => _.ServerRemote_Equip(item, itemsContainer));
            }
        }

        public override void ClientOnItemPick(IItem item, IItemsContainer fromContainer)
        {
            base.ClientOnItemPick(item, fromContainer);
            if (fromContainer?.ProtoItemsContainer is ItemsContainerCharacterEquipment)
            {
                this.CallServer(_ => _.ServerRemote_Unequip(item, fromContainer));
            }
        }

        protected override string GenerateIconPath()
        {
            return "Items/ExpanderDevices/" + this.GetType().Name;
        }

        protected virtual void PrepareProtoItemExpanderDevice()
        {
        }

        protected virtual void ServerOnEquip(ICharacter character, IItemsContainer equipmentSlot)
        {
            IItemsContainer container = GetItemsContainer(character);

            if (container is null)
            {
                return;
            }

            int newContainerSize = container.SlotsCount + ContainerSlotIncrease;
            if (ContainerType == ContainerType.HotBar && newContainerSize > 10)
            {
                newContainerSize = 10;
            }
            else if (ContainerType == ContainerType.Inventory && newContainerSize > 255)
            {
                newContainerSize = 255;
            }

            Server.Items.SetSlotsCount(container, (byte)newContainerSize);
        }

        protected virtual void ServerOnUnequip(ICharacter character, IItemsContainer equipmentSlot)
        {
            IItemsContainer container = GetItemsContainer(character);

            if (container is null)
            {
                return;
            }

            int newContainerSize = container.SlotsCount - ContainerSlotIncrease;
            if (newContainerSize < 1)
            {
                newContainerSize = 1;
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

        [RemoteCallSettings(DeliveryMode.ReliableOrdered, timeInterval: 0.2, clientMaxSendQueueSize: 20)]
        private void ServerRemote_Equip(IItem item, IItemsContainer equipmentSlot)
        {
            var character = ServerRemoteContext.Character;
            this.ServerValidateItemForRemoteCall(item, character);

            this.ServerOnEquip(character, equipmentSlot);

            Logger.Important(character + " has equipped " + item);
        }

        [RemoteCallSettings(DeliveryMode.ReliableOrdered, timeInterval: 0.2, clientMaxSendQueueSize: 20)]
        private void ServerRemote_Unequip(IItem item, IItemsContainer equipmentSlot)
        {
            var character = ServerRemoteContext.Character;
            this.ServerValidateItemForRemoteCall(item, character);

            this.ServerOnUnequip(character, equipmentSlot);

            Logger.Important(character + " has unequipped " + item);
        }
    }

    public enum ContainerType
    {
        Inventory,
        HotBar
    }

    /// <summary>
    /// Item prototype for expander devices (without state).
    /// </summary>
    public abstract class ProtoItemExpanderDevice : ProtoItemExpanderDevice<EmptyPrivateState, EmptyPublicState, EmptyClientState>
    {
    }
}