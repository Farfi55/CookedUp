% ========================== STATE GET INGREDIENTS ==========================

ingredient_Available(IngredientName, TargetID) :-
    containerCounter_KOType(TargetID, IngredientName).

ingredient_Available(IngredientName, TargetID) :-
    counter_HasAny(TargetID),
    counter_KitchenObjects(TargetID, _, IngredientName).

ingredient_Available_Any(IngredientName) :-
    ingredient_Available(IngredientName, _).

ingredient_NotAvailable(IngredientName) :-
    c_KO_NAME(IngredientName),
    not ingredient_Available_Any(IngredientName).


ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_COOKING_RECIPE(RecipeName, InputIngredientName, OutputIngredientName, _, _).

ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName) :-
    ingredient_NotAvailable(IngredientName),
    IngredientName = OutputIngredientName,
    c_CUTTING_RECIPE(RecipeName, InputIngredientName, OutputIngredientName, _).

ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCooking(IngredientName, RecipeName, InputIngredientName).

ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName) :- 
    ingredient_NeedsCutting(IngredientName, RecipeName, InputIngredientName).


stateGI_nextIngredient(IngredientName) :-
    state_GetIngredient,
    curr_Player_ID(PlayerID),
    playerBot_MissingIngredients_Index(PlayerID, IngredientName, 0).


subStateGI("Available") :- 
    state_GetIngredient,
    stateGI_nextIngredient(IngredientName),
    ingredient_Available_Any(IngredientName).

subStateGI("NeedsCooking") :-
    state_GetIngredient,
    stateGI_nextIngredient(IngredientName),
    ingredient_NeedsCooking(IngredientName, _, _).

subStateGI("NeedsCutting") :-
    state_GetIngredient,
    stateGI_nextIngredient(IngredientName),
    ingredient_NeedsCutting(IngredientName, _, _).


% available
stateGI_Target1(TargetID) :-
    state_GetIngredient,
    subStateGI("Available"),
    stateGI_nextIngredient(IngredientName),
    ingredient_Available(IngredientName, TargetID).


% needs cooking or cutting
stateGI_Target1(TargetID) :-
    state_GetIngredient,
    not subStateGI("Available"),
    stateGI_nextIngredient(IngredientName),
    ingredient_NeedsWork(IngredientName, RecipeName, InputIngredientName),
    ingredient_Available(InputIngredientName, TargetID).


% needs cooking
stateGI_Target2(TargetID) :-
    state_GetIngredient,
    subStateGI("NeedsCooking"),
    stoveCounter_ID(TargetID),
    counter_HasSpace(TargetID).


% needs cutting
stateGI_Target2(TargetID) :-
    state_GetIngredient,
    subStateGI("NeedsCutting"),
    cuttingCounter_ID(TargetID),
    counter_HasSpace(TargetID).

stateGI_Target_Final(TargetID) :-
    state_GetIngredient,
    playerBot_Plate_Container_ID(PlayerID, _, TargetID).
    



a_MoveTo_Target(ActionIndex, TargetID) :-
    state_GetIngredient,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    stateGI_Target1(TargetID).


a_PickUp(ActionIndex, TargetID) :-
    state_GetIngredient,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex),
    stateGI_Target1(TargetID).


a_MoveTo_Target(ActionIndex, TargetID) :-
    state_GetIngredient,
    not subStateGI("Available"),
    ActionIndex = FirstActionIndex + 2,
    firstActionIndex(FirstActionIndex),
    stateGI_Target2(TargetID).




a_Place(ActionIndex, TargetID) :- 
    state_GetIngredient,
    not subStateGI("Available"),
    ActionIndex = FirstActionIndex + 3,
    firstActionIndex(FirstActionIndex),
    stateGI_Target2(TargetID).


% needs cooking
a_WaitToCook(ActionIndex, TargetID) :-
    state_GetIngredient,
    subStateGI("NeedsCooking"),
    ActionIndex = FirstActionIndex + 4,
    firstActionIndex(FirstActionIndex),
    stateGI_Target2(TargetID).


% needs cutting
a_Cut(ActionIndex, TargetID) :-
    state_GetIngredient,
    subStateGI("NeedsCutting"),
    ActionIndex = FirstActionIndex + 4,
    firstActionIndex(FirstActionIndex),
    stateGI_Target2(TargetID).


a_PickUp(ActionIndex, TargetID) :-
    state_GetIngredient,
    not subStateGI("Available"),
    ActionIndex = FirstActionIndex + 5,
    firstActionIndex(FirstActionIndex),
    stateGI_Target2(TargetID).


finalActionIndex(Index) :-
    state_GetIngredient,
    firstActionIndex(FirstActionIndex),
    Index = FirstActionIndex + 2,
    subStateGI("Available").

finalActionIndex(Index) :-
    state_GetIngredient,
    firstActionIndex(FirstActionIndex),
    Index = FirstActionIndex + 6,
    not subStateGI("Available").



a_MoveTo_Target(ActionIndex, TargetID) :-
    state_GetIngredient,
    ActionIndex = FinalActionIndex,
    finalActionIndex(FinalActionIndex),
    stateGI_Target_Final(TargetID).

a_Place(ActionIndex, TargetID) :- 
    state_GetIngredient,
    ActionIndex = FinalActionIndex + 1,
    finalActionIndex(FinalActionIndex),
    stateGI_Target_Final(TargetID).


a_Wait(ActionIndex) :-
    state_GetIngredient,
    ActionIndex = FinalActionIndex + 2,
    finalActionIndex(FinalActionIndex).
