using ChemLab.Chemistry;
using UnityEngine;

namespace ChemLab.Equipment
{
    public class CoolerEquipment : LabEquipmentBase
    {
        [SerializeField] private float targetTemperature = -40f;

        public override void Process(ChemicalContainer container, float deltaTime)
        {
            var t = Mathf.MoveTowards(container.TemperatureC, targetTemperature, processSpeed * deltaTime);
            container.SetTemperature(t);
        }
    }
}
