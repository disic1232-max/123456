using System.Collections.Generic;
using System.Linq;
using ChemLab.Chemistry.Data;
using UnityEngine;

namespace ChemLab.Chemistry
{
    /// <summary>
    /// Проверяет условия и применяет реакции к смеси.
    /// </summary>
    public class ReactionEngine
    {
        private readonly IReadOnlyList<ChemicalReactionData> _reactions;

        public ReactionEngine(IReadOnlyList<ChemicalReactionData> reactions)
        {
            _reactions = reactions;
        }

        public ReactionResult TryReact(
            List<SubstanceStack> mixture,
            float temperatureC,
            float pressureAtm,
            bool electrolysisEnabled)
        {
            foreach (var reaction in _reactions)
            {
                if (!CheckConditions(reaction, mixture, temperatureC, pressureAtm, electrolysisEnabled))
                    continue;

                ApplyStoichiometry(reaction, mixture);
                return new ReactionResult
                {
                    HasReaction = true,
                    Explosion = reaction.canExplode,
                    ToxicGas = reaction.canReleaseToxicGas,
                    Bubbles = reaction.spawnBubbles,
                    Smoke = reaction.spawnSmoke,
                    Precipitate = reaction.spawnPrecipitate
                };
            }

            return default;
        }

        private static bool CheckConditions(
            ChemicalReactionData reaction,
            List<SubstanceStack> mixture,
            float temperatureC,
            float pressureAtm,
            bool electrolysisEnabled)
        {
            var c = reaction.conditions;

            if (temperatureC < c.temperatureRangeC.x || temperatureC > c.temperatureRangeC.y)
                return false;
            if (pressureAtm < c.minPressureAtm || pressureAtm > c.maxPressureAtm)
                return false;
            if (c.requiresElectrolysis && !electrolysisEnabled)
                return false;

            if (c.requiredCatalyst != null && !mixture.Any(s => s.element == c.requiredCatalyst && s.moles > 0f))
                return false;

            foreach (var reactant in reaction.reactants)
            {
                var have = mixture.FirstOrDefault(s => s.element == reactant.element).moles;
                if (have < reactant.moles)
                    return false;
            }

            return true;
        }

        private static void ApplyStoichiometry(ChemicalReactionData reaction, List<SubstanceStack> mixture)
        {
            foreach (var reactant in reaction.reactants)
            {
                ChangeAmount(mixture, reactant.element, -reactant.moles);
            }

            foreach (var product in reaction.products)
            {
                ChangeAmount(mixture, product.element, product.moles);
            }

            mixture.RemoveAll(s => s.moles <= 0.0001f);
        }

        private static void ChangeAmount(List<SubstanceStack> mixture, ChemicalElementData element, float delta)
        {
            var index = mixture.FindIndex(s => s.element == element);
            if (index >= 0)
            {
                var stack = mixture[index];
                stack.moles += delta;
                mixture[index] = stack;
                return;
            }

            if (delta > 0f)
                mixture.Add(new SubstanceStack(element, delta));
        }
    }
}
