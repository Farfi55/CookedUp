%For runtime instantiated GameObject, only the prefab mapping is provided. Use that one substituting the gameobject name accordingly.
 %Sensors.
%s_Player_ID(player,objectIndex(Index),Value).
%s_Player_Type(player,objectIndex(Index),Value).
%s_Player_Name(player,objectIndex(Index),Value).
%s_Player_ContainerID(player,objectIndex(Index),Value).
%s_Player_SizeLimit(player,objectIndex(Index),Value).
%s_Player_Count(player,objectIndex(Index),Value).
%s_Player_FirstKitchenObject_Name(player,objectIndex(Index),Value).
%s_Player_FirstKitchenObject_ID(player,objectIndex(Index),Value).
%s_Player_FirstKitchenObject_ContainerID(player,objectIndex(Index),Value).
%s_Player_HasSpace(player,objectIndex(Index),Value).
%s_Player_HasAny(player,objectIndex(Index),Value).
%s_Player_HasSelectedInteractable(player,objectIndex(Index),Value).
%s_Player_SelectedInteractableType(player,objectIndex(Index),Value).
%s_Player_SelectedInteractableID(player,objectIndex(Index),Value).
%s_Player_X(player,objectIndex(Index),Value).
%s_Player_Y(player,objectIndex(Index),Value).
%s_Delivery_RecipeRequest_ID(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_TimeToComplete(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_RemainingTimeToComplete(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_Name(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_IngredientsNames(deliverySensor,objectIndex(Index),Index1,Index2,Value).
%s_Delivery_RecipeRequest_Value(deliverySensor,objectIndex(Index),Index1,Value).
%s_Const_KitchenObjectsNames0(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_Name(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_InputKOName(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_OutputKOName(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_TimeToCut(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_IsBurningRecipe(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_TimeToCook(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_OutputKOName(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_InputKOName(constantsSensor,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_Name(constantsSensor,objectIndex(Index),Index1,Value).
%s_PlayerBot_HasRecipe(player,objectIndex(Index),Value).
%s_PlayerBot_RecipeName(player,objectIndex(Index),Value).
%s_PlayerBot_HasPlate(player,objectIndex(Index),Value).
%s_PlayerBot_PlateForRecipeID(player,objectIndex(Index),Value).
%s_PlayerBot_HasMissingIngredients(player,objectIndex(Index),Value).
%s_PlayerBot_MissingIngredientsNames(player,objectIndex(Index),Index1,Value).
%s_PlayerBot_HasInvalidIngredients(player,objectIndex(Index),Value).
%s_PlayerBot_PlayerBotSensorData_CurrentRecipeRequestASP_ID(player,objectIndex(Index),Value).
% Predicates for Action invokation.
% applyAction(OrderOfExecution,ActionClassName).
% actionArgument(ActionOrder,ArgumentName, ArgumentValue).
