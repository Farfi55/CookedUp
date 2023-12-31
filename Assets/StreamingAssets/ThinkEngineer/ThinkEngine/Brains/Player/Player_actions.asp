% ================================== Move To ==================================

% a_MoveTo_Target(ActionIndex, TargetID).
% a_MoveTo_Pos(ActionIndex, GridX,GridY).
#show a_MoveTo_Target/2.
#show a_MoveTo_Pos/3.


a_MoveTo_Generic(ActionIndex) :- 
    a_MoveTo_Target(ActionIndex, _).

a_MoveTo_Generic(ActionIndex) :- 
    a_MoveTo_Pos(ActionIndex, GridX, GridY).


applyAction(ActionIndex, "MoveToAction") :- a_MoveTo_Generic(ActionIndex).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_MoveTo_Generic(ActionIndex), curr_Player_ID(PlayerID).

% using a_MoveTo_Target
    actionArgument(ActionIndex, "TargetID", TargetID) :- a_MoveTo_Target(ActionIndex, TargetID).


% using a_MoveTo_Pos
    actionArgument(ActionIndex, "GridX", GridX) :- a_MoveTo_Pos(ActionIndex, GridX, _).
    actionArgument(ActionIndex, "GridY", GridY) :- a_MoveTo_Pos(ActionIndex, _, GridY).


% ================================== Pick Up ==================================

% a_PickUp(ActionIndex, TargetInteractableID).
#show a_PickUp/2.

applyAction(ActionIndex, "PickUpAction") :- 
    a_PickUp(ActionIndex, _).


actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_PickUp(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_PickUp(ActionIndex, TargetID).


% ================================== Pick Up Ingredient ==================================

% a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID).
% a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName).
#show a_PickUpIngredient/3.

a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID) :-
    a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName),
    curr_Player_ID(PlayerID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).

applyAction(ActionIndex, "PickUpIngredientAction") :- 
    a_PickUpIngredient(ActionIndex, _, _, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_PickUpIngredient(ActionIndex, _, _, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_PickUpIngredient(ActionIndex, TargetID, _, _).
actionArgument(ActionIndex, "IngredientName", IngredientName) :- a_PickUpIngredient(ActionIndex, _, IngredientName, _).
actionArgument(ActionIndex, "RecipeRequestID", RecipeRequestID) :- a_PickUpIngredient(ActionIndex, _, _, RecipeRequestID).


% ================================== Pick Up Ingredient To Cook ==================================

% a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID).
% a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName).
#show a_PickUpIngredient_ToCook/3.

a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID) :-
    a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName),
    curr_Player_ID(PlayerID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).

a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID) :-
    a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID).

actionArgument(ActionIndex, "RequiresCooking", true) :- a_PickUpIngredient_ToCook(ActionIndex, _, _, _).


% ================================== Pick Up Ingredient To Cook ==================================

% a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID).
% a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName).
#show a_PickUpIngredient_ToCut/3.

a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID) :-
    a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName),
    curr_Player_ID(PlayerID),
    playerBot_RecipeRequest_ID(PlayerID, RecipeRequestID).

a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID) :-
    a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName, RecipeRequestID).

actionArgument(ActionIndex, "RequiresCutting", true) :- a_PickUpIngredient_ToCut(ActionIndex, _, _, _).


% ================================== Drop ==================================

% a_Drop(ActionIndex, TargetInteractableID).
% a_Place(ActionIndex, TargetInteractableID).
#show a_Drop/2.

a_Drop(ActionIndex, TargetInteractableID) :- 
    a_Place(ActionIndex, TargetInteractableID).

a_Place(ActionIndex, TargetInteractableID) :- 
    a_Drop(ActionIndex, TargetInteractableID).

applyAction(ActionIndex, "DropAction") :- 
    a_Drop(ActionIndex, _).


actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Drop(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_Drop(ActionIndex, TargetID).


% ================================== Cut ==================================

% a_Cut(ActionIndex, TargetInteractableID).
#show a_Cut/2.

applyAction(ActionIndex, "CutAction") :-  
    a_Cut(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Cut(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_Cut(ActionIndex, TargetID).


% ================================== Cook ==================================

% a_WaitToCook(ActionIndex, TargetInteractableID, FinalIngredient).
#show a_WaitToCook/3.

applyAction(ActionIndex, "WaitToCookAction") :- 
    a_WaitToCook(ActionIndex, _, _).


actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_WaitToCook(ActionIndex, _, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_WaitToCook(ActionIndex, TargetID, _).
actionArgument(ActionIndex, "FinalIngredient", FinalIngredient) :- a_WaitToCook(ActionIndex, _, FinalIngredient).


% ================================== Set Plate ==================================

% a_SetPlate(ActionIndex, PlateID).
#show a_SetPlate/2.

applyAction(ActionIndex, "SetPlateAction") :- 
    a_SetPlate(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_SetPlate(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "PlateID", PlateID) :- a_SetPlate(ActionIndex, PlateID).


% ================================== Wait ==================================

% a_Wait(ActionIndex, MilliSecondsToWait).
% a_Wait(ActionIndex).
#show a_Wait/2.

a_Wait_DefaultMilliSecondsToWait(10).

a_Wait(ActionIndex, MilliSecondsToWait) :- 
    a_Wait_DefaultMilliSecondsToWait(MilliSecondsToWait),
    a_Wait(ActionIndex).

applyAction(ActionIndex, "WaitAction") :- 
    a_Wait(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Wait(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TimeToWait", MilliSecondsToWait) :- a_Wait(ActionIndex, MilliSecondsToWait).


% ================================== Set State Name ==================================

% a_SetStateName(ActionIndex, StateName).
#show a_SetStateName/2.

applyAction(ActionIndex, "SetStateNameAction") :- 
    a_SetStateName(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_SetStateName(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "StateName", StateName) :- a_SetStateName(ActionIndex, StateName).


