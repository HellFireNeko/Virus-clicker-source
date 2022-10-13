using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts
{
    [RequireComponent(typeof(UIDocument))]
    public class ClickerButton : MonoBehaviour
    {
        private VisualElement button;

        private void Start()
        {
            var doc = GetComponent<UIDocument>();

            button = doc.rootVisualElement.Q("ClickerButton");

            button.RegisterCallback<ClickEvent>(OnClick);
        }

        private void OnEnable()
        {
            if (button != null)
                button.RegisterCallback<ClickEvent>(OnClick);
        }

        private void OnDisable()
        {
            if (button != null)
                button.UnregisterCallback<ClickEvent>(OnClick);
        }

        private void OnClick(ClickEvent e)
        {
            Debug.Log("Click");
            GameManager.Instance.ClickerClicked();
        }
    }
}
