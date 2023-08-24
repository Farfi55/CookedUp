% ========================== STATE RECIPE FAILED ==========================

#show rf_State/1.
#show playerBot_HasRecipeRequest/1.
#show playerBot_HasNoRecipeRequest/1.

rf_State("Plate On Delivery Counter") :-
    state_Recipe_Failed,
    curr_Player_ID(PlayerID),
    ko_Player_ID(KitchenObjectID, PlayerID),
    not playerBot_IsPlateBeingCarried(PlayerID),
    ko_HasOwnerContainer(KitchenObjectID),
    ko_OwnerContainer_ID(KitchenObjectID, DeliveryCounterID),
    deliveryCounter_ID(DeliveryCounterID).

rf_State("Recipe Request Expired") :-
    state_Recipe_Failed,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasNoRecipeRequest(PlayerID).

rf_State("Plate Has Invalid Ingredients") :-
    state_Recipe_Failed,
    curr_Player_ID(PlayerID),
    playerBot_HasPlate(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_HasInvalidIngredients(PlayerID).
    



rf_Must_PickUp_OldPlate :- rf_State("Plate On Delivery Counter").

rf_Must_Place_OldPlate :- rf_Must_PickUp_OldPlate.
rf_Must_Place_OldPlate :- rf_State("Recipe Request Expired"), playerBot_IsPlateBeingCarried(PlayerID), curr_Player_ID(PlayerID).
rf_Must_Place_OldPlate :- rf_State("Plate Has Invalid Ingredients"), playerBot_IsPlateBeingCarried(PlayerID), curr_Player_ID(PlayerID).

% rf_Must_PickUp_NewPlate :- rf_Must_Place_OldPlate.
rf_Must_PickUp_NewPlate :- state_Recipe_Failed.


tmp_rf_FirstActionIndex(Index) :-
    Index = BaseFirstActionIndex - 0,
    firstActionIndex(BaseFirstActionIndex).

tmp_rf_FirstActionIndex(Index) :-
    Index = BaseFirstActionIndex - 1,
    firstActionIndex(BaseFirstActionIndex),
    not rf_Must_PickUp_OldPlate.

tmp_rf_FirstActionIndex(Index) :-
    Index = BaseFirstActionIndex - 2,
    firstActionIndex(BaseFirstActionIndex),
    not rf_Must_Place_OldPlate.

rf_FirstActionIndex(Index) :-
    state_Recipe_Failed,
    Index = #min{TmpIndex : tmp_rf_FirstActionIndex(TmpIndex)}.


rf_Any_AvailableCounter :- 
    clearCounter_HasSpace(TargetID).

rf_Any_AvailablePlates :- 
    platesCounter_ID(PlatesCounterID),
    counter_HasAny(PlatesCounterID).

rf_PickUp_OldPlate_Target(TargetID) :- 
    state_Recipe_Failed,
    rf_Must_PickUp_OldPlate,
    rf_State("Plate On Delivery Counter"),
    ko_Curr_Player(KitchenObjectID),
    ko_HasOwnerContainer(KitchenObjectID),
    ko_OwnerContainer_ID(KitchenObjectID, DeliveryCounterID),
    deliveryCounter_ID(DeliveryCounterID),
    TargetID = DeliveryCounterID.


tmp_rf_Place_OldPlate_Target(TargetID) :- 
    state_Recipe_Failed,
    rf_Must_Place_OldPlate,
    rf_Any_AvailableCounter,
    clearCounter_HasSpace(TargetID).

tmp_rf_Place_OldPlate_Target(TargetID) :- 
    state_Recipe_Failed,
    rf_Must_Place_OldPlate,
    not rf_Any_AvailableCounter,
    trashCounter_ID(TargetID).

rf_Place_OldPlate_Target(TargetID) :-
    state_Recipe_Failed,
    tmp_rf_Place_OldPlate_Target(_),
    TargetID = #max{TargetID1 : tmp_rf_Place_OldPlate_Target(TargetID1)}.


tmp_rf_PickUp_NewPlate_Target(TargetID) :- 
    state_Recipe_Failed,
    rf_Must_PickUp_NewPlate,
    rf_Any_AvailablePlates,
    platesCounter_ID(TargetID),
    counter_HasAny(TargetID),
    PlatesCount = #max{PlatesCount1 : platesCounter_Count(_, PlatesCount1)},
    platesCounter_Count(TargetID, PlatesCount).


tmp_rf_PickUp_NewPlate_Target(TargetID) :- 
    state_Recipe_Failed,
    rf_Must_PickUp_NewPlate,
    not rf_Any_AvailablePlates,
    platesCounter_ID(TargetID),
    TimeToNextPlate = #max{TimeToNextPlate1 : platesCounter_TimeToNextPlate(_, TimeToNextPlate1)},
    platesCounter_TimeToNextPlate(TargetID, TimeToNextPlate).
    
rf_PickUp_NewPlate_Target(TargetID) :- 
    state_Recipe_Failed,
    tmp_rf_PickUp_NewPlate_Target(_),
    TargetID = #max{TargetID1 : tmp_rf_PickUp_NewPlate_Target(TargetID1)}.


% Action 1: 
% PickUp old plate
a_PickUp(ActionIndex, TargetID) :-
    state_Recipe_Failed,
    rf_Must_PickUp_OldPlate,
    ActionIndex = FirstActionIndex,
    rf_FirstActionIndex(FirstActionIndex),
    rf_PickUp_OldPlate_Target(TargetID).

% Action 2:
% Place old plate
a_Place(ActionIndex, TargetID) :-
    state_Recipe_Failed,
    rf_Must_Place_OldPlate,
    ActionIndex = FirstActionIndex + 1,
    rf_FirstActionIndex(FirstActionIndex),
    rf_Place_OldPlate_Target(TargetID).

%TODO: 
% change this with a a_SetPlate(ActionIndex, 0).
% 0 meaning no plate
% so that this will reutilize the pick up plate state insted

% Action 3:
% PickUp new plate
a_PickUp(ActionIndex, TargetID) :-
    state_Recipe_Failed,
    rf_Must_PickUp_NewPlate,
    ActionIndex = FirstActionIndex + 2,
    rf_FirstActionIndex(FirstActionIndex),
    rf_PickUp_NewPlate_Target(TargetID).


a_Wait(ActionIndex) :-
    state_Recipe_Failed,
    ActionIndex = FirstActionIndex + 3,
    rf_FirstActionIndex(FirstActionIndex).
