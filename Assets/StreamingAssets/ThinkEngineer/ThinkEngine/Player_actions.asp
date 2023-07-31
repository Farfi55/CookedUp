% ================================== Move To ==================================

% a_MoveTo_Target(ActionOrder, PlayerID, TargetID).
% a_MoveTo_Pos(ActionOrder, PlayerID, GridX,GridY).

a_MoveTo_Generic(ActionOrder, PlayerID) :- 
    a_MoveTo_Target(ActionOrder, PlayerID, _).

a_MoveTo_Generic(ActionOrder, PlayerID) :- 
    a_MoveTo_Pos(ActionOrder, PlayerID, GridX,GridY).


applyAction(ActionOrder, "MoveToAction") :- a_MoveTo_Generic(ActionOrder, _).

actionArgument(ActionOrder, "PlayerID", PlayerID) :- a_MoveTo_Generic(ActionOrder, PlayerID).

% using a_MoveTo_Target
    actionArgument(ActionOrder, "TargetID", TargetID) :- a_MoveTo_Target(ActionOrder, _, TargetID).


% using a_MoveTo_Pos
    actionArgument(ActionOrder, "GridX", GridX) :- a_MoveTo_Pos(ActionOrder, _, GridX, _).
    actionArgument(ActionOrder, "GridY", GridY) :- a_MoveTo_Pos(ActionOrder, _, _, GridY).


% ================================== Pick Up ==================================

% a_PickUp(ActionOrder, PlayerID, TargetInteractableID).

applyAction(ActionOrder, "PickUpAction") :- 
    a_PickUp(ActionOrder, _, _).


actionArgument(ActionOrder, "PlayerID", PlayerID) :- a_PickUp(ActionOrder, PlayerID, _).
actionArgument(ActionOrder, "TargetID", TargetID) :- a_PickUp(ActionOrder, _, TargetID).


% ================================== Drop ==================================

% a_Drop(ActionOrder, PlayerID, TargetInteractableID).
% a_Place(ActionOrder, PlayerID, TargetInteractableID).

a_Drop(ActionOrder, PlayerID, TargetInteractableID) :- 
    a_Place(ActionOrder, PlayerID, TargetInteractableID).

a_Place(ActionOrder, PlayerID, TargetInteractableID) :- 
    a_Drop(ActionOrder, PlayerID, TargetInteractableID).

applyAction(ActionOrder, "DropAction") :- 
    a_Drop(ActionOrder, _, _).


actionArgument(ActionOrder, "PlayerID", PlayerID) :- a_Drop(ActionOrder, PlayerID, _).
actionArgument(ActionOrder, "TargetID", TargetID) :- a_Drop(ActionOrder, _, TargetID).


% ================================== Cut ==================================

% a_Cut(ActionOrder, PlayerID, TargetInteractableID).

applyAction(ActionOrder, "CutAction") :-  
    a_Cut(ActionOrder, _, _).

actionArgument(ActionOrder, "PlayerID", PlayerID) :- a_Cut(ActionOrder, PlayerID, _).
actionArgument(ActionOrder, "TargetID", TargetID) :- a_Cut(ActionOrder, _, TargetID).

% ================================== Cook ==================================

% a_WaitToCook(ActionOrder, PlayerID, TargetInteractableID).

applyAction(ActionOrder, "CookAction") :- 
    a_Cook(ActionOrder, _, _).

actionArgument(ActionOrder, "PlayerID", PlayerID) :- a_Cook(ActionOrder, PlayerID, _).
actionArgument(ActionOrder, "TargetID", TargetID) :- a_Cook(ActionOrder, _, TargetID).
