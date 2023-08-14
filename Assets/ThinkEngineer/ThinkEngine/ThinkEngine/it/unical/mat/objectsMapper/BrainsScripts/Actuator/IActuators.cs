using System.Collections.Generic;

namespace ThinkEngine
{
    public interface IActuators
    {
        List<MonoBehaviourActuator> GetActuatorsList();
    }
}