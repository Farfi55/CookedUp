%s_Const_CompleteRecipe_IngredientsNames(constantSensors,objectIndex(Index),Index1,Index2,Value).
%s_Const_CompleteRecipe_Name(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CompleteRecipe_Value(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_InputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_IsBurningRecipe(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_Name(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_OutputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CookingRecipe_TimeToCook(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_InputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_Name(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_OutputKOName(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_CuttingRecipe_TimeToCut(constantSensors,objectIndex(Index),Index1,Value).
%s_Const_KitchenObjectsNames(constantSensors,objectIndex(Index),Index1,Value).

c_KO_NAME(KitchenObjectName) :- s_Const_KitchenObjectsNames(_,_,_,KitchenObjectName).

c_COMPLETE_RECIPE(RecipeName, RecipeValue) :- 
    s_Const_CompleteRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CompleteRecipe_Value(_,_,Index1, RecipeValue).

c_COMPLETE_RECIPE_NAME(RecipeName) :- 
    c_COMPLETE_RECIPE(RecipeName, _).

c_COMPLETE_RECIPE_INGREDIENT(RecipeName, IngredientName) :-
    s_Const_CompleteRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CompleteRecipe_IngredientsNames(_,_,Index1,_, IngredientName).

c_COOKING_RECIPE(RecipeName, InputKOName, OutputKOName, TimeToCook, IsBurningRecipe) :-
    s_Const_CookingRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CookingRecipe_InputKOName(_,_,Index1, InputKOName), 
    s_Const_CookingRecipe_OutputKOName(_,_,Index1, OutputKOName), 
    s_Const_CookingRecipe_TimeToCook(_,_,Index1, TimeToCook), 
    s_Const_CookingRecipe_IsBurningRecipe(_,_,Index1, IsBurningRecipe).

c_COOKING_RECIPE_NAME(RecipeName) :-
    c_COOKING_RECIPE(RecipeName, _, _, _, _).

c_CUTTING_RECIPE(RecipeName, InputKOName, OutputKOName, TimeToCut) :-
    s_Const_CuttingRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CuttingRecipe_InputKOName(_,_,Index1, InputKOName), 
    s_Const_CuttingRecipe_OutputKOName(_,_,Index1, OutputKOName), 
    s_Const_CuttingRecipe_TimeToCut(_,_,Index1, TimeToCut).

c_CUTTING_RECIPE_NAME(RecipeName) :-
    c_CUTTING_RECIPE(RecipeName, _, _, _).


