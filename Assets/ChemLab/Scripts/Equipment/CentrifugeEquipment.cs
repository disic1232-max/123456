using ChemLab.Chemistry;
using UnityEngine;

namespace ChemLab.Equipment
{
    /// <summary>
    /// Упрощенная модель: центрифуга повышает "эффективное" давление.
    /// </summary>
    public class CentrifugeEquipment : LabEquipmentBase
    {
        [SerializeField] private float pressureBoostAtm = 0.5f;

        public override void Process(ChemicalContainer container, float deltaTime)
        {
            container.SetPressure(container.PressureAtm + pressureBoostAtm * processSpeed * deltaTime);
        }
    }
}
