using ChemLab.Chemistry;
using UnityEngine;

namespace ChemLab.Equipment
{
    /// <summary>
    /// Заготовка: здесь можно отделять осадок по правилам растворимости.
    /// </summary>
    public class FilterEquipment : LabEquipmentBase
    {
        public override void Process(ChemicalContainer container, float deltaTime)
        {
            // TODO: реализация фильтрации осадка в отдельный контейнер.
        }
    }
}
