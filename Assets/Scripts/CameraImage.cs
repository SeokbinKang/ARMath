﻿using System.Collections;
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
    public Color32[] ProcessImage_twinFrame(int l_or_r)
    {
        //crop
        TextureTools.RectOptions option = TextureTools.RectOptions.TopLeft;
        if (l_or_r == 1) option = TextureTools.RectOptions.TopRight;
        var cropped = TextureTools.CropTexture(webcamTexture, option);
        //   Debug.Log("Original Image size = " + webcamTexture.width + " " + webcamTexture.height);
        //   Debug.Log("Cropped  Image size = " + cropped.width + " " + cropped.height);
        //scale
        var scaled = TextureTools.scaled(cropped, SystemParam.image_size, SystemParam.image_size, FilterMode.Bilinear);
        //run detection
        return scaled.GetPixels32();
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
