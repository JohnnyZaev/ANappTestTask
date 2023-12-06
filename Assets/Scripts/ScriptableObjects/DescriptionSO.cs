using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace ScriptableObjects
{
    /// <summary>
    /// This is a base ScriptableObject that adds a description field.
    /// </summary>
    public class DescriptionSo : ScriptableObject
    {
        [FormerlySerializedAs("m_Description")]
        [TextArea(5, 20)]
        [SerializeField] [Optional] protected string mDescription;
    }
}