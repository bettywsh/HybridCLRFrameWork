using UnityEngine;
using System.Collections;

public class HUDFPS : MonoBehaviour
{
    public float updateInterval = 0.5F;

    private float accum = 0;
    private int frames = 0;
    private float timeleft;
    private float fps = 0f;

    GUIStyle guiStyle = new GUIStyle();
    void Start()
    {
        timeleft = updateInterval;

        guiStyle.normal.background = null;
        guiStyle.normal.textColor = new Color(1, 0, 0);
        guiStyle.fontSize = 30;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            fps = accum / frames;

            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }

    void OnGUI()
    {
        string format = System.String.Format("{0:F2}", fps);
        
        if (fps < 30)
            guiStyle.normal.textColor = Color.yellow;
        else
            if (fps < 10)
                guiStyle.normal.textColor = Color.red;
            else
                guiStyle.normal.textColor = Color.green;

        GUI.Label(new Rect(Screen.width - 120, 10f, 60f, 20f), format, guiStyle);
        GUI.Label(new Rect(Screen.width - 120, 40f, 60f, 20f), Time.time.ToString("F2"), guiStyle);
    }
}