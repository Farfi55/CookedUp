% ================================== PLAYER ==================================

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


player_ID_Index(ID,Index) :- s_Player_ID(_,objectIndex(Index),ID).
player_ID(ID) :- player_ID_Index(ID,_).
player_ID_And_0(ID) :- player_ID(ID).
player_ID_And_0(0).

curr_Player_ID(ID) :- 
    playerBot_ID_Index(ID, Index), 
    currentBrainID(Index).


player(ID, Type, Name) :- 
    player_ID_Index(ID,Index),
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
    not player_HasSelectedInteractable(ID).

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
    not player_HasSpace(ID).

player_HasAny(ID) :-
    player_ID_Index(ID,Index),
    s_Player_HasAny(_,objectIndex(Index), true).

player_HasNone(ID) :-
    player_ID_Index(ID,Index),
    not player_HasAny(ID).

% Player Kitchen Objects

player_KO(PlayerID, KitchenObjectID, KitchenObjectName) :-
    player_ID_Index(PlayerID, Index),
    player_HasAny(PlayerID),
    s_Player_FirstKitchenObject_ID(_,objectIndex(Index), KitchenObjectID),
    s_Player_FirstKitchenObject_Name(_,objectIndex(Index), KitchenObjectName).
    % s_Player_FirstKitchenObject_ContainerID(_,objectIndex(Index),ContainerID).

player_KO_ID(PlayerID, KitchenObjectID) :-
    player_KO(PlayerID, KitchenObjectID, _).

player_KO_Name(PlayerID, KitchenObjectName) :-
    player_KO(PlayerID, _, KitchenObjectName).

% player owned plates
player_OwnedPlate(PlayerID, PlateID) :-
    player_ID(PlayerID),
    plate_ID(PlateID),
    ko_HasPlayer(PlateID),
    ko_Player_ID(PlateID, PlayerID).
 
player_OwnedPlate_ValidForRecipe(PlayerID, PlateID, RecipeName) :-
    player_OwnedPlate(PlayerID, PlateID),
    c_CompleteRecipe_Name(RecipeName),
    not plate_Any_InvalidIngredients(PlateID, RecipeName).

player_OwnsAnyPlate(PlayerID) :-
    player_OwnedPlate(PlayerID, _).

player_OwnsAnyPlate_ValidForRecipe(PlayerID, RecipeName) :-
    player_OwnedPlate_ValidForRecipe(PlayerID, _, RecipeName).



% ================================== PLAYER BOT ==================================

%s_PlayerBot_ID(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_HasRecipe(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_RecipeName(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_HasPlate(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_PlateForRecipeID(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_HasMissingIngredients(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_MissingIngredientsNames(playerSensors,objectIndex(Index),Index1,Value).
%s_PlayerBot_HasInvalidIngredients(playerSensors,objectIndex(Index),Value).
%s_PlayerBot_CurrentRecipeRequestASP_ID(playerSensors,objectIndex(Index),Value).

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

playerBot_Plate_ID(PlayerID, PlateID) :-
    playerBot_HasPlate(PlayerID),
    playerBot_ID_Index(PlayerID,Index),
    s_PlayerBot_PlateForRecipeID(_,objectIndex(Index),PlateID).

playerBot_IsPlateBeingCarried(PlayerID) :-
    playerBot_HasPlate(PlayerID),
    player_KO_ID(PlayerID, PlateID),
    playerBot_Plate_ID(PlayerID, PlateID).

% ID of the container that the plate is in
playerBot_Plate_Container_ID(PlayerID, PlateID, ContainerID) :-
    playerBot_HasPlate(PlayerID),
    playerBot_Plate_ID(PlayerID, PlateID),
    ko_OwnerContainer_ID(PlateID, ContainerID).

% Player Recipe

playerBot_HasRecipeRequest(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasRecipe(_,objectIndex(Index), true).

playerBot_HasNoRecipeRequest(ID) :-
    playerBot_ID_Index(ID,Index),
    not playerBot_HasRecipeRequest(ID).


playerBot_RecipeRequest_Name(PlayerID, RecipeName) :-
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_ID_Index(PlayerID,Index),
    s_PlayerBot_RecipeName(_,objectIndex(Index),RecipeName).

playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID) :-
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_ID_Index(PlayerID,Index),
    s_PlayerBot_CurrentRecipeRequest_ID(_,objectIndex(Index),RecipeRequestID).
    

% Player Recipe Ingredients

playerBot_IngredientsNames_Index(PlayerID, IngredientNames, Index) :-
    playerBot_ID_Index(PlayerID,Index1),
    s_PlayerBot_IngredientsNames(_,objectIndex(Index1), Index, IngredientNames).

playerBot_IngredientsNames(PlayerID, IngredientNames) :-
    playerBot_ID_Index(PlayerID,Index),
    playerBot_IngredientsNames_Index(PlayerID, IngredientNames, _).


playerBot_HasMissingIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasMissingIngredients(_,objectIndex(Index), true).

playerBot_HasNoMissingIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    not playerBot_HasMissingIngredients(ID).

playerBot_MissingIngredients_Index(PlayerID, IngredientName, Index) :-
    playerBot_HasMissingIngredients(PlayerID),
    playerBot_ID_Index(PlayerID,Index1),
    s_PlayerBot_MissingIngredientsNames(_,objectIndex(Index1),Index,IngredientName).

playerBot_MissingIngredients(PlayerID, IngredientName) :- 
    playerBot_MissingIngredients_Index(PlayerID, IngredientName, _).

playerBot_MissingBaseIngredients(PlayerID, IngredientName, BaseIngredientName) :- 
    playerBot_MissingIngredients(PlayerID, IngredientName),
    ingredient_NeedsWork(IngredientName, _, BaseIngredientName).

playerBot_MissingIngredients_Or_Base(PlayerID, IngredientName) :- 
    playerBot_MissingIngredients(PlayerID, IngredientName).

playerBot_MissingIngredients_Or_Base(PlayerID, BaseIngredientName) :- 
    playerBot_MissingBaseIngredients(PlayerID, _, BaseIngredientName).


playerBot_HasInvalidIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    s_PlayerBot_HasInvalidIngredients(_,objectIndex(Index), true).

playerBot_HasNoInvalidIngredients(ID) :-
    playerBot_ID_Index(ID,Index),
    not playerBot_HasInvalidIngredients(ID).

playerBot_HasCompletedRecipe(ID) :-
    playerBot_HasRecipeRequest(ID),
    playerBot_HasNoMissingIngredients(ID),
    playerBot_HasNoInvalidIngredients(ID).
