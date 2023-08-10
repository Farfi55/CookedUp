
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
    not plate_IsInContainer(PlateID).

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
    c_CompleteRecipe_Ingredient(RecipeName, MissingIngredientName),
    not plate_Ingredients_Name(PlateID, MissingIngredientName).

plate_AnyMissingIngredients(PlateID, RecipeName) :-
    plate_MissingIngredients_Name(PlateID, RecipeName, _).

plate_InvalidIngredients(PlateID, RecipeName) :-
    plate_Ingredients_Name(PlateID, IngredientName),
    c_CompleteRecipe_Name(RecipeName),
    not c_CompleteRecipe_Ingredient(RecipeName, IngredientName).

plate_CompletedRecipe(PlateID, RecipeName) :-
    plate_ID(PlateID),
    c_CompleteRecipe_Name(RecipeName),
    not plate_AnyMissingIngredients(PlateID, RecipeName),
    not plate_InvalidIngredients(PlateID, RecipeName).

