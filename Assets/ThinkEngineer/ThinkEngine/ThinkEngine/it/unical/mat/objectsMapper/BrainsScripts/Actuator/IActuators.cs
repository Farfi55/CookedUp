using System.Collections.Generic;

namespace ThinkEngine
{
    internal interface IActuators
    {
        List<MonoBehaviourActuator> GetActuatorsList();
    }
}