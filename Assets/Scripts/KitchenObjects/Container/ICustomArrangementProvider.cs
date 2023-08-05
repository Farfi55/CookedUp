using System.Collections.Generic;

namespace KitchenObjects.Container
{
    public interface ICustomArrangementProvider {
        void SetTransforms(List<KitchenObject> kitchenObjects);
    }
}
