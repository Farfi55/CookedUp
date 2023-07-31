% STEP 1
% FIND THE STATE WE ARE IN

state_WaitingForRecipe :- 
curr_Player_ID(PlayerID),
player_HasNoRecipe(PlayerID).

state_PickUp_Plate :-
    curr_Player_ID(PlayerID),
    player_HasRecipe(PlayerID),
    player_HasNoPlate(PlayerID).

state_PlacePlate :-
    curr_Player_ID(PlayerID),
    player_HasPlate(PlayerID),
    player_IsPlateBeingCarried(PlayerID),
    not player_HasCompletedRecipe(PlayerID).

state_GetIngredients :-
    curr_Player_ID(PlayerID),
    player_HasPlate(PlayerID),
    not player_IsPlateBeingCarried(PlayerID),
    not player_HasCompletedRecipe(PlayerID).

state_PickUp_CompletedPlate :-
    curr_Player_ID(PlayerID),
    player_HasPlate(PlayerID),
    not player_IsPlateBeingCarried(PlayerID),
    player_HasCompletedRecipe(PlayerID).

state_Delivery :-
    curr_Player_ID(PlayerID),
    player_HasPlate(PlayerID),
    player_IsPlateBeingCarried(PlayerID),
    player_HasCompletedRecipe(PlayerID).

state("WaitingForRecipe") :- state_WaitingForRecipe.
state("PickUp_Plate") :- state_PickUp_Plate.
state("PlacePlate") :- state_PlacePlate.
state("GetIngredients") :- state_GetIngredients.
state("PickUp_CompletedPlate") :- state_PickUp_CompletedPlate.
state("Delivery") :- state_Delivery.


:- #count{X: state(X)} != 1.

firstActionIndex(1).

% ========================== STATE PICKUP PLATE ==========================

state_PickUp_Plate__Target(PlateCounterID) :-
    state_PickUp_Plate,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).


a_MoveTo_Target(ActionIndex, PlayerID, PlatesCounterID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    state_PickUp_Plate__Target(PlatesCounterID).


a_PickUp(ActionIndex, PlayerID, PlatesCounterID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    state_PickUp_Plate__Target(PlatesCounterID).


% ========================== STATE PLACE PLATE ==========================

state_PlacePlate__Target(TargetID) :-
    state_PlacePlate,
    clearCounter_ID(TargetID),
    counter_HasSpace(TargetID).


a_MoveTo_Target(ActionIndex, PlayerID, TargetID) :-
    state_PlacePlate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    state_PlacePlate__Target(TargetID).

a_Place(ActionIndex, PlayerID, TargetID) :-
    state_PlacePlate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    state_PlacePlate__Target(TargetID).
    

% ========================== STATE GET INGREDIENTS ==========================

state_GetIngredients__nextIngredient(IngredientName) :-
    state_GetIngredients,
    curr_Player_ID(PlayerID),
    player_MissingIngredients_Index(PlayerID, IngredientName, 0).

state_GetIngredients__nextIngredient_Available(IngredientName, TargetID) :-
    state_GetIngredients,
    state_GetIngredients__nextIngredient(IngredientName),
    containerCounter_KOType(TargetID, IngredientName).

state_GetIngredients__nextIngredient_Available(IngredientName, TargetID) :-
    state_GetIngredients,
    curr_Player_ID(PlayerID),
    state_GetIngredients__nextIngredient(IngredientName),
    clearCounter_ID(TargetID),
    counter_HasAny(TargetID),
    counter_KitchenObjects(TargetID, _, IngredientName).

state_GetIngredients__Target(TargetID) :-
    state_GetIngredients,
    state_GetIngredients__nextIngredient_Available(_, TargetID).


a_MoveTo_Target(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    state_GetIngredients__Target(TargetID).


a_PickUp(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    state_GetIngredients__Target(TargetID).




a_MoveTo_Target(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    ActionIndex = FirstActionIndex + 2,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    player_Plate_Container_ID(PlayerID, _, TargetID).

a_Place(ActionIndex, PlayerID, TargetID) :- 
    state_GetIngredients,
    ActionIndex = FirstActionIndex + 3,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    player_Plate_Container_ID(PlayerID, _, TargetID).



    
#show state/1.
#show applyAction/2.
#show actionArgument/3.
