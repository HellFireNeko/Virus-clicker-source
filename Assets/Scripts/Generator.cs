using System;
using UnityEngine;
using UnityEngine.UIElements;

using BreakInfinity;
using static BreakInfinity.BigDouble;

namespace Scripts
{
    [Serializable]
    public class Generator
    {
        public event Action<string, BigDouble> OnTryBuy;

        public string ResourceName;
        [Multiline] public string Description;

        public bool isCpsMultiplier;

        [SerializeField] private double m_baseGeneration;
        [HideInInspector] public BigDouble baseGeneration = 0;
        [SerializeField] private double m_generationMultiplier;
        [HideInInspector] public BigDouble generationMultiplier = 0;

        public BigDouble Generation
        {
            get
            {
                if (ownedCount == 0) return 0;
                return baseGeneration * (generationMultiplier * ownedCount);
            }
        }

        [SerializeField] private double m_baseCost;
        [HideInInspector] public BigDouble baseCost = 0;
        [SerializeField] private double m_costMultiplier;
        [HideInInspector] public BigDouble costMultiplier = 0;

        public BigDouble Cost
        {
            get
            {
                return baseCost * Pow(costMultiplier, ownedCount);
            }
        }

        public int ownedCount = 0;

        private VisualElement button;
        private Label countLabel;
        private Label costLabel;

        public void Initialize(VisualElement target, VisualTreeAsset asset)
        {
            baseGeneration = m_baseGeneration;
            baseCost = m_baseCost;
            generationMultiplier = m_generationMultiplier;
            costMultiplier = m_costMultiplier;

            asset.CloneTree(target);
            var nameTemplate = target.Q("NameTemplate");
            nameTemplate.name = ResourceName;
            var nameLabel = nameTemplate.Q("NameLabel") as Label;
            nameLabel.text = ResourceName;
            var descriptionLabel = nameTemplate.Q("DescriptionText") as Label;
            descriptionLabel.text = Description;
            countLabel = nameTemplate.Q("CountLabel") as Label;
            countLabel.text = $"{ownedCount}";
            costLabel = nameTemplate.Q("CostLabel") as Label;
            costLabel.text = GameManager.NotationMethod(Cost);
            button = nameTemplate.Q("BuyButton");
            button.RegisterCallback<ClickEvent>(OnBuy);

            GameManager.OnGameTick += UpdateButton;
        }

        private void OnBuy(ClickEvent e)
        {
            OnTryBuy.Invoke(ResourceName, Cost);
        }

        public void UpdateDisplays()
        {
            countLabel.text = $"{ownedCount}";
            costLabel.text = $"Cost: {GameManager.NotationMethod(Cost)}";
        }

        public void UpdateButton(BigDouble currentMoney)
        {
            if (currentMoney >= Cost)
            {
                button.SetEnabled(true);
            }
            else
            {
                button.SetEnabled(false);
            }
        }
    }
}
