using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScriptGeneric : MonoBehaviour
{
    private GameWorldScript gameLogic;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.FindObjectOfType<GameWorldScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetHit()
    {
         Destroy(gameObject);
    }
}
