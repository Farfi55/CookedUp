% ========================== STATE ADD INGREDIENT ==========================

stateAI_Target(TargetID) :-
    state_AddIngredient,
    curr_Player_ID(PlayerID),
    playerBot_Plate_Container_ID(PlayerID, _, TargetID).

    
a_Place(ActionIndex, TargetID) :-
    state_AddIngredient,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    stateAI_Target(TargetID).

a_Wait(ActionIndex) :-
    state_AddIngredient,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).


