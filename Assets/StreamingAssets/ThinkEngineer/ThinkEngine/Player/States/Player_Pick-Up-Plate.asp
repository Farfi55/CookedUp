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
#show pup_ValidPlate/2.
#show pup_ValidPlate_Recipe_GetTime/2.

pup_ValidPlate(PlateID, OwnerContainerID) :- 
    state_PickUp_Plate,
    plate_ID(PlateID),
    ko_Curr_Player(PlateID),
    ko_HasOwnerContainer(PlateID),
    not plate_Any_InvalidIngredients(PlateID, RecipeName),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_Name(PlayerID, RecipeName),
    ko_OwnerContainer_ID(PlateID, OwnerContainerID),
    counter_ID(OwnerContainerID),
    not platesCounter_ID(OwnerContainerID).

pup_ValidPlate_Recipe_GetTime(PlateID, Time) :-
    state_PickUp_Plate,
    pup_ValidPlate(PlateID, _),
    plate_Recipe_ExpectedTime(PlateID, RecipeName, Time),
    playerBot_HasRecipeRequest(PlayerID),
    playerBot_RecipeRequest_Name(PlayerID, RecipeName).
    


pup_Any_ValidPlate :- pup_ValidPlate(_, _).

pup_Target(TargetID) :-
    state_PickUp_Plate,
    pup_Any_ValidPlate,
    % take the plate with the lowest recipe completion time
    TargetTime = #min{Time1 : pup_ValidPlate_Recipe_GetTime(PlateID1, Time1)},
    % if there are multiple with the same time, take the closest one
    TargetDistance = #min{Distance2 : 
        curr_Player_Counter_Distance(TargetID2, Distance2),
        pup_ValidPlate(PlateID2, TargetID2),
        pup_ValidPlate_Recipe_GetTime(PlateID2, TargetTime)
    },
    % if there are multiple at the same distance, take the one with the highest ID
    TargetID = #max{TargetID3 : 
        curr_Player_Counter_Distance(TargetID3, TargetDistance),
        pup_ValidPlate(PlateID3, TargetID3),
        pup_ValidPlate_Recipe_GetTime(PlateID3, TargetTime)
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
