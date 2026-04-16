using ChemLab.Chemistry;
using UnityEngine;

namespace ChemLab.Equipment
{
    public abstract class LabEquipmentBase : MonoBehaviour
    {
        [SerializeField] protected float processSpeed = 1f;

        public abstract void Process(ChemicalContainer container, float deltaTime);
    }
}
