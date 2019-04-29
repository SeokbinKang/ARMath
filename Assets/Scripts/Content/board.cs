using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class board : MonoBehaviour {
    public GameObject IconContainer;
    public GameObject math_text;
    public GameObject math_input;
    private bool num_enable;
    private bool icon_enable;
	// Use this for initialization
	void Start () {
        if (IconContainer!=null) IconContainer.SetActive(true);
        if (math_text != null) math_text.SetActive(true);
        num_enable = true;
        icon_enable = false;
      //  setMathText("");

    }
	
	// Update is called once per frame
	void Update () {
        
         if(IconContainer != null) IconContainer.SetActive(icon_enable);
        if (math_text != null) math_text.SetActive(num_enable);


    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        setMathText("");        
        
    }
    public void enable_number_only(string obj_name, int count)
    {
        setMathText(count.ToString());
        setIcon(obj_name, count);
        num_enable = true;
        icon_enable = false;

    }
    public void toggleView()
    {
        num_enable = !num_enable;
        icon_enable = !icon_enable;
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
        if (IconContainer && IconContainer.GetComponent<IconContainer>()!=null) IconContainer.GetComponent<IconContainer>().SetIcon(obj_name, count);
        //set math input answer
        
    }
}
