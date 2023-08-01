%s_Player_ID(player,objectIndex(Index),Value).
%s_Player_Type(player,objectIndex(Index),Value).
%s_Player_Name(player,objectIndex(Index),Value).
%s_Player_HasSelectedInteractable(player,objectIndex(Index),Value).
%s_Player_SelectedInteractableType(player,objectIndex(Index),Value).
%s_Player_SelectedInteractableID(player,objectIndex(Index),Value).
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
curr_Player_ID(ID) :- playerBot_ID(ID).

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


% ================================== PLAYER BOT ==================================

%s_PlayerBot_ID(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_HasRecipe(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_RecipeName(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_HasPlate(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_PlateForRecipeID(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_HasMissingIngredients(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_MissingIngredientsNames(playerSensors,objectIndex(Index),Index1,Value).
%s_PlayerBot_HasInvalidIngredients(playerSensors,objectIndex(Index),Value).

playerBot_ID_Index(ID,Index) :- 
    s_PlayerBot_ID(_,objectIndex(Index),ID).

playerBot_ID(ID) :- 
    playerBot_ID_Index(ID,_).

player_IsBot(ID) :- 
    player_ID(ID),
    playerBot_ID(ID).

player_IsNotBot(ID) :- 
    player_ID(ID),
    not player_IsBot(ID).

% Player Plate

playerBot_HasPlate(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasPlate(_,objectIndex(Index), true).

playerBot_HasNoPlate(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasPlate(_,objectIndex(Index), false).

playerBot_Plate_ID(PlayerID, PlateID) :-
    playerBot_HasPlate(PlayerID),
    playerBot_ID_Index(PlayerID,Index),
    s_PlayerBot_PlateForRecipeID(_,objectIndex(Index),PlateID).

playerBot_IsPlateBeingCarried(PlayerID) :-
    playerBot_HasPlate(PlayerID),
    player_KitchenObject(PlayerID, PlateID, _),
    playerBot_Plate_ID(PlayerID, PlateID).

% ID of the container that the plate is in
playerBot_Plate_Container_ID(PlayerID, PlateID, ContainerID) :-
    playerBot_HasPlate(PlayerID),
    playerBot_Plate_ID(PlayerID, PlateID),
    plate_Container_ID(PlateID, ContainerID).

% Player Recipe

playerBot_HasRecipe(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasRecipe(_,objectIndex(Index), true).

playerBot_HasNoRecipe(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasRecipe(_,objectIndex(Index), false).


playerBot_Recipe(PlayerID, RecipeName) :-
    playerBot_HasRecipe(PlayerID),
    playerBot_ID_Index(PlayerID,Index),
    s_PlayerBot_RecipeName(_,objectIndex(Index),RecipeName).



% Player Recipe Ingredients

playerBot_HasMissingIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasMissingIngredients(_,objectIndex(Index), true).

playerBot_HasNoMissingIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasMissingIngredients(_,objectIndex(Index), false).

playerBot_MissingIngredients_Index(PlayerID, IngredientName, Index) :-
    playerBot_HasMissingIngredients(PlayerID),
    playerBot_ID_Index(PlayerID,Index1),
    s_PlayerBot_MissingIngredientsNames(_,objectIndex(Index1),Index,IngredientName).

playerBot_MissingIngredients(PlayerID, IngredientName) :- 
    playerBot_MissingIngredients_Index(PlayerID, IngredientName, _).

playerBot_HasInvalidIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasInvalidIngredients(_,objectIndex(Index), true).

playerBot_HasNoInvalidIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasInvalidIngredients(_,objectIndex(Index), false).

playerBot_HasCompletedRecipe(ID) :-
    playerBot_HasRecipe(ID),
    playerBot_HasNoMissingIngredients(ID),
    playerBot_HasNoInvalidIngredients(ID).




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

containerCounter_ID_Index(ID,Index) :- 
    s_ContainerCounter_ID(_,objectIndex(Index),ID).

containerCounter_ID(ID) :-
    containerCounter_ID_Index(ID,_).

containerCounter_KOType(ID, KitchenObjectType) :-
    containerCounter_ID_Index(ID,Index),
    s_ContainerCounter_KOType(_,objectIndex(Index),KitchenObjectType).


% ================================== PLATES COUNTER ==================================

%s_PlatesCounter_ID(counterSensor,objectIndex(Index),Value).
%s_PlatesCounter_PlatesLimit(counterSensor,objectIndex(Index),Value).
%s_PlatesCounter_PlatesCount(counterSensor,objectIndex(Index),Value).

platesCounter_ID_Index(ID,Index) :- 
    s_PlatesCounter_ID(_,objectIndex(Index),ID).

platesCounter_ID(ID) :-
    platesCounter_ID_Index(ID,_).

platesCounter_Limit_Count(ID, PlatesLimit, PlatesCount) :-
    platesCounter_ID_Index(ID,Index),
    s_PlatesCounter_PlatesLimit(_,objectIndex(Index),PlatesLimit),
    s_PlatesCounter_PlatesCount(_,objectIndex(Index),PlatesCount).


% ================================== CUTTING COUNTER ==================================

%s_CuttingCounter_ID(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_HasAny(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CanCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_TimeRemainingToCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_Name(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_InputKOName(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_OutputKOName(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_TimeToCut(cuttingCounterSensor,objectIndex(Index),Value).

cuttingCounter_ID_Index(ID,Index) :- 
    s_CuttingCounter_ID(_,objectIndex(Index),ID).

cuttingCounter_ID(ID) :-
    cuttingCounter_ID_Index(ID,_).

cuttingCounter_HasAny(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    s_CuttingCounter_HasAny(_,objectIndex(Index), true).

cuttingCounter_HasNone(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    s_CuttingCounter_HasAny(_,objectIndex(Index), false).

cuttingCounter_CanCut(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    s_CuttingCounter_CanCut(_,objectIndex(Index), true).

cuttingCounter_CannotCut(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    s_CuttingCounter_CanCut(_,objectIndex(Index), false).

cuttingCounter_TimeRemainingToCut(ID, TimeRemainingToCut) :-
    cuttingCounter_ID_Index(ID,Index),
    cuttingCounter_CanCut(ID),
    s_CuttingCounter_TimeRemainingToCut(_,objectIndex(Index), TimeRemainingToCut).

cuttingCounter_CurrentCuttingRecipe_Name(ID, RecipeName) :-
    cuttingCounter_ID_Index(ID,Index),
    cuttingCounter_CanCut(ID),
    s_CuttingCounter_CurrentCuttingRecipe_Name(_,objectIndex(Index), RecipeName).

cuttingCounter_CurrentCuttingRecipe(ID, RecipeName, RecipeInput, RecipeOutput, RecipeTimeToCut) :-
    cuttingCounter_ID_Index(ID,Index),
    cuttingCounter_CanCut(ID),
    s_CuttingCounter_CurrentCuttingRecipe_Name(_,objectIndex(Index), RecipeName),
    s_CuttingCounter_CurrentCuttingRecipe_InputKOName(_,objectIndex(Index), RecipeInput),
    s_CuttingCounter_CurrentCuttingRecipe_OutputKOName(_,objectIndex(Index), RecipeOutput),
    s_CuttingCounter_CurrentCuttingRecipe_TimeToCut(_,objectIndex(Index), RecipeTimeToCut).



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

stoveCounter_ID_Index(ID,Index) :- 
    s_StoveCounter_ID(_,objectIndex(Index),ID).

stoveCounter_ID(ID) :-
    stoveCounter_ID_Index(ID,_).

stoveCounter_HasAny(ID) :-
    stoveCounter_ID_Index(ID,Index),
    s_StoveCounter_HasAny(_,objectIndex(Index), true).

stoveCounter_HasNone(ID) :-
    stoveCounter_ID_Index(ID,Index),
    s_StoveCounter_HasAny(_,objectIndex(Index), false).

stoveCounter_CanCook(ID) :-
    stoveCounter_ID_Index(ID,Index),
    s_StoveCounter_CanCook(_,objectIndex(Index), true).

stoveCounter_CannotCook(ID) :-
    stoveCounter_ID_Index(ID,Index),
    s_StoveCounter_CanCook(_,objectIndex(Index), false).

stoveCounter_TimeRemainingToCook(ID, TimeRemainingToCook) :-
    stoveCounter_ID_Index(ID,Index),
    stoveCounter_CanCook(ID),
    s_StoveCounter_TimeRemainingToCook(_,objectIndex(Index), TimeRemainingToCook).

stoveCounter_IsBurning(ID) :-
    stoveCounter_ID_Index(ID,Index),
    stoveCounter_CanCook(ID),
    s_StoveCounter_IsBurning(_,objectIndex(Index), true).

stoveCounter_IsNotBurning(ID) :-
    stoveCounter_ID_Index(ID,Index),
    stoveCounter_CanCook(ID),
    s_StoveCounter_IsBurning(_,objectIndex(Index), false).


stoveCounter_CurrentCookingRecipe_Name(ID, RecipeName) :-
    stoveCounter_ID_Index(ID,Index),
    stoveCounter_CanCook(ID),
    s_StoveCounter_CurrentCookingRecipe_Name(_,objectIndex(Index), RecipeName).

stoveCounter_CurrentCookingRecipe(ID, RecipeName, RecipeInput, RecipeOutput, RecipeTimeToCook, RecipeIsBurning) :-
    stoveCounter_ID_Index(ID,Index),
    stoveCounter_CanCook(ID),
    s_StoveCounter_CurrentCookingRecipe_Name(_,objectIndex(Index), RecipeName),
    s_StoveCounter_CurrentCookingRecipe_InputKOName(_,objectIndex(Index), RecipeInput),
    s_StoveCounter_CurrentCookingRecipe_OutputKOName(_,objectIndex(Index), RecipeOutput),
    s_StoveCounter_CurrentCookingRecipe_TimeToCook(_,objectIndex(Index), RecipeTimeToCook),
    s_StoveCounter_CurrentCookingRecipe_IsBurningRecipe(_,objectIndex(Index), RecipeIsBurning).


% ================================== OTHER COUNTERS ==================================

%s_Counter_ID(counterSensor,objectIndex(Index),Value).
%s_Counter_Type(counterSensor,objectIndex(Index),Value).
%s_Counter_Name(counterSensor,objectIndex(Index),Value).

% CLEAR COUNTER 

clearCounter_ID_Index(ID,Index) :- 
    s_Counter_ID(_,objectIndex(Index),ID),
    s_Counter_Type(_,objectIndex(Index),"ClearCounter").

clearCounter_ID(ID) :-
    clearCounter_ID_Index(ID,_).

% TRASH COUNTER 

trashCounter_ID_Index(ID,Index) :- 
    s_Counter_ID(_,objectIndex(Index),ID),
    s_Counter_Type(_,objectIndex(Index),"TrashCounter").

trashCounter_ID(ID) :- 
    trashCounter_ID_Index(ID,_).

% DELIVERY COUNTER

deliveryCounter_ID_Index(ID,Index) :- 
    s_Counter_ID(_,objectIndex(Index),ID),
    s_Counter_Type(_,objectIndex(Index),"DeliveryCounter").

deliveryCounter_ID(ID) :-
    deliveryCounter_ID_Index(ID,_).


% ================================== KITCHEN OBJECTS ==================================

any_KitchenObject_Index(OwnerID, KitchenObjectID, KitchenObjectName, 0) :-
    player_KitchenObject(OwnerID, KitchenObjectID, KitchenObjectName).

any_KitchenObject_Index(OwnerID, KitchenObjectID, KitchenObjectName, KitchenObjectIndex) :-
    counter_KitchenObjects_Index(OwnerID, KitchenObjectID, KitchenObjectName, KitchenObjectIndex).

any_KitchenObject(OwnerID, KitchenObjectID, KitchenObjectName) :-
    any_KitchenObject_Index(OwnerID, KitchenObjectID, KitchenObjectName, _).

% ================================== PLATE KITCHEN OBJECT ==================================

%s_Plate_ID(plateSensor,objectIndex(Index),Value).
%s_Plate_Type(plateSensor,objectIndex(Index),Value).
%s_Plate_Name(plateSensor,objectIndex(Index),Value).
%s_Plate_ContainerID(plateSensor,objectIndex(Index),Value).
%s_Plate_Count(plateSensor,objectIndex(Index),Value).
%s_Plate_KitchenObject_Name(plateSensor,objectIndex(Index),Index1,Value).
%s_Plate_KitchenObject_ID(plateSensor,objectIndex(Index),Index1,Value).
%s_Plate_KitchenObject_ContainerID(plateSensor,objectIndex(Index),Index1,Value).
%s_Plate_FirstKitchenObject_Name(plateSensor,objectIndex(Index),Value).
%s_Plate_FirstKitchenObject_ID(plateSensor,objectIndex(Index),Value).
%s_Plate_FirstKitchenObject_ContainerID(plateSensor,objectIndex(Index),Value).
%s_Plate_HasSpace(plateSensor,objectIndex(Index),Value).
%s_Plate_HasAny(plateSensor,objectIndex(Index),Value).
%s_Plate_SizeLimit(plateSensor,objectIndex(Index),Value).
%s_Plate_IsInContainer(plateSensor,objectIndex(Index),Value).
%s_Plate_OwnerContainerID(plateSensor,objectIndex(Index),Value).

any_Plate(OwnerID, KitchenObjectID) :-
    any_KitchenObject(OwnerID, KitchenObjectID, "Plate").

plate_ID_Index(PlateID, Index) :-
    s_Plate_ID(_,objectIndex(Index),PlateID).

plate_ID(PlateID) :-
    plate_ID_Index(PlateID,_).

plate_IsInContainer(PlateID) :-
    plate_ID_Index(PlateID, Index),
    s_Plate_IsInContainer(_,objectIndex(Index), true).

plate_IsNotInContainer(PlateID) :-
    plate_ID_Index(PlateID, Index),
    s_Plate_IsInContainer(_,objectIndex(Index), false).

% container which contains the plate
plate_Container_ID(PlateID, ContainerID) :-
    plate_ID_Index(PlateID, Index),
    plate_IsInContainer(PlateID),
    s_Plate_OwnerContainerID(_,objectIndex(Index),ContainerID).


% Plate ingredients

% plate ingredients's container 
plate_IngredientsContainer_ID(PlateID, IngredientsContainerID) :-
    plate_ID_Index(PlateID, Index),
    s_Plate_ContainerID(_,objectIndex(Index),IngredientsContainerID).

plate_HasAnyIngredients(PlateID) :-
    plate_ID_Index(PlateID, Index),
    s_Plate_HasAny(_,objectIndex(Index), true).

plate_Ingredients_Count(PlateID, IngredientsCount) :-
    plate_ID_Index(PlateID, Index),
    s_Plate_Count(_,objectIndex(Index),IngredientsCount).

plate_Ingredients_Name(PlateID, IngredientName) :-
    plate_ID_Index(PlateID, Index),
    s_Plate_FirstKitchenObject_Name(_,objectIndex(Index),IngredientName).


% Plate Recipes utilities

plate_MissingIngredients_Name(PlateID, RecipeName, MissingIngredientName) :-
    plate_ID(PlateID),
    c_COMPLETE_RECIPE_INGREDIENT(RecipeName, MissingIngredientName),
    not plate_Ingredients_Name(PlateID, MissingIngredientName).

plate_AnyMissingIngredients(PlateID, RecipeName) :-
    plate_MissingIngredients_Name(PlateID, RecipeName, _).

plate_InvalidIngredients(PlateID, RecipeName) :-
    plate_Ingredients_Name(PlateID, IngredientName),
    c_COMPLETE_RECIPE_NAME(RecipeName),
    not c_COMPLETE_RECIPE_INGREDIENT(RecipeName, IngredientName).

plate_CompletedRecipe(PlateID, RecipeName) :-
    plate_ID(PlateID),
    c_COMPLETE_RECIPE_NAME(RecipeName),
    not plate_AnyMissingIngredients(PlateID, RecipeName),
    not plate_InvalidIngredients(PlateID, RecipeName).


