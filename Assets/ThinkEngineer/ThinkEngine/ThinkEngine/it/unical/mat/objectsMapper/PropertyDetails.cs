using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkEngine.it.unical.mat.objectsMapper
{
    internal class PropertyDetails
    {
        MyListString property;
        internal List<MyListString> derivedProperties;//the list of the properties of the property;
        internal object propertyValue;//the actual instantiation of the property (if not null)
        internal Type propertyType;//the type of the property

        internal PropertyDetails( MyListString p)
        {
            property = p;
            derivedProperties = new List<MyListString>();
        }
    }
}
