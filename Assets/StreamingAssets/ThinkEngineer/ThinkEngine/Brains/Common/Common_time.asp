ingredient_ExpectedGetTime(IngredientName, PlayerID, Time) :-
    player_ID_And_0(PlayerID),
    c_KO_Name(IngredientName),
    ingredient_Available_ForPlayer(IngredientName, PlayerID),
    Time = 2000.

ingredient_ExpectedGetTime(IngredientName, PlayerID, Time) :-
    player_ID_And_0(PlayerID),
    c_KO_Name(IngredientName),
    ingredient_NeedsCooking_ForPlayer(IngredientName, PlayerID, RecipeName, _),
    c_CookingRecipe(RecipeName, _, IngredientName, TimeToCook, _),
    Time = 3000 + TimeToCook.

ingredient_ExpectedGetTime(IngredientName, PlayerID, Time) :-
    player_ID_And_0(PlayerID),
    c_KO_Name(IngredientName),
    ingredient_NeedsCutting_ForPlayer(IngredientName, PlayerID, RecipeName, _),
    c_CuttingRecipe(RecipeName, _, IngredientName, TimeToCut),
    Time = 3000 + TimeToCut.


plate_Recipe_ExpectedTime(PlateID, PlayerID, RecipeName, ExpectedTime) :-
    plate_ID(PlateID),
    player_ID_And_0(PlayerID),
    c_CompleteRecipe_Name(RecipeName),
    not plate_Any_InvalidIngredients(PlateID, RecipeName),
    plate_Any_MissingIngredients(PlateID, RecipeName),
    ExpectedTime = #sum{ Time, IngredientName :
        plate_MissingIngredients_Name(PlateID, RecipeName, IngredientName),
        ingredient_ExpectedGetTime(IngredientName, PlayerID, Time) 
            
    }.

plate_Recipe_ExpectedTime(PlateID, PlayerID, RecipeName, ExpectedTime) :-
    plate_ID(PlateID),
    player_ID_And_0(PlayerID),
    c_CompleteRecipe_Name(RecipeName),
    plate_Any_InvalidIngredients(PlateID, RecipeName),
    ExpectedTime = 1000000.

plate_Recipe_ExpectedTime(PlateID, PlayerID, RecipeName, ExpectedTime) :-
    plate_ID(PlateID),
    player_ID_And_0(PlayerID),
    c_CompleteRecipe_Name(RecipeName),
    plate_CompletedRecipe(PlateID, RecipeName),
    ExpectedTime = 0.



player_Best_Plate_ValidForRecipe(PlayerID, PlateID, RecipeName) :-
    player_ID(PlayerID),
    c_CompleteRecipe_Name(RecipeName),
    player_OwnsAnyPlate_ValidForRecipe(PlayerID, RecipeName),
    ExpectedTime = #min{ ExpectedTime1 : 
        player_OwnedPlate_ValidForRecipe(PlayerID, PlateID1, RecipeName),
        plate_Recipe_ExpectedTime(PlateID1, PlayerID, RecipeName, ExpectedTime1)
    },
    player_OwnedPlate_ValidForRecipe(PlayerID, PlateID, RecipeName),
    plate_Recipe_ExpectedTime(PlateID, PlayerID, RecipeName, ExpectedTime).

player_Best_Plate_ValidForRecipe_MaxID(PlayerID, PlateID, RecipeName) :-
    player_Best_Plate_ValidForRecipe(PlayerID, _, RecipeName),
    PlateID = #max{ PlateID1 : 
        player_OwnedPlate_ValidForRecipe(PlayerID, PlateID1, RecipeName)
    }.




    


    
        
    
