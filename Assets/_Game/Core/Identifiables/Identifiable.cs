using UnityEngine;

namespace HerghysStudio.Survivor.Identifiables
{
    public interface Identifiable<T>
    {
        public T Id { get; set; }
    }
}
