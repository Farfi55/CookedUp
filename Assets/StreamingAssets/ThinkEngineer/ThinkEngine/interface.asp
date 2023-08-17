% ================================== BUILT IN ==================================

currentBrainID(Id).
applyAction(ActionIndex, ActionType).
actionArgument(ActionIndex, ArgumentName, ArgumentValue).



% ================================== PLAYER ACTIONS ==================================

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


% ================================== STRATEGIC ACTIONS ==================================

a_SetRecipeRequest(ActionIndex, PlayerID, RecipeID).


% ================================== CONSTANTS ==================================

c_KO_Name(KitchenObjectName).

c_CompleteRecipe(RecipeName, RecipeValue).
c_CompleteRecipe_Name(RecipeName).
c_CompleteRecipe_Ingredient(RecipeName, IngredientName).

c_CookingRecipe(RecipeName, InputKOName, OutputKOName, TimeToCook, IsBurningRecipe).
c_CookingRecipe_Name(RecipeName).

c_CuttingRecipe(RecipeName, InputKOName, OutputKOName, TimeToCut).
c_CuttingRecipe_Name(RecipeName).


% ================================== RECIPE REQUESTS ==================================

recipeRequest_ID_Index(ID, Index, Index1).

recipeRequest_ID(ID).

recipeRequest(ID, RecipeName).

recipeRequest_IngredientsName(ID, IngredientName).
recipeRequest_TimeToComplete(ID, TimeToComplete).

recipeRequest_RemainingTimeToComplete(ID, RemainingTimeToComplete).

recipeRequest_Value(ID, Value).

recipeRequest_Full(ID, RecipeName, TimeToComplete, RemainingTimeToComplete, Value).


% ================================== COMMON INGREDIENTS ==================================


ingredient_Available_Target(IngredientName, TargetID).
ingredient_Available(IngredientName).
ingredient_NotAvailable(IngredientName).

ingredient_NeedsCooking(IngredientName, RecipeName, BaseIngredientName).
ingredient_NeedsCutting(IngredientName, RecipeName, BaseIngredientName).
ingredient_NeedsWork(IngredientName, RecipeName, BaseIngredientName) .

ingredient_ExpectedGetTime(IngredientName, Time).


% ================================== COMMON DISTANCE ==================================

player_Counter_Distance(PlayerID, CounterID, Distance).
curr_Player_Counter_Distance(CounterID, Distance).
counter_Distance(CounterID1, CounterID2, Distance).

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
player_KO(PlayerID, KitchenObjectID, KitchenObjectName).
player_KO_ID(PlayerID, KitchenObjectID).
player_KO_Name(PlayerID, KitchenObjectName).


% ================================== PLAYER BOT ==================================

% Player Plate
playerBot_HasPlate(ID).
playerBot_Plate_ID(PlayerID, PlateID).
playerBot_IsPlateBeingCarried(PlayerID).
playerBot_Plate_Container_ID(PlayerID, PlateID, ContainerID).

% Player Recipe
playerBot_HasRecipeRequest(ID).
playerBot_HasNoRecipeRequest(ID).
playerBot_RecipeRequest_Name(PlayerID, RecipeName).
playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).


% Player Recipe Ingredients
playerBot_IngredientsNames_Index(PlayerID, IngredientNames, Index).
playerBot_IngredientsNames(PlayerID, IngredientNames).
playerBot_HasMissingIngredients(ID).
playerBot_HasNoMissingIngredients(ID).
playerBot_MissingIngredients_Index(PlayerID, IngredientName, Index).
playerBot_MissingIngredients(PlayerID, IngredientName).
playerBot_MissingBaseIngredients(PlayerID, IngredientName, BaseIngredientName).
playerBot_MissingIngredients_Or_Base(PlayerID, IngredientName).
playerBot_HasInvalidIngredients(ID).
playerBot_HasNoInvalidIngredients(ID).
playerBot_HasCompletedRecipe(ID).


% ================================== KITCHEN OBJECTS ==================================

ko_ID_Index(KitchenObjectID, Index).
ko_ID(KitchenObjectID).
ko_Name(KitchenObjectID, KitchenObjectName).
ko(KitchenObjectID, KitchenObjectName, OwnerContainerID).

ko_HasOwnerContainer(KitchenObjectID).
ko_OwnerContainer_ID(KitchenObjectID, OwnerContainerID).
ko_HasPlayer(KitchenObjectID).
ko_Player_ID(KitchenObjectID, PlayerID).
ko_Curr_Player(KitchenObjectID). 

% ================================== PLATE KITCHEN OBJECT ==================================


plate_ID_Index(PlateID, Index).
plate_ID(PlateID).

% Plate ingredients
plate_IngredientsContainer_ID(PlateID, IngredientsContainerID). % plate ingredients's container 
plate_HasAnyIngredients(PlateID).
plate_Ingredients_Count(PlateID, IngredientsCount).

plate_Ingredients_Index(PlateID, IngredientID, IngredientName, Index).
plate_Ingredients(PlateID, IngredientID, IngredientName).
plate_Ingredients_ID(PlateID, IngredientID).
plate_Ingredients_Name(PlateID, IngredientName).

% Plate Recipes utilities

plate_MissingIngredients_Name(PlateID, RecipeName, MissingIngredientName).
plate_Any_MissingIngredients(PlateID, RecipeName).
plate_Any_InvalidIngredients(PlateID, RecipeName).
plate_CompletedRecipe(PlateID, RecipeName).
plate_Any_CompletedRecipe(PlateID).

% ================================== COUNTERS ==================================

counter_ID_Index(ID,Index).
counter(ID, Type, Name).
counter_Pos(ID, X, Y).

% Counter Container
counter_ContainerID(ID, ContainerID).
counter_Container_Count(ID, Count).
counter_HasSpace(ID).
counter_HasAny(ID).

% Counter Kitchen Objects
counter_KO(CounterID, KitchenObjectID, KitchenObjectName).
counter_KO_ID(CounterID, KitchenObjectID).
counter_KO_Name(CounterID, KitchenObjectName).

counter_KOs_Index(CounterID, KitchenObjectID, KitchenObjectName, KitchenObjectIndex).
counter_KOs(CounterID, KitchenObjectID, KitchenObjectName).
counter_KOs_ID(CounterID, KitchenObjectID).
counter_KOs_Name(CounterID, KitchenObjectName).

% ================================== CONTAINER COUNTER ==================================


containerCounter_ID_Index(ID,Index).
containerCounter_ID(ID).
containerCounter_KOType(ID, KitchenObjectType).


% ================================== PLATES COUNTER ==================================

platesCounter_ID_Index(ID,Index).
platesCounter_ID(ID).
platesCounter_Limit(ID, PlatesLimit).
platesCounter_Count(ID, PlatesCount).
platesCounter_TimeToNextPlate(ID, TimeToNextPlate).

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


