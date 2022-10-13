using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class MainMenuLogic : MonoBehaviour
    {
        public void OnPlayClicked()
        {
            SceneManager.LoadScene(1);
        }

        public void OnResetClicked()
        {
            Save.ResetData();
        }

        public void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
