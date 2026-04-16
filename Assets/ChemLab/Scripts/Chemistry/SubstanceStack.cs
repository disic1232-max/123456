using ChemLab.Chemistry.Data;

namespace ChemLab.Chemistry
{
    /// <summary>
    /// Количество конкретного вещества в молях.
    /// </summary>
    [System.Serializable]
    public struct SubstanceStack
    {
        public ChemicalElementData element;
        public float moles;

        public SubstanceStack(ChemicalElementData element, float moles)
        {
            this.element = element;
            this.moles = moles;
        }
    }
}
