
% ================================== KITCHEN OBJECTS ==================================



% ================================== KITCHEN OBJECTS ==================================

%s_KO_ID(kitchenObjectSensor,objectIndex(Index),Value).
%s_KO_Type(kitchenObjectSensor,objectIndex(Index),Value).
%s_KO_Name(kitchenObjectSensor,objectIndex(Index),Value).
%s_KO_HasOwnerContainer(kitchenObjectSensor,objectIndex(Index),Value).
%s_KO_OwnerContainerID(kitchenObjectSensor,objectIndex(Index),Value).
%s_KO_HasPlayer(kitchenObjectSensor,objectIndex(Index),Value).
%s_KO_PlayerID(kitchenObjectSensor,objectIndex(Index),Value).


ko_ID_Index(KitchenObjectID, Index) :-
    s_KO_ID(_,objectIndex(Index),KitchenObjectID).

ko_ID(KitchenObjectID) :-
    ko_ID_Index(KitchenObjectID,_).

ko_Name(KitchenObjectID, KitchenObjectName) :-
    ko_ID_Index(KitchenObjectID, Index),
    s_KO_Name(_,objectIndex(Index),KitchenObjectName).

ko(KitchenObjectID, KitchenObjectName, OwnerContainerID) :-
    ko_ID(KitchenObjectID),
    ko_Name(KitchenObjectID, KitchenObjectName),
    ko_HasOwnerContainer(KitchenObjectID),
    ko_OwnerContainer_ID(KitchenObjectID, OwnerContainerID).


ko_HasOwnerContainer(KitchenObjectID) :-
    ko_ID_Index(KitchenObjectID, Index),
    s_KO_HasOwnerContainer(_,objectIndex(Index), true).

ko_OwnerContainer_ID(KitchenObjectID, OwnerContainerID) :-
    ko_ID_Index(KitchenObjectID, Index),
    ko_HasOwnerContainer(KitchenObjectID),
    s_KO_OwnerContainerID(_,objectIndex(Index),OwnerContainerID).

ko_HasPlayer(KitchenObjectID) :-
    ko_ID_Index(KitchenObjectID, Index),
    s_KO_HasPlayer(_,objectIndex(Index), true).

ko_Player_ID(KitchenObjectID, PlayerID) :-
    ko_ID_Index(KitchenObjectID, Index),
    ko_HasPlayer(KitchenObjectID),
    s_KO_PlayerID(_,objectIndex(Index),PlayerID).


% ================================== PLATE KITCHEN OBJECT ==================================

%s_Plate_ID(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_ContainerID(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_SizeLimit(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_Count(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_HasSpace(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_HasAny(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_KitchenObjectASP_Name(kitchenObjectSensor,objectIndex(Index),Index1,Value).
%s_Plate_KitchenObjectASP_ID(kitchenObjectSensor,objectIndex(Index),Index1,Value).
%s_Plate_KitchenObjectASP_ContainerID(kitchenObjectSensor,objectIndex(Index),Index1,Value).
%s_Plate_FirstKitchenObject_Name(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_FirstKitchenObject_ID(kitchenObjectSensor,objectIndex(Index),Value).
%s_Plate_FirstKitchenObject_ContainerID(kitchenObjectSensor,objectIndex(Index),Value).


plate_ID_Index(PlateID, Index) :-
    s_Plate_ID(_,objectIndex(Index),PlateID).

plate_ID(PlateID) :-
    plate_ID_Index(PlateID,_).


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


plate_Ingredients_Index(PlateID, IngredientID, IngredientName, Index) :-
    plate_ID_Index(PlateID, PlateIndex),
    plate_HasAnyIngredients(PlateID),
    s_Plate_KitchenObjectASP_ID(_,objectIndex(PlateIndex),Index, IngredientID),
    s_Plate_KitchenObjectASP_Name(_,objectIndex(PlateIndex),Index, IngredientName).

plate_Ingredients(PlateID, IngredientID, IngredientName) :-
    plate_Ingredients_Index(PlateID, IngredientID, IngredientName, _).

plate_Ingredients_ID(PlateID, IngredientID) :-
    plate_Ingredients(PlateID, IngredientID, _).

plate_Ingredients_Name(PlateID, IngredientName) :-
    plate_Ingredients(PlateID, _, IngredientName).


% Plate Recipes utilities

plate_MissingIngredients_Name(PlateID, RecipeName, MissingIngredientName) :-
    plate_ID(PlateID),
    c_CompleteRecipe_Ingredient(RecipeName, MissingIngredientName),
    not plate_Ingredients_Name(PlateID, MissingIngredientName).

plate_Any_MissingIngredients(PlateID, RecipeName) :-
    plate_MissingIngredients_Name(PlateID, RecipeName, _).

plate_Any_InvalidIngredients(PlateID, RecipeName) :-
    plate_Ingredients_Name(PlateID, IngredientName),
    c_CompleteRecipe_Name(RecipeName),
    not c_CompleteRecipe_Ingredient(RecipeName, IngredientName).

plate_CompletedRecipe(PlateID, RecipeName) :-
    plate_ID(PlateID),
    c_CompleteRecipe_Name(RecipeName),
    not plate_Any_MissingIngredients(PlateID, RecipeName),
    not plate_Any_InvalidIngredients(PlateID, RecipeName).

plate_Any_CompletedRecipe(PlateID) :-
    plate_CompletedRecipe(PlateID, _).
