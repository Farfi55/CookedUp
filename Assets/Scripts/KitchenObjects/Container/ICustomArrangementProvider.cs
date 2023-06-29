using System.Collections.Generic;

namespace KitchenObjects.Container
{
    public interface ICustomArrangementProvider {
        void SetTrasforms(List<KitchenObject> kitchenObjects);
    }
}
