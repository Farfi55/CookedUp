ingredient_ExpectedGetTime(IngredientName, Time) :-
    c_KO_Name(IngredientName),
    ingredient_Available(IngredientName),
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


plate_Recipe_ExpectedTime(PlateID, RecipeName, ExpectedTime) :-
    plate_ID(PlateID),
    c_CompleteRecipe_Name(RecipeName),
    not plate_Any_InvalidIngredients(PlateID, RecipeName),
    plate_Any_MissingIngredients(PlateID, RecipeName),
    ExpectedTime = #sum{ Time, IngredientName :
        plate_MissingIngredients_Name(PlateID, RecipeName, IngredientName),
        ingredient_ExpectedGetTime(IngredientName, Time) 
    }.

plate_Recipe_ExpectedTime(PlateID, RecipeName, ExpectedTime) :-
    plate_ID(PlateID),
    c_CompleteRecipe_Name(RecipeName),
    plate_Any_InvalidIngredients(PlateID, RecipeName),
    ExpectedTime = 1000000.

plate_Recipe_ExpectedTime(PlateID, RecipeName, ExpectedTime) :-
    plate_ID(PlateID),
    c_CompleteRecipe_Name(RecipeName),
    plate_CompletedRecipe(PlateID, RecipeName),
    ExpectedTime = 0.


    
        
    
