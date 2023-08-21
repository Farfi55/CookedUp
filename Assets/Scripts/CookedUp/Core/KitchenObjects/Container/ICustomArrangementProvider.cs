using System.Collections.Generic;

namespace CookedUp.Core.KitchenObjects.Container
{
    public interface ICustomArrangementProvider {
        void SetTransforms(List<KitchenObject> kitchenObjects);
    }
}
