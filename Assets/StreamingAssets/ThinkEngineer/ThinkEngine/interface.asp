% ================================== BUILT IN ==================================

currentBrainID(Id).


% ================================== ACTIONS ==================================

a_MoveTo_Target(ActionIndex, TargetID).
a_MoveTo_Pos(ActionIndex, GridX,GridY).

a_PickUp(ActionIndex, TargetInteractableID).

a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeName).
a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName).

a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName, RecipeName).
a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName).

a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName, RecipeName).
a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName).

a_Drop(ActionIndex, TargetInteractableID).
a_Place(ActionIndex, TargetInteractableID).

a_Cut(ActionIndex, TargetInteractableID).

a_WaitToCook(ActionIndex, TargetInteractableID).

a_Wait(ActionIndex, MilliSecondsToWait).
a_Wait(ActionIndex).


% ================================== CONSTANTS ==================================

c_KO_NAME(KitchenObjectName).

c_COMPLETE_RECIPE(RecipeName, RecipeValue).
c_COMPLETE_RECIPE_NAME(RecipeName).
c_COMPLETE_RECIPE_INGREDIENT(RecipeName, IngredientName).

c_COOKING_RECIPE(RecipeName, InputKOName, OutputKOName, TimeToCook, IsBurningRecipe).
c_COOKING_RECIPE_NAME(RecipeName).

c_CUTTING_RECIPE(RecipeName, InputKOName, OutputKOName, TimeToCut).
c_CUTTING_RECIPE_NAME(RecipeName) :-


% ================================== PLAYER ==================================

player_ID_Index(ID,Index).
player_ID(ID).
curr_Player_ID(ID).

player(ID, Type, Name).
player_Pos(ID, X, Y).

% Player Selected Interactable
player_HasSelectedInteractable(ID).
player_HasNoSelectedInteractable(ID).
player_SelectedInteractable(PlayerID, InteractableID, InteractableType).

% Player Container
player_Container_Count(ID, Count).
player_HasSpace(ID).
player_HasNoSpace(ID).
player_HasAny(ID).
player_HasNone(ID).

% Player Kitchen Objects
player_KitchenObject(PlayerID, KitchenObjectID, KitchenObjectName).


% ================================== PLAYER BOT ==================================

% Player Plate
playerBot_HasPlate(ID).
playerBot_HasNoPlate(ID).
playerBot_Plate_ID(PlayerID, PlateID).
playerBot_IsPlateBeingCarried

% Player Recipe
playerBot_HasRecipe(ID).
playerBot_HasNoRecipe(ID).
playerBot_Recipe(PlayerID, RecipeName).

% Player Recipe Ingredients
playerBot_HasMissingIngredients(ID).
playerBot_HasNoMissingIngredients(ID).
playerBot_MissingIngredients_Index(PlayerID, IngredientName, Index).
playerBot_MissingIngredients(PlayerID, IngredientName).
playerBot_HasInvalidIngredients(ID).
playerBot_HasNoInvalidIngredients(ID).
playerBot_HasCompletedRecipe(ID).


% ================================== KITCHEN OBJECTS ==================================

any_KitchenObject_Index(OwnerID, KitchenObjectID, KitchenObjectName, 0).
any_KitchenObject_Index(OwnerID, KitchenObjectID, KitchenObjectName, KitchenObjectIndex).
any_KitchenObject(OwnerID, KitchenObjectID, KitchenObjectName).


% ================================== PLATE KITCHEN OBJECT ==================================

any_Plate(OwnerID, KitchenObjectID).
plate_ID_Index(PlateID, Index).
plate_ID(PlateID).
plate_IsInContainer(PlateID).
plate_IsNotInContainer(PlateID).

plate_Container_ID(PlateID, ContainerID). % container which contains the plate

% Plate ingredients
plate_IngredientsContainer_ID(PlateID, IngredientsContainerID). % plate ingredients's container 
plate_HasAnyIngredients(PlateID).
plate_Ingredients_Count(PlateID, IngredientsCount).
plate_Ingredients_Name(PlateID, IngredientName).

% Plate Recipes utilities

plate_MissingIngredients_Name(PlateID, RecipeName, MissingIngredientName).
plate_AnyMissingIngredients(PlateID, RecipeName).
plate_InvalidIngredients(PlateID, RecipeName).
plate_CompletedRecipe(PlateID, RecipeName).


% ================================== COUNTERS ==================================

counter_ID_Index(ID,Index).
counter(ID, Type, Name).
counter_Pos(ID, X, Y).

% Counter Container
counter_ContainerID(ID, ContainerID).
counter_Container_Count(ID, Count).
counter_HasSpace(ID).
counter_HasNoSpace(ID).
counter_HasAny(ID).
counter_HasNone(ID).

% Counter Kitchen Objects
counter_KitchenObject(CounterID, KitchenObjectID, KitchenObjectName).
counter_KitchenObjects_Index(CounterID, KitchenObjectID, KitchenObjectName, KitchenObjectIndex).
counter_KitchenObjects(CounterID, KitchenObjectID, KitchenObjectName).


% ================================== CONTAINER COUNTER ==================================


containerCounter_ID_Index(ID,Index).
containerCounter_ID(ID).
containerCounter_KOType(ID, KitchenObjectType).


% ================================== PLATES COUNTER ==================================

platesCounter_ID_Index(ID,Index).
platesCounter_ID(ID).
platesCounter_Limit_Count(ID, PlatesLimit, PlatesCount).


% ================================== CUTTING COUNTER ==================================

cuttingCounter_ID_Index(ID,Index).
cuttingCounter_ID(ID).
cuttingCounter_HasAny(ID).
cuttingCounter_HasNone(ID).
cuttingCounter_CanCut(ID).
cuttingCounter_CannotCut(ID).
cuttingCounter_TimeRemainingToCut(ID, TimeRemainingToCut).
cuttingCounter_CurrentCuttingRecipe_Name(ID, RecipeName).
cuttingCounter_CurrentCuttingRecipe(ID, RecipeName, RecipeInput, RecipeOutput, RecipeTimeToCut).


% ================================== STOVE COUNTER ==================================

stoveCounter_ID_Index(ID,Index).
stoveCounter_ID(ID).
stoveCounter_HasAny(ID).
stoveCounter_HasNone(ID).
stoveCounter_CanCook(ID).
stoveCounter_CannotCook(ID).
stoveCounter_TimeRemainingToCook(ID, TimeRemainingToCook).
stoveCounter_IsBurning(ID).
stoveCounter_IsNotBurning(ID).

stoveCounter_CurrentCookingRecipe_Name(ID, RecipeName).
stoveCounter_CurrentCookingRecipe(ID, RecipeName, RecipeInput, RecipeOutput, RecipeTimeToCook, RecipeIsBurning).


% ================================== CLEAR COUNTERS ==================================

clearCounter_ID_Index(ID,Index).
clearCounter_ID(ID).


% ================================== TRASH COUNTERS ==================================

trashCounter_ID_Index(ID,Index).
trashCounter_ID(ID).


% ================================== DELIVERY COUNTERS ==================================

deliveryCounter_ID_Index(ID,Index).
deliveryCounter_ID(ID).


