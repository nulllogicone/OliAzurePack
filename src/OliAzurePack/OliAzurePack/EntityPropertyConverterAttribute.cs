using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OliAzurePack
{
    // https://www.devprotocol.com/azure-table-storage-and-complex-types-stored-in-json/

    [AttributeUsage(AttributeTargets.Property)]
    public class EntityPropertyConverterAttribute : Attribute
    {
        public Type ConvertToType;

        public EntityPropertyConverterAttribute()
        {

        }
        public EntityPropertyConverterAttribute(Type convertToType)
        {
            ConvertToType = convertToType;
        }
    }
}
