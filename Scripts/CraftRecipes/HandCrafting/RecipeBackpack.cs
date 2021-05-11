namespace CryoFall.InventoryExpansion.Recipes
{
    using AtomicTorch.CBND.CoreMod.Items.Generic;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.CraftingStations;
    using AtomicTorch.CBND.CoreMod.Systems;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using CryoFall.InventoryExpansion.Items;
    using System;

    public class RecipeBackpack : Recipe.RecipeForHandCrafting
    {
        public override bool IsAutoUnlocked => true;

        protected override void SetupRecipe(out TimeSpan duration, InputItems inputItems, OutputItems outputItems, StationsList optionalStations)
        {
            optionalStations.Add<ObjectWorkbench>();

            duration = CraftingDuration.Medium;

            inputItems.Add<ItemPlanks>(count: 10);

            outputItems.Add<ItemBackpack>(count: 1);
        }
    }
}