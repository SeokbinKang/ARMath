using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalGemStat_Element : MonoBehaviour {
    public GameObject icon;
    public GameObject count_label;    
    public Texture[] gem_sprites;

    public ProblemType problem_type;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateGem(int sprite_index, int count, ProblemType p_type)
    {
        icon.GetComponent<RawImage>().texture = gem_sprites[sprite_index];
        count_label.GetComponent<Text>().text = count.ToString();
        this.problem_type = p_type;
    }
}
