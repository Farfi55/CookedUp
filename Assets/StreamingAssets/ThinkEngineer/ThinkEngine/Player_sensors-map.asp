%s_Player_ID(player,objectIndex(Index),Value).
%s_Player_Type(player,objectIndex(Index),Value).
%s_Player_Name(player,objectIndex(Index),Value).
%s_Player_HasSelectedInteractable(player,objectIndex(Index),Value).
%s_Player_SelectedInteractableType(player,objectIndex(Index),Value).
%s_Player_SelectedInteractableID(player,objectIndex(Index),Value).
%s_Player_HasRecipe(player,objectIndex(Index),Value).
%s_Player_RecipeName(player,objectIndex(Index),Value).
%s_Player_HasPlate(player,objectIndex(Index),Value).
%s_Player_PlateForRecipeID(player,objectIndex(Index),Value).
%s_Player_HasMissingIngredients(player,objectIndex(Index),Value).
%s_Player_MissingIngredientsNames(player,objectIndex(Index),Index1,Value).
%s_Player_HasInvalidIngredients(player,objectIndex(Index),Value).
%s_Player_X(player,objectIndex(Index),Value).
%s_Player_Y(player,objectIndex(Index),Value).
%s_Player_Container_Count(player,objectIndex(Index),Value).
%s_Player_HasSpace(player,objectIndex(Index),Value).
%s_Player_HasAny(player,objectIndex(Index),Value).
%s_Player_KitchenObject_Name(player,objectIndex(Index),Index1,Value).
%s_Player_KitchenObject_ID(player,objectIndex(Index),Index1,Value).
%s_Player_KitchenObject_ContainerID(player,objectIndex(Index),Index1,Value).
%s_Player_FirstKitchenObject_Name(player,objectIndex(Index),Value).
%s_Player_FirstKitchenObject_ID(player,objectIndex(Index),Value).
%s_Player_FirstKitchenObject_ContainerID(player,objectIndex(Index),Value).
%s_Player_Container_SizeLimit(player,objectIndex(Index),Value).
%s_Player_ContainerID(player,objectIndex(Index),Value).


% ================================== PLAYER ==================================

player_ID_Index(ID,Index) :- s_Player_ID(_,objectIndex(Index),ID).
player_ID(ID) :- player_ID_Index(ID,_).

player(ID, Type, Name) :- 
    s_Player_ID(_,objectIndex(Index),ID),
    s_Player_Type(_,objectIndex(Index),Type),
    s_Player_Name(_,objectIndex(Index),Name).

player_Pos(ID, X, Y) :-
    player_ID_Index(ID,Index),
    s_Player_X(_,objectIndex(Index),X),
    s_Player_Y(_,objectIndex(Index),Y).


% Player Selected Interactable

player_HasSelectedInteractable(ID) :- 
    player_ID_Index(ID,Index),
    s_Player_HasSelectedInteractable(_,objectIndex(Index), true).

player_HasNoSelectedInteractable(ID) :- 
    player_ID_Index(ID,Index),
    s_Player_HasSelectedInteractable(_,objectIndex(Index), false).

player_SelectedInteractable(PlayerID, InteractableID, InteractableType) :-
    player_HasSelectedInteractable(PlayerID),
    player_ID_Index(PlayerID,Index),
    s_Player_SelectedInteractableID(_,objectIndex(Index),InteractableID),
    s_Player_SelectedInteractableType(_,objectIndex(Index),InteractableType).

player_HasRecipe(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasRecipe(_,objectIndex(Index), true).

player_HasNoRecipe(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasRecipe(_,objectIndex(Index), false).


player_Recipe(PlayerID, RecipeName) :-
    player_HasRecipe(PlayerID),
    player_ID_Index(PlayerID,Index),
    s_Player_RecipeName(_,objectIndex(Index),RecipeName).


% Player Plate

player_HasPlate(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasPlate(_,objectIndex(Index), true).

player_HasNoPlate(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasPlate(_,objectIndex(Index), false).

player_Plate_ID(PlayerID, PlateID) :-
    player_HasPlate(PlayerID),
    player_ID_Index(PlayerID,Index),
    s_Player_PlateForRecipeID(_,objectIndex(Index),PlateID).


% Player Recipe Ingredients

player_HasMissingIngredients(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasMissingIngredients(_,objectIndex(Index), true).

player_HasNoMissingIngredients(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasMissingIngredients(_,objectIndex(Index), false).

player_MissingIngredients_Index(PlayerID, IngredientName, Index) :-
    player_HasMissingIngredients(PlayerID),
    player_ID_Index(PlayerID,Index1),
    s_Player_MissingIngredientsNames(_,objectIndex(Index1),Index,IngredientName).

player_MissingIngredients(PlayerID, IngredientName) :- 
    player_MissingIngredients_Index(PlayerID, IngredientName, _).

player_HasInvalidIngredients(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasInvalidIngredients(_,objectIndex(Index), true).

player_HasNoInvalidIngredients(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasInvalidIngredients(_,objectIndex(Index), false).


% Player Container

player_Container_Count(ID, Count) :-
    player_ID_Index(ID,Index),
    s_Player_Container_Count(_,objectIndex(Index),Count).

player_HasSpace(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasSpace(_,objectIndex(Index), true).

player_HasNoSpace(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasSpace(_,objectIndex(Index), false).

player_HasAny(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasAny(_,objectIndex(Index), true).

player_HasNone(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasAny(_,objectIndex(Index), false).

% Player Kitchen Objects

player_KitchenObject(PlayerID, KitchenObjectID, KitchenObjectName) :-
    player_ID_Index(PlayerID, Index),
    player_HasAny(PlayerID),
    s_Player_FirstKitchenObject_ID(_,objectIndex(Index), KitchenObjectID),
    s_Player_FirstKitchenObject_Name(_,objectIndex(Index), KitchenObjectName).
    % s_Player_FirstKitchenObject_ContainerID(_,objectIndex(Index),ContainerID).



% ================================== COUNTERS ==================================

%s_Counter_ID(counterSensor,objectIndex(Index),Value).
%s_Counter_Type(counterSensor,objectIndex(Index),Value).
%s_Counter_Name(counterSensor,objectIndex(Index),Value).
%s_Counter_X(counterSensor,objectIndex(Index),Value).
%s_Counter_Y(counterSensor,objectIndex(Index),Value).
%s_Counter_ContainerID(counterSensor,objectIndex(Index),Value).
%s_Counter_SizeLimit(counterSensor,objectIndex(Index),Value).
%s_Counter_Count(counterSensor,objectIndex(Index),Value).
%s_Counter_KitchenObject_Name(counterSensor,objectIndex(Index),Index1,Value).
%s_Counter_KitchenObject_ID(counterSensor,objectIndex(Index),Index1,Value).
%s_Counter_KitchenObject_ContainerID(counterSensor,objectIndex(Index),Index1,Value).
%s_Counter_FirstKitchenObject_Name(counterSensor,objectIndex(Index),Value).
%s_Counter_FirstKitchenObject_ID(counterSensor,objectIndex(Index),Value).
%s_Counter_FirstKitchenObject_ContainerID(counterSensor,objectIndex(Index),Value).
%s_Counter_HasAny(counterSensor,objectIndex(Index),Value).
%s_Counter_HasSpace(counterSensor,objectIndex(Index),Value).


counter_ID_Index(ID,Index) :- 
    s_Counter_ID(_,objectIndex(Index),ID).

counter(ID, Type, Name) :- 
    s_Counter_ID(_,objectIndex(Index),ID),
    s_Counter_Type(_,objectIndex(Index),Type),
    s_Counter_Name(_,objectIndex(Index),Name).

counter_Pos(ID, X, Y) :-
    counter_ID_Index(ID,Index),
    s_Counter_X(_,objectIndex(Index),X),
    s_Counter_Y(_,objectIndex(Index),Y).

% Counter Container

counter_ContainerID(ID, ContainerID) :-
    counter_ID_Index(ID,Index),
    s_Counter_ContainerID(_,objectIndex(Index),ContainerID).

counter_Container_Count(ID, Count) :-
    counter_ID_Index(ID,Index),
    s_Counter_Container_Count(_,objectIndex(Index),Count).

counter_HasSpace(ID) :-
    counter_ID_Index(ID,Index),
    s_Counter_HasSpace(_,objectIndex(Index), true).

counter_HasNoSpace(ID) :-
    counter_ID_Index(ID,Index),
    s_Counter_HasSpace(_,objectIndex(Index), false).

counter_HasAny(ID) :-
    counter_ID_Index(ID,Index),
    s_Counter_HasAny(_,objectIndex(Index), true).

counter_HasNone(ID) :-
    counter_ID_Index(ID,Index),
    s_Counter_HasAny(_,objectIndex(Index), false).

% Counter Kitchen Objects

counter_KitchenObject(CounterID, KitchenObjectID, KitchenObjectName) :-
    counter_ID_Index(CounterID, Index),
    counter_HasAny(ID),
    s_Counter_FirstKitchenObject_ID(_,objectIndex(Index), KitchenObjectID),
    s_Counter_FirstKitchenObject_Name(_,objectIndex(Index), KitchenObjectName).
    % s_Counter_FirstKitchenObject_ContainerID(_,objectIndex(Index),ContainerID).

counter_KitchenObjects_Index(CounterID, KitchenObjectID, KitchenObjectName, KitchenObjectIndex) :-
    counter_ID_Index(CounterID, Index),
    counter_HasAny(ID),
    s_Counter_KitchenObject_ID(_,objectIndex(Index), KitchenObjectIndex, KitchenObjectID),
    s_Counter_KitchenObject_Name(_,objectIndex(Index), KitchenObjectIndex, KitchenObjectName).
    % s_Counter_KitchenObject_ContainerID(_,objectIndex(Index), KitchenObjectIndex,ContainerID).

counter_KitchenObjects(CounterID, KitchenObjectID, KitchenObjectName) :-
    counter_KitchenObjects_Index(CounterID, KitchenObjectID, KitchenObjectName, _).


% ================================== CONTAINER COUNTER ==================================


%s_ContainerCounter_ID(counterSensor,objectIndex(Index),Value).
%s_ContainerCounter_KOType(counterSensor,objectIndex(Index),Value).

% TODO: Container Counter


% ================================== PLATES COUNTER ==================================

%s_PlatesCounter_ID(counterSensor,objectIndex(Index),Value).
%s_PlatesCounter_PlatesLimit(counterSensor,objectIndex(Index),Value).
%s_PlatesCounter_PlatesCount(counterSensor,objectIndex(Index),Value).

% TODO: Plates Counter


% ================================== CUTTING COUNTER ==================================

%s_CuttingCounter_ID(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_HasAny(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CanCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_TimeRemainingToCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_Name(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_InputKOName(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_OutputKOName(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_TimeToCut(cuttingCounterSensor,objectIndex(Index),Value).

% TODO: Cutting Counter


% ================================== STOVE COUNTER ==================================


%s_StoveCounter_ID(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_HasAny(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CanCook(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_TimeRemainingToCook(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_IsBurning(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CurrentCookingRecipe_Name(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CurrentCookingRecipe_InputKOName(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CurrentCookingRecipe_OutputKOName(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CurrentCookingRecipe_TimeToCook(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CurrentCookingRecipe_IsBurningRecipe(counterSensor,objectIndex(Index),Value).

% TODO: Stove Counter
