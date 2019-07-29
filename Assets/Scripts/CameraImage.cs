using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraImage : MonoBehaviour {

    static CameraImage mThis;
    WebCamTexture webcamTexture;
    RawImage image;
    public GameObject canvas;
    private Vector2Int webcamSize;
    private Vector2Int cropSize;
    void Start() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //  Screen.SetResolution(Screen.width, Screen.width * 9 / 16, false);        
        //delay initialize camera
        //webcamTexture = new WebCamTexture(1920  ,1200);         //tab s5e

        //Debug.Log(Screen.currentResolution.width);
        //tab s3        
        //        webcamTexture = new WebCamTexture(1600, 1200); //tab 3 2048x1280 , 1600,1200
        //tab s5e
        //webcamTexture = new WebCamTexture(1920, 1200);
        int cam_width = Screen.currentResolution.width;
        int cam_height = Screen.currentResolution.height;
        if(cam_width>2100)
        {  //for better FPS
            cam_width = cam_width * 3 / 4;
            cam_height = cam_height * 3 / 4;
        }
     //   Debug.Log("[ARMath] screen "+ Screen.currentResolution+"cam "+cam_width + "  " + cam_height);
        webcamTexture = new WebCamTexture(cam_width, cam_height); //tab 3 2048x1280 , 1600,1200
        
        image = GetComponent<RawImage>();
        
        image.texture = webcamTexture;
        //else
        //{
        //    SpriteRenderer sr = image_texture_obj.GetComponent<SpriteRenderer>();
        //    if(!sr) sr.material.mainTexture = webcamTexture;
        //}
        webcamTexture.Play();

        mThis = this;



    }
    public static void pause_image()
    {
        Debug.Log("[ARMath] CAM Paused");
        mThis.webcamTexture.Pause();
    }
    public static void resume_image()
    {
        Debug.Log("[ARMath] CAM Paused");
        mThis.webcamTexture.Play();
    }

    public Texture2D ProcessImage_twinFrame_texture2d(int l_or_r)
    {
        //crop
        TextureTools.RectOptions option = TextureTools.RectOptions.TopLeft;
        if (l_or_r == 1) option = TextureTools.RectOptions.TopRight;
        var cropped = TextureTools.CropTexture(webcamTexture, option);
        //  Debug.Log("Original Image size = " + webcamTexture.width + " " + webcamTexture.height);
        //   Debug.Log("Cropped  Image size = " + cropped.width + " " + cropped.height);
        //scale
        var scaled = TextureTools.scaled(cropped, SystemParam.image_size, SystemParam.image_size, FilterMode.Bilinear);
        UnityEngine.Object.Destroy(cropped);
        //run detection
        return scaled;
    }

    public Color32[] ProcessImage(){
        //crop
        var cropped = TextureTools.CropTexture(webcamTexture);
     //   Debug.Log("Original Image size = " + webcamTexture.width + " " + webcamTexture.height);
     //   Debug.Log("Cropped  Image size = " + cropped.width + " " + cropped.height);
        //scale
        var scaled = TextureTools.scaled(cropped, SystemParam.image_size, SystemParam.image_size, FilterMode.Bilinear);
        //run detection
        return scaled.GetPixels32();
    }

    public Vector2Int convertCroppedXYtoScreenXY(Vector2Int croppedXY)
    {
        //TBD
        Vector2Int ret = new Vector2Int();
        return ret;

    }
}
