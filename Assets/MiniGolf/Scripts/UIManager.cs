using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Image powerBar;        
    [SerializeField] private Text shotText;         
    [SerializeField] private GameObject mainMenu, gameMenu, gameOverPanel, retryBtn, nextBtn;   
    [SerializeField] private GameObject container, lvlBtnPrefab;    

    public Text ShotText => shotText;
    public Image PowerBar => powerBar;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        powerBar.fillAmount = 0;                        
    }

    private void Start()
    {
        if (GameManager.singleton.gameStatus == GameStatus.None) 
        {
            CreateLevelButtons();
        }
        else if (GameManager.singleton.gameStatus == GameStatus.Failed ||
                 GameManager.singleton.gameStatus == GameStatus.Complete)
        {
            mainMenu.SetActive(false);                                      
            gameMenu.SetActive(true);                                       
            LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex);  
        }
    }

    private void CreateLevelButtons()
    {
        for (int i = 0; i < LevelManager.instance.LevelDatas.Length; i++)
        {
            GameObject buttonObj = Instantiate(lvlBtnPrefab, container.transform);   
            buttonObj.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1);   
            Button button = buttonObj.GetComponent<Button>();                           
            button.onClick.AddListener(() => OnClick(button));                          
        }
    }

    private void OnClick(Button btn)
    {
        mainMenu.SetActive(false);                                                      
        gameMenu.SetActive(true);                                                       
        GameManager.singleton.currentLevelIndex = btn.transform.GetSiblingIndex();     
        LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex);      
    }

    public void GameResult()
    {
        gameOverPanel.SetActive(true);
        gameMenu.SetActive(false);
        switch (GameManager.singleton.gameStatus)
        {
            case GameStatus.Complete:                       
                nextBtn.SetActive(true);                    
                break;
            case GameStatus.Failed:                         
                retryBtn.SetActive(true);                   
                break;
        }
    }

    public void HomeBtn()
    {
        GameManager.singleton.gameStatus = GameStatus.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextRetryBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}