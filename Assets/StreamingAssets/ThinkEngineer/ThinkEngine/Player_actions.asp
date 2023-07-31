% ================================== Move To ==================================

% a_MoveTo_Target(ActionIndex, PlayerID, TargetID).
% a_MoveTo_Pos(ActionIndex, PlayerID, GridX,GridY).

a_MoveTo_Generic(ActionIndex, PlayerID) :- 
    a_MoveTo_Target(ActionIndex, PlayerID, _).

a_MoveTo_Generic(ActionIndex, PlayerID) :- 
    a_MoveTo_Pos(ActionIndex, PlayerID, GridX,GridY).


applyAction(ActionIndex, "MoveToAction") :- a_MoveTo_Generic(ActionIndex, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_MoveTo_Generic(ActionIndex, PlayerID).

% using a_MoveTo_Target
    actionArgument(ActionIndex, "TargetID", TargetID) :- a_MoveTo_Target(ActionIndex, _, TargetID).


% using a_MoveTo_Pos
    actionArgument(ActionIndex, "GridX", GridX) :- a_MoveTo_Pos(ActionIndex, _, GridX, _).
    actionArgument(ActionIndex, "GridY", GridY) :- a_MoveTo_Pos(ActionIndex, _, _, GridY).


% ================================== Pick Up ==================================

% a_PickUp(ActionIndex, PlayerID, TargetInteractableID).

applyAction(ActionIndex, "PickUpAction") :- 
    a_PickUp(ActionIndex, _, _).


actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_PickUp(ActionIndex, PlayerID, _).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_PickUp(ActionIndex, _, TargetID).


% ================================== Drop ==================================

% a_Drop(ActionIndex, PlayerID, TargetInteractableID).
% a_Place(ActionIndex, PlayerID, TargetInteractableID).

a_Drop(ActionIndex, PlayerID, TargetInteractableID) :- 
    a_Place(ActionIndex, PlayerID, TargetInteractableID).

a_Place(ActionIndex, PlayerID, TargetInteractableID) :- 
    a_Drop(ActionIndex, PlayerID, TargetInteractableID).

applyAction(ActionIndex, "DropAction") :- 
    a_Drop(ActionIndex, _, _).


actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Drop(ActionIndex, PlayerID, _).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_Drop(ActionIndex, _, TargetID).


% ================================== Cut ==================================

% a_Cut(ActionIndex, PlayerID, TargetInteractableID).

applyAction(ActionIndex, "CutAction") :-  
    a_Cut(ActionIndex, _, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Cut(ActionIndex, PlayerID, _).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_Cut(ActionIndex, _, TargetID).

% ================================== Cook ==================================

% a_WaitToCook(ActionIndex, PlayerID, TargetInteractableID).

applyAction(ActionIndex, "CookAction") :- 
    a_Cook(ActionIndex, _, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Cook(ActionIndex, PlayerID, _).
actionArgument(ActionIndex, "TargetID", TargetID) :- a_Cook(ActionIndex, _, TargetID).

% ================================== Wait ==================================

% a_Wait(ActionIndex, PlayerID, MilliSecondsToWait).
% a_Wait(ActionIndex, PlayerID).

a_Wait_DefaultMilliSecondsToWait(200).

a_Wait(ActionIndex, PlayerID, MilliSecondsToWait) :- 
    a_Wait_DefaultMilliSecondsToWait(MilliSecondsToWait),
    a_Wait(ActionIndex, PlayerID).

applyAction(ActionIndex, "WaitAction") :- 
    a_Wait(ActionIndex, _, _).

actionArgument(ActionIndex, "PlayerID", PlayerID) :- a_Wait(ActionIndex, PlayerID, _).
actionArgument(ActionIndex, "TimeToWait", MilliSecondsToWait) :- a_Wait(ActionIndex, _, MilliSecondsToWait).
