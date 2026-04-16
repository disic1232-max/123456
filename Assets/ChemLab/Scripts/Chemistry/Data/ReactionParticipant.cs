using UnityEngine;

namespace ChemLab.Chemistry.Data
{
    [System.Serializable]
    public struct ReactionParticipant
    {
        public ChemicalElementData element;
        [Min(0.01f)] public float moles;
    }
}
