% ========================== STATE INGREDIENT BURNING ==========================

% if the player has something in hands
ib_MustDrop_HeldObject :- 
    state_Ingredient_Burning,
    curr_Player_ID(PlayerID),
    player_HasAny(PlayerID).

% is that thing in hands worth keeping
ib_KeepIngredient :- 
    state_Ingredient_Burning,
    ib_MustDrop_HeldObject,
    clearCounter_HasSpace(CounterID),
    not ingredient_IsTrash(KOName),
    player_KO_Name(PlayerID, KOName).


ib_FirstActionIndex(Index) :-
    state_Ingredient_Burning,
    ib_MustDrop_HeldObject,
    Index = BaseFirstActionIndex,
    firstActionIndex(BaseFirstActionIndex).

ib_FirstActionIndex(Index) :-
    state_Ingredient_Burning,
    not ib_MustDrop_HeldObject,
    Index = BaseFirstActionIndex - 1,
    firstActionIndex(BaseFirstActionIndex).




tmp_ib_HeldObject_Drop_Target(TargetID) :-
    state_Ingredient_Burning,
    ib_MustDrop_HeldObject,
    ib_KeepIngredient,
    clearCounter_HasSpace(TargetID).


tmp_ib_HeldObject_Drop_Target(TargetID) :-
    state_Ingredient_Burning,
    ib_MustDrop_HeldObject,
    not ib_KeepIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, KOName),
    containerCounter_KOType(TargetID, KOName).
    
tmp_ib_HeldObject_Drop_Target(TargetID) :-
    state_Ingredient_Burning,
    ib_MustDrop_HeldObject,
    not ib_KeepIngredient,
    curr_Player_ID(PlayerID),
    player_KO_Name(PlayerID, KOName),
    trashCounter_ID(TargetID).


ib_HeldObject_Drop_Target(TargetID) :-
    state_Ingredient_Burning,
    ib_MustDrop_HeldObject,
    tmp_ib_HeldObject_Drop_Target(_),
    TargetDistance = #min{ Distance1 : 
        tmp_ib_HeldObject_Drop_Target(TargetID1),
        curr_Player_Counter_Distance(TargetID1, Distance1)
    },
    TargetID = #min{ TargetID2 : 
        tmp_ib_HeldObject_Drop_Target(TargetID2),
        curr_Player_Counter_Distance(TargetID2, TargetDistance)
    }.

ib_Ingredient_IsBurning :-
    state_Ingredient_Burning,
    stoveCounter_IsBurning(StoveCounterID),
    counter_KO_ID(StoveCounterID, KitchenObjectID),
    ko_Curr_Player(KitchenObjectID).

ib_Ingredient_HasBurned :- 
    state_Ingredient_Burning,
    not ib_Ingredient_IsBurning.
    

tmp_ib_Ingredient_PickUp_Target(TargetID) :-
    state_Ingredient_Burning,
    ib_Ingredient_IsBurning,
    stoveCounter_IsBurning(TargetID),
    counter_KO(TargetID, KitchenObjectID, KitchenObjectName),
    ko_Curr_Player(KitchenObjectID),
    ingredient_IsTrash(IngredientName).


tmp_ib_Ingredient_PickUp_Target(TargetID) :-
    state_Ingredient_Burning,
    ib_Ingredient_HasBurned,
    counter_KO(TargetID, KitchenObjectID, KitchenObjectName),
    ko_Curr_Player(KitchenObjectID),
    ingredient_IsTrash(IngredientName).


ib_Ingredient_PickUp_Target(TargetID) :-
    state_Ingredient_Burning,
    tmp_ib_Ingredient_PickUp_Target(_),
    TargetDistance = #min{ Distance1 : 
        tmp_ib_Ingredient_PickUp_Target(TargetID1),
        curr_Player_Counter_Distance(TargetID1, Distance1)
    },
    TargetID = #min{ TargetID2 : 
        tmp_ib_Ingredient_PickUp_Target(TargetID2),
        curr_Player_Counter_Distance(TargetID2, TargetDistance)
    }.


a_Place(ActionIndex, TargetID) :-
    state_Ingredient_Burning,
    ActionIndex = FirstActionIndex,
    ib_FirstActionIndex(FirstActionIndex),
    ib_HeldObject_Drop_Target(TargetID).

a_PickUp(ActionIndex, TargetID) :-
    state_Ingredient_Burning,
    ActionIndex = FirstActionIndex + 1,
    ib_FirstActionIndex(FirstActionIndex),
    ib_Ingredient_PickUp_Target(TargetID).

a_Wait(ActionIndex) :-
    state_Ingredient_Burning,
    ActionIndex = FirstActionIndex + 2,
    ib_FirstActionIndex(FirstActionIndex).

#show ib_Ingredient_IsBurning/0.
#show ib_Ingredient_HasBurned/0.
#show ib_KeepIngredient/0.
#show ib_MustDrop_HeldObject/0.
#show state_Ingredient_Burning/0.
