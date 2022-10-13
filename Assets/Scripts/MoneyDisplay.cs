using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts
{
    [RequireComponent(typeof(UIDocument))]
    public class MoneyDisplay : MonoBehaviour
    {
        protected UIDocument document;

        private Label Target;

        public static string TextToDisplay = "0$";

        private void Start()
        {
            document = GetComponent<UIDocument>();

            Target = document.rootVisualElement.Q("MoneyDisplay") as Label;
        }

        private void FixedUpdate()
        {
            Target.text = TextToDisplay;
        }
    }
}
