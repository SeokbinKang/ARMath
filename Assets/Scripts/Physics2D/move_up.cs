using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_up : MonoBehaviour {

    // Use this for initialization
    private bool init = false;
    Vector3 init_pos;
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Rigidbody2D rb2D = this.GetComponent<Rigidbody2D>();
        
     //   Debug.Log(rb2D.velocity);
    }
    public void initialize_pos()
    {
        if (!init)
        {
            init = true;
            init_pos = this.GetComponent<RectTransform>().position;
        }
        else
        {
            this.GetComponent<RectTransform>().position = init_pos;
        }
    }
    private void OnEnable()
    {
        initialize_pos();
        move_upup();

    }
    private void move_upup()
    {
        System.Random random = new System.Random();
        Rigidbody2D rb2D = this.GetComponent<Rigidbody2D>();        
        rb2D.velocity = new Vector2(random.Next(5, 100), random.Next(250, 600));
    }

}
