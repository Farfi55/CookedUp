using System.Collections.Generic;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine.Mappers
{
    interface IInfoAndValue
    {
        object GetValuesForPlaceholders();
    }
}