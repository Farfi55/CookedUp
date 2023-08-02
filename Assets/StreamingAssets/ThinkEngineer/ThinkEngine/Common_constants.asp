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

c_KO_NAME("Bread").
c_KO_NAME("Cabbage").
c_KO_NAME("CabbageSlices").
c_KO_NAME("CheeseBlock").
c_KO_NAME("CheeseSlices").
c_KO_NAME("MeatPattyBurned").
c_KO_NAME("MeatPattyCooked").
c_KO_NAME("MeatPattyUncooked").
c_KO_NAME("Plate").
c_KO_NAME("Tomato").
c_KO_NAME("TomatoSlices").


c_COMPLETE_RECIPE_NAME("Burger", 10).
c_COMPLETE_RECIPE_NAME("CheeseBurger", 15).
c_COMPLETE_RECIPE_NAME("MegaBurger", 30).
c_COMPLETE_RECIPE_NAME("Salad", 8).

c_COMPLETE_RECIPE_INGREDIENT("Burger", "Bread").
c_COMPLETE_RECIPE_INGREDIENT("Burger", "MeatPattyCooked").

c_COMPLETE_RECIPE_INGREDIENT("CheeseBurger", "Bread").
c_COMPLETE_RECIPE_INGREDIENT("CheeseBurger", "MeatPattyCooked").
c_COMPLETE_RECIPE_INGREDIENT("CheeseBurger", "CheeseSlices").

c_COMPLETE_RECIPE_INGREDIENT("MegaBurger", "Bread").
c_COMPLETE_RECIPE_INGREDIENT("MegaBurger", "MeatPattyCooked").
c_COMPLETE_RECIPE_INGREDIENT("MegaBurger", "CheeseSlices").
c_COMPLETE_RECIPE_INGREDIENT("MegaBurger", "TomatoSlices").
c_COMPLETE_RECIPE_INGREDIENT("MegaBurger", "CabbageSlices").

c_COMPLETE_RECIPE_INGREDIENT("Salad", "TomatoSlices").
c_COMPLETE_RECIPE_INGREDIENT("Salad", "CabbageSlices").


c_COOKING_RECIPE(
    "MeatPattyUncooked-MeatPattyCooked", 
    "MeatPattyUncooked", "MeatPattyCooked", 
    5000, false).

c_COOKING_RECIPE(
    "MeatPattyCooked-MeatPattyBurned", 
    "MeatPattyCooked", "MeatPattyBurned", 
    3500, true).


c_CUTTING_RECIPE(
    "Cabbage-CabbageSlices", 
    "Cabbage", "CabbageSlices", 
    2000).

c_CUTTING_RECIPE(
    "Tomato-TomatoSlices", 
    "Tomato", "TomatoSlices", 
    1000).

c_CUTTING_RECIPE(
    "CheeseBlock-CheeseSlices", 
    "CheeseBlock", "CheeseSlices", 
    1500).
