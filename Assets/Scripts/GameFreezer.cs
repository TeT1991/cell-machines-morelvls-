using UnityEngine;

public class GameFreezer : MonoBehaviour
{
    public void Freeze()
    {
        Time.timeScale = 0;
    }

    public void Unfreeze()
    {
        Time.timeScale = 1;
    }
}
