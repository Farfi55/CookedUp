using System.Collections;

namespace ThinkEngine.Mappers
{
    public class OperationContainer
    {
        public delegate object Operation(IList values, object value=null, int counter=0);
    }
}
