%playerSensor(sensors,objectIndex(Index),playerBotSensorData(hasSelectedInteractable(Value))).
%playerSensor(sensors,objectIndex(Index),playerBotSensorData(selectedInteractableName(Value))).
%playerSensor(sensors,objectIndex(Index),playerBotSensorData(pos(x(Value)))).
%playerSensor(sensors,objectIndex(Index),playerBotSensorData(pos(y(Value)))).
%playerSensor(sensors,objectIndex(Index),kOContainerSensorData(sizeLimit(Value))).
%playerSensor(sensors,objectIndex(Index),kOContainerSensorData(count(Value))).
%playerSensor(sensors,objectIndex(Index),kOContainerSensorData(kosNames(Index1,Value))).
%playerSensor(sensors,objectIndex(Index),kOContainerSensorData(koName(Value))).
%playerSensor(sensors,objectIndex(Index),kOContainerSensorData(hasSpace(Value))).
%playerSensor(sensors,objectIndex(Index),kOContainerSensorData(hasAny(Value))).
%For ASP programs:
% Predicates for Action invokation.
% applyAction(OrderOfExecution,ActionClassName).
% actionArgument(ActionOrder,ArgumentName, ArgumentValue).



applyAction(1, "TestAction").
actionArgument(1, "TestInt", 14).
actionArgument(1, "testString", "ciaooo").
actionArgument(1, "testBool1", "false").
actionArgument(1, "testBool2", "false").





