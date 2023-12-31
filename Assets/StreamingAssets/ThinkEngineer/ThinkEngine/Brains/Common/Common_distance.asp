tmp_Player_Counter_Distance(PlayerID, CounterID, DistanceX, DistanceY) :-
    player_Pos(PlayerID, PX, PY),
    counter_Pos(CounterID, CX, CY),
    DistanceX = PX - CX,
    DistanceY = PY - CY.

player_Counter_Distance(PlayerID, CounterID, Distance) :-
    tmp_Player_Counter_Distance(PlayerID, CounterID, DistanceX, DistanceY),
    c_abs(DistanceX, DistanceXAbs),
    c_abs(DistanceY, DistanceYAbs), 
    c_Distance(DistanceXAbs, DistanceYAbs, Distance).

curr_Player_Counter_Distance(CounterID, Distance) :-
    curr_Player_ID(PlayerID),
    player_Counter_Distance(PlayerID, CounterID, Distance).

counter_Distance(CounterID1, CounterID2, Distance) :-
    counter_Pos(CounterID1, X1, Y1),
    counter_Pos(CounterID2, X2, Y2),
    DistanceX = X1 - X2,
    DistanceY = Y1 - Y2,
    c_abs(DistanceX, DistanceXAbs),
    c_abs(DistanceY, DistanceYAbs), 
    c_Distance(DistanceXAbs, DistanceYAbs, Distance),
    CounterID1 > CounterID2.

counter_Distance(CounterID1, CounterID2, Distance) :- counter_Distance(CounterID2, CounterID1, Distance).

#show debug_dist_0_Player_Counter/6.

debug_dist_0_Player_Counter(PlayerID, CounterID, CounterType, CounterName, PosX, PosY) :-
    player_Counter_Distance(PlayerID, CounterID, Distance),
    counter(CounterID, CounterType, CounterName),
    counter_Pos(CounterID, PosX, PosY),    
    Distance = 0.
