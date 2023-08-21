% ========================== STATE PICKUP PLATE ==========================

pup_Any_PlatesCounter_HasAny :-
    state_PickUp_Plate,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).

pup_PlatesCounter(PlateCounterID, TimeToNextPlate) :-
    state_PickUp_Plate,
    not pup_Any_PlatesCounter_HasAny,
    platesCounter_ID(PlateCounterID),
    not counter_HasAny(PlateCounterID),
    platesCounter_TimeToNextPlate(PlateCounterID, TimeToNextPlate).

tmp_pup_PlatesCounter_Target(PlateCounterID) :-
    state_PickUp_Plate,
    not pup_Any_ValidPlate,
    pup_Any_PlatesCounter_HasAny,
    platesCounter_ID(PlateCounterID),
    counter_HasAny(PlateCounterID).

tmp_pup_PlatesCounter_Target(PlateCounterID) :-
    state_PickUp_Plate,
    not pup_Any_PlatesCounter_HasAny,
    not pup_Any_ValidPlate,
    TimeToNextPlate = #min{TimeToNextPlate1 : 
        platesCounter_TimeToNextPlate(PlateCounterID1, TimeToNextPlate1)
    },
    pup_PlatesCounter(PlateCounterID, TimeToNextPlate).

pup_Target(TargetID) :-
    state_PickUp_Plate,
    not pup_Any_ValidPlate,
    tmp_pup_PlatesCounter_Target(_),
    % take the closest one
    TargetDistance = #min{Dist :   
        curr_Player_Counter_Distance(TargetID1, Dist), 
        tmp_pup_PlatesCounter_Target(TargetID1)
    },
    % if there are multiple at the same distance, take the one with the highest ID 
    TargetID = #max{TargetID2 :    
        curr_Player_Counter_Distance(TargetID2, TargetDistance), 
        tmp_pup_PlatesCounter_Target(TargetID2)
    }.



#show ko_Curr_Player/1.
#show plate_Any_InvalidIngredients/2.
#show playerBot_HasRecipeRequest/1.
#show playerBot_RecipeRequest_Name/2.


% counter(-3608,"ClearCounter","ClearCounter_3").
% ko_OwnerContainer_ID(-36994,-3608).

#show tmp_pup_PlatesCounter_Target/1.
#show pup_Target/1.
#show pup_Best_ValidPlate/2.
#show pup_Any_ValidPlate/0.
#show player_Best_Plate_ValidForRecipe/3.
#show player_OwnsAnyPlate_ValidForRecipe/2.


pup_Best_ValidPlate(PlateID, OwnerContainerID) :- 
    state_PickUp_Plate,
    curr_Player_ID(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_Name(PlayerID, RecipeName),
    player_Best_Plate_ValidForRecipe(PlayerID, PlateID, RecipeName),
    ko_HasOwnerContainer(PlateID),
    ko_OwnerContainer_ID(PlateID, OwnerContainerID).

pup_Any_ValidPlate :- 
    state_PickUp_Plate,
    curr_Player_ID(PlayerID),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_Name(PlayerID, RecipeName),
    player_OwnsAnyPlate_ValidForRecipe(PlayerID, RecipeName).


pup_Target(TargetID) :-
    state_PickUp_Plate,
    pup_Any_ValidPlate,
    % if there are multiple pup_Best_ValidPlate, take the closest one
    TargetDistance = #min{Distance1 : 
        curr_Player_Counter_Distance(TargetID1, Distance1),
        pup_Best_ValidPlate(PlateID1, TargetID1)
    },
    % if there are multiple at the same distance, take the one with the highest ID
    TargetID = #max{TargetID2 : 
        curr_Player_Counter_Distance(TargetID2, TargetDistance),
        pup_Best_ValidPlate(PlateID2, TargetID2)
    }.
    
a_PickUp(ActionIndex, TargetID) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex,
    firstActionIndex(FirstActionIndex),
    pup_Target(TargetID).

a_Wait(ActionIndex) :-
    state_PickUp_Plate,
    ActionIndex = FirstActionIndex + 1,
    firstActionIndex(FirstActionIndex).
