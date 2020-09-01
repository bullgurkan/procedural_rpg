using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector2 input;
    Character character;

    void Start()
    {
        
    }

    
    void Update()
    {
        input += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)input;
        input = Vector2.zero;
    }
}
