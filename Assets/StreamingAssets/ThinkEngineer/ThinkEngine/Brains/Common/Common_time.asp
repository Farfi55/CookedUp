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


% player_Recipe_ExpectedTime is an approximation of the time for a player to complete a
% recipe if it were assigned to him right now
% it accounts for time spent cutting or cooking
% if player already has a plate, the time to get ingredients already in the plate is not added to the time

% CASE: 1
% player with a valid plate
player_Recipe_ExpectedTime(PlayerID, RecipeName, ExpectedTime) :-
    c_CompleteRecipe_Name(RecipeName),
    playerBot_ID(PlayerID),
    player_OwnsAnyPlate_ValidForRecipe(PlayerID, RecipeName),
    ExpectedTime = #min{ ExpectedTime1 : 
        plate_Recipe_ExpectedTime(PlateID, PlayerID, RecipeName, ExpectedTime1),
        player_OwnedPlate_ValidForRecipe(PlayerID, PlateID, RecipeName)
    }.

% CASE 2:
% player without valid plates
player_Recipe_ExpectedTime(PlayerID, RecipeName, ExpectedTime) :-
    c_CompleteRecipe_Name(RecipeName),
    playerBot_ID(PlayerID),
    not player_OwnsAnyPlate_ValidForRecipe(PlayerID, RecipeName),
    IngredientsExpectedTime = #sum{ Time, IngredientName : 
        ingredient_ExpectedGetTime(IngredientName, PlayerID, Time),
        c_CompleteRecipe_Ingredient(RecipeName, IngredientName)
    },
    ExpectedTime = IngredientsExpectedTime + 1000.





    


    
        
    
