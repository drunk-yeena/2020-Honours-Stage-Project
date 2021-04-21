using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualTimerScript : MonoBehaviour
{
    private GameWorldScript gamelogic;
    private TextMesh visualTimer;

    // Start is called before the first frame update
    void Start()
    {
        gamelogic = GameObject.FindObjectOfType<GameWorldScript>();
        visualTimer = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        visualTimer.text = gamelogic.ElapsedTimeString;
    }
}
