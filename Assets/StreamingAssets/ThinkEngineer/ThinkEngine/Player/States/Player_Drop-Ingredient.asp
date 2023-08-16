% ========================== STATE DROP INGREDIENT ==========================

stateDI_Target(TargetID) :-
    state_DropIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, KitchenObjectType),
    containerCounter_KOType(TargetID, KitchenObjectType).
    
stateDI_Target(TargetID) :-
    state_DropIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, KitchenObjectType),
    not containerCounter_KOType(TargetID, KitchenObjectType),
    trashCounter_ID(TargetID).
    
a_Place(ActionIndex, TargetID) :-
    state_DropIngredient,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    stateDI_Target(TargetID).

a_Wait(ActionIndex) :-
    state_DropIngredient,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).


