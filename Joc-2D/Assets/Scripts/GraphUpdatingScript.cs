using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GraphUpdatingScript : MonoBehaviour
{
    public float scanRate = 1f;
    float nextScan = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nextScan < Time.time)
        {
            AstarPath.active.Scan();
            nextScan = Time.time + scanRate;
        }
    }
}
