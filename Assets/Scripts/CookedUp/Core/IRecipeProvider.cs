using System;
using KitchenObjects.ScriptableObjects;

public interface IRecipeProvider {
    BaseRecipeSO CurrentRecipe { get; }
    event EventHandler<ValueChangedEvent<BaseRecipeSO>> OnRecipeChanged;
}
