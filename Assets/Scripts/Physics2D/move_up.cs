using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_up : MonoBehaviour {

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Rigidbody2D rb2D = this.GetComponent<Rigidbody2D>();
        
        Debug.Log(rb2D.velocity);
    }
    private void OnEnable()
    {
        move_upup();
    }
    private void move_upup()
    {
        System.Random random = new System.Random();
        Rigidbody2D rb2D = this.GetComponent<Rigidbody2D>();        
        rb2D.velocity = new Vector2(random.Next(5, 100), random.Next(250, 600));
    }

}
