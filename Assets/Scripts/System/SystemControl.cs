using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemControl : MonoBehaviour {
    public static SystemControl mSystemControl;
    public SystemStatus current_status;
    public GameObject system_opening;
    public GameObject system_mainmenu;
    public GameObject system_selectionquestion;
    public GameObject system_content;
    public GameObject system_user;

    // Use this for initialization
    void Start () {
        current_status = SystemStatus.Opening;
        mSystemControl = this;
    }
	
	// Update is called once per frame
	void Update () {
        updateSystemObject();
        keyInput();
    }
    public static void onMainMenuGlobal()
    {
        mSystemControl.onMainMenu();
    }
    public static void onContentGlobal()
    {
        mSystemControl.onContent();
    }
    public void onMainMenu()
    {
        current_status = SystemStatus.MainMenu;
    }
    public void onOpening()
    {
        current_status = SystemStatus.Opening;
    }
    public void onUser()
    {
        current_status = SystemStatus.UserAccount;
    }
    public void onSelectionQuestion()
    {
        current_status = SystemStatus.SelectionQuestion;
    }
    public void onContent()
    {
        current_status = SystemStatus.Content;
    }
    private void keyInput()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                onMainMenu();
                return;
            }
        }
    }
    private void updateSystemObject()
    {
       
        if (current_status== SystemStatus.Opening)
        {
            system_opening.SetActive(true);
            system_mainmenu.SetActive(false);
            system_selectionquestion.SetActive(false);
            system_content.SetActive(false);
            system_user.SetActive(false);
        } else if (current_status == SystemStatus.MainMenu)
        {
            system_mainmenu.SetActive(true);
            system_opening.SetActive(false);            
            system_selectionquestion.SetActive(false);
            system_content.SetActive(false);
            system_user.SetActive(false);
        } else if (current_status == SystemStatus.UserAccount)
        {
            system_user.SetActive(true);
            system_opening.SetActive(false);
            system_mainmenu.SetActive(false);
            system_selectionquestion.SetActive(false);
            system_content.SetActive(false);
            

        } else if (current_status == SystemStatus.SelectionQuestion)
        {
            system_selectionquestion.SetActive(true);
            system_opening.SetActive(false);
            system_mainmenu.SetActive(false);
            
            system_content.SetActive(false);
            system_user.SetActive(false);
        } else if (current_status == SystemStatus.Content)
        {
            system_content.SetActive(true);
            system_opening.SetActive(false);
            system_mainmenu.SetActive(false);
            system_selectionquestion.SetActive(false);
            
            system_user.SetActive(false);
        }
    }



}
