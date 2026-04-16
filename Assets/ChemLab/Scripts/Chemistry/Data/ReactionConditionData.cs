using UnityEngine;

namespace ChemLab.Chemistry.Data
{
    [System.Serializable]
    public struct ReactionConditionData
    {
        public Vector2 temperatureRangeC;
        public ChemicalElementData requiredCatalyst;
        [Range(0f, 5f)] public float minPressureAtm;
        [Range(0f, 5f)] public float maxPressureAtm;
        public bool requiresElectrolysis;
    }
}
