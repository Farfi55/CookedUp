% base case:
% if the ingredient is available
% then the player can just go and get it
player_CanWork_ToGet_Ingredient(PlayerID, IngredientName) :-
    player_ID(PlayerID),
    ingredient_Available_ForPlayer(IngredientName, PlayerID).


% if the player is holding the ingredient
% then he already has it
player_CanWork_ToGet_Ingredient(PlayerID, IngredientName) :-
    player_ID(PlayerID),
    player_KO_Name(PlayerID, IngredientName).    

% recursive case:
% if the ingredient is not available
% but we can work on the base ingredient 
% to get the ingredient we want
player_CanWork_ToGet_Ingredient(PlayerID, IngredientName) :-
    player_ID(PlayerID),
    ingredient_NeedsWork_ForPlayer(IngredientName, PlayerID, _, InputIngredientName),
    ingredient_Any_Available_WorkCounter(IngredientName),
    player_CanWork_ToGet_Ingredient(PlayerID, InputIngredientName).
