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

a_Wait(ActionIndex, PlayerID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex + 2,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID).

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

a_Wait(ActionIndex, PlayerID) :-
    state_PlacePlate,
    ActionIndex = FirstActionIndex + 2,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID).
    

% ========================== STATE GET INGREDIENTS ==========================

ingredient_Available(IngredientName, TargetID) :-
    containerCounter_KOType(TargetID, IngredientName).

ingredient_Available(IngredientName, TargetID) :-
    counter_HasAny(TargetID),
    counter_KitchenObjects(TargetID, _, IngredientName).

ingredient_Available_Any(IngredientName) :-
    ingredient_Available(IngredientName, _).

ingredient_NotAvailable(IngredientName) :-
    c_KO_NAME(IngredientName),
    not ingredient_Available_Any(IngredientName).


ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_COOKING_RECIPE(RecipeName, InputIngredientName, OutputIngredientName, _, _).

ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_CUTTING_RECIPE(RecipeName, InputIngredientName, OutputIngredientName, _).

ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName).

ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName).


stateGI_nextIngredient(IngredientName) :-
    state_GetIngredients,
    curr_Player_ID(PlayerID),
    player_MissingIngredients_Index(PlayerID, IngredientName, 0).


subStateGI("Available") :- 
    state_GetIngredients,
    stateGI_nextIngredient(IngredientName),
    ingredient_Available_Any(IngredientName).

subStateGI("NeedsCooking") :-
    state_GetIngredients,
    stateGI_nextIngredient(IngredientName),
    ingredient_NeedsCooking(IngredientName, _, _).

subStateGI("NeedsCutting") :-
    state_GetIngredients,
    stateGI_nextIngredient(IngredientName),
    ingredient_NeedsCutting(IngredientName, _, _).


% available
stateGI_Target1(TargetID) :-
    state_GetIngredients,
    subStateGI("Available"),
    stateGI_nextIngredient(IngredientName),
    ingredient_Available(IngredientName, TargetID).


% needs cooking or cutting
stateGI_Target1(TargetID) :-
    state_GetIngredients,
    not subStateGI("Available"),
    stateGI_nextIngredient(IngredientName),
    ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName),
    ingredient_Available(InputIngredientName, TargetID).


% needs cooking
stateGI_Target2(TargetID) :-
    state_GetIngredients,
    subStateGI("NeedsCooking"),
    stoveCounter_ID(TargetID),
    counter_HasSpace(TargetID).


% needs cutting
stateGI_Target2(TargetID) :-
    state_GetIngredients,
    subStateGI("NeedsCutting"),
    cuttingCounter_ID(TargetID),
    counter_HasSpace(TargetID).

stateGI_Target_Final(TargetID) :-
    state_GetIngredients,
    player_Plate_Container_ID(PlayerID, _, TargetID).
    



a_MoveTo_Target(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target1(TargetID).


a_PickUp(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target1(TargetID).


a_MoveTo_Target(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    ActionIndex = FirstActionIndex + 2,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target2(TargetID).




a_Place(ActionIndex, PlayerID, TargetID) :- 
    state_GetIngredients,
    not subStateGI("Available"),
    ActionIndex = FirstActionIndex + 3,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target2(TargetID).


% needs cooking
a_WaitToCook(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    subStateGI("NeedsCooking"),
    ActionIndex = FirstActionIndex + 4,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target2(TargetID).


% needs cutting
a_Cut(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    subStateGI("NeedsCutting"),
    ActionIndex = FirstActionIndex + 4,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target2(TargetID).


a_PickUp(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    not subStateGI("Available"),
    ActionIndex = FirstActionIndex + 5,
    firstActionIndex(FirstActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target2(TargetID).


finalActionIndex(Index) :-
    state_GetIngredients,
    firstActionIndex(FirstActionIndex),
    Index = FirstActionIndex + 3,
    subStateGI("Available").

finalActionIndex(Index) :-
    state_GetIngredients,
    firstActionIndex(FirstActionIndex),
    Index = FirstActionIndex + 6,
    not subStateGI("Available").



a_MoveTo_Target(ActionIndex, PlayerID, TargetID) :-
    state_GetIngredients,
    ActionIndex = FinalActionIndex,
    finalActionIndex(FinalActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target_Final(TargetID).

a_Place(ActionIndex, PlayerID, TargetID) :- 
    state_GetIngredients,
    ActionIndex = FinalActionIndex + 1,
    finalActionIndex(FinalActionIndex),
    curr_Player_ID(PlayerID),
    stateGI_Target_Final(TargetID).


a_Wait(ActionIndex, PlayerID) :-
    state_GetIngredients,
    ActionIndex = FinalActionIndex + 2,
    finalActionIndex(FinalActionIndex),
    curr_Player_ID(PlayerID).




    
#show state/1.
#show applyAction/2.
#show actionArgument/3.
