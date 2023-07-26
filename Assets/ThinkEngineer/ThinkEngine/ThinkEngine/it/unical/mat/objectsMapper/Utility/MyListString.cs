using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThinkEngine
{
    [Serializable]
    public class MyListString
    {
        public List<string> myStrings;
        public int Count;

        public MyListString()
        {
            Count = 0;
            myStrings = new List<string>();
        }
        public MyListString(List<string> clone)
        {
            myStrings = new List<string>();
            myStrings.AddRange(clone);
            Count = myStrings.Count;
        }
        public MyListString(string first)
        {
            myStrings = new List<string>();
            myStrings.Add(first);
            Count = 1;
        }
        public override bool Equals(object obj)
        {
            return myStrings.SequenceEqual(((MyListString)obj).myStrings);
        }
        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            foreach (string s in myStrings)
            {
                result = result * prime + s.GetHashCode();
            }
            return result;
        }
        public MyListString GetRange(int start, int count)
        {
            return new MyListString(myStrings.GetRange(start, count));
        }

        public void Add(string s)
        {
            myStrings.Add(s);
            Count = myStrings.Count;
        }
        public void Remove(string s)
        {
            myStrings.Remove(s);
            Count = myStrings.Count;
        }
        public void RemoveAt(int index)
        {
            myStrings.RemoveAt(index);
            Count = myStrings.Count;
        }
        public string this[int key]
        {
            get => myStrings[key];
            set => myStrings[key] = value;
        }
        public override string ToString()
        {
            if (Count == 0)
            {
                return "";
            }
            string toReturn = myStrings[0];
            for (int i = 1; i < myStrings.Count; i++)
            {
                toReturn += "^" + myStrings[i];
            }
            return toReturn;
        }
        internal MyListString GetClone()
        {
            return GetRange(0, Count);
        }
    }
}