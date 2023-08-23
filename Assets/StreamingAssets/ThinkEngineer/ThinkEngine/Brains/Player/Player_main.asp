any_Urgent_State :- state_Ingredient_Burning.

state("Pick Up Plate") :- state_PickUp_Plate.
% CASE 1:
% Player does not have a plate assigned to him.
state_PickUp_Plate :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    not playerBot_HasPlate(PlayerID),
    not player_HasAny(PlayerID).

% CASE 2:
% Player has a plate assigned to him, but it is not the best one for the current recipe.
state_PickUp_Plate :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_Name(PlayerID, RecipeName),
    playerBot_HasPlate(PlayerID),
    playerBot_Plate_ID(PlayerID, PlateID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not player_Best_Plate_ValidForRecipe(PlayerID, PlateID, RecipeName),
    not playerBot_HasInvalidIngredients(PlayerID).

state("Place Plate") :- state_PlacePlate.
state_PlacePlate :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

state("Get Ingredients") :- state_GetIngredient.

% CASE 1:
% Player is not carrying anything, and has a valid recipe request.
state_GetIngredient :-
    not any_Urgent_State,
    not state_PickUp_Plate,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasNone(PlayerID),
    not playerBot_HasInvalidIngredients(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

% CASE 2:
% Player is not carrying an ingredient for his recipe.
state_GetIngredient :-
    not any_Urgent_State,
    not state_PickUp_Plate,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID),
    player_KO_Name(PlayerID, KOName),
    playerBot_MissingIngredients_Or_Base(PlayerID, KOName),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

state("Ingredient Burning") :- state_Ingredient_Burning.
% CASE 1:
% a player's ingredient is burning.
state_Ingredient_Burning :-
    stoveCounter_ID(StoveCounterID),
    stoveCounter_IsBurning(StoveCounterID),
    counter_KO_ID(StoveCounterID, KitchenObjectID),
    ko_Curr_Player(KitchenObjectID).

% CASE 2:
% a player's ingredient has burned. (it is now trash)
state_Ingredient_Burning :-
    counter_KO(CounterID, KitchenObjectID, KitchenObjectName),
    ko_Curr_Player(KitchenObjectID),
    ingredient_IsTrash(KitchenObjectName).


state("Drop Ingredient") :- state_DropIngredient.
% CASE 1:
% Player is carrying an ingredient which is not useful for his recipe.
state_DropIngredient :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID),
    player_KO_Name(PlayerID, KOName),
    not playerBot_MissingIngredients_Or_Base(PlayerID, KOName),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

% CASE 2:
% Player is carrying an ingredient even tho the recipe is complete.
state_DropIngredient :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).

% CASE 3:
% Player is carrying an ingredient without having a plate.
state_DropIngredient :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    not playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID).

% CASE 4:
% Player is carrying an ingredient without having a recipe request.
state_DropIngredient :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    not playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID).


state("Pick Up Completed Plate") :- state_PickUp_CompletedPlate.
state_PickUp_CompletedPlate :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasNone(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).

state("Deliver") :- state_Deliver.
state_Deliver :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).


state("Recipe Failed") :- state_Recipe_Failed.
% CASE 1: Player has placed a plate on the delivery counter, 
%         but the delivery was not successful.
state_Recipe_Failed :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_Plate_Container_ID(PlayerID, _, DeliveryCounterID),
    deliveryCounter_ID(DeliveryCounterID).

% CASE 2: Player's recipe request expired.
state_Recipe_Failed :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasNoRecipeRequest(PlayerID).

% CASE 3: Player's plate contains invalid ingredients.
state_Recipe_Failed :-
    not any_Urgent_State,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_HasInvalidIngredients(PlayerID).
    



conf_Strict_Level(0).

conf_ExtraStrict :- conf_Strict_Level(Level), Level >= 2.
conf_Strict :- conf_Strict_Level(Level), Level >= 1.


:- conf_Strict, #count{X: state(X)} > 1.

% Can't have multiple actions at the same ActionIndex
:- conf_Strict, 
    #count{ ActionName : 
        applyAction(ActionIndex, ActionName)
    } > 1, 
    applyAction(ActionIndex, _).

tmp_AnyAction(ActionIndex) :- applyAction(ActionIndex, _).

% Can't have arguments for an action that doesn't exist.
:- conf_Strict, actionArgument(ActionIndex, _, _), not tmp_AnyAction(ActionIndex).

% % Can't have the same argument multiple times for a single Action
:- conf_ExtraStrict, #count{ArgumentValue : actionArgument(ActionIndex, ArgumentName, ArgumentValue)} > 1, 
    actionArgument(ActionIndex, ArgumentName, _).




firstActionIndex(0).

curr_Player_Name(PlayerID, PlayerName) :- 
    curr_Player_ID(PlayerID), 
    player(PlayerID, _, PlayerName).

#show curr_Player_ID/1.
#show curr_Player_Name/2.
#show state/1.
#show applyAction/2.
#show actionArgument/3.
#show playerBot_Plate_Container_ID/3.
#show playerBot_Plate_ID/2.
#show player_HasAny/1.
#show player_KO_Name/2.
#show player_KO/3.
#show playerBot_IngredientsNames/2.
#show playerBot_HasInvalidIngredients/1.
#show playerBot_MissingIngredients/2.
#show playerBot_IsPlateBeingCarried/1.
#show playerBot_HasCompletedRecipe/1.
#show playerBot_HasPlate/1.
#show playerBot_HasRecipeRequest/1.
#show playerBot_RecipeRequest_Name/2.

