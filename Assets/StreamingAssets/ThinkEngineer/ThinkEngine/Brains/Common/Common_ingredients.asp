ingredient_Available_Target(IngredientName, TargetID) :-
    containerCounter_KOType(TargetID, IngredientName).

ingredient_Available_Target(IngredientName, TargetID) :-
    counter_HasAny(TargetID),
    counter_KOs(TargetID, _, IngredientName).

ingredient_Available(IngredientName) :-
    ingredient_Available_Target(IngredientName, _).

ingredient_NotAvailable(IngredientName) :-
    c_KO_Name(IngredientName),
    not ingredient_Available(IngredientName).


ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_CookingRecipe(RecipeName, InputIngredientName, OutputIngredientName, _, _).

ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_CuttingRecipe(RecipeName, InputIngredientName, OutputIngredientName, _).

ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName).

ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName).


% TODO: create a predicate that checks if an ingredient has a use in a recipe
% or can be used to make another ingredient
ingredient_IsTrash("MeatPattyBurned").
