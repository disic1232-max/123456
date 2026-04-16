using System.Collections.Generic;
using ChemLab.Chemistry.Data;
using TMPro;
using UnityEngine;

namespace ChemLab.UI
{
    /// <summary>
    /// Tab: открытые элементы, реакции, подсказки.
    /// </summary>
    public class ChemistryCodexUI : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TMP_Text elementsText;
        [SerializeField] private TMP_Text reactionsText;
        [SerializeField] private TMP_Text hintsText;

        private readonly HashSet<string> _openedElements = new();
        private readonly HashSet<string> _openedReactions = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                root.SetActive(!root.activeSelf);
                Refresh();
            }
        }

        public void RegisterElement(ChemicalElementData element)
        {
            if (element == null) return;
            _openedElements.Add($"{element.displayName} ({element.formula})");
        }

        public void RegisterReaction(ChemicalReactionData reaction)
        {
            if (reaction == null) return;
            _openedReactions.Add(reaction.description);
        }

        public void SetHints(string hints) => hintsText.text = hints;

        private void Refresh()
        {
            elementsText.text = string.Join("\n", _openedElements);
            reactionsText.text = string.Join("\n", _openedReactions);
        }
    }
}
