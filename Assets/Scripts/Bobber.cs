using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    public bool up;
    [SerializeField]
    float amplitude;
    [SerializeField]
    float bobTime;
    [SerializeField]
    bool horizontalize = false;

    // Update is called once per frame
    void Update() {
        if (horizontalize) {
            transform.position += new Vector3(Mathf.Sin(Time.timeSinceLevelLoad / bobTime) * Time.deltaTime,0);
        } else {
            transform.position += new Vector3(0, Mathf.Sin(Time.timeSinceLevelLoad / bobTime) * Time.deltaTime);
        }
    }
}
