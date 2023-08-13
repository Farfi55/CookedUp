% ========================== STATE PICKUP PLATE ==========================

statePUP_AnyPlate_ReadyToPickUp :-
    state_PickUp_Plate,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).


statePUP_Target(PlateCounterID) :-
    state_PickUp_Plate,
    statePUP_AnyPlate_ReadyToPickUp,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).

statePUP_PlatesCounter(PlateCounterID, TimeToNextPlate) :-
    state_PickUp_Plate,
    not statePUP_AnyPlate_ReadyToPickUp,
    platesCounter_ID(PlateCounterID),
    not counter_HasAny(PlateCounterID),
    platesCounter_TimeToNextPlate(PlateCounterID, TimeToNextPlate).

statePUP_Target(PlateCounterID) :-
    state_PickUp_Plate,
    not statePUP_AnyPlate_ReadyToPickUp,
    TimeToNextPlate = #min{TimeToNextPlate1 : 
        platesCounter_TimeToNextPlate(PlateCounterID1, TimeToNextPlate1)
    },
    statePUP_PlatesCounter(PlateCounterID, TimeToNextPlate).


a_PickUp(ActionIndex, PlatesCounterID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    statePUP_Target(PlatesCounterID).

a_Wait(ActionIndex) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).
