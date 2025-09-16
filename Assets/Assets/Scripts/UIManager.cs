using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TMP_Text scoreTMP;
    int score;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void IncreaseScore(int amount)
    {
        score = score + amount;
        scoreTMP.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

}
