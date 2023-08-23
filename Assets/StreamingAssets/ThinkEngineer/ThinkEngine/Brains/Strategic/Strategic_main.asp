
strat_Any_RecipeRequest() :-
    recipeRequest(_,_).


strat_Must_Assign_Player(PlayerID) :- 
    playerBot_HasNoRecipeRequest(PlayerID).

strat_Eligible_Player(PlayerID) :-
    strat_Must_Assign_Player(PlayerID).

strat_Eligible_Player(PlayerID) :-
    recipeRequest_HasPlayer(RecipeRequestID),
    recipeRequest_Player(RecipeRequestID, PlayerID),
    not strat_RecipeRequest_BestPlayer(RecipeRequestID, PlayerID).



strat_RecipeRequest_BestPlayer(RecipeRequestID, PlayerID) :-
    recipeRequest(RecipeRequestID, RecipeName),
    recipeRequest_HasPlayer(RecipeRequestID),
    BestExpectedTime = #min{ ExpectedTime1 :  
        player_Recipe_ExpectedTime(PlayerID1, RecipeName, ExpectedTime1)
    },
    PlayerID = #min{ PlayerID2 :  
        player_Recipe_ExpectedTime(PlayerID2, RecipeName, BestExpectedTime)
    }.

strat_Eligible_Player_Count(Count) :-
    Count = #count{ PlayerID : strat_Eligible_Player(PlayerID) }.

strat_Eligible_RecipeRequest(RecipeRequestID) :-
    recipeRequest_HasNoPlayer(RecipeRequestID).

strat_Eligible_RecipeRequest(RecipeRequestID) :-
    strat_Min_recipeRequest_Player_Count(MinCount),
    recipeRequest_Player_Count(RecipeRequestID, MinCount),
    strat_Max_recipeRequest_Player_Count(MaxCount),
    MinCount <= MaxCount.

strat_Eligible_RecipeRequest_Count(Count) :-
    Count = #count{RecipeRequestID : strat_Eligible_RecipeRequest(RecipeRequestID)}.



{ strat_RecipeRequest_Player_Added(RecipeRequestID, PlayerID) : 
    strat_Eligible_Player(PlayerID) } :-
    strat_Eligible_RecipeRequest(RecipeRequestID).

strat_Assigned_Player(PlayerID) :- 
    strat_RecipeRequest_Player_Added(_, PlayerID).

strat_Assigned_RecipeRequest(RecipeRequestID) :-
    strat_RecipeRequest_Player_Added(RecipeRequestID, _).

:- strat_Must_Assign_Player(PlayerID),
    not strat_Assigned_Player(PlayerID).




% there can be only one recipe request assigned to a player
:- strat_Assigned_Player(PlayerID),
    #count{RecipeRequestID : strat_RecipeRequest_Player_Added(RecipeRequestID, PlayerID)} != 1.

% player cant be assigned to the same recipe request he already has
:- strat_RecipeRequest_Player_Added(RecipeRequestID, PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).

% action index logic, based on player id
% which ensures that every action index is assigned to a different player id
% and vice versa

tmp_strat_Player_AllPrev(PlayerID, PrevPlayerID) :-
    strat_Assigned_Player(PlayerID),
    strat_Assigned_Player(PrevPlayerID),
    PrevPlayerID < PlayerID.

strat_PlayerID_Prev(PlayerID, PrevPlayerID) :-
    strat_Assigned_Player(PlayerID),
    PrevPlayerID = #max{ PrevPlayerID1 : 
        tmp_strat_Player_AllPrev(PlayerID, PrevPlayerID1) 
    }.  


strat_ActionIndex_PlayerID(0, PlayerID) :-
    strat_Assigned_Player(_),
    PlayerID = #min{ PlayerID1 : 
        strat_Assigned_Player(PlayerID1) 
    }.

strat_ActionIndex_PlayerID(ActionIndex, PlayerID) :-
    ActionIndex = PrevActionIndex + 1,
    strat_ActionIndex_PlayerID(PrevActionIndex, PrevPlayerID),
    strat_PlayerID_Prev(PlayerID, PrevPlayerID).


a_SetRecipeRequest(ActionIndex, PlayerID, RecipeRequestID) :-
    strat_ActionIndex_PlayerID(ActionIndex, PlayerID),
    strat_RecipeRequest_Player_Added(RecipeRequestID, PlayerID).



% ================================== WEAK CONSTRAINTS ==================================

% in order of priority, from most important to least important

% :~ strat_Must_Assign_Player(PlayerID),
%     not strat_Assigned_Player(PlayerID).
%     [1@11, PlayerID]

% improvement on the distribution of recipe requests among players
:~ strat_recipeRequest_Player_Diff(InitDiff),
    strat_RecipeRequest_Player_Final_Diff(FinalDiff),
    FinalDiff < InitDiff.
    [1@10]

% distribution of recipe requests among players same as initial
:~ strat_recipeRequest_Player_Diff(InitDiff),
    strat_RecipeRequest_Player_Final_Diff(FinalDiff),
    FinalDiff = InitDiff.
    [2@10]

% worsening on the distribution of recipe requests among players
:~ strat_recipeRequest_Player_Diff(InitDiff),
    strat_RecipeRequest_Player_Final_Diff(FinalDiff),
    FinalDiff > InitDiff.
    [3@10]





% if there's not enough time to complete a recipe prioritize other recipesRequests
:~ recipeRequest_RemainingTimeToComplete(RecipeRequestID, RemainingTimeToComplete),
    recipeRequest(RecipeRequestID, RecipeName),
    player_Recipe_ExpectedTime(PlayerID, RecipeName, ExpectedTime),
    strat_RecipeRequest_Player_Added(RecipeRequestID, PlayerID),
    RemainingTimeToComplete < ExpectedTime,
    Cost = ExpectedTime - RemainingTimeToComplete.
    [Cost@8, RecipeRequestID, PlayerID] 



% prioritize recipe request with the highest value
:~ strat_Assigned_RecipeRequest(RecipeRequestID),
    recipeRequest_Value(RecipeRequestID, Value),
    Cost = -Value.
    [Cost@5, RecipeRequestID]


% prioritize recipe request with less remaining time to complete
% aka the most urgent recipe requests
:~ strat_Assigned_RecipeRequest(RecipeRequestID),
    recipeRequest_RemainingTimeToComplete(RecipeRequestID, RemainingTimeToComplete),
    Cost = RemainingTimeToComplete.
    [Cost@5, RecipeRequestID]



#show applyAction/2.
#show actionArgument/3.
#show a_SetRecipeRequest/3.
#show strat_Eligible_RecipeRequest_Count/1.
#show strat_Eligible_RecipeRequest/1.
#show strat_Eligible_Player_Count/1.
#show strat_Eligible_Player/1.
#show strat_RecipeRequest_Player_Added/2.
#show strat_RecipeRequest_Player_Removed/2.
#show strat_Must_Assign_Player/1.

#show player/3.
#show playerBot_ID/1.
#show playerBot_HasNoRecipeRequest/1.
#show playerBot_HasRecipeRequest/1.
#show playerBot_RecipeRequest_ID/2.

#show recipeRequest_Player_Count/2.
#show recipeRequest/2.




% % if a player has a plate which contains ingredients that are not required for the recipe,
% % then try to assign the recipe request to another player 
% :~ strat_RecipeRequest_Player_Added(RecipeRequestID, PlayerID),
%     recipeRequest(RecipeRequestID, RecipeName),
%     playerBot_HasInvalidIngredients_ForRecipe(PlayerID, RecipeName). 
%     [1@5, RecipeRequestID, PlayerID, RecipeName]

