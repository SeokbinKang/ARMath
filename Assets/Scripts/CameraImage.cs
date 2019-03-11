using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraImage : MonoBehaviour {

    WebCamTexture webcamTexture;
    RawImage image;

    private Vector2Int webcamSize;
    private Vector2Int cropSize;
    void Start() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //delay initialize camera
        webcamTexture = new WebCamTexture(1600,1200);
        image = GetComponent<RawImage>();
        image.texture = webcamTexture;
        
        webcamTexture.Play();
    }

    public Color32[] ProcessImage(){
        //crop
        var cropped = TextureTools.CropTexture(webcamTexture);
     //   Debug.Log("Original Image size = " + webcamTexture.width + " " + webcamTexture.height);
     //   Debug.Log("Cropped  Image size = " + cropped.width + " " + cropped.height);
        //scale
        var scaled = TextureTools.scaled(cropped, 224, 224, FilterMode.Bilinear);
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
