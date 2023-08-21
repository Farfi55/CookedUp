% ================================== Set Recipe Request ==================================

% a_SetRecipeRequest(ActionIndex, PlayerID, RecipeRequestID).

applyAction(ActionIndex, "SetRecipeRequestAction") :- 
    a_SetRecipeRequest(ActionIndex, _, _).


actionArgument(ActionIndex, "PlayerID", PlayerID) :- 
    a_SetRecipeRequest(ActionIndex, PlayerID, _).

actionArgument(ActionIndex, "RecipeRequestID", RecipeRequestID) :- 
    a_SetRecipeRequest(ActionIndex, _, RecipeRequestID).

