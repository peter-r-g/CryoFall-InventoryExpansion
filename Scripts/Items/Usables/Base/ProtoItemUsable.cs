namespace CryoFall.InventoryExpansion.Items.Usables
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.Items;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Data.State;
    using AtomicTorch.CBND.GameApi.Scripting.Network;

    /// <summary>
    /// Item prototype for usable items (with state).
    /// </summary>
    public abstract class ProtoItemUsable<TPrivateState, TPublicState, TClientState> : ProtoItem<TPrivateState, TPublicState, TClientState>, IProtoItemUsable
        where TPrivateState : BasePrivateState, new()
        where TPublicState : BasePublicState, new()
        where TClientState : BaseClientState, new()
    {
        public override bool CanBeSelectedInVehicle => true;

        public string ItemUseCaption => ItemUseCaptions.Use;

        public override ushort MaxItemsPerStack => ItemStackSize.Small;

        protected override bool ClientItemUseFinish(ClientItemData data)
        {
            var item = data.Item;
            var character = Client.Characters.CurrentPlayerCharacter;

            if (!this.SharedCanUse(character))
            {
                return false;
            }

            this.ClientOnUse(character);

            this.CallServer(_ => _.ServerRemote_Use(item));
            return true;
        }

        protected override string GenerateIconPath()
        {
            return "Items/Usables/" + this.GetType().Name;
        }

        protected override void PrepareHints(List<string> hints)
        {
            base.PrepareHints(hints);
            hints.Add(ItemHints.AltClickToUseItem);
        }

        protected virtual void PrepareProtoItemUsable()
        {
        }

        protected virtual void ClientOnUse(ICharacter character)
        {
        }

        protected virtual void ServerOnUse(ICharacter character)
        {
        }

        protected virtual bool SharedCanUse(ICharacter character)
        {
            return true;
        }

        [RemoteCallSettings(DeliveryMode.ReliableOrdered, timeInterval: 0.2, clientMaxSendQueueSize: 20)]
        private void ServerRemote_Use(IItem item)
        {
            var character = ServerRemoteContext.Character;
            this.ServerValidateItemForRemoteCall(item, character);

            if (!this.SharedCanUse(character))
            {
                return;
            }

            this.ServerOnUse(character);

            Logger.Important(character + " has used " + item);

            this.ServerNotifyItemUsed(character, item);
            // decrease item count
            Server.Items.SetCount(item, (ushort)(item.Count - 1));
        }
    }

    /// <summary>
    /// Item prototype for usable items (without state).
    /// </summary>
    public abstract class ProtoItemUsable : ProtoItemUsable<EmptyPrivateState, EmptyPublicState, EmptyClientState>
    {
    }
}