using UnityEngine;

namespace ChemLab.Chemistry.Data
{
    /// <summary>
    /// Описание одной химической реакции.
    /// </summary>
    [CreateAssetMenu(menuName = "ChemLab/Chemistry/Reaction", fileName = "Reaction_")]
    public class ChemicalReactionData : ScriptableObject
    {
        [Header("Meta")]
        public string reactionId;
        [TextArea] public string description;
        public bool hiddenEasterEgg;

        [Header("Stoichiometry")]
        public ReactionParticipant[] reactants;
        public ReactionParticipant[] products;

        [Header("Conditions")]
        public ReactionConditionData conditions;
        public float energyDeltaKJ;
        public bool canExplode;
        public bool canReleaseToxicGas;

        [Header("Visual Effects")]
        public Color targetColor = Color.white;
        public bool spawnBubbles;
        public bool spawnSmoke;
        public bool spawnPrecipitate;
    }
}
