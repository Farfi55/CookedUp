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

counter_ID(ID) :-
    counter_ID_Index(ID,_).

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

counter_HasAny(ID) :-
    counter_ID_Index(ID,Index),
    s_Counter_HasAny(_,objectIndex(Index), true).

% Counter Kitchen Objects

counter_KO(CounterID, KitchenObjectID, KitchenObjectName) :-
    counter_ID_Index(CounterID, Index),
    counter_HasAny(ID),
    s_Counter_FirstKitchenObject_ID(_,objectIndex(Index), KitchenObjectID),
    s_Counter_FirstKitchenObject_Name(_,objectIndex(Index), KitchenObjectName).
    % s_Counter_FirstKitchenObject_ContainerID(_,objectIndex(Index),ContainerID).

counter_KO_ID(CounterID, KitchenObjectID) :-
    counter_KO(CounterID, KitchenObjectID, _).

counter_KO_Name(CounterID, KitchenObjectName) :-
    counter_KO(CounterID, _, KitchenObjectName).


counter_KOs_Index(CounterID, KitchenObjectID, KitchenObjectName, KitchenObjectIndex) :-
    counter_ID_Index(CounterID, Index),
    counter_HasAny(ID),
    s_Counter_KitchenObject_ID(_,objectIndex(Index), KitchenObjectIndex, KitchenObjectID),
    s_Counter_KitchenObject_Name(_,objectIndex(Index), KitchenObjectIndex, KitchenObjectName).
    % s_Counter_KitchenObject_ContainerID(_,objectIndex(Index), KitchenObjectIndex,ContainerID).

counter_KOs(CounterID, KitchenObjectID, KitchenObjectName) :-
    counter_KOs_Index(CounterID, KitchenObjectID, KitchenObjectName, _).

counter_KOs_ID(CounterID, KitchenObjectID) :-
    counter_KOs_Index(CounterID, KitchenObjectID, _, _).

counter_KOs_Name(CounterID, KitchenObjectName) :-
    counter_KOs_Index(CounterID, _, KitchenObjectName, _).

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
%s_PlatesCounter_TimeToNextPlate(counterSensor,objectIndex(Index),Value).

platesCounter_ID_Index(ID,Index) :- 
    s_PlatesCounter_ID(_,objectIndex(Index),ID).

platesCounter_ID(ID) :-
    platesCounter_ID_Index(ID,_).

platesCounter_Limit(ID, PlatesLimit) :-
    platesCounter_ID_Index(ID,Index),
    s_PlatesCounter_PlatesLimit(_,objectIndex(Index),PlatesLimit).

platesCounter_Count(ID, PlatesCount) :-
    platesCounter_ID_Index(ID,Index),
    s_PlatesCounter_PlatesCount(_,objectIndex(Index),PlatesCount).

platesCounter_TimeToNextPlate(ID, TimeToNextPlate) :-
    platesCounter_ID_Index(ID,Index),
    s_PlatesCounter_TimeToNextPlate(_,objectIndex(Index),TimeToNextPlate).


% ================================== CUTTING COUNTER ==================================

%s_CuttingCounter_ID(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_HasAny(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CanCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_TimeRemainingToCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_Name(cuttingCounterSensor,objectIndex(Index),Value).

cuttingCounter_ID_Index(ID,Index) :- 
    s_CuttingCounter_ID(_,objectIndex(Index),ID).

cuttingCounter_ID(ID) :-
    cuttingCounter_ID_Index(ID,_).

cuttingCounter_HasAny(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    s_CuttingCounter_HasAny(_,objectIndex(Index), true).

cuttingCounter_HasNone(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    not cuttingCounter_HasAny(ID).

cuttingCounter_CanCut(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    s_CuttingCounter_CanCut(_,objectIndex(Index), true).

cuttingCounter_CannotCut(ID) :-
    cuttingCounter_ID_Index(ID,Index),
    not cuttingCounter_CanCut(ID).

cuttingCounter_TimeRemainingToCut(ID, TimeRemainingToCut) :-
    cuttingCounter_ID_Index(ID,Index),
    cuttingCounter_CanCut(ID),
    s_CuttingCounter_TimeRemainingToCut(_,objectIndex(Index), TimeRemainingToCut).

cuttingCounter_CurrentCuttingRecipe_Name(ID, RecipeName) :-
    cuttingCounter_ID_Index(ID,Index),
    cuttingCounter_CanCut(ID),
    s_CuttingCounter_CurrentCuttingRecipe_Name(_,objectIndex(Index), RecipeName).

cuttingCounter_CurrentCuttingRecipe(ID, RecipeName, RecipeInput, RecipeOutput, TimeToCut) :-
    cuttingCounter_CurrentCuttingRecipe_Name(ID, RecipeName),
    c_CuttingRecipe_Name(RecipeName, RecipeInput, RecipeOutput, TimeToCut).
    



% ================================== STOVE COUNTER ==================================


%s_StoveCounter_ID(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_HasAny(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CanCook(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_TimeRemainingToCook(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_IsBurning(counterSensor,objectIndex(Index),Value).
%s_StoveCounter_CurrentCookingRecipe_Name(counterSensor,objectIndex(Index),Value).

stoveCounter_ID_Index(ID,Index) :- 
    s_StoveCounter_ID(_,objectIndex(Index),ID).

stoveCounter_ID(ID) :-
    stoveCounter_ID_Index(ID,_).

stoveCounter_HasAny(ID) :-
    stoveCounter_ID_Index(ID,Index),
    s_StoveCounter_HasAny(_,objectIndex(Index), true).

stoveCounter_HasNone(ID) :-
    stoveCounter_ID_Index(ID,Index),
    not stoveCounter_HasAny(ID).

stoveCounter_CanCook(ID) :-
    stoveCounter_ID_Index(ID,Index),
    s_StoveCounter_CanCook(_,objectIndex(Index), true).

stoveCounter_CannotCook(ID) :-
    stoveCounter_ID_Index(ID,Index),
    not stoveCounter_CanCook(ID).

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
    not stoveCounter_IsBurning(ID).


stoveCounter_CurrentCookingRecipe_Name(ID, RecipeName) :-
    stoveCounter_ID_Index(ID,Index),
    stoveCounter_CanCook(ID),
    s_StoveCounter_CurrentCookingRecipe_Name(_,objectIndex(Index), RecipeName).

stoveCounter_CurrentCookingRecipe(ID, RecipeName, RecipeInput, RecipeOutput, TimeToCook, IsBurningRecipe) :-
    stoveCounter_CurrentCookingRecipe_Name(ID, RecipeName),
    c_CookingRecipe(RecipeName, RecipeInput, RecipeOutput, TimeToCook, IsBurningRecipe).



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

clearCounter_HasSpace(ID) :-
    clearCounter_ID(ID),
    counter_HasSpace(ID).

clearCounter_HasAny(ID) :-
    clearCounter_ID(ID),
    counter_HasAny(ID).


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

