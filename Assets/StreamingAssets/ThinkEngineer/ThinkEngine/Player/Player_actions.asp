% ================================== Move To ==================================

% a_MoveTo_Target(ActionIndex, TargetID).
% a_MoveTo_Pos(ActionIndex, GridX,GridY).

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

applyAction(ActionIndex, "PickUpAction") :- 
    a_PickUp(ActionIndex, _).


actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_PickUp(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_PickUp(ActionIndex, TargetID).


% ================================== Pick Up Ingredient ==================================

% a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeName).
% a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName).

a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeName) :-
    a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName),
    curr_Player_ID(PlayerID),
    playerBot_Recipe(PlayerID, RecipeName).

applyAction(ActionIndex, "PickUpIngredientAction") :- 
    a_PickUpIngredient(ActionIndex, _, _, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_PickUpIngredient(ActionIndex, _, _, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_PickUpIngredient(ActionIndex, TargetID, _, _).
actionArgument(ActionIndex, "IngredientName", IngredientName) :- a_PickUpIngredient(ActionIndex, _, IngredientName, _).
actionArgument(ActionIndex, "RecipeName", RecipeName) :- a_PickUpIngredient(ActionIndex, _, _, RecipeName).


% ================================== Pick Up Ingredient To Cook ==================================

% a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName, RecipeName).
% a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName).

a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName, RecipeName) :-
    a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName),
    curr_Player_ID(PlayerID),
    playerBot_Recipe(PlayerID, RecipeName).

a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeName) :-
    a_PickUpIngredient_ToCook(ActionIndex, TargetInteractableID, IngredientName, RecipeName).

actionArgument(ActionIndex, "RequiresCooking", true) :- a_PickUpIngredient_ToCook(ActionIndex, _, _, _).


% ================================== Pick Up Ingredient To Cook ==================================

% a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName, RecipeName).
% a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName).

a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName, RecipeName) :-
    a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName),
    curr_Player_ID(PlayerID),
    playerBot_Recipe(PlayerID, RecipeName).

a_PickUpIngredient(ActionIndex, TargetInteractableID, IngredientName, RecipeName) :-
    a_PickUpIngredient_ToCut(ActionIndex, TargetInteractableID, IngredientName, RecipeName).

actionArgument(ActionIndex, "RequiresCutting", true) :- a_PickUpIngredient_ToCut(ActionIndex, _, _, _).


% ================================== Drop ==================================

% a_Drop(ActionIndex, TargetInteractableID).
% a_Place(ActionIndex, TargetInteractableID).

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

applyAction(ActionIndex, "CutAction") :-  
    a_Cut(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Cut(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_Cut(ActionIndex, TargetID).

% ================================== Cook ==================================

% a_WaitToCook(ActionIndex, TargetInteractableID).

applyAction(ActionIndex, "WaitToCookAction") :- 
    a_WaitToCook(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_WaitToCook(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_WaitToCook(ActionIndex, TargetID).

% ================================== Wait ==================================

% a_Wait(ActionIndex, MilliSecondsToWait).
% a_Wait(ActionIndex).

a_Wait_DefaultMilliSecondsToWait(300).

a_Wait(ActionIndex, MilliSecondsToWait) :- 
    a_Wait_DefaultMilliSecondsToWait(MilliSecondsToWait),
    a_Wait(ActionIndex).

applyAction(ActionIndex, "WaitAction") :- 
    a_Wait(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Wait(ActionIndex, _), curr_Player_ID(PlayerID).
actionArgument(ActionIndex, "TimeToWait", MilliSecondsToWait) :- a_Wait(ActionIndex, MilliSecondsToWait).
