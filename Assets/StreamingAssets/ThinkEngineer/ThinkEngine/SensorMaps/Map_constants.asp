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

c_KO_Name(KitchenObjectName) :- s_Const_KitchenObjectsNames(_,_,_,KitchenObjectName).
c_KO_Name(KitchenObjectName) :- s_Const_KitchenObjectsNames0(_,_,_,KitchenObjectName).

c_CompleteRecipe(RecipeName, RecipeValue) :- 
    s_Const_CompleteRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CompleteRecipe_Value(_,_,Index1, RecipeValue).

c_CompleteRecipe_Name(RecipeName) :- 
    c_CompleteRecipe(RecipeName, _).

c_CompleteRecipe_Ingredient(RecipeName, IngredientName) :-
    s_Const_CompleteRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CompleteRecipe_IngredientsNames(_,_,Index1,_, IngredientName).

c_CookingRecipe(RecipeName, InputKOName, OutputKOName, TimeToCook, IsBurningRecipe) :-
    s_Const_CookingRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CookingRecipe_InputKOName(_,_,Index1, InputKOName), 
    s_Const_CookingRecipe_OutputKOName(_,_,Index1, OutputKOName), 
    s_Const_CookingRecipe_TimeToCook(_,_,Index1, TimeToCook), 
    s_Const_CookingRecipe_IsBurningRecipe(_,_,Index1, IsBurningRecipe).

c_CookingRecipe_Name(RecipeName) :-
    c_CookingRecipe(RecipeName, _, _, _, _).

c_CuttingRecipe(RecipeName, InputKOName, OutputKOName, TimeToCut) :-
    s_Const_CuttingRecipe_Name(_,_,Index1, RecipeName), 
    s_Const_CuttingRecipe_InputKOName(_,_,Index1, InputKOName), 
    s_Const_CuttingRecipe_OutputKOName(_,_,Index1, OutputKOName), 
    s_Const_CuttingRecipe_TimeToCut(_,_,Index1, TimeToCut).

c_CuttingRecipe_Name(RecipeName) :-
    c_CuttingRecipe_Name(RecipeName, _, _, _).
