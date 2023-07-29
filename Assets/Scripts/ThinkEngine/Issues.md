# Issues

## Bugs

1. In generated sensor scripts, mappingTemplate objectIndex is Hardcoded to 1, example:

    ```cs
    mappingTemplate = "s_Player_FirstKitchenObject_Name(player,objectIndex(1),{0})." + Environment.NewLine;

    // should be
    mappingTemplate = "s_Player_FirstKitchenObject_Name(player,objectIndex({1}),{0})." + Environment.NewLine;
    ```

2. In generated sensor scripts, sometimes operationResult is null

    ```cs
    public override string Map()
    { 
        object operationResult = operation(values, specificValue, counter);
        // sometimes operationResult is null and throws an exception on the next line
        return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));
    }
    ```

3. (To be tested) when brain file matches multiple prefixes, it gets added multiple times to the brains list.

## UX Issues

1. No feedback for missing sensor scripts.
2. No feedback for AIfiles prefixes missing.
3. AIFiles prefixes are case sensitive.

## Feature Requests

1. Auto generated sensor scripts should be automatically added to the Serialized scripts sensor list.

