using System;
using System.Collections.Generic;
using ChemLab.Chemistry.Data;
using UnityEngine;

namespace ChemLab.Chemistry
{
    /// <summary>
    /// Пробирка/колба: хранит смесь и запускает реакции.
    /// </summary>
    public class ChemicalContainer : MonoBehaviour
    {
        [SerializeField] private List<ChemicalReactionData> knownReactions;
        [SerializeField] private Renderer liquidRenderer;
        [SerializeField] private ParticleSystem smokeFx;
        [SerializeField] private ParticleSystem bubblesFx;
        [SerializeField] private ParticleSystem precipitateFx;

        private readonly List<SubstanceStack> _mixture = new();
        private ReactionEngine _reactionEngine;

        public float TemperatureC { get; private set; } = 25f;
        public float PressureAtm { get; private set; } = 1f;
        public bool ElectrolysisEnabled { get; private set; }

        public event Action<ReactionResult> OnReactionHappened;

        private void Awake()
        {
            _reactionEngine = new ReactionEngine(knownReactions);
        }

        public void AddSubstance(ChemicalElementData element, float moles)
        {
            if (moles <= 0f || element == null)
                return;

            var index = _mixture.FindIndex(s => s.element == element);
            if (index >= 0)
            {
                var stack = _mixture[index];
                stack.moles += moles;
                _mixture[index] = stack;
            }
            else
            {
                _mixture.Add(new SubstanceStack(element, moles));
            }

            TryRunReaction();
            RefreshVisual();
        }

        public void SetTemperature(float valueC)
        {
            TemperatureC = valueC;
            TryRunReaction();
        }

        public void SetPressure(float valueAtm)
        {
            PressureAtm = Mathf.Max(0.01f, valueAtm);
            TryRunReaction();
        }

        public void EnableElectrolysis(bool enabled) => ElectrolysisEnabled = enabled;

        private void TryRunReaction()
        {
            var result = _reactionEngine.TryReact(_mixture, TemperatureC, PressureAtm, ElectrolysisEnabled);
            if (!result.HasReaction)
                return;

            OnReactionHappened?.Invoke(result);
            PlayReactionFx(result);
            RefreshVisual();
        }

        private void PlayReactionFx(ReactionResult result)
        {
            if (result.Smoke && smokeFx != null) smokeFx.Play();
            if (result.Bubbles && bubblesFx != null) bubblesFx.Play();
            if (result.Precipitate && precipitateFx != null) precipitateFx.Play();

            if (result.Explosion)
            {
                // Здесь можно дернуть систему урона и импульс Rigidbody.
                Debug.LogWarning("Explosion! Неправильная реакция привела к взрыву.");
            }

            if (result.ToxicGas)
            {
                // Здесь можно активировать механику токсичного облака.
                Debug.LogWarning("Toxic gas released!");
            }
        }

        private void RefreshVisual()
        {
            if (liquidRenderer == null || _mixture.Count == 0)
                return;

            var avgColor = Color.black;
            var sum = 0f;
            foreach (var stack in _mixture)
            {
                avgColor += stack.element.baseColor * stack.moles;
                sum += stack.moles;
            }

            avgColor /= Mathf.Max(0.0001f, sum);
            liquidRenderer.material.color = avgColor;
        }
    }
}
