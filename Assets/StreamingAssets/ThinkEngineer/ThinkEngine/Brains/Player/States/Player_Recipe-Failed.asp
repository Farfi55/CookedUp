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

rf_Must_Reset_Plate :- state_Recipe_Failed.


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

% Action 3:
% Set plate to null
a_SetPlate(ActionIndex, 0) :-
    state_Recipe_Failed,
    rf_Must_Reset_Plate,
    ActionIndex = FirstActionIndex + 2,
    rf_FirstActionIndex(FirstActionIndex).


a_Wait(ActionIndex) :-
    state_Recipe_Failed,
    ActionIndex = FirstActionIndex + 3,
    rf_FirstActionIndex(FirstActionIndex).
