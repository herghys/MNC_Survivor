/*
 * Copyright (c) L�o CHAUMARTIN 2021-2023
 * All Rights Reserved
 * 
 * File: ImpostorReference.cs
 */

using UnityEngine;

namespace AutoLOD.Impostors
{
    /// <summary>
    /// This class keeps a reference an impostor GameObject to be able to detect if a LODGroup
    /// already have an impostor in it. It is used for baking, to be able replace an existing impostor.
    /// Used in editor-only scope.
    /// </summary>
    [DisallowMultipleComponent]
    public class ImpostorReference : MonoBehaviour
    {
        /// <summary>
        /// The impostor gameObject
        /// </summary>
        public GameObject impostorObject;
    }
}
