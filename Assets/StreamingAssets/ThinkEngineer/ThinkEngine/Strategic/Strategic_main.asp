playerBot_NoRecipeRequestCount(Count) :-
    Count = #count{ID : playerBot_HasNoRecipeRequest(ID)}.


recipeRequest_Count(Count) :-
    Count = #count{ID : recipeRequest_ID(ID)}.


recipeRequest_HasPlayer(RecipeRequestID) :-
    recipeRequest_ID(RecipeRequestID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).

recipeRequest_HasNoPlayer(RecipeRequestID) :-
    recipeRequest_ID(RecipeRequestID),
    not recipeRequest_HasPlayer(RecipeRequestID).

recipeRequest_PlayerCount(RecipeRequestID, Count) :-
    recipeRequest_ID(RecipeRequestID),
    Count = #count{PlayerID : playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID)}.

recipeRequest_PlayerCount(RecipeRequestID, 0) :-
    recipeRequest_ID(RecipeRequestID),
    recipeRequest_HasNoPlayer(RecipeRequestID).


recipeRequest_Player(RecipeRequestID, PlayerID) :-
    recipeRequest_ID(RecipeRequestID),
    recipeRequest_PlayerCount(RecipeRequestID, 1),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).    




recipeRequestToAssign(RecipeRequestID) | recipeRequestToNotAssign(RecipeRequestID)  :-
    playerBot_NoRecipeRequestCount(Count),
    recipeRequest_HasNoPlayer(RecipeRequestID).


% a_SetRecipeRequest(ActionIndex, PlayerID, RecipeRequestID).


recipeRequestToAssign_Count(Count) :-
    Count = #count{RecipeRequestID : recipeRequestToAssign(RecipeRequestID)}.

actionIndex(0) :- 
    recipeRequestToAssign_Count(Count),
    Count > 0.

actionIndex(Index) :- 
    actionIndex(Index - 1), 
    recipeRequestToAssign_Count(Count), 
    Index < Count.

{ a_SetRecipeRequest(ActionIndex, PlayerID, RecipeRequestID) } = Count :-
    recipeRequestToAssign_Count(Count),
    recipeRequestToAssign(RecipeRequestID),
    actionIndex(ActionIndex),
    playerBot_HasNoRecipeRequest(PlayerID).

:- a_SetRecipeRequest(ActionIndex1, PlayerID1, RecipeRequestID1),
    a_SetRecipeRequest(ActionIndex2, PlayerID2, RecipeRequestID2),
    ActionIndex1 = ActionIndex2,
    RecipeRequestID1 != RecipeRequestID2.

:- a_SetRecipeRequest(ActionIndex1, PlayerID1, RecipeRequestID1),
    a_SetRecipeRequest(ActionIndex2, PlayerID2, RecipeRequestID2),
    PlayerID1 = PlayerID2,
    RecipeRequestID1 != RecipeRequestID2.

:- a_SetRecipeRequest(ActionIndex1, PlayerID1, RecipeRequestID1),
    a_SetRecipeRequest(ActionIndex2, PlayerID2, RecipeRequestID2),
    PlayerID1 != PlayerID2,
    RecipeRequestID1 = RecipeRequestID2.


:~ playerBot_NoRecipeRequestCount(P_Count),
    recipeRequestToAssign_Count(R_Count),
    Cost = P_Count-R_Count. 
    [Cost@10, 172365172635176235716235]



#show applyAction/2.
#show actionArgument/3.
#show a_SetRecipeRequest/3.
#show recipeRequestToAssign_Count/1.
#show recipeRequestToAssign/1.
#show recipeRequestToNotAssign/1.
#show playerBot_ID/1.
#show playerBot_NoRecipeRequestCount/1.
#show playerBot_HasNoRecipeRequest/1.
#show recipeRequest_HasNoPlayer/1.
#show recipeRequest_HasPlayer/1.
#show recipeRequest_Player/2.
#show recipeRequest_PlayerCount/2.
#show recipeRequest/2.
#show recipeRequest_ID/1.
