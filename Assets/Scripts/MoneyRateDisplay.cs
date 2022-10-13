using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts
{
    [RequireComponent(typeof(UIDocument))]
    public class MoneyRateDisplay : MonoBehaviour
    {
        protected UIDocument document;

        private Label Target;

        public static string TextToDisplay = "0$/s";

        private void Start()
        {
            document = GetComponent<UIDocument>();

            Target = document.rootVisualElement.Q("MoneyRateDisplay") as Label;
        }

        private void FixedUpdate()
        {
            Target.text = TextToDisplay;
        }
    }
}
