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
            Name = ko.KitchenObjectSO.name;
            ID = ko.GetInstanceID();
            ContainerID = ko.Container.gameObject.GetInstanceID();
        }
    }
}
