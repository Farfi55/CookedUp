using System;

namespace ThinkEngine.Models
{
    [Serializable]
    public class KitchenObject
    {
        public string Name;
        public int ID;
        public int ContainerID;

        public KitchenObject(string name, int id, int containerID) {
            Name = name;
            ID = id;
            ContainerID = containerID;
        }
        
        public KitchenObject(KitchenObjects.KitchenObject ko) {
            var idManager = IDManager.Instance;
            Name = ko.KitchenObjectSO.name;
            ID = idManager.GetID(ko.gameObject);
            ContainerID = idManager.GetID(ko.Container.gameObject);
        }
    }
}
