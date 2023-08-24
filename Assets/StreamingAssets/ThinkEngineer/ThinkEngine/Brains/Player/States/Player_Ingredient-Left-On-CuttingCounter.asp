% ======== STATE INGRDEIENT LEFT ON CUTTING COUNTER ========

tmp_ilocc_Target(TargetID) :-
    state_Ingredient_Left_On_CuttingCounter,
    not state_Ingredient_Burning,
    curr_Player_ID(PlayerID),
    cuttingCounter_ID(TargetID),
    ko_Curr_Player(KitchenObjectID),
    ko(KitchenObjectID, KitchenObjectName, TargetID),
    not playerBot_MissingIngredients_Or_Base(PlayerID, KitchenObjectName).    



ilocc_Target(TargetID) :-
    state_Ingredient_Left_On_CuttingCounter,
    tmp_ilocc_Target(_),
    TargetDistance = #min{ Distance1 : 
        tmp_ilocc_Target(TargetID1),
        curr_Player_Counter_Distance(TargetID1, Distance1)
    },
    TargetID = #min{ TargetID2 : 
        tmp_ilocc_Target(TargetID2),
        curr_Player_Counter_Distance(TargetID2, TargetDistance)
    }.

a_PickUp(ActionIndex, TargetID) :-
    state_Ingredient_Left_On_CuttingCounter,
    ilocc_Target(TargetID),
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex).

a_Wait(ActionIndex) :-
    state_Ingredient_Left_On_CuttingCounter,
    ilocc_Target(TargetID),
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).

