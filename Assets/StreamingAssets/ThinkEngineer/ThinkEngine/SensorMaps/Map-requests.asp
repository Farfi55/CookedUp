%s_Delivery_RecipeRequest_ID(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_TimeToComplete(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_RemainingTimeToComplete(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_Name(deliverySensor,objectIndex(Index),Index1,Value).
%s_Delivery_RecipeRequest_IngredientsNames(deliverySensor,objectIndex(Index),Index1,Index2,Value).
%s_Delivery_RecipeRequest_Value(deliverySensor,objectIndex(Index),Index1,Value).

recipeRequest_ID_Index(ID, Index, Index1) :- 
    s_Delivery_RecipeRequest_ID(_,objectIndex(Index),Index1,ID).


recipeRequest_ID(ID) :- 
    recipeRequest_ID_Index(ID, _). 

recipeRequest(ID, RecipeName) :-
    recipeRequest_ID_Index(ID, Index, Index1),
    s_Delivery_RecipeRequest_Name(_,objectIndex(Index),Index1,RecipeName).

recipeRequest_IngredientsName(ID, IngredientName) :-
    recipeRequest_ID_Index(ID, Index, Index1),
    s_Delivery_RecipeRequest_IngredientsNames(_,objectIndex(Index),Index1,Index2,IngredientName).
    
recipeRequest_TimeToComplete(ID, TimeToComplete) :-
    recipeRequest_ID_Index(ID, Index, Index1),
    s_Delivery_RecipeRequest_TimeToComplete(_,objectIndex(Index),Index1,TimeToComplete).

recipeRequest_RemainingTimeToComplete(ID, RemainingTimeToComplete) :-
    recipeRequest_ID_Index(ID, Index, Index1),
    s_Delivery_RecipeRequest_RemainingTimeToComplete(_,objectIndex(Index),Index1,RemainingTimeToComplete).

recipeRequest_Value(ID, Value) :-
    recipeRequest_ID_Index(ID, Index, Index1),
    s_Delivery_RecipeRequest_Value(_,objectIndex(Index),Index1,Value).

recipeRequest_Full(ID, RecipeName, TimeToComplete, RemainingTimeToComplete, Value) :-
    recipeRequest(ID, RecipeName),
    recipeRequest_TimeToComplete(ID, TimeToComplete),
    recipeRequest_RemainingTimeToComplete(ID, RemainingTimeToComplete),
    recipeRequest_Value(ID, Value).

    
