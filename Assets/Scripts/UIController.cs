/*using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text resourcesText;
    [SerializeField] private GameObject noResourcesText;

    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject startPanel;

    private bool _isGamePaused = false;
    private bool _isHelpMenu = false;
    private bool _isSpedUp = false;
    private bool _hasGameStarted = false;

    [SerializeField] private TowerData[] towers;
    private List<GameObject> activeCards = new List<GameObject>();

    private Platform _currentPlatform;

    private void Start()
    {
        _hasGameStarted = false;
        _isSpedUp = false;
        startPanel.SetActive(true);
        GameManager.Instance.SetTimeScale(0f);
    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLiveschanged += UpdateLivesText;
        GameManager.OnResourcesChanged += UpdateResourcesText;
        Platform.OnPlatformClicked += HandlePlatformClicked;
        TowerCard.OnTowerSelected += HandleTowerSelected;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLiveschanged -= UpdateLivesText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
        Platform.OnPlatformClicked -= HandlePlatformClicked;
        TowerCard.OnTowerSelected -= HandleTowerSelected;
    }

    public void StartGame()
    {
        _hasGameStarted = true;
        startPanel.SetActive(false);
        GameManager.Instance.SetTimeScale(1f);
    }

    private void ResumeTime()
    {
        if (!_hasGameStarted)
        {
            GameManager.Instance.SetTimeScale(0f);
            return;
        }

        float targetScale = _isSpedUp ? 2f : 1f;
        GameManager.Instance.SetTimeScale(targetScale);
    }

    private void UpdateWaveText(int currentWave)
    {
        waveText.text = $"Wave: {currentWave + 1}";
        if (currentWave >= 20)
        {
            ShowVictory();
        }
    }

    private void UpdateLivesText(int currentLives)
    {
        livesText.text = $"Lives: {currentLives}";

        if (currentLives <= 0)
        {
            ShowGameOver();
        }
    }

    private void UpdateResourcesText(int currentResources)
    {
        resourcesText.text = $"Materials: {currentResources}";
    }

    private void HandlePlatformClicked(Platform platform)
    {
        _currentPlatform = platform;
        ShowTowerPanel();
    }

    private void ShowTowerPanel()
    {
        if (_isGamePaused) return;

        towerPanel.SetActive(true);
        Platform.towerPanelOpen = true;
        GameManager.Instance.SetTimeScale(0f);
        PopulateTowerCards();
    }

    public void hideTowerPanel()
    {
        towerPanel.SetActive(false);
        Platform.towerPanelOpen = false;
        ResumeTime();
    }

    private void PopulateTowerCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        foreach (var data in towers)
        {
            GameObject cardGameObject = Instantiate(towerCardPrefab, cardsContainer);
            TowerCard card = cardGameObject.GetComponent<TowerCard>();
            card.Initialize(data);
            activeCards.Add(cardGameObject);
        }
    }

    public void FastForwardToggle()
    {
        if (_isGamePaused || towerPanel.activeSelf || !_hasGameStarted) return;

        if (!_isSpedUp)
        {
            GameManager.Instance.SetTimeScale(2f);
            _isSpedUp = true;
        }
        else
        {
            GameManager.Instance.SetTimeScale(1f);
            _isSpedUp = false;
        }
    }

    private void HandleTowerSelected(TowerData towerData)
    {
        if (GameManager.Instance.Resources >= towerData.cost)
        {
            GameManager.Instance.SpendResources(towerData.cost);
            _currentPlatform.PlaceTower(towerData);
        }
        else
        {
            StartCoroutine(ShowNoResourcesMessage());
        }

        hideTowerPanel();
    }

    private IEnumerator ShowNoResourcesMessage()
    {
        noResourcesText.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        noResourcesText.SetActive(false);
    }

    public void togglePause()
    {
        if (towerPanel.activeSelf || !_hasGameStarted) return;

        if (_isGamePaused)
        {
            pausePanel.SetActive(false);
            _isGamePaused = false;
            ResumeTime();
        }
        else
        {
            pausePanel.SetActive(true);
            _isGamePaused = true;
            GameManager.Instance.SetTimeScale(0f);
        }
    }

    public void toggleHelp()
    {
        if (towerPanel.activeSelf || _isGamePaused) return;

        if (_isHelpMenu)
        {
            helpPanel.SetActive(false);
            _isHelpMenu = false;
            ResumeTime();
        }
        else
        {
            helpPanel.SetActive(true);
            _isHelpMenu = true;
            GameManager.Instance.SetTimeScale(0f);
        }
    }

    public void restartGame()
    {
        _isSpedUp = false;
        GameManager.Instance.SetTimeScale(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void quitToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    private void ShowGameOver()
    {
        GameManager.Instance.SetTimeScale(0f);
        gameOverPanel.SetActive(true);
    }

    private void ShowVictory()
    {
        GameManager.Instance.SetTimeScale(0f);
        victoryPanel.SetActive(true);
    }

    public void LoadNextScene()
    {
        Platform.towerPanelOpen = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}


using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text resourcesText;
    [SerializeField] private GameObject noResourcesText;

    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject startPanel;

    private bool _isGamePaused = false;
    private bool _isHelpMenu = false;
    private bool _isSpedUp = false;
    private bool _hasGameStarted = false;

    [SerializeField] private TowerData[] towers;
    private List<GameObject> activeCards = new List<GameObject>();

    private Platform _currentPlatform;

    private void Start()
    {
        Platform.towerPanelOpen = false;
        _hasGameStarted = false;
        _isSpedUp = false;
        startPanel.SetActive(true);
        GameManager.Instance.SetTimeScale(0f);
    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLiveschanged += UpdateLivesText;
        GameManager.OnResourcesChanged += UpdateResourcesText;
        Platform.OnPlatformClicked += HandlePlatformClicked;
        TowerCard.OnTowerSelected += HandleTowerSelected;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLiveschanged -= UpdateLivesText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
        Platform.OnPlatformClicked -= HandlePlatformClicked;
        TowerCard.OnTowerSelected -= HandleTowerSelected;
    }

    public void StartGame()
    {
        _hasGameStarted = true;
        startPanel.SetActive(false);
        GameManager.Instance.SetTimeScale(1f);
    }

    private void ResumeTime()
    {
        if (!_hasGameStarted)
        {
            GameManager.Instance.SetTimeScale(0f);
            return;
        }

        float targetScale = _isSpedUp ? 2f : 1f;
        GameManager.Instance.SetTimeScale(targetScale);
    }

    private void UpdateWaveText(int currentWave)
    {
        waveText.text = $"Wave: {currentWave + 1}";
        if (currentWave >= 20)
        {
            ShowVictory();
        }
    }

    private void UpdateLivesText(int currentLives)
    {
        livesText.text = $"Lives: {currentLives}";

        if (currentLives <= 0)
        {
            ShowGameOver();
        }
    }

    private void UpdateResourcesText(int currentResources)
    {
        resourcesText.text = $"Materials: {currentResources}";
    }

    private void HandlePlatformClicked(Platform platform)
    {
        if (_isGamePaused || _isHelpMenu) return;
        _currentPlatform = platform;
        ShowTowerPanel();
    }

    private void ShowTowerPanel()
    {
        if (_isGamePaused || _isHelpMenu) return;

        towerPanel.SetActive(true);
        Platform.towerPanelOpen = true;
        GameManager.Instance.SetTimeScale(0f);
        PopulateTowerCards();
    }

    public void hideTowerPanel()
    {
        towerPanel.SetActive(false);
        Platform.towerPanelOpen = false;
        ResumeTime();
    }

    private void PopulateTowerCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        foreach (var data in towers)
        {
            GameObject cardGameObject = Instantiate(towerCardPrefab, cardsContainer);
            TowerCard card = cardGameObject.GetComponent<TowerCard>();
            card.Initialize(data);
            activeCards.Add(cardGameObject);
        }
    }

    public void FastForwardToggle()
    {
        if (_isGamePaused || towerPanel.activeSelf || !_hasGameStarted || _isHelpMenu) return;

        if (!_isSpedUp)
        {
            GameManager.Instance.SetTimeScale(2f);
            _isSpedUp = true;
        }
        else
        {
            GameManager.Instance.SetTimeScale(1f);
            _isSpedUp = false;
        }
    }

    private void HandleTowerSelected(TowerData towerData)
    {
        if (GameManager.Instance.Resources >= towerData.cost)
        {
            GameManager.Instance.SpendResources(towerData.cost);
            _currentPlatform.PlaceTower(towerData);
        }
        else
        {
            StartCoroutine(ShowNoResourcesMessage());
        }

        hideTowerPanel();
    }

    private IEnumerator ShowNoResourcesMessage()
    {
        noResourcesText.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        noResourcesText.SetActive(false);
    }

    public void togglePause()
    {
        if (towerPanel.activeSelf || !_hasGameStarted || _isHelpMenu) return;

        if (_isGamePaused)
        {
            pausePanel.SetActive(false);
            _isGamePaused = false;
            ResumeTime();
        }
        else
        {
            pausePanel.SetActive(true);
            _isGamePaused = true;
            GameManager.Instance.SetTimeScale(0f);
        }
    }

    public void toggleHelp()
    {
        if (towerPanel.activeSelf || _isGamePaused) return;

        if (_isHelpMenu)
        {
            helpPanel.SetActive(false);
            _isHelpMenu = false;
            ResumeTime();
        }
        else
        {
            helpPanel.SetActive(true);
            _isHelpMenu = true;
            GameManager.Instance.SetTimeScale(0f);
        }
    }

    public void restartGame()
    {
        Platform.towerPanelOpen = false;
        _isSpedUp = false;
        GameManager.Instance.SetTimeScale(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void quitToMenu()
    {
        Platform.towerPanelOpen = false;
        SceneManager.LoadScene("TitleScreen");
    }

    private void ShowGameOver()
    {
        GameManager.Instance.SetTimeScale(0f);
        gameOverPanel.SetActive(true);
    }

    private void ShowVictory()
    {
        GameManager.Instance.SetTimeScale(0f);
        victoryPanel.SetActive(true);
    }

    public void LoadNextScene()
    {
        Platform.towerPanelOpen = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}

*/

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text resourcesText;
    [SerializeField] private GameObject noResourcesText;

    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject startPanel;

    private bool _isGamePaused = false;
    private bool _isHelpMenu = false;
    private bool _isSpedUp = false;
    private bool _hasGameStarted = false;

    [SerializeField] private TowerData[] towers;
    private List<GameObject> activeCards = new List<GameObject>();

    private Platform _currentPlatform;

    private void Start()
    {
        Platform.towerPanelOpen = false;
        _hasGameStarted = false;
        _isSpedUp = false;
        startPanel.SetActive(true);
        GameManager.Instance.SetTimeScale(0f);
    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLiveschanged += UpdateLivesText;
        GameManager.OnResourcesChanged += UpdateResourcesText;
        Platform.OnPlatformClicked += HandlePlatformClicked;
        TowerCard.OnTowerSelected += HandleTowerSelected;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLiveschanged -= UpdateLivesText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
        Platform.OnPlatformClicked -= HandlePlatformClicked;
        TowerCard.OnTowerSelected -= HandleTowerSelected;
    }

    public void StartGame()
    {
        _hasGameStarted = true;
        startPanel.SetActive(false);
        GameManager.Instance.SetTimeScale(1f);
    }

    private void ResumeTime()
    {
        if (!_hasGameStarted)
        {
            GameManager.Instance.SetTimeScale(0f);
            return;
        }

        float targetScale = _isSpedUp ? 2f : 1f;
        GameManager.Instance.SetTimeScale(targetScale);
    }

    private void UpdateWaveText(int currentWave)
    {
        waveText.text = $"Wave: {currentWave + 1}";
        if (currentWave >= 20)
        {
            ShowVictory();
        }
    }

    private void UpdateLivesText(int currentLives)
    {
        livesText.text = $"Lives: {currentLives}";

        if (currentLives <= 0)
        {
            ShowGameOver();
        }
    }

    private void UpdateResourcesText(int currentResources)
    {
        resourcesText.text = $"Materials: {currentResources}";
    }

    private void HandlePlatformClicked(Platform platform)
    {
        if (_isGamePaused || _isHelpMenu) return;
        _currentPlatform = platform;
        ShowTowerPanel();
    }

    private void ShowTowerPanel()
    {
        if (_isGamePaused || _isHelpMenu) return;

        towerPanel.SetActive(true);
        Platform.towerPanelOpen = true;
        GameManager.Instance.SetTimeScale(0f);
        PopulateTowerCards();
    }

    public void hideTowerPanel()
    {
        towerPanel.SetActive(false);
        Platform.towerPanelOpen = false;
        ResumeTime();
    }

    private void PopulateTowerCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        foreach (var data in towers)
        {
            GameObject cardGameObject = Instantiate(towerCardPrefab, cardsContainer);
            TowerCard card = cardGameObject.GetComponent<TowerCard>();
            card.Initialize(data);
            activeCards.Add(cardGameObject);
        }
    }

    public void FastForwardToggle()
    {
        if (_isGamePaused || towerPanel.activeSelf || !_hasGameStarted || _isHelpMenu) return;

        if (!_isSpedUp)
        {
            GameManager.Instance.SetTimeScale(2f);
            _isSpedUp = true;
        }
        else
        {
            GameManager.Instance.SetTimeScale(1f);
            _isSpedUp = false;
        }
    }

    private void HandleTowerSelected(TowerData towerData)
    {
        if (GameManager.Instance.Resources >= towerData.cost)
        {
            GameManager.Instance.SpendResources(towerData.cost);
            _currentPlatform.PlaceTower(towerData);
        }
        else
        {
            StartCoroutine(ShowNoResourcesMessage());
        }

        hideTowerPanel();
    }

    private IEnumerator ShowNoResourcesMessage()
    {
        noResourcesText.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        noResourcesText.SetActive(false);
    }

    public void togglePause()
    {
        if (towerPanel.activeSelf || !_hasGameStarted || _isHelpMenu) return;

        if (_isGamePaused)
        {
            pausePanel.SetActive(false);
            _isGamePaused = false;
            ResumeTime();
        }
        else
        {
            pausePanel.SetActive(true);
            _isGamePaused = true;
            GameManager.Instance.SetTimeScale(0f);
        }
    }

    public void toggleHelp()
    {
        if (towerPanel.activeSelf || _isGamePaused) return;

        if (_isHelpMenu)
        {
            helpPanel.SetActive(false);
            _isHelpMenu = false;
            ResumeTime();
        }
        else
        {
            helpPanel.SetActive(true);
            _isHelpMenu = true;
            GameManager.Instance.SetTimeScale(0f);
        }
    }

    public void restartGame()
    {
        Platform.towerPanelOpen = false;
        _isSpedUp = false;
        GameManager.Instance.SetTimeScale(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void quitToMenu()
    {
        Platform.towerPanelOpen = false;
        SceneManager.LoadScene("TitleScreen");
    }

    private void ShowGameOver()
    {
        GameManager.Instance.SetTimeScale(0f);
        gameOverPanel.SetActive(true);
    }

    private void ShowVictory()
    {
        GameManager.Instance.SetTimeScale(0f);
        victoryPanel.SetActive(true);
    }

    public void LoadNextScene()
    {
        Platform.towerPanelOpen = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}