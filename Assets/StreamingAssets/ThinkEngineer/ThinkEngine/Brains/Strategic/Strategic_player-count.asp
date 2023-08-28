% ============================ INITIAL PLAYER COUNT ============================

strat_RecipeRequest_Count(Count) :-
    Count = #count{ID : recipeRequest_ID(ID)}.
    
strat_RecipeRequest_Count(0) :-
    not strat_Any_RecipeRequest().


% the number of players currently assigned to the recipe request with the most players
strat_Max_recipeRequest_Player_Count(Count) :-
    Count = #max{Count1 : recipeRequest_Player_Count(RecipeRequestID1, Count1)}. 

strat_Max_recipeRequest_Player_Count(0) :-
    strat_RecipeRequest_Count(0).

% the number of players currently assigned to the recipe request with the least players
strat_Min_recipeRequest_Player_Count(Count) :-
    Count = #min{Count1 : recipeRequest_Player_Count(RecipeRequestID1, Count1)}. 

strat_Min_recipeRequest_Player_Count(0) :-
    strat_RecipeRequest_Count(0).

strat_recipeRequest_Player_Diff(Diff) :-
    strat_Max_recipeRequest_Player_Count(MaxCount),
    strat_Min_recipeRequest_Player_Count(MinCount),
    Diff = MaxCount - MinCount.

% ============================ PLAYER COUNT CHANGES ============================

% has this recipe request had any players added to it?
strat_RecipeRequest_AnyPlayer_Added(RecipeRequestID) :-
    strat_RecipeRequest_Player_Added(RecipeRequestID, _).

% has this recipe request had any players removed from it?
strat_RecipeRequest_AnyPlayer_Removed(RecipeRequestID) :-
    strat_RecipeRequest_Player_Removed(RecipeRequestID, _).

% players that were previously assigned to RecipeRequestID
% but are no longer assigned to it
strat_RecipeRequest_Player_Removed(RecipeRequestID, PlayerID) :-
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID),
    strat_RecipeRequest_Player_Added(RecipeRequestID1, PlayerID),
    RecipeRequestID != RecipeRequestID1.
    

strat_recipeRequest_Player_Added_Count(RecipeRequestID, Count) :-
    recipeRequest_ID(RecipeRequestID),
    strat_RecipeRequest_AnyPlayer_Added(RecipeRequestID),
    Count = #count{ PlayerID1 : 
        strat_RecipeRequest_Player_Added(RecipeRequestID, PlayerID1)
    }.
    
strat_recipeRequest_Player_Added_Count(RecipeRequestID, 0) :-
    recipeRequest_ID(RecipeRequestID),
    not strat_RecipeRequest_AnyPlayer_Added(RecipeRequestID).

strat_recipeRequest_Player_Removed_Count(RecipeRequestID, Count) :-
    recipeRequest_ID(RecipeRequestID),
    strat_RecipeRequest_AnyPlayer_Removed(RecipeRequestID),
    Count = #count{ PlayerID1 : 
        strat_RecipeRequest_Player_Removed(RecipeRequestID, PlayerID1)
    }.

strat_recipeRequest_Player_Removed_Count(RecipeRequestID, 0) :-
    recipeRequest_ID(RecipeRequestID),
    not strat_RecipeRequest_AnyPlayer_Removed(RecipeRequestID).


% ============================ FINAL PLAYER COUNT ============================

% the number of players on this recipe request after all changes have been made
strat_RecipeRequest_Player_Final_Count(RecipeRequestID, Count) :-
    recipeRequest_Player_Count(RecipeRequestID, BaseCount),
    strat_recipeRequest_Player_Added_Count(RecipeRequestID, AddedCount),
    strat_recipeRequest_Player_Removed_Count(RecipeRequestID, RemovedCount),
    Count = BaseCount + AddedCount - RemovedCount.

% the number of players on the recipe request with 
% the most players after all changes have been made    
strat_Max_RecipeRequest_Player_Final_Count(Count) :-
    Count = #max{ Count1 : 
        strat_RecipeRequest_Player_Final_Count(RecipeRequestID, Count1)
    }.

% the number of players on the recipe request with
% the least players after all changes have been made
strat_Min_RecipeRequest_Player_Final_Count(Count) :-
    Count = #min{ Count1 : 
        strat_RecipeRequest_Player_Final_Count(RecipeRequestID, Count1)
    }.

% the difference between the number of players on the recipe request with
% the most players and the recipe request with the least players
strat_RecipeRequest_Player_Final_Diff(Diff) :-
    strat_Max_RecipeRequest_Player_Final_Count(MaxCount),
    strat_Min_RecipeRequest_Player_Final_Count(MinCount),
    Diff = MaxCount - MinCount.

#show strat_recipeRequest_Player_Diff/1.
#show strat_Max_recipeRequest_Player_Count/1.
#show strat_Min_recipeRequest_Player_Count/1.
#show strat_RecipeRequest_Player_Final_Count/2.
#show strat_RecipeRequest_Player_Final_Diff/1.
#show strat_Max_RecipeRequest_Player_Final_Count/1.
#show strat_Min_RecipeRequest_Player_Final_Count/1.
