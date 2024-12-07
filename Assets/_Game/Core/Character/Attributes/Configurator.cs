using System;
using System.Collections.Generic;
using System.Linq;

using HerghysStudio.Survivor.Identifiables;
using HerghysStudio.Survivor.Stats;
using HerghysStudio.Survivor.Utility;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class Configurator<TId, TStats> where TStats : Identifiable<TId>
    {
        private readonly Dictionary<TId, List<Action<TStats>>> _configurations = new();

        /// <summary>
        /// Add by Id
        /// </summary>
        /// <param Name="id"></param>
        /// <param Name="action"></param>
        public void Add(TId id, Action<TStats> action)
        {
            Console.WriteLine(id);
            if (!_configurations.ContainsKey(id))
            {
                _configurations[id] = new();
            }

            // Add the action to the list associated with the specified key
            _configurations[id].Add(action);
        }

        /// <summary>
        /// Add By ObjectID
        /// </summary>
        /// <param Name="objectId"></param>
        /// <param Name="action"></param>
        public void Add(ObjectId<TId> objectId, Action<TStats> action)
        {
            // If the Id is the default value, apply the action to all weapons
            if (EqualityComparer<TId>.Default.Equals(objectId.Id, default))
            {
                foreach (var id in ObjectId<TId>.GetAllIds())
                {
                    Add(id, action);
                }
            }
            else
            {
                Add(objectId.Id, action); // Add action for specific ID
            }
        }

        /// <summary>
        /// Remove By Id
        /// </summary>
        /// <param Name="id"></param>
        /// <param Name="action"></param>
        public void Remove(TId id, Action<TStats> action)
        {
            if (_configurations.ContainsKey((id)))
            {
                _configurations[id].Remove(action);

                if (_configurations[id].Count == 0)
                {
                    _configurations.Remove(id);
                }
            }
        }

        /// <summary>
        /// Remove by ObjectId
        /// </summary>
        /// <param Name="objectId"></param>
        /// <param Name="action"></param>
        public void Remove(ObjectId<TId> objectId, Action<TStats> action)
        {
            // If the objectId wraps all IDs, remove the action from all
            if (objectId.Id.Equals(default(TId)))
            {
                foreach (var key in _configurations.Keys)
                {
                    _configurations[key].Remove(action);
                }
            }
            else
            {
                Remove(objectId.Id, action);
            }
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param Name="stats"></param>
        public void Configure(TStats stats)
        {
            Console.WriteLine("Configure");
            if (_configurations.ContainsKey(stats.Id))
            {
                foreach (var action in _configurations[stats.Id])
                {
                    Console.WriteLine(stats.Id);
                    action(stats);
                }
            }
        }
    }
}