using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DelayedEvents : MonoBehaviour
{
    // Start is called before the first frame update
    private AstarPath astar;
    private GameObject canvas;
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        Invoke("ScanAgain", .25f);
    }

    private void ScanAgain(){
        astar = FindObjectOfType<AstarPath>();
        canvas.GetComponent<ColorStats>().enabled = true;
        astar.Scan();
    }
}
