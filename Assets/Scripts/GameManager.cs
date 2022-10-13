using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

using BreakInfinity;
using static BreakInfinity.BigDouble;

namespace Scripts
{
    [RequireComponent(typeof(UIDocument))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static Action<BigDouble> OnGameTick;

        [SerializeField] private VisualTreeAsset visualTreeAsset;

        [SerializeField] private Generator[] generators;

        private UIDocument TargetPanel;

        [HideInInspector] public BigDouble baseClickGeneration = 1;

        public BigDouble ClickGeneration
        {
            get
            {
                BigDouble generation = baseClickGeneration;
                foreach (Generator generator in generators)
                {
                    if (generator.isCpsMultiplier && generator.ownedCount > 0)
                        generation += generator.Generation;
                }
                return generation;
            }
        }

        /// <summary>
        /// generation per second
        /// </summary>
        public BigDouble MoneyGeneration
        {
            get
            {
                BigDouble generation = 0;
                foreach (Generator generator in generators)
                {
                    if (!generator.isCpsMultiplier)
                        generation += generator.Generation;
                }
                return generation;
            }
        }

        private Save save;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
            save.SaveData();
        }

        private void Start()
        {
            TargetPanel = GetComponent<UIDocument>();

            save = Save.LoadData();

            if (!save.IsSavedataReset)
            {
                // Load save data...
                foreach (var generator in generators)
                {
                    generator.ownedCount = save.generators[generator.ResourceName];
                }
            }
            else
            {
                // Generate save data...
                foreach (var generator in generators)
                {
                    save.generators.Add(generator.ResourceName, 0);
                }
            }

            AwayTimeCalculate();

            TargetPanel.rootVisualElement.Q("CloseButton").RegisterCallback<ClickEvent>(OnQuitClicked);

            Init(TargetPanel.rootVisualElement);

            StartCoroutine(SaveTimer());
            StartCoroutine(GameTicker());

            Debug.Log($"Money per click: {ClickGeneration}");
        }

        private void OnQuitClicked(ClickEvent e)
        {
            StopAllCoroutines();
            save.SaveData();
            SceneManager.LoadScene(0);
        }

        private void UpdateTextDisplays()
        {
            // Money Display...
            MoneyDisplay.TextToDisplay = NotationMethod(save.Money);
            // Money Rate Display...
            MoneyRateDisplay.TextToDisplay = $"{NotationMethod(MoneyGeneration)}/second";
        }

        public static string NotationMethod(BigDouble x)
        {
            var prefixes = new Dictionary<BigDouble, string>
            {
                { 3, "Thousand" },
                { 6, "Million" },
                { 9, "Billion" },
                { 12, "Trillion" },
                { 15, "Quadrillion" },
                { 18, "Quintillion" },
                { 21, "Sextillion" },
                { 24, "Septillion" },
                { 27, "Octillion" },
                { 30, "Nonillion" },
                { 33, "Decillion" },
                { 36, "Undecillion" },
                { 39, "Duodecillion" },
                { 42, "Tredecillion" },
                { 45, "Quattuordecillion" },
                { 48, "Quindecillion" },
                { 51, "Sexdecillion" },
                { 54, "Septendecillion" },
                { 57, "Octodecillion" },
                { 60, "Novemdecillion" },
                { 63, "Vigintillion" },
                { 66, "Unvigintillion" },
                { 69, "Duovigintillion" },
                { 72, "Trevigintillion" },
            };

            var exponent = Floor(Log10(x));
            var exponentEng = 3 * Floor(exponent / 3);

            var mantissa = x / Pow(10, exponentEng);

            if (x <= 1000) return x.ToString("F2");
            if (x >= 1e75) return mantissa.ToString("F2") + "e" + exponentEng;
            return mantissa.ToString("F2") + " " + prefixes[exponentEng];
        }

        private void AwayTimeCalculate()
        {
            var awayTime = (double)Mathf.Clamp((int)(System.DateTime.Now - save.SaveDate).TotalSeconds, 0, 14400);

            var generated = MoneyGeneration * awayTime;

            save.Money += generated.ToDouble();
        }

        private void Init(VisualElement root)
        {
            var buyList = root.Q("unity-content-container");

            foreach (var generator in generators)
            {
                generator.Initialize(buyList, visualTreeAsset);
                generator.OnTryBuy += OnTryBuyItem;
            }
        }

        public void ClickerClicked()
        {
            Debug.Log("Clicker clicked");
            if (ClickGeneration == 0) save.Money += 1;
            else save.Money += ClickGeneration.ToDouble();
            AudioManager.Instance.PlaySound(AudioSelection.Hit);
        }

        private void OnTryBuyItem(string ResourceName, BigDouble cost)
        {
            if (save.Money >= cost)
            {
                save.Money -= cost.ToDouble();
                var gen = generators.First(x => x.ResourceName == ResourceName);
                gen.ownedCount++;
                save.generators[ResourceName]++;
                gen.UpdateDisplays();
                AudioManager.Instance.PlaySound(AudioSelection.Upgrade);
            }
        }

        private IEnumerator SaveTimer()
        {
            while (true)
            {
                save.SaveData();
                Debug.Log("Save");
                yield return new WaitForSeconds(300);
            }
        }

        private IEnumerator GameTicker()
        {
            while (true)
            {
                //Tick all generators...
                save.Money += MoneyGeneration.ToDouble();
                OnGameTick.Invoke(save.Money);
                UpdateTextDisplays();
                Debug.Log("Tick");
                yield return new WaitForSeconds(1);
            }
        }
    }
}
