using System;
using System.Collections.Generic;
using ThinkEngine.it.unical.mat.objectsMapper;
using ThinkEngine.Mappers;
using UnityEngine;

namespace ThinkEngine
{
    internal class ObjectTracker
    {
        public delegate Dictionary<MyListString, KeyValuePair<Type, object>> RetrieveProperties(Type t, MyListString currentHierarchy, object currentObject = null);

        Dictionary<MyListString, int> propertiesIndex;
        GameObject rootObject;
        internal Dictionary<int, PropertyDetails> propertiesDetails;
        internal Type PropertyType(MyListString property)
        {
            CheckPropertyExistence(property);
            return propertiesDetails[propertiesIndex[property]].propertyType;
        }

        public ObjectTracker(object root, bool nullObject = false)
        {
            propertiesIndex = new Dictionary<MyListString, int>();
            propertiesDetails = new Dictionary<int, PropertyDetails>();
            if (root == null || nullObject && !(root is Type))
            {
                throw new Exception("Invalid parameters.");
            }
            if (!nullObject)
            {

                rootObject = (GameObject)root;
                RetrieveObjectProperties(root);
            }
            else
            {
                RetrieveTypeProperties((Type)root);
            }
        }
        internal bool IsFinal(MyListString property)
        {
            return MapperManager.IsFinal(PropertyType(property));
        }
        private List<MyListString> GetRootObjectProperties()
        {
            List<MyListString> toReturn = new List<MyListString>();
            foreach (MyListString property in propertiesIndex.Keys)
            {
                if (property.Count == 1)
                {
                    toReturn.Add(property);
                }
            }
            return toReturn;
        }

        internal List<MyListString> GetMemberProperties(MyListString memberName)
        {
            if (memberName.Count == 0)
            {
                return GetRootObjectProperties();
            }
            if (propertiesIndex.ContainsKey(memberName) || GetMemberProperties(memberName.GetRange(0, memberName.Count - 1)) != null)
            {
                int index = propertiesIndex[memberName];
                if (propertiesDetails[index].derivedProperties.Count>0 || RetrieveMemberProperties(memberName))
                {
                    return propertiesDetails[index].derivedProperties;
                }
            }
            throw new Exception("Property " + memberName + " does not exist.");
        }

        private bool RetrieveMemberProperties(MyListString memberName)
        {
            if (!propertiesIndex.ContainsKey(memberName))
            {
                throw new Exception("Property " + memberName + " not yet discovered.");
            }
            int memberIndex = propertiesIndex[memberName];
            PropertyDetails details = propertiesDetails[memberIndex];
            if (details.derivedProperties.Count > 0)
            {
                return true;
            }
            if (details.propertyValue != null)
            {
                details.derivedProperties.AddRange(RetrieveObjectProperties(details.propertyValue, memberName));
                return true;
            }
            if (details.propertyType!=null)
            {
                details.derivedProperties.AddRange(RetrieveTypeProperties(details.propertyType, memberName));
                return true;
            }
            return false;
        }

        private List<MyListString> RetrieveObjectProperties(object currentObject, MyListString fatherName = null)
        {
            if (currentObject == null)
            {
                throw new Exception("Cannot retrieve properties of null.");
            }
            return RetrieveTypeProperties(currentObject.GetType(), fatherName, currentObject);
        }
        private List<MyListString> RetrieveTypeProperties(Type type, MyListString fatherMember = null, object currentObject = null)
        {
            if (currentObject != null && !currentObject.GetType().Equals(type))
            {
                throw new Exception("Actual object type differs from declared one.");
            }
            List<MyListString> toReturn = new List<MyListString>();
            MyListString baseMember = fatherMember ?? new MyListString();
            Dictionary<MyListString, KeyValuePair<Type, object>> retrievedProperties = MapperManager.RetrieveProperties(type, baseMember, currentObject);
            foreach (MyListString property in retrievedProperties.Keys)
            {
                if (!propertiesIndex.ContainsKey(property))
                {
                    int index = GetIndex(property);
                    propertiesIndex.Add(property, index);
                    propertiesDetails.Add(index, new PropertyDetails( property));
                    PropertyDetails details = propertiesDetails[index];
                    details.propertyType = retrievedProperties[property].Key;
                    details.propertyValue = retrievedProperties[property].Value;
                    /*
                    if (!propertiesValue.ContainsValue(retrievedProperties[property].Value))
                    {
                        propertiesValue.Add(index, retrievedProperties[property].Value);
                    }
                    else
                    {
                        propertiesValue.Add(index, null);
                    }
                    *///TO CHECK FOR BUGS!
                }
                toReturn.Add(property);
            }
            return toReturn;
        }

        private static int GetIndex(MyListString property)
        {
            return property.GetHashCode();
        }

        private void CheckPropertyExistence(MyListString property)
        {
            if (!propertiesIndex.ContainsKey(property))
            {
                RetrieveMemberProperties(property.GetRange(0, property.Count - 1));
            }
            if (!propertiesIndex.ContainsKey(property))
            {
                throw new Exception("Property " + property + " does not exist.");
            }
        }

        internal bool IsPropertyExpandable(MyListString property)
        {
            CheckPropertyExistence(property);
            return MapperManager.IsTypeExpandable(propertiesDetails[propertiesIndex[property]].propertyType);
        }
    }
}