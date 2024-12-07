using System.Collections.Generic;
using System;

using UnityEngine;
using System.Linq;

namespace HerghysStudio.Survivor.Identifiables
{
    public class ObjectId<T>
    {
        public T Id { get; }

        public ObjectId(T id = default) => this.Id = id;

        public static ObjectId<T> Any => new ObjectId<T>();

        public static ObjectId<T> GetSpecific(T id) => new ObjectId<T>(id);

        public static List<T> GetAllIds()
        {
            if (typeof(T).IsEnum)
            {
                return Enum.GetValues(typeof(T)).Cast<T>().ToList();
            }

            return new List<T>();
        }

        public static bool IsAny(T id)
        {
            return EqualityComparer<T>.Default.Equals(id, default);
        }
    }
}
