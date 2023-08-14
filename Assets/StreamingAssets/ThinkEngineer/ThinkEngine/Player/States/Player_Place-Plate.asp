% ========================== STATE PLACE PLATE ==========================

state_PlacePlate__Target(TargetID) :-
    state_PlacePlate,
    clearCounter_ID(TargetID),
    counter_HasSpace(TargetID).


a_Place(ActionIndex, TargetID) :-
    state_PlacePlate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    state_PlacePlate__Target(TargetID).

a_Wait(ActionIndex) :-
    state_PlacePlate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).
