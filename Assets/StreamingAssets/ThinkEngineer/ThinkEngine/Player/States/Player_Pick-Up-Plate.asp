% ========================== STATE PICKUP PLATE ==========================

pup_PlatesCounter_HasAny :-
    state_PickUp_Plate,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).

pup_PlatesCounter(PlateCounterID, TimeToNextPlate) :-
    state_PickUp_Plate,
    not pup_PlatesCounter_HasAny,
    platesCounter_ID(PlateCounterID),
    not counter_HasAny(PlateCounterID),
    platesCounter_TimeToNextPlate(PlateCounterID, TimeToNextPlate).

tmp_pup_Target(PlateCounterID) :-
    state_PickUp_Plate,
    pup_PlatesCounter_HasAny,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).

tmp_pup_Target(PlateCounterID) :-
    state_PickUp_Plate,
    not pup_PlatesCounter_HasAny,
    TimeToNextPlate = #min{TimeToNextPlate1 : 
        platesCounter_TimeToNextPlate(PlateCounterID1, TimeToNextPlate1)
    },
    pup_PlatesCounter(PlateCounterID, TimeToNextPlate).

pup_Target(TargetID) :-
    state_PickUp_Plate,
    tmp_pup_Target(_),
    % take the closest one
    TargetDistance = #min{Dist :   
        curr_Player_Counter_Distance(TargetID1, Dist), 
        tmp_pup_Target(TargetID1)
    },
    % if there are multiple at the same distance, take the one with the highest ID 
    TargetID = #max{TargetID2 :    
        curr_Player_Counter_Distance(TargetID2, TargetDistance), 
        tmp_pup_Target(TargetID2)
    }.


a_PickUp(ActionIndex, TargetID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    pup_Target(TargetID).

a_Wait(ActionIndex) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).
