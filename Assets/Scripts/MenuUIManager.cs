using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Scripts
{
    public class MenuUIManager : MonoBehaviour
    {
        [SerializeField] private UIDocument TargetPanel;

        [SerializeField] private UIButtonEventReference[] ButtonReferences;

        private void Awake()
        {
            foreach (var button in ButtonReferences)
            {
                button.Initialize(TargetPanel.rootVisualElement);
            }
        }

        private void Update()
        {
            foreach (var button in ButtonReferences)
            {
                button.UpdateButton();
            }
        }

        private void OnEnable()
        {
            if (TargetPanel == null)
            {
                if (!TryGetComponent(out TargetPanel))
                {
                    Debug.LogWarning("No UI Document was found");
                    this.enabled = false;
                    return;
                }
                SetButtonRefState(true);
            }
            else
            {
                SetButtonRefState(true);
            }
        }

        private void OnDisable()
        {
            if (TargetPanel == null)
            {
                if (!TryGetComponent(out TargetPanel))
                {
                    Debug.LogWarning("No UI Document was found");
                    this.enabled = false;
                    return;
                }
                SetButtonRefState(false);
            }
            else
            {
                SetButtonRefState(false);
            }
        }

        private void SetButtonRefState(bool state)
        {
            if (ButtonReferences != null && ButtonReferences.Length > 0)
            {
                foreach (var reference in ButtonReferences)
                {
                    if (state) reference.OnEnable();
                    else reference.OnDisable();
                }
            }
        }
    }

    [System.Serializable]
    public class UIButtonEventReference
    {
        public string ButtonName;
        public UnityEvent OnClicked;
        public bool IsResetButton = false;

        private VisualElement button;

        public void UpdateButton()
        {
            if (IsResetButton)
            {
                if (Save.CheckResetStatus())
                {
                    button.SetEnabled(false);
                }
            }
        }

        public void Initialize(VisualElement root)
        {
            button = root.Q(ButtonName);

            UpdateButton();
        }

        public void OnEnable()
        {
            button.RegisterCallback<ClickEvent>(UIT_OnClicked);
        }

        public void OnDisable()
        {
            button.UnregisterCallback<ClickEvent>(UIT_OnClicked);
        }

        public void UIT_OnClicked(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(AudioSelection.Select);
            OnClicked.Invoke();
        }
    }
}
