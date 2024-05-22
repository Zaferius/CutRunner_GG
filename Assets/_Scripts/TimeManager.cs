using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager i;

    private void Awake()
    {
        if (i)
        {
            Destroy(this);
        }
        else
        {
            i = this;
        }
    }
}
