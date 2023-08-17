% ========================== STATE PLACE PLATE ==========================

% TODO: come up with a better counter target strategy, 
% maybe using distance between counters that will be used for the recipe 

tmp_pp_Target(TargetID) :-
    state_PlacePlate,
    clearCounter_HasSpace(TargetID).

pp_Target(TargetID) :-
    tmp_pp_Target(_),
    % take the closest one
    TargetDistance = #min{ Dist :   
        curr_Player_Counter_Distance(TargetID1, Dist), 
        tmp_pp_Target(TargetID1)
    },
    % if there are multiple at the same distance, take the one with the highest ID 
    TargetID = #max{ TargetID2 :    
        curr_Player_Counter_Distance(TargetID2, TargetDistance), 
        tmp_pp_Target(TargetID2)
    }.



a_Place(ActionIndex, TargetID) :-
    state_PlacePlate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    pp_Target(TargetID).

a_Wait(ActionIndex) :-
    state_PlacePlate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).
