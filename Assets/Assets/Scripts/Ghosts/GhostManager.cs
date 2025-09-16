using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance { get; private set; }
    [SerializeField] List<Ghost> ghosts = new List<Ghost>();

    int[] ChaseTiming = { 10, 12, 15, 20, 100000 };
    int[] ScatterTiming = { 12, 9, 7, 5, 2 };
    int FrightenedTiming = 10;
    public bool isFrightened = false;

    bool leftPen = false;
    bool timer = true;
    public int currentPhase = 0;
    public int modeTime = 0;
    public GhostStates ghostState = GhostStates.Scatter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftPen == false)
            LeavingPen();

        if (isFrightened == true)
        {
            return;
        }
        
        if (timer == false)
            StartCoroutine(Pause(2));

    }
    IEnumerator Pause(float delay)
    {
        timer = true;
        yield return new WaitForSeconds(delay);
        ScatterChaseSwitch();
    }

    private void LeavingPen()
    {
        if (UIManager.Instance.GetScore() == 5)
        {
            ghosts.FirstOrDefault(g => g.ghostType == GhostType.Blinky).SetState(GhostStates.LeavingPen);
        }
        if (UIManager.Instance.GetScore() == 12)
        {
            ghosts.FirstOrDefault(g => g.ghostType == GhostType.Pinky).SetState(GhostStates.LeavingPen);
        }
        if (UIManager.Instance.GetScore() == 17)
        {
            ghosts.FirstOrDefault(g => g.ghostType == GhostType.Inky).SetState(GhostStates.LeavingPen);
        }
        if (UIManager.Instance.GetScore() == 20)
        {
            ghosts.FirstOrDefault(g => g.ghostType == GhostType.Clyde).SetState(GhostStates.LeavingPen);
            leftPen = true;
            timer = false;
        }
    }


    private void ScatterChaseSwitch()
    {
        if (ghostState == GhostStates.Scatter)
            modeTime = ScatterTiming[currentPhase];

        if (ghostState == GhostStates.Chase)
            modeTime = ChaseTiming[currentPhase];

        foreach (Ghost ghost in ghosts)
        {
            ghost.SetState(ghostState);
        }
        StartCoroutine(GhostMove(modeTime));


    }

    private IEnumerator GhostMove(int modeTime)
    {
        yield return new WaitForSeconds(modeTime);


        if (ghostState == GhostStates.Scatter)
        {
            ghostState = GhostStates.Chase;
        }
        else if (ghostState == GhostStates.Chase)
        {
            ghostState = GhostStates.Scatter;
            currentPhase++;
        }
        timer = false;
        if (isFrightened == true)
        {
            isFrightened = false;
        }
        Debug.Log(currentPhase + "  " + modeTime);
    }

    public void Frightened()
    {
        if (isFrightened == true)
        {
            return;
        }
        isFrightened = true;
        foreach (Ghost ghost in ghosts)
        {
            ghost.SetState(GhostStates.Frightened);
        }
        modeTime = FrightenedTiming;
    }

}
