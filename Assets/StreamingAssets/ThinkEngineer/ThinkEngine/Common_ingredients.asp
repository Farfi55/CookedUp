ingredient_Available(IngredientName, TargetID) :-
    containerCounter_KOType(TargetID, IngredientName).

ingredient_Available(IngredientName, TargetID) :-
    counter_HasAny(TargetID),
    counter_KitchenObjects(TargetID, _, IngredientName).

ingredient_Available_Any(IngredientName) :-
    ingredient_Available(IngredientName, _).

ingredient_NotAvailable(IngredientName) :-
    c_KO_Name(IngredientName),
    not ingredient_Available_Any(IngredientName).


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


ingredient_ExpectedGetTime(IngredientName, Time) :-
    c_KO_Name(IngredientName),
    ingredient_Available_Any(IngredientName),
    Time = 2000.

ingredient_ExpectedGetTime(IngredientName, Time) :-
    c_KO_Name(IngredientName),
    ingredient_NeedsCooking(IngredientName, RecipeName, _),
    c_CookingRecipe(RecipeName, _, IngredientName, TimeToCook, _),
    Time = 3000 + TimeToCook.

ingredient_ExpectedGetTime(IngredientName, Time) :-
    c_KO_Name(IngredientName),
    ingredient_NeedsCutting(IngredientName, RecipeName, _),
    c_CuttingRecipe(RecipeName, _, IngredientName, TimeToCut),
    Time = 3000 + TimeToCut.

