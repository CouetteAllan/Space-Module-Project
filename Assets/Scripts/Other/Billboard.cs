using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform rect;

    // Update is called once per frame
    void Update()
    {
        rect.rotation = Quaternion.identity;
        //rect.rotation = Quaternion.identity;
    }
}
