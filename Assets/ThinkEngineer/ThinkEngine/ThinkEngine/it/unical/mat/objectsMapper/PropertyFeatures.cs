using System;
using System.Collections.Generic;
using UnityEngine;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    [Serializable]
    internal class PropertyFeatures : ISerializationCallbackReceiver
    {
        private static HashSet<int> usedPropertyAlias = new HashSet<int>();
        [SerializeField,HideInInspector]
        internal MyListString property;
        [SerializeField,HideInInspector]
        internal int windowWidth = 200;
        [SerializeField,HideInInspector]
        private string _propertyAlias;
        [SerializeField,HideInInspector]
        private int propertyAliasHash;
        [SerializeField,HideInInspector]
        internal int operation;
        [SerializeField,HideInInspector]
        internal string specificValue;
        [SerializeField, HideInInspector]
        internal int counter;//for atLeast and count operations

        internal int GetPropertyIndex { get; }
        internal string PropertyAlias
        {
            get
            {
                return _propertyAlias;
            }
            set
            {
                ValidateNameAndAssign(value);
            }
        }

        internal PropertyFeatures(GameObject go, MyListString p)
        {
            property = p.GetClone();
            GeneratePropertyName(go.name);
        }

        private void ValidateNameAndAssign(string value)
        {
            if(_propertyAlias == value)
            {
                Debug.Log("SAME VALUE! "+_propertyAlias+" "+value);
                return;
            }
            if (usedPropertyAlias.Contains(value.GetHashCode()))
            {
                Debug.Log("THROWING!");
                throw new Exception("InvalidName");
            }
            if(_propertyAlias!=null && usedPropertyAlias.Contains(propertyAliasHash))
            {
                usedPropertyAlias.Remove(propertyAliasHash);
            }
            Debug.Log("setting _property_name to " + value);
            _propertyAlias = value;
            propertyAliasHash = PropertyAlias.GetHashCode();
            usedPropertyAlias.Add(propertyAliasHash);
        }

        private void GeneratePropertyName(string goName)
        {
            string prefix = goName;
            string suffix ="";
            int count = 0;
            Debug.Log(property);
            if (property.Count > 0)
            {
                count = property.Count - 1;
                suffix = "_" + property[count];
            }
            while (true)
            {
                try
                {
                    PropertyAlias = prefix + suffix;
                    break;
                }
                catch (Exception e)
                {
                    if (e.Message.Equals("InvalidName"))
                    {
                        if(count!= 0 && property.Count >= count)
                        {
                            count--;
                            suffix = "_" + property[count] + suffix;
                        }
                        else
                        {
                            suffix += "0";
                        }
                        continue;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        internal void Remove()
        {
            usedPropertyAlias.Remove(propertyAliasHash);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            usedPropertyAlias.Add(propertyAliasHash);
        }
    }
}