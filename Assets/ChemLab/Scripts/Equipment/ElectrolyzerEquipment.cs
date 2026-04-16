using ChemLab.Chemistry;

namespace ChemLab.Equipment
{
    public class ElectrolyzerEquipment : LabEquipmentBase
    {
        public override void Process(ChemicalContainer container, float deltaTime)
        {
            container.EnableElectrolysis(true);
        }
    }
}
