
%constantSensor(_,_,constantsSensorData(completeRecipes(Index1,completeRecipe(name(Value))))).
%constantSensor(_,_,constantsSensorData(completeRecipes(Index1,completeRecipe(ingredientsNames(Index2,Value))))).
%constantSensor(_,_,constantsSensorData(completeRecipes(Index1,completeRecipe(value(Value))))).

%constantSensor(_,_,constantsSensorData(cookingRecipes(Index1,cookingRecipe(name(Value))))).
%constantSensor(_,_,constantsSensorData(cookingRecipes(Index1,cookingRecipe(inputKOName(Value))))).
%constantSensor(_,_,constantsSensorData(cookingRecipes(Index1,cookingRecipe(outputKOName(Value))))).
%constantSensor(_,_,constantsSensorData(cookingRecipes(Index1,cookingRecipe(timeToCook(Value))))).
%constantSensor(_,_,constantsSensorData(cookingRecipes(Index1,cookingRecipe(isBurningRecipe(Value))))).

%constantSensor(_,_,constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(name(Value))))).
%constantSensor(_,_,constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(inputKOName(Value))))).
%constantSensor(_,_,constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(outputKOName(Value))))).
%constantSensor(_,_,constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(timeToCut(Value))))).

%constantSensor(_,_,constantsSensorData(kitchenObjectsNames(Index1,Value))).


koName(Value) :- constantsSensorData(kitchenObjectsNames(Index1,Value)).

completeRecipe(RecipeName, RecipeValue) :- 
    constantsSensorData(completeRecipes(Index1,completeRecipe(name(RecipeName)))), 
    constantsSensorData(completeRecipes(Index1,completeRecipe(value(RecipeValue)))).

completeRecipeIngredient(RecipeName, IngredientName) :- 
    constantsSensorData(completeRecipes(Index1,completeRecipe(name(RecipeName)))), 
    constantsSensorData(completeRecipes(Index1,completeRecipe(ingredientsNames(Index2,IngredientName)))).


cookingRecipe(RecipeName, InputKOName, OutputKOName, TimeToCook, IsBurningRecipe) :- 
    constantsSensorData(cookingRecipes(Index1,cookingRecipe(name(RecipeName)))), 
    constantsSensorData(cookingRecipes(Index1,cookingRecipe(inputKOName(InputKOName)))), 
    constantsSensorData(cookingRecipes(Index1,cookingRecipe(outputKOName(OutputKOName)))), 
    constantsSensorData(cookingRecipes(Index1,cookingRecipe(timeToCook(TimeToCook)))), 
    constantsSensorData(cookingRecipes(Index1,cookingRecipe(isBurningRecipe(IsBurningRecipe)))).


cuttingRecipe(RecipeName, InputKOName, OutputKOName, TimeToCut) :-
    constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(name(RecipeName)))), 
    constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(inputKOName(InputKOName)))), 
    constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(outputKOName(OutputKOName)))), 
    constantsSensorData(cuttingRecipes(Index1,cuttingRecipe(timeToCut(TimeToCut)))).



