% ========================== STATE PICKUP PLATE ==========================

statePUP_Target(PlateCounterID) :-
    state_PickUp_Plate,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).


a_MoveTo_Target(ActionIndex, PlatesCounterID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    statePUP_Target(PlatesCounterID).


a_PickUp(ActionIndex, PlatesCounterID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex),
    statePUP_Target(PlatesCounterID).

a_Wait(ActionIndex) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex + 2,
    firstActionIndex(FirstActionIndex).
