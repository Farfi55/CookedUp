tmp_Player_Counter_Distance(PlayerID, CounterID, DistanceX, DistanceY) :-
    player_Pos(PlayerID, PX, PY),
    counter_Pos(CounterID, CX, CY),
    DistanceX = PX - CX,
    DistanceY = PY - CY.

player_Counter_Distance(PlayerID, CounterID, Distance) :-
    tmp_Player_Counter_Distance(PlayerID, CounterID, DistanceX, DistanceY),
    &abs(DistanceX; DistanceXAbs), % maybe this is DLV specific, not sure
    &abs(DistanceY; DistanceYAbs), 
    Distance = DistanceXAbs + DistanceYAbs.

counter_Distance(CounterID1, CounterID2, Distance) :-
    counter_Pos(CounterID1, X1, Y1),
    counter_Pos(CounterID2, X2, Y2),
    DistanceX = X1 - X2,
    DistanceY = Y1 - Y2,
    &abs(DistanceX; DistanceXAbs),
    &abs(DistanceY; DistanceYAbs), 
    Distance = DistanceXAbs + DistanceYAbs,
    CounterID1 > CounterID2.

counter_Distance(CounterID1, CounterID2, Distance) :- counter_Distance(CounterID2, CounterID1, Distance).

#show counter_Distance/3.
