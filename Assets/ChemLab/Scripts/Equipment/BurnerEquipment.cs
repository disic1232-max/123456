using ChemLab.Chemistry;
using UnityEngine;

namespace ChemLab.Equipment
{
    public class BurnerEquipment : LabEquipmentBase
    {
        [SerializeField] private float maxTemperature = 1200f;

        public override void Process(ChemicalContainer container, float deltaTime)
        {
            var t = Mathf.MoveTowards(container.TemperatureC, maxTemperature, processSpeed * deltaTime);
            container.SetTemperature(t);
        }
    }
}
