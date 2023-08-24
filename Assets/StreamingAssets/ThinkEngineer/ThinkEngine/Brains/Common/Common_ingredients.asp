ingredient_Available_Target(IngredientName, TargetID) :-
    containerCounter_KOType(TargetID, IngredientName).

% is there a kitchen object which has no player owning it
% and is contained in a counter
ingredient_Available_Target(IngredientName, TargetID) :-
    counter_HasAny(TargetID),
    counter_KOs(TargetID, IngredientID, IngredientName),
    not ko_HasPlayer(IngredientID).

ingredient_Available(IngredientName) :-
    ingredient_Available_Target(IngredientName, _).

ingredient_NotAvailable(IngredientName) :-
    c_KO_Name(IngredientName),
    not ingredient_Available(IngredientName).

% we use 0 as a special player ID to indicate no player
% wich should give the same result as the predicate without the player ID
% e.g. ingredient_Available_Target/2 in this case
ingredient_Available_ForPlayer_Target(IngredientName, PlayerID, TargetID) :-
    player_ID_And_0(PlayerID),
    ingredient_Available_Target(IngredientName, TargetID).

ingredient_Available_ForPlayer_Target(IngredientName, PlayerID, TargetID) :-
    player_ID(PlayerID),
    counter_HasAny(TargetID),
    counter_KOs(TargetID, IngredientID, IngredientName),
    ko_HasPlayer(IngredientID),
    ko_Player_ID(IngredientID, PlayerID).

ingredient_Available_ForPlayer(IngredientName, PlayerID) :-
    ingredient_Available_ForPlayer_Target(IngredientName, PlayerID, _).

ingredient_NotAvailable_ForPlayer(IngredientName, PlayerID) :-
    c_KO_Name(IngredientName),
    player_ID_And_0(PlayerID),
    not ingredient_Available_ForPlayer(IngredientName, PlayerID).

ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_CookingRecipe(RecipeName, InputIngredientName, OutputIngredientName, _, _).

ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_CuttingRecipe(RecipeName, InputIngredientName, OutputIngredientName, _).

% NeedsWork means NeedsCooking or NeedsCutting
ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName).

ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName).

ingredient_NeedsCooking_ForPlayer(IngredientName, PlayerID, RecipeName, InputIngredientName) :-
    player_ID_And_0(PlayerID),
    ingredient_NotAvailable_ForPlayer(IngredientName, PlayerID),
    IngredientName = OutputIngredientName,
    c_CookingRecipe(RecipeName, InputIngredientName, OutputIngredientName, _, _).

ingredient_NeedsCutting_ForPlayer(IngredientName, PlayerID, RecipeName, InputIngredientName) :-
    player_ID_And_0(PlayerID),
    ingredient_NotAvailable_ForPlayer(IngredientName, PlayerID),
    IngredientName = OutputIngredientName,
    c_CuttingRecipe(RecipeName, InputIngredientName, OutputIngredientName, _).

ingredient_NeedsWork_ForPlayer(IngredientName, PlayerID, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCooking_ForPlayer(IngredientName, PlayerID, RecipeName, InputIngredientName).

ingredient_NeedsWork_ForPlayer(IngredientName, PlayerID, RecipeName, InputIngredientName) :-
    ingredient_NeedsCutting_ForPlayer(IngredientName, PlayerID, RecipeName, InputIngredientName).


% TODO: create a predicate that checks if an ingredient has a use in a recipe
% or can be used to make another ingredient
ingredient_IsTrash("MeatPattyBurned").


any_Ingredient_WorkCounter_ID(WorkCounterID) :-
    stoveCounter_ID(WorkCounterID).

any_Ingredient_WorkCounter_ID(WorkCounterID) :-
    cuttingCounter_ID(WorkCounterID).

ingredient_Available_WorkCounter(IngredientName, WorkCounterID) :-
    ingredient_NeedsCooking(IngredientName, _, _),
    stoveCounter_ID(WorkCounterID),
    counter_HasSpace(WorkCounterID).

ingredient_Available_WorkCounter(IngredientName, WorkCounterID) :-
    ingredient_NeedsCutting(IngredientName, _, _),
    cuttingCounter_ID(WorkCounterID),
    counter_HasSpace(WorkCounterID).

ingredient_Any_Available_WorkCounter(IngredientName) :-
    ingredient_Available_WorkCounter(IngredientName, _).


player_Ingredient_On_WorkCounter(PlayerID, IngredientName, WorkCounterID) :-
    player_ID(PlayerID),
    ko_HasPlayer(IngredientID),
    ko_Player_ID(IngredientID, PlayerID),
    ko(IngredientID, IngredientName, WorkCounterID),
    any_Ingredient_WorkCounter_ID(WorkCounterID).

player_Ingredient_On_Any_WorkCounter(PlayerID, IngredientName) :-
    player_Ingredient_On_WorkCounter(PlayerID, IngredientName, _).

