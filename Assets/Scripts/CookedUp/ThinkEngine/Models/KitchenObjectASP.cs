using System;
using CookedUp.Core.KitchenObjects;

namespace CookedUp.ThinkEngine.Models
{
    [Serializable]
    public class KitchenObjectASP
    {
        public string Name;
        public int ID;
        public int ContainerID;

        public KitchenObjectASP(string name, int id, int containerID) {
            Name = name;
            ID = id;
            ContainerID = containerID;
        }
        
        public KitchenObjectASP(KitchenObject ko) {
            var idManager = IDManager.Instance;
            Name = ko.KitchenObjectSO.name;
            ID = idManager.GetID(ko.gameObject);
            ContainerID = idManager.GetID(ko.Container.gameObject);
        }
    }
}
