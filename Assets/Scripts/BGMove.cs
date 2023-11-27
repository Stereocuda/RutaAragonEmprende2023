using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float posY=moveSpeed*Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y - posY, transform.position.z);

    }
}
