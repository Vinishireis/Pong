using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PauseMenu : MonoBehaviour
{
    public event Action<int> OnDifficultyChanged = delegate { };

    [SerializeField] private Image background;
    [SerializeField] private Toggle[] difficultyToggles;

    private bool firstDisplay = true;
    private GameObject lastSelected;

    public bool Displayed
    {
        get { return gameObject.activeSelf; }
        set
        {
            gameObject.SetActive(value);
            CursorDisplayed = value;
            if (value)
            {
                SolidBackground = firstDisplay;
                firstDisplay = false;
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    private bool SolidBackground
    {
        set
        {
            if (background != null)
            {
                Color newColor = background.color;
                newColor.a = value ? 1 : 0.95f;
                background.color = newColor;
            }
            else
            {
                Debug.LogError("Background image is not assigned in the Inspector.");
            }
        }
    }

    private bool CursorDisplayed
    {
        set
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void Awake()
    {
        if (difficultyToggles == null || difficultyToggles.Length == 0)
        {
            Debug.LogError("Difficulty toggles are not assigned in the Inspector.");
        }
        else
        {
            for (int i = 0; i < difficultyToggles.Length; i++)
            {
                int index = i;
                difficultyToggles[i].onValueChanged.AddListener(
                    (bool on) => { if (on) { OnDifficultyChanged(index); } });
            }
        }
    }

    private void Start()
    {
        if (difficultyToggles != null)
        {
            for (int i = 0; i < difficultyToggles.Length; i++)
            {
                if (difficultyToggles[i].isOn) { OnDifficultyChanged(i); }
            }
        }
    }

    private void Update()
    {
        if (Displayed)
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                lastSelected = selected;
            }
        }
    }
}
