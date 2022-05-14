using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HyperCore.Utility
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class AddJsonTypenameAttribute : Attribute
    {
    }

    public class AddJsonTypenameContractResolver : DefaultContractResolver
    {
        public new static readonly AddJsonTypenameContractResolver Instance = new();
        

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            return base.CreateProperty(member, memberSerialization).ApplyAddTypenameAttribute();
        }

        protected override JsonArrayContract CreateArrayContract(Type objectType)
        {
            return base.CreateArrayContract(objectType).ApplyAddTypenameAttribute();
        }
    }

    public static class ContractResolverExtensions
    {
        public static JsonProperty ApplyAddTypenameAttribute(this JsonProperty jsonProperty)
        {
            if (jsonProperty.TypeNameHandling == null)
            {
                if (jsonProperty.PropertyType.GetCustomAttribute<AddJsonTypenameAttribute>(false) != null)
                {
                    jsonProperty.TypeNameHandling = TypeNameHandling.All;
                }
            }

            return jsonProperty;
        }

        public static JsonArrayContract ApplyAddTypenameAttribute(this JsonArrayContract contract)
        {
            if (contract.ItemTypeNameHandling == null)
            {
                if (contract.CollectionItemType.GetCustomAttribute<AddJsonTypenameAttribute>(false) != null)
                {
                    contract.ItemTypeNameHandling = TypeNameHandling.All;
                }
            }

            return contract;
        }
    }
}