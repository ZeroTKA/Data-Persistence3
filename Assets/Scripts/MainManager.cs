using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] Text _HighScoreTextField;
    [SerializeField] GameObject _GameOverBackground;
    [SerializeField] GameObject _MainMenuButton;
    [SerializeField] GameObject _RestartButton;
    [SerializeField] GameObject _QuitButton;
  
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_CurrentHighScore;
    private string m_HighScoreName;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        SetHighScoreText($"Highscore: {m_CurrentHighScore} by {m_HighScoreName}");
        if (SavedData.Instance != null)
        {
            // code for the users display name
        }
        //controls how many go horizontal
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        // LineCount is being weird. Changing it above does nothing.
        // Hardcoding the same number does what you expect. It's being messed with somewhere?
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        // do I even need this if we have a start button?
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        // whenever we add points, check for highscore and if it is the new high score, reflect that.
        if(m_Points > m_CurrentHighScore)
        {
            SetHighScoreText($"New Highscore: {m_Points} !!!");
        }
    }

    //The restart function just reloads the scene completely. So there's no need to disable any menu items. Is that even good? 
    public void GameOver()
    {
        m_GameOver = true;
        if(m_Points > m_CurrentHighScore)
        {
            SaveScore();
        }

        GameOverText.SetActive(true);
        //black background with opacity
        _GameOverBackground.SetActive(true);
        _MainMenuButton.SetActive(true);
        _RestartButton.SetActive(true);
        _QuitButton.SetActive(true);
   

    }
    private void SetHighScoreText(string text)
    {
        _HighScoreTextField.text = text;
    }

    [System.Serializable]
    // data I want to save between scenes
    class SavedHighScoreData
    {
        public int Score;
        public string DisplayName;
    }
    //saving data long term. Just
    public void SaveScore()
    {
        SavedHighScoreData data = new SavedHighScoreData();
        data.Score = m_Points;
        data.DisplayName = SavedData.Instance.DisplayName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    // Loading longterm data
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedHighScoreData data = JsonUtility.FromJson<SavedHighScoreData>(json);
            m_CurrentHighScore = data.Score;
            m_HighScoreName = data.DisplayName;
        }
        // for first time running the game.
        else
        {
            
            m_CurrentHighScore = 0;
            m_HighScoreName = "Nobody";
        }
    }
    //testing out high score related things.
    public void ResetHighScore()
    {
        SavedHighScoreData data = new SavedHighScoreData();
        data.Score = 0;
        data.DisplayName = "Nobody";

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }
}
