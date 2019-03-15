using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class board : MonoBehaviour {
    public GameObject IconContainer;
    public GameObject math_text;
    public GameObject math_input;
	// Use this for initialization
	void Start () {
        if (IconContainer) IconContainer.SetActive(true);
        if (math_text != null) math_text.SetActive(true);
      //  setMathText("");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        setMathText("");
        setAnswer(0);
        
    }
    public void setProblemType(ProblemType p)
    {
        if (math_input) math_input.GetComponent<InputNumber>().setProblemType(p);
    }
    public void setMathText(string t)
    {
     //   Debug.Log("[ARMath] Setting math text...->" + t);
        
        if(math_text !=null) math_text.GetComponent<Text>().text = t;
    }
    public void setAnswer(int answer)
    {
        if (math_input) math_input.GetComponent<InputNumber>().setAnswer(answer);
    }
    public void setIcon(string obj_name, int count)
    {
        if (IconContainer) IconContainer.GetComponent<IconContainer>().SetIcon(obj_name, count);
        //set math input answer
        
    }
}
