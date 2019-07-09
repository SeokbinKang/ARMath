using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour {

    public static EffectControl mThis;

    public GameObject balloon;
    public GameObject gem;

    public GameObject move_left;
	// Use this for initialization
	void Start () {
        mThis = this;
       
    }
    private int tea_time = 4;
	// Update is called once per frame
	void Update () {
      /*  if (Time.time > tea_time)
        {
            tea_time += 3;
            gem_ceremony(ProblemType.p2_addition);
        }*/
        check_n_disable();

    }
    private void Reset()
    {
        balloon.SetActive(false);
        move_left.SetActive(false);



    }
    public static void ballon_ceremony()
    {
        mThis.Reset();
        mThis.balloon.SetActive(true);
        foreach(move_up mv in mThis.balloon.GetComponentsInChildren<move_up>())
        {
            mv.initialize_pos();
        }
        
    }
    public static void gem_ceremony(ProblemType p)
    {
        if (mThis.gem.activeSelf) return;
        SystemContent.EnableLeftUserUI(true);
        mThis.gem.SetActive(true);
        mThis.gem.GetComponent<effect_gem>().load_gem(p);
        
    }
    private void check_n_disable()
    {
        if (gem.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish"))
        {
            gem.SetActive(false);
            // Avoid any reload.
        }
        if (move_left.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish"))
        {
            move_left.SetActive(false);
            // Avoid any reload.
        }
    }
    public static void prompt_move_left(Vector3 pos)
    {
        mThis.move_left.SetActive(true);
        ARMathUtils.move2D_ScreenCoordinate(mThis.move_left, pos);
        
    }
}
