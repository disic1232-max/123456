using UnityEngine;

namespace ChemLab.Chemistry.Data
{
    /// <summary>
    /// Базовые данные химического элемента/соединения.
    /// </summary>
    [CreateAssetMenu(menuName = "ChemLab/Chemistry/Element", fileName = "Element_")]
    public class ChemicalElementData : ScriptableObject
    {
        [Header("Identity")]
        public string elementId;
        public string displayName;
        public string formula;

        [Header("Physical")]
        public ChemicalState defaultState;
        public float meltingPointC;
        public float boilingPointC;
        [Range(0f, 1f)] public float reactivity = 0.5f;
        [Range(0f, 14f)] public float ph = 7f;
        public bool radioactive;

        [Header("Visual")]
        public Color baseColor = Color.white;
    }
}
