state("WaitingForRecipe") :- state_WaitingForRecipe.
state_WaitingForRecipe :- 
    curr_Player_ID(PlayerID),
    playerBot_HasNoRecipe(PlayerID).

state("PickUp_Plate") :- state_PickUp_Plate.
state_PickUp_Plate :-
    curr_Player_ID(PlayerID),
    playerBot_HasRecipe(PlayerID),
    playerBot_HasNoPlate(PlayerID).

state("PlacePlate") :- state_PlacePlate.
state_PlacePlate :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

state("GetIngredients") :- state_GetIngredient.
state_GetIngredient :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    player_HasNone(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).


state("DropIngredient") :- state_DropIngredient.
state_DropIngredient :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    player_HasAny(PlayerID),
    player_KitchenObject(PlayerID, _, KitchenObjectName),
    not playerBot_MissingIngredients(PlayerID, KitchenObjectName),
    not playerBot_IsPlateBeingCarried(PlayerID),
    not playerBot_HasCompletedRecipe(PlayerID).

state_DropIngredient :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    player_HasAny(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).

state("PickUp_CompletedPlate") :- state_PickUp_CompletedPlate.
state_PickUp_CompletedPlate :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    player_HasNone(PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).

state("Deliver") :- state_Deliver.
state_Deliver :-
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_IsPlateBeingCarried(PlayerID),
    playerBot_HasCompletedRecipe(PlayerID).


:- #count{X: state(X)} != 1.

firstActionIndex(0).

    
#show state/1.
#show applyAction/2.
#show actionArgument/3.
#show subStateGI/1.
#show stateGI_nextIngredient/1.
#show stateGI_Target1/1.
#show stateGI_Target2/1.
#show stateGI_Target_Final/1.
#show playerBot_Plate_Container_ID/3.
#show playerBot_Plate_ID/2.
#show plate_ID/1.
#show player_HasAny/1.
#show player_KitchenObject/3.
#show playerBot_MissingIngredients/2.
