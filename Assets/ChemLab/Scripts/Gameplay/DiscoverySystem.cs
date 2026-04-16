using System.Collections.Generic;
using ChemLab.Chemistry.Data;
using UnityEngine;

namespace ChemLab.Gameplay
{
    /// <summary>
    /// Система открытий: хранит найденные элементы и реакции.
    /// </summary>
    public class DiscoverySystem : MonoBehaviour
    {
        private readonly HashSet<string> _discoveredElements = new();
        private readonly HashSet<string> _discoveredReactions = new();

        public bool DiscoverElement(ChemicalElementData element)
        {
            if (element == null) return false;
            return _discoveredElements.Add(element.elementId);
        }

        public bool DiscoverReaction(ChemicalReactionData reaction)
        {
            if (reaction == null) return false;
            return _discoveredReactions.Add(reaction.reactionId);
        }

        public bool IsElementDiscovered(string elementId) => _discoveredElements.Contains(elementId);
        public bool IsReactionDiscovered(string reactionId) => _discoveredReactions.Contains(reactionId);
    }
}
