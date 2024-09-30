using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject BallPrefab;           
    public Vector3 BallSpawnPos;            

    public LevelData[] LevelDatas;          

    private int shotCount = 0, maxShotCount;              

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SpawnLevel(int levelIndex)
    {
        Instantiate(LevelDatas[levelIndex].levelPrefab, Vector3.zero, Quaternion.identity);
        shotCount = LevelDatas[levelIndex].shotLimit;
        maxShotCount = LevelDatas[levelIndex].shotLimit;
        UIManager.instance.ShotText.text = shotCount.ToString();

        GameObject ball = Instantiate(BallPrefab, BallSpawnPos, Quaternion.identity);
        CameraFollow.instance.SetTarget(ball);
        GameManager.singleton.gameStatus = GameStatus.Playing;
        UIManager.instance.ShotText.text = shotCount + "/" + maxShotCount;
    }

    public void ShotTaken()
    {
        if (shotCount > 0)                                          
        {
            shotCount--;                                            
            UIManager.instance.ShotText.text = shotCount + "/" + maxShotCount;      

            if (shotCount <= 0) LevelFailed();
        }
    }

    public void LevelFailed()
    {
        if (GameManager.singleton.gameStatus == GameStatus.Playing) 
        {
            GameManager.singleton.gameStatus = GameStatus.Failed;   
            UIManager.instance.GameResult();                        
        }
    }

    public void LevelComplete()
    {
        if (GameManager.singleton.gameStatus == GameStatus.Playing) 
        {
            if (GameManager.singleton.currentLevelIndex < LevelDatas.Length)
            {
                GameManager.singleton.currentLevelIndex++;  
            }
            else
            {
                
                GameManager.singleton.currentLevelIndex = 0;
            }
            GameManager.singleton.gameStatus = GameStatus.Complete; 
            UIManager.instance.GameResult();                        
        }
    }
}