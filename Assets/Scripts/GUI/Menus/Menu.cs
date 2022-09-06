using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class Menu : MonoBehaviour
{
    [SerializeField] Selectable _defaultSelected;

    [Inject] protected MenuManager menuManager;

    void OnEnable()
    {
        if (_defaultSelected)
            _defaultSelected.Select();
    }

    public void GoBackOrResume()
    {
        menuManager.GoBackOrResume();
    }

    public void GoBack()
    {
        menuManager.GoBack();
    }

    public void OpenMenu(GameObject menu)
    {
        menuManager.OpenMenu(menu);
    }
}