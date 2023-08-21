using System;
using CookedUp.Core.KitchenObjects;
using Shared;

namespace CookedUp.Core
{
    public interface IRecipeProvider {
        BaseRecipeSO CurrentRecipe { get; }
        event EventHandler<ValueChangedEvent<BaseRecipeSO>> OnRecipeChanged;
    }
}
