using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PauseMenu : Menu
{
    [SerializeField] GameObject pauseMenuUi;

    [Inject] GameManager _gameManager;

    PlayerInputActions _playerControls;

    void Awake()
    {
        _playerControls = new PlayerInputActions();

        pauseMenuUi.SetActive(false);
    }

    void OnEnable()
    {
        _playerControls.Player.Pause.Enable();
        _playerControls.Player.Pause.performed += (_) => OnPauseInput();
        _gameManager.OnGamePausedChange += OnGamePauseChange;
    }

    void OnDisable()
    {
        _playerControls.Player.Pause.Disable();
        _gameManager.OnGamePausedChange -= OnGamePauseChange;
    }

    void OnPauseInput()
    {
        _gameManager.SetPaused(!_gameManager.isPaused);
    }

    void OnGamePauseChange(bool paused)
    {
        if (paused) PauseCallback();
        else ResumeCallback();
    }

    void PauseCallback()
    {
        menuManager.CloseAll();
        menuManager.OpenMenu(pauseMenuUi);
    }

    void ResumeCallback()
    {
        menuManager.CloseAll();
    }

    public void Quit()
    {
        GameUtilities.Quit();
    }
}