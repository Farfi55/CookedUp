using System.Collections;

namespace ThinkEngine.Mappers
{
    class OperationContainer
    {
        internal delegate object Operation(IList values, object value=null, int counter=0);
    }
}
