% ========================== STATE GET INGREDIENTS ==========================


% STEP 1: 
% select final ingredient to add to plate

#show gi_FinalIngredient/1.
#show gi_BaseIngredient/1.
#show gi_IngredientAvailability/1.
#show gi_State/1.

{ gi_FinalIngredient(IngredientName) } <= 1 :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_HasNone(PlayerID),
    playerBot_MissingIngredients(PlayerID, IngredientName).

:- state_GetIngredient,
    #count{IngredientName : gi_FinalIngredient(IngredientName)} != 1.


% if player is holding an ingredient, then that determines the final ingredient
gi_FinalIngredient(IngredientName) :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, IngredientName),
    playerBot_MissingIngredients(PlayerID, IngredientName).

gi_FinalIngredient(IngredientName) :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, BaseIngredientName),
    ingredient_NeedsWork(IngredientName, _, BaseIngredientName),
    playerBot_MissingIngredients(PlayerID, IngredientName).

gi_BaseIngredient(BaseIngredientName) :-
    state_GetIngredient,
    gi_FinalIngredient(FinalIngredientName),
    not gi_IngredientAvailability("Available"),
    ingredient_NeedsWork(FinalIngredientName, _, BaseIngredientName).


gi_IngredientAvailability("Available") :- 
    state_GetIngredient,
    gi_FinalIngredient(IngredientName),
    curr_Player_ID(PlayerID),
    ingredient_Available_ForPlayer(IngredientName, PlayerID).

gi_IngredientAvailability("NeedsCooking") :-
    state_GetIngredient,
    gi_FinalIngredient(IngredientName),
    curr_Player_ID(PlayerID),
    ingredient_NeedsCooking_ForPlayer(IngredientName, PlayerID, _, _).

gi_IngredientAvailability("NeedsCutting") :-
    state_GetIngredient,
    gi_FinalIngredient(IngredientName),
    curr_Player_ID(PlayerID),
    ingredient_NeedsCutting_ForPlayer(IngredientName, PlayerID, _, _).


gi_State("PickUp BaseIngredient") :- 
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_HasNone(PlayerID),
    not gi_IngredientAvailability("Available"),
    not gi_Any_WorkCounter_HasBaseIngredient.


gi_State("Place BaseIngredient") :-
    state_GetIngredient,
    gi_BaseIngredient(BaseIngredientName),
    curr_Player_ID(PlayerID),
    player_HasAny(PlayerID),
    player_KO_Name(PlayerID, BaseIngredientName).


gi_State("Work On BaseIngredient") :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_HasNone(PlayerID),
    not gi_IngredientAvailability("Available"),
    gi_Any_WorkCounter_HasBaseIngredient.

gi_State("PickUp FinalIngredient") :-    
    state_GetIngredient,
    gi_FinalIngredient(IngredientName),
    curr_Player_ID(PlayerID),
    player_HasNone(PlayerID),
    gi_IngredientAvailability("Available").

gi_State("Add FinalIngredient To Plate") :-
    state_GetIngredient,
    gi_FinalIngredient(IngredientName),
    curr_Player_ID(PlayerID),
    player_HasAny(PlayerID),
    player_KO_Name(PlayerID, IngredientName).


:-  state_GetIngredient, 
    gi_State(State1), 
    gi_State(State2), 
    State1 != State2.


gi_Must_PickUpBaseIngredient  :- gi_State("PickUp BaseIngredient").

gi_Must_PlaceBaseIngredient   :- gi_State("PickUp BaseIngredient").
gi_Must_PlaceBaseIngredient   :- gi_State("Place BaseIngredient").

gi_Must_WorkOnBaseIngredient  :- gi_State("PickUp BaseIngredient").
gi_Must_WorkOnBaseIngredient  :- gi_State("Place BaseIngredient").
gi_Must_WorkOnBaseIngredient  :- gi_State("Work On BaseIngredient").

gi_Must_PickUpFinalIngredient :- gi_State("PickUp BaseIngredient").
gi_Must_PickUpFinalIngredient :- gi_State("Place BaseIngredient").
gi_Must_PickUpFinalIngredient :- gi_State("Work On BaseIngredient").
gi_Must_PickUpFinalIngredient :- gi_State("PickUp FinalIngredient").

gi_Must_AddFinalIngredientToPlate :- state_GetIngredient.


tmp_gi_FirstActionIndex(Index) :- 
    Index = BaseFirstActionIndex - 0,
    firstActionIndex(BaseFirstActionIndex),
    gi_State("PickUp BaseIngredient").

tmp_gi_FirstActionIndex(Index) :- 
    Index = BaseFirstActionIndex - 1,
    firstActionIndex(BaseFirstActionIndex),
    gi_State("Place BaseIngredient").

tmp_gi_FirstActionIndex(Index) :-
    Index = BaseFirstActionIndex - 2,
    firstActionIndex(BaseFirstActionIndex),
    gi_State("Work On BaseIngredient").
    
tmp_gi_FirstActionIndex(Index) :-
    Index = BaseFirstActionIndex - 3,
    firstActionIndex(BaseFirstActionIndex),
    gi_State("PickUp FinalIngredient").

tmp_gi_FirstActionIndex(Index) :-
    Index = BaseFirstActionIndex - 4,
    firstActionIndex(BaseFirstActionIndex),
    gi_State("Add FinalIngredient To Plate").

gi_FirstActionIndex(Index) :-
    state_GetIngredient,
    Index = #min{TmpIndex : tmp_gi_FirstActionIndex(TmpIndex)}.



gi_WorkCounter(CounterID) :-
    state_GetIngredient,
    stoveCounter_ID(CounterID).

gi_WorkCounter(CounterID) :-
    state_GetIngredient,
    cuttingCounter_ID(CounterID).

gi_WorkCounter_HasBaseIngredient(CounterID) :-
    ko(KOID, BaseIngredientName, CounterID),
    ko_Curr_Player(KOID),
    gi_BaseIngredient(BaseIngredientName),
    counter_HasAny(CounterID),
    gi_WorkCounter(CounterID).
    


gi_Any_WorkCounter_HasBaseIngredient :- gi_WorkCounter_HasBaseIngredient(_).
    


% needs cooking or cutting
tmp_gi_BaseIngredient_Target(TargetID) :-
    state_GetIngredient,
    not gi_IngredientAvailability("Available"),
    gi_BaseIngredient(BaseIngredientName),
    curr_Player_ID(PlayerID),
    ingredient_Available_ForPlayer_Target(BaseIngredientName, PlayerID, TargetID).

gi_BaseIngredient_Target(TargetID) :-
    state_GetIngredient,
    tmp_gi_BaseIngredient_Target(_),
    TargetDistance = #min{Distance1 : 
        tmp_gi_BaseIngredient_Target(TargetID1),
        curr_Player_Counter_Distance(TargetID1, Distance1)
    },
    TargetID = #max{TargetID2 : 
        tmp_gi_BaseIngredient_Target(TargetID2),
        curr_Player_Counter_Distance(TargetID2, TargetDistance)
    }.



% when the ingredient needs to be worked find a free work counter
tmp_gi_WorkCounter_Target(TargetID) :-
    state_GetIngredient,
    gi_Must_PlaceBaseIngredient,
    gi_IngredientAvailability("NeedsCooking"),
    stoveCounter_ID(TargetID),
    counter_HasSpace(TargetID).

tmp_gi_WorkCounter_Target(TargetID) :-
    state_GetIngredient,
    gi_Must_PlaceBaseIngredient,
    gi_IngredientAvailability("NeedsCutting"),
    cuttingCounter_ID(TargetID),
    counter_HasSpace(TargetID).

% if we are in a state where the ingredient is already on the counter, 
% then we get the counter with that ingredient on it

tmp_gi_WorkCounter_Target(TargetID) :-
    state_GetIngredient,
    not gi_Must_PlaceBaseIngredient,
    gi_IngredientAvailability("NeedsCooking"),
    stoveCounter_ID(TargetID),
    gi_WorkCounter_HasBaseIngredient(TargetID).

tmp_gi_WorkCounter_Target(TargetID) :-
    state_GetIngredient,
    not gi_Must_PlaceBaseIngredient,
    gi_IngredientAvailability("NeedsCutting"),
    cuttingCounter_ID(TargetID),
    gi_WorkCounter_HasBaseIngredient(TargetID).


gi_WorkCounter_Target(TargetID) :-
    state_GetIngredient,
    tmp_gi_WorkCounter_Target(_),
    Distance = #min{ Distance1 :
        tmp_gi_WorkCounter_Target(TargetID1),
        curr_Player_Counter_Distance(TargetID1, Distance1)
    },
    TargetID = #max{ TargetID2 : 
        tmp_gi_WorkCounter_Target(TargetID2),
        curr_Player_Counter_Distance(TargetID2, Distance)
    }.

gi_Any_WorkCounter :- gi_WorkCounter_Target(_).

% if there are no available work counter
% then try to work on another ingredient
:~ state_GetIngredient, 
    conf_gi_MustHave_WorkCounter,
    gi_Must_WorkOnBaseIngredient, 
    not gi_Any_WorkCounter.
    [1@10]

gi_Any_WorkCounter_IfNeeded :- gi_Any_WorkCounter.
gi_Any_WorkCounter_IfNeeded :- not gi_Must_WorkOnBaseIngredient.

tmp_gi_FinalIngredient_Target(TargetID) :-
    state_GetIngredient,
    gi_IngredientAvailability("Available"),
    gi_FinalIngredient(IngredientName),
    curr_Player_ID(PlayerID),
    ingredient_Available_ForPlayer_Target(IngredientName, PlayerID, TargetID).



tmp_gi_FinalIngredient_Target(TargetID) :-
    state_GetIngredient,
    not gi_IngredientAvailability("Available"),
    gi_WorkCounter_Target(TargetID).

gi_FinalIngredient_Target(TargetID) :-
    state_GetIngredient,
    tmp_gi_FinalIngredient_Target(_),
    TargetDistance = #min{Distance1 : 
        tmp_gi_FinalIngredient_Target(TargetID1),
        curr_Player_Counter_Distance(TargetID1, Distance1)
    },
    TargetID = #max{TargetID2 : 
        tmp_gi_FinalIngredient_Target(TargetID2),
        curr_Player_Counter_Distance(TargetID2, TargetDistance)
    }.


gi_Plate_Target(TargetID) :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    playerBot_Plate_Container_ID(PlayerID, _, TargetID).

% ACTION 1:
% get base ingredient
a_PickUpIngredient_ToCook(ActionIndex, TargetID, BaseIngredientName) :-
    state_GetIngredient,
    gi_Must_PickUpBaseIngredient,
    gi_IngredientAvailability("NeedsCooking"),
    ActionIndex = FirstActionIndex,
    gi_FirstActionIndex(FirstActionIndex),
    gi_BaseIngredient_Target(TargetID),
    gi_BaseIngredient(BaseIngredientName).


a_PickUpIngredient_ToCut(ActionIndex, TargetID, BaseIngredientName) :-
    state_GetIngredient,
    gi_Must_PickUpBaseIngredient,
    gi_IngredientAvailability("NeedsCutting"),
    ActionIndex = FirstActionIndex,
    gi_FirstActionIndex(FirstActionIndex),
    gi_BaseIngredient_Target(TargetID),
    gi_BaseIngredient(BaseIngredientName).


% ACTION 2:
% place base ingredient on work counter (stove or cutting counter)
a_Place(ActionIndex, TargetID) :- 
    state_GetIngredient,
    gi_Must_PlaceBaseIngredient,
    ActionIndex = FirstActionIndex + 1,
    gi_FirstActionIndex(FirstActionIndex),
    gi_WorkCounter_Target(TargetID).


% ACTION 3:
% work on base ingredient

% needs cooking
a_WaitToCook(ActionIndex, TargetID, FinalIngredient) :-
    state_GetIngredient,
    gi_Must_WorkOnBaseIngredient,
    gi_IngredientAvailability("NeedsCooking"),
    ActionIndex = FirstActionIndex + 2,
    gi_FirstActionIndex(FirstActionIndex),
    gi_FinalIngredient(FinalIngredient),
    gi_WorkCounter_Target(TargetID).


% needs cutting
a_Cut(ActionIndex, TargetID) :-
    state_GetIngredient,
    gi_Must_WorkOnBaseIngredient,
    gi_IngredientAvailability("NeedsCutting"),
    ActionIndex = FirstActionIndex + 2,
    gi_FirstActionIndex(FirstActionIndex),
    gi_WorkCounter_Target(TargetID).


% ACTION 4:
% pick up final ingredient

a_PickUpIngredient(ActionIndex, TargetID, IngredientName) :-
    state_GetIngredient,
    gi_Must_PickUpFinalIngredient,
    gi_Any_WorkCounter_IfNeeded,
    ActionIndex = FirstActionIndex + 3,
    gi_FirstActionIndex(FirstActionIndex),
    gi_FinalIngredient(IngredientName),
    gi_FinalIngredient_Target(TargetID).


% ACTION 5:
% add final ingredient to plate

a_Place(ActionIndex, TargetID) :- 
    state_GetIngredient,
    gi_Must_AddFinalIngredientToPlate,
    gi_Any_WorkCounter_IfNeeded,
    ActionIndex = FirstActionIndex + 4,
    gi_FirstActionIndex(FirstActionIndex),
    gi_Plate_Target(TargetID).


% EXTRA ACTION:
% wait

a_Wait(ActionIndex) :-
    state_GetIngredient,
    ActionIndex = FirstActionIndex + 5,
    gi_FirstActionIndex(FirstActionIndex).


