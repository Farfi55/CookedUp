% ========================== STATE DELIVER ==========================

deliver_Target(TargetID) :-
    state_Deliver,
    deliveryCounter_ID(TargetID),
    counter_HasSpace(TargetID).

a_Place(ActionIndex, TargetID) :-
    state_Deliver,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    deliver_Target(TargetID).

a_Wait(ActionIndex) :-
    state_Deliver,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).

