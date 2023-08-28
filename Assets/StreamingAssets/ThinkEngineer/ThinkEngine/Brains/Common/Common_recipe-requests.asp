recipeRequest_HasPlayer(RecipeRequestID) :-
    recipeRequest_ID(RecipeRequestID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).

recipeRequest_HasNoPlayer(RecipeRequestID) :-
    recipeRequest_ID(RecipeRequestID),
    not recipeRequest_HasPlayer(RecipeRequestID).

recipeRequest_Player_Count(RecipeRequestID, Count) :-
    recipeRequest_ID(RecipeRequestID),
    Count = #count{ PlayerID : 
        playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID) 
    }.


recipeRequest_Player(RecipeRequestID, PlayerID) :-
    recipeRequest_ID(RecipeRequestID),
    recipeRequest_HasPlayer(RecipeRequestID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).  
