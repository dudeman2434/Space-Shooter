using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _score_text,_highScoreText;
    [SerializeField]
    Image _lifeUI;
    [SerializeField]
    Sprite[] _lives;
    [SerializeField]
    Text _gameOverText;
    [SerializeField]
    Text _restartText;
    private GameManager _gameManager;
    int highScore=0;
    // Start is called before the first frame update
    void Start()
    {
        _score_text.text = "Score: " + 0;
        _highScoreText.text = "High-score: " + PlayerPrefs.GetInt("hiScore").ToString();
        _gameOverText.gameObject.SetActive(true);
        _gameOverText.text = "";
        _restartText.gameObject.SetActive(false);
        _gameManager= GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            _gameManager.Pause();
        }
    }

    public void UpdateScore(int points)
    {
        _score_text.text = "Score: " + points.ToString();
        if (PlayerPrefs.HasKey("hiScore")){
            if (points > PlayerPrefs.GetInt("hiScore"))
            {
                highScore = points;
                PlayerPrefs.SetInt("hiScore", highScore);
                PlayerPrefs.Save();
            }
        }
        else
        {
            if (points > highScore)
            {
                highScore = points;
                PlayerPrefs.SetInt("hiScore", highScore);
                PlayerPrefs.Save();
            }
        }
        if (points > PlayerPrefs.GetInt("hiScore"))
        {
            _highScoreText.text = "High-score: " + points.ToString();
        }
        
    }
    public void UILifeUpdate(int life)
    {
        if (life >= 0)
        {
            _lifeUI.sprite = _lives[life];
        }
        else
        {
            StartCoroutine("GameOver");
            _restartText.gameObject.SetActive(true);
        }
    }
    
    public IEnumerator GameOver()
    {
        
        while (true)
        {

            _gameOverText.text = "Game over";
            yield return new WaitForSeconds(0.75f);
            _gameOverText.text="";
            yield return new WaitForSeconds(0.75f);    
        }     
    }
}
