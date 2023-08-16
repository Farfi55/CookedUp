% ========================== STATE GET INGREDIENTS ==========================


% STEP 1: 
% select final ingredient to add to plate

#show gi_FinalIngredient/1.
#show gi_BaseIngredient/1.
#show gi_State/1.

{ gi_FinalIngredient(IngredientName) } <= 1 :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_HasNone(PlayerID),
    playerBot_MissingIngredients(PlayerID, IngredientName).

:-  state_GetIngredient,
    #count{IngredientName : gi_FinalIngredient(IngredientName)} != 1.

% if player is holding an ingredient, then that determines the final ingredient
gi_FinalIngredient(IngredientName) :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_KitchenObject(PlayerID, _, IngredientName),
    ingredient_Available(IngredientName),
    playerBot_MissingIngredients(PlayerID, IngredientName).

gi_FinalIngredient(IngredientName) :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_KitchenObject(PlayerID, _, BaseIngredientName),
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
    ingredient_Available(IngredientName).

gi_IngredientAvailability("NeedsCooking") :-
    state_GetIngredient,
    gi_FinalIngredient(IngredientName),
    ingredient_NeedsCooking(IngredientName, _, _).

gi_IngredientAvailability("NeedsCutting") :-
    state_GetIngredient,
    gi_FinalIngredient(IngredientName),
    ingredient_NeedsCutting(IngredientName, _, _).


gi_State("PickUp BaseIngredient") :- 
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_HasNone(PlayerID),
    not gi_IngredientAvailability("Available"),
    not gi_Any_WorkCounter_HasCurrent_BaseIngredient.


gi_State("Place BaseIngredient") :-
    state_GetIngredient,
    gi_BaseIngredient(BaseIngredientName),
    curr_Player_ID(PlayerID),
    player_HasAny(PlayerID),
    player_KitchenObject(PlayerID, _, BaseIngredientName).



% TODO: 
% this should be done only if the curr_Player was the one who placed the ingredient on the counter
gi_State("Work On BaseIngredient") :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    player_HasNone(PlayerID),
    not gi_IngredientAvailability("Available"),
    gi_Any_WorkCounter_HasCurrent_BaseIngredient.

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
    player_KitchenObject(PlayerID, _, IngredientName).


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




gi_StoveCounter_HasAny_Ingredient_Name(CounterID, IngredientName) :-
    state_GetIngredient,
    stoveCounter_HasAny(CounterID),
    counter_KitchenObject(CounterID, _, IngredientName).

gi_CuttingCounter_HasAny_Ingredient_Name(CounterID, IngredientName) :-
    state_GetIngredient,
    cuttingCounter_HasAny(CounterID),
    counter_KitchenObject(CounterID, _, IngredientName).

gi_WorkCounter_HasAny_Ingredient_Name(CounterID, IngredientName) :-
    gi_StoveCounter_HasAny_Ingredient_Name(CounterID, IngredientName).

gi_WorkCounter_HasAny_Ingredient_Name(CounterID, IngredientName) :-
    gi_CuttingCounter_HasAny_Ingredient_Name(CounterID, IngredientName).

gi_WorkCounter_HasCurrent_BaseIngredient(CounterID) :-
    gi_WorkCounter_HasAny_Ingredient_Name(CounterID, BaseIngredientName),
    gi_BaseIngredient(BaseIngredientName).

gi_WorkCounter_HasCurrent_FinalIngredient(CounterID) :-
    gi_WorkCounter_HasAny_Ingredient_Name(CounterID, IngredientName),
    gi_FinalIngredient(IngredientName).

gi_Any_WorkCounter_HasCurrent_BaseIngredient :-
    gi_WorkCounter_HasCurrent_BaseIngredient(_).

gi_Any_WorkCounter_HasCurrent_FinalIngredient :-
    gi_WorkCounter_HasCurrent_FinalIngredient(_).
    



% needs cooking or cutting
tmp_gi_BaseIngredient_Target(TargetID) :-
    state_GetIngredient,
    not gi_IngredientAvailability("Available"),
    gi_BaseIngredient(BaseIngredientName),
    ingredient_Available_Target(BaseIngredientName, TargetID).

gi_BaseIngredient_Target(TargetID) :-
    state_GetIngredient,
    tmp_gi_BaseIngredient_Target(_),
    TargetID = #max{TargetID1 : tmp_gi_BaseIngredient_Target(TargetID1)}.


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
    counter_HasAny(TargetID),
    gi_WorkCounter_HasCurrent_BaseIngredient(TargetID).

tmp_gi_WorkCounter_Target(TargetID) :-
    state_GetIngredient,
    not gi_Must_PlaceBaseIngredient,
    gi_IngredientAvailability("NeedsCutting"),
    cuttingCounter_ID(TargetID),
    gi_WorkCounter_HasCurrent_BaseIngredient(TargetID).


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

tmp_gi_FinalIngredient_Target(TargetID) :-
    state_GetIngredient,
    gi_IngredientAvailability("Available"),
    gi_FinalIngredient(IngredientName),
    ingredient_Available_Target(IngredientName, TargetID).


tmp_gi_FinalIngredient_Target(TargetID) :-
    state_GetIngredient,
    not gi_IngredientAvailability("Available"),
    gi_WorkCounter_Target(TargetID).

gi_FinalIngredient_Target(TargetID) :-
    state_GetIngredient,
    tmp_gi_FinalIngredient_Target(_),
    TargetID = #max{TargetID1 : tmp_gi_FinalIngredient_Target(TargetID1)}.


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
a_WaitToCook(ActionIndex, TargetID) :-
    state_GetIngredient,
    gi_Must_WorkOnBaseIngredient,
    gi_IngredientAvailability("NeedsCooking"),
    ActionIndex = FirstActionIndex + 2,
    gi_FirstActionIndex(FirstActionIndex),
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
    ActionIndex = FirstActionIndex + 3,
    gi_FirstActionIndex(FirstActionIndex),
    gi_FinalIngredient(IngredientName),
    gi_FinalIngredient_Target(TargetID).


% ACTION 5:
% add final ingredient to plate

a_Place(ActionIndex, TargetID) :- 
    state_GetIngredient,
    gi_Must_AddFinalIngredientToPlate,
    ActionIndex = FirstActionIndex + 4,
    gi_FirstActionIndex(FirstActionIndex),
    gi_Plate_Target(TargetID).


% EXTRA ACTION:
% wait

a_Wait(ActionIndex) :-
    state_GetIngredient,
    ActionIndex = FirstActionIndex + 5,
    gi_FirstActionIndex(FirstActionIndex).


