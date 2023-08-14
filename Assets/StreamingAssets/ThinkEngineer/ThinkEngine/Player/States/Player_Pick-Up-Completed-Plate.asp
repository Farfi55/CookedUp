% ========================== STATE PICK UP COMPLETED PLATE ==========================

statePUCP_Target(TargetID) :-
    state_PickUp_CompletedPlate,
    curr_Player_ID(PlayerID),
    playerBot_Plate_Container_ID(PlayerID, PlateID, TargetID).


a_PickUp(ActionIndex, TargetID) :-
    state_PickUp_CompletedPlate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    statePUCP_Target(TargetID).


a_Wait(ActionIndex) :-
    state_PickUp_CompletedPlate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).
