using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThinkEngine
{
    [Serializable, ExecuteInEditMode]
    public abstract class AbstractConfiguration : MonoBehaviour
    {
        internal ObjectTracker _objectTracker;
        internal ObjectTracker ObjectTracker
        {
            get
            {
                if (_objectTracker == null)
                {
                    _objectTracker = new ObjectTracker(gameObject);
                }
                return _objectTracker;
            }
        }
        [SerializeField, HideInInspector]
        internal List<MyListString> _savedProperties;
        internal List<MyListString> SavedProperties
        {
            get
            {
                if (_savedProperties == null)
                {
                    _savedProperties = new List<MyListString>();
                }
                return _savedProperties;
            }
            set
            {
                _savedProperties = value;
            }
        }
        [SerializeField, HideInInspector]
        internal List<MyListString> _toMapProperties;
        [SerializeField, HideInInspector]
        internal List<PropertyFeatures> _propertyFeatures;
        internal List<PropertyFeatures> PropertyFeaturesList
        {
            get
            {
                if (_propertyFeatures == null)
                {
                    _propertyFeatures = new List<PropertyFeatures>();
                }
                return _propertyFeatures;
            }
            set
            {
                _propertyFeatures = value;
            }
        }
        internal List<MyListString> ToMapProperties
        {
            get
            {
                if (_toMapProperties == null)
                {
                    _toMapProperties = new List<MyListString>();
                }
                return _toMapProperties;
            }
            set
            {
                _toMapProperties = value;
            }
        }
        [SerializeField, HideInInspector]
        internal string _configurationName;
        internal virtual string ConfigurationName
        {
            get
            {
                if (_configurationName == null)
                {
                    _configurationName = GetAutoConfigurationName();
                }
                return _configurationName;
            }
            set { }
        }
        void OnDestroy()
        {
            foreach (PropertyFeatures property in _propertyFeatures)
            {
                property.Remove();
            }

        }
        internal void RefreshObjectTracker()
        {
            _objectTracker = new ObjectTracker(gameObject);
        }
        internal abstract bool IsSensor();
        internal abstract string GetAutoConfigurationName();

        internal virtual void Clear()
        {
            _objectTracker = new ObjectTracker(gameObject);
            SavedProperties = new List<MyListString>();
            ToMapProperties = new List<MyListString>();
            foreach (PropertyFeatures property in PropertyFeaturesList)
            {
                property.Remove();
            }
            PropertyFeaturesList = new List<PropertyFeatures>();
        }
        internal bool IsPropertySelected(MyListString property)
        {
            if(SavedProperties.Contains(property))
            {
                return true;
            }
            return false;
        }
        internal void ToggleProperty(MyListString property, bool isActive)
        {
            if (isActive)
            {
                SavedProperties.Add(property);
                if (ObjectTracker.IsFinal(property))
                {
                    ToMapProperties.Add(property);
                    PropertyFeaturesList.Add(new PropertyFeatures(ConfigurationName, property));
                    PropertySelected(property);
                }
            }
            else
            {
                PropertyDeleted(property);
                SavedProperties.Remove(property);
                if (ToMapProperties.Contains(property))
                {
                    ToMapProperties.Remove(property);
                    PropertyFeatures p =PropertyFeaturesList.Find(x => x.property.Equals(property));
                    p.Remove();
                    PropertyFeaturesList.Remove(p) ;
                }
            }
        }

        internal void RefreshDefaultPropertiesAlias()
        {
            foreach(PropertyFeatures p in PropertyFeaturesList)
            {
                Debug.Log(p.PropertyAlias);
                if(p.configurationName != "")
                {
                    Debug.Log("changing " + p.PropertyAlias);
                    p.PropertyAlias = ConfigurationName+ p.PropertyAlias.Substring(p.configurationName.Length);
                    p.configurationName = ConfigurationName;
                    Debug.Log("changed " + p.PropertyAlias);
                }
            }
            PropertiesAliasChanged();
        }

        protected virtual void PropertiesAliasChanged() { }

        protected virtual void PropertyDeleted(MyListString property) { }
        protected virtual void PropertySelected(MyListString property) { }
        internal abstract bool IsAValidName(string temporaryName);

    }
}