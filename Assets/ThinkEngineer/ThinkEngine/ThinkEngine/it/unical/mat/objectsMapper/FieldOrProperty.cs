using System;
using System.Reflection;
using UnityEngine;

namespace ThinkEngine
{
    //Wrapper class for a member of a class: is it a field or a property?
    public class FieldOrProperty
    {
        private FieldInfo f;
        private PropertyInfo p;

        public FieldOrProperty(MemberInfo m)
        {
            //Debug.Log(m.Name+" "+ m.MemberType);
            if (m.MemberType == MemberTypes.Field)
            {
                f = (FieldInfo)m;
            }
            else if (m.MemberType == MemberTypes.Property)
            {
                p = (PropertyInfo)m;
            }
        }

        public Type Type()
        {
            return f == null ? p.PropertyType : f.FieldType;
        }

        public string Name()
        {
            return f == null ? p.Name : f.Name;
        }

        public object GetValue(object ob)
        {
            return f == null ? p.GetValue(ob, null) : f.GetValue(ob);
        }

        public void SetValue(object obj, object value)
        {
            if (f != null)
            {
                f.SetValue(obj, value);
            }
            else
            {
                p.SetValue(obj, value, null);
            }
        }
    }
}