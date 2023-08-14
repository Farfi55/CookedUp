state("PickUp_Plate") :- state_PickUp_Plate.
state_PickUp_Plate :-
    curr_Player_ID(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_HasNoPlate(PlayerID).

state("PlacePlate") :- state_PlacePlate.
state_PlacePlate :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

state("GetIngredients") :- state_GetIngredient.
state_GetIngredient :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasNone(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

state("AddIngredient") :- state_AddIngredient.
state_AddIngredient :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID),
    player_KitchenObject(PlayerID, _, KitchenObjectName),
    playerBot_MissingIngredients(PlayerID, KitchenObjectName),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).


state("DropIngredient") :- state_DropIngredient.
state_DropIngredient :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID),
    player_KitchenObject(PlayerID, _, KitchenObjectName),
    not playerBot_MissingIngredients(PlayerID, KitchenObjectName),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

state_DropIngredient :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasAny(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).

state("PickUp_CompletedPlate") :- state_PickUp_CompletedPlate.
state_PickUp_CompletedPlate :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    player_HasNone(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).

state("Deliver") :- state_Deliver.
state_Deliver :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).


state("Recipe Failed") :- state_Recipe_Failed.
% CASE 1: Player has placed a plate on the delivery counter, 
%         but the delivery was not successful.
state_Recipe_Failed :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_Plate_Container_ID(PlayerID, _, DeliveryCounterID),
    deliveryCounter_ID(DeliveryCounterID).

% CASE 2: Player's recipe request expired.
state_Recipe_Failed :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasNoRecipeRequest(RecipeRequestID).

% CASE 3: Player's plate contains invalid ingredients.
state_Recipe_Failed :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(RecipeRequestID),
    playerBot_HasInvalidIngredients(PlayerID).
    


:- #count{X: state(X)} != 1.

% Can't have multiple actions at the same ActionIndex
:- #count{ActionName: applyAction(ActionIndex, ActionName)} > 1, applyAction(ActionIndex, _).

tmp_AnyAction(ActionIndex) :- applyAction(ActionIndex, _).

% Can't have arguments for an action that doesn't exist.
:- actionArgument(ActionIndex, _, _), not tmp_AnyAction(ActionIndex).

% % Can't have the same argument multiple times for a single Action
% :- #count{ArgumentValue : actionArgument(ActionIndex, ArgumentName, ArgumentValue)} > 1, 
%     actionArgument(ActionIndex, ArgumentName, _).




firstActionIndex(0).

#show curr_Player_ID/1.
#show state/1.
#show applyAction/2.
#show actionArgument/3.
#show playerBot_Plate_Container_ID/3.
#show playerBot_Plate_ID/2.
#show plate_ID/1.
#show player_HasAny/1.
#show player_KitchenObject/3.
#show playerBot_IngredientsNames/2.
#show playerBot_MissingIngredients/2.
#show playerBot_IsPlateBeingCarried/1.
#show playerBot_HasCompletedRecipe/1.
#show playerBot_HasPlate/1.
#show playerBot_HasRecipeRequest/1.
#show playerBot_RecipeRequest_Name/2.

