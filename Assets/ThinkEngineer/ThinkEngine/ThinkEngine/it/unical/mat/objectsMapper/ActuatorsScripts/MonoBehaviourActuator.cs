using it.unical.mat.embasp.languages.asp;
using ThinkEngine.Mappers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ThinkEngine
{
    [RequireComponent(typeof(IndexTracker))]
    internal class MonoBehaviourActuator : MonoBehaviour
    {
        internal string configurationName;
        private object toPass;
        [SerializeField]
        private MyListString property;
        private string mappingToCompare;
        private List<IInfoAndValue> _propertyInfo;
        internal List<IInfoAndValue> PropertyInfo
        {
            get
            {
                if (_propertyInfo == null)
                {
                    _propertyInfo = new List<IInfoAndValue>();
                }
                return _propertyInfo;
            }
        }
        private string _mapping;
        internal string Mapping
        {
            get
            {
                return _mapping;
            }
        }
        private string _toSet;
        private string suffix;

        internal string ToSet
        {
            get
            {
                return _toSet;
            }
            set
            {
                _toSet = value;
                ApplyToGameLogic();
            }
        }

        internal void Configure(InstantiationInformation information, string mapping)
        {
            toPass = gameObject;
            configurationName = information.configuration.ConfigurationName;
            property = new MyListString(information.propertyHierarchy.myStrings);
            PropertyInfo.AddRange(information.hierarchyInfo);
            int index = GetComponent<IndexTracker>().CurrentIndex;
            _mapping = "setOnActuator(" + ASPMapperHelper.AspFormat(configurationName) + "(" + ASPMapperHelper.AspFormat(gameObject.name) + ",objectIndex(" + index + ")," + mapping + "))";
            List<object> temp = new List<object>();
            for (int i = 0; i < PropertyInfo.Count; i++)
            {
                object currentValue = PropertyInfo[i].GetValuesForPlaceholders();
                if (currentValue.GetType().IsArray)
                {
                    Array array = (Array)currentValue;
                    for (int j = 0; j < array.Length; j++)
                    {
                        temp.Add(array.GetValue(j));
                    }
                }
                else
                {
                    if (temp != null)
                    {
                        temp.Add(currentValue);
                    }
                    else
                    {
                        temp.Add("");
                    }
                }
            }
            mappingToCompare = _mapping.Substring(0, _mapping.IndexOf("{" + temp.Count + "}"));
            int indexOfValue = _mapping.IndexOf("{" + temp.Count + "}");
            suffix = _mapping.Substring(indexOfValue + 3);
            mappingToCompare = string.Format(mappingToCompare, temp.ToArray());
        }
        void Update()
        {
        }

        void OnDisable()
        {
            Destroy(this);
        }

        private void ApplyToGameLogic()
        {
            if (_toSet == null)
            {
                return;
            }
            MapperManager.SetPropertyValue(this, new MyListString(property.myStrings), toPass, _toSet, 0);

        }
        internal string Parse(AnswerSet answerSet)
        {
            string pattern = "objectIndex\\(([0-9]+)\\)";
            Regex regex = new Regex(@pattern);
            int myIndex = gameObject.GetComponent<IndexTracker>().CurrentIndex;
            foreach (string literal in answerSet.GetAnswerSet())
            {
                string literalPrefix = literal.Substring(0, Math.Min(mappingToCompare.Length, literal.Length));
                Match matcher = regex.Match(literalPrefix);
                if (matcher.Success && int.Parse(matcher.Groups[1].Value) == myIndex)//if the index of the object associated to the actuator is different from the one of the literal
                {
                    if (literalPrefix.Contains(mappingToCompare))
                    {
                        string partialRes = literal.Substring(mappingToCompare.Length);
                        partialRes = partialRes.Remove(partialRes.IndexOf(suffix));
                        return partialRes.Trim('\"', ' ');//trim " to avoid conversion problems
                    }
                }
            }
            return null;
        }
    }
}