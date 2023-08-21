% ========================== STATE DROP INGREDIENT ==========================

di_KeepIngredient :- 
    state_DropIngredient,
    clearCounter_HasSpace(CounterID),
    ingredient_IsTrash(KOName),
    player_KO_Name(PlayerID, KOName).


tmp_di_Target(TargetID) :-
    state_DropIngredient,
    di_KeepIngredient,
    clearCounter_HasSpace(TargetID).


tmp_di_Target(TargetID) :-
    state_DropIngredient,
    not di_KeepIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, KOName),
    containerCounter_KOType(TargetID, KOName).
    
tmp_di_Target(TargetID) :-
    state_DropIngredient,
    not di_KeepIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, KOName),
    trashCounter_ID(TargetID).


di_Target(TargetID) :-
    state_DropIngredient,
    tmp_di_Target(_),
    TargetDistance = #min{ Distance1 : 
        tmp_di_Target(TargetID1),
        curr_Player_Counter_Distance(TargetID1, Distance1)
    },
    TargetID = #min{ TargetID2 : 
        tmp_di_Target(TargetID2),
        curr_Player_Counter_Distance(TargetID2, TargetDistance)
    }.

a_Place(ActionIndex, TargetID) :-
    state_DropIngredient,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    di_Target(TargetID).

a_Wait(ActionIndex) :-
    state_DropIngredient,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).


