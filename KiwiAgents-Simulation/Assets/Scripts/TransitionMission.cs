using EasyTransition;
using UnityEngine;

public class TransitionMission : MonoBehaviour
{
    public TransitionSettings RectangleWipe;
    public float startDelay;
    public string GameMap;

    public void LoadScene()
    {
        TransitionManager.Instance().Transition("GameMap", RectangleWipe, startDelay);
    }
}
