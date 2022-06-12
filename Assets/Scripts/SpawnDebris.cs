using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDebris : MonoBehaviour
{
    public GameObject debrisPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 8; x++)
        {
            Instantiate(debrisPrefab, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
