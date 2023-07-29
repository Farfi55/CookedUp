%For runtime instantiated GameObject, only the prefab mapping is provided. Use that one substituting the gameobject name accordingly.
 %Sensors.
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
%s_Const_KitchenObjectsNames(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CompleteRecipe_Name(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CompleteRecipe_IngredientNames(constantSensors,objectIndex(Index),Index1,Index2,Value).
%s_Const_CompleteRecipe_Value(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_Name(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_OutputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_TimeToCook(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_IsBurningRecipe(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_Name(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_InputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_OutputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_TimeToCut(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_InputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_CuttingCounter_ID(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_HasAny(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CanCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_TimeRemainingToCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_Name(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_InputKOName(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_OutputKOName(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_CurrentCuttingRecipe_TimeToCut(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_Type(cuttingCounterSensor,objectIndex(Index),Value).
%s_CuttingCounter_Name(cuttingCounterSensor,objectIndex(Index),Value).
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
%For ASP programs:
% Predicates for Action invokation.
% applyAction(OrderOfExecution,ActionClassName).
% actionArgument(ActionOrder,ArgumentName, ArgumentValue).
