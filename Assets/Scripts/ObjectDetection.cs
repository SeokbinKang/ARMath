using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using TensorFlow;
using System.Threading;
using System.Threading.Tasks;

public class ObjectDetection : MonoBehaviour {

    [Header("Constants")]
    private const float MIN_SCORE = 0.25f;
    /*private const int INPUT_SIZE = 224;
    private const int INPUT_WIDTH = 224;
    private const int INPUT_HEIGHT = 224;*/
    private int INPUT_SIZE = 300;

    private const int IMAGE_MEAN = 0;
    private const float IMAGE_STD = 1;

    [Header("Inspector Stuff")]
    public CameraImage cameraImage;
    public TextAsset labelMap;
    public TextAsset model;
    public TextAsset model_faster_rcnn_resnet101;
    public TextAsset model_ssdlite_v2_mobilenet;
    public Color objectColor;
    public Texture2D tex;
    public GameObject ContentRoot;

    [Header("Private member")]
    private GUIStyle style = new GUIStyle();
    private TFGraph graph;
    private TFSession session;
    private IEnumerable<CatalogItem> _catalog;
    private List<CatalogItem> items = new List<CatalogItem>();
    private float x_stretch = 1;

    [Header("Thread stuff")]
    Thread _thread;
    byte[] pixels;
    byte[] pixels_L;
    byte[] pixels_R;
    Color32 pixel;
    Color32 pixel_L;
    Color32 pixel_R;
    Color32[] colorPixels;
    Color32[] colorPixels_L;
    Color32[] colorPixels_R;
    TFTensor[] output;
    bool pixelsUpdated = false;
    bool processingImage = true;

    // Use this for initialization
    public GameObject AppCountUI;
    private float nextActionTime = 0.0f;

    IEnumerator Start() {
#if UNITY_ANDROID && !UNITY_EDITOR
TensorFlowSharp.Android.NativeBinding.Init();
#endif
        INPUT_SIZE = SystemParam.image_size;
        pixels = new byte[INPUT_SIZE * INPUT_SIZE * 3];
        pixels_L = new byte[INPUT_SIZE * INPUT_SIZE * 3];
        pixels_R = new byte[INPUT_SIZE * INPUT_SIZE * 3];
        _catalog = CatalogUtil.ReadCatalogItems(labelMap.text);
        Debug.Log("Loading graph...");
        graph = new TFGraph();
        //graph.Import(model.bytes);  //ssd , mobilenet
        graph.Import(model.bytes);
        session = new TFSession(graph);
        Debug.Log("Graph Loaded!!!");

        //set style of labels and boxes
        if (Screen.currentResolution.width == 2560 && Screen.currentResolution.height == 1600)
        { // for screen 16:10 while the camera stream is 16:9
            x_stretch = 0.9f;
        }
        else x_stretch = 1;

        // Begin our heavy work on a new thread.
        _thread = new Thread(ThreadedWork_twinFrame);
        _thread.Start();
        //do this to avoid warnings
        processingImage = true;
        yield return new WaitForEndOfFrame();
        processingImage = false;
    }
    
    void ThreadedWork_twinFrame()
    {
        while (true)
        {
            if (pixelsUpdated)
            {

                //left frame
                TFShape shape = new TFShape(1, INPUT_SIZE, INPUT_SIZE, 3);
                var tensor = TFTensor.FromBuffer(shape, pixels_L, 0, pixels_L.Length);
                var runner = session.GetRunner();
                runner.AddInput(graph["image_tensor"][0], tensor).Fetch(
                    graph["detection_boxes"][0],
                    graph["detection_scores"][0],
                    graph["num_detections"][0],
                    graph["detection_classes"][0]);
                output = runner.Run();

                var boxes = (float[,,])output[0].GetValue(jagged: false);
                var scores = (float[,])output[1].GetValue(jagged: false);
                var num = (float[])output[2].GetValue(jagged: false);
                var classes = (float[,])output[3].GetValue(jagged: false);
                items.Clear();
                //loop through all detected objects
              //  Debug.Log("[ARMath] "+Time.time+" object detected #:" + num.Length);
                for (int i = 0; i < num.Length; i++)
                {
                //    Debug.Log("[ARMath] num of a type of objects:" + num[i]);
                    //  for (int j = 0; j < scores.GetLength(i); j++) {
                    for (int j = 0; j < num[i]; j++)
                    {
                        float score = scores[i, j];
                        if (score > MIN_SCORE)
                        {
                            CatalogItem catalogItem = _catalog.FirstOrDefault(item => item.Id == Convert.ToInt32(classes[i, j]));
                            catalogItem.Score = score;
                      
                            float ymin = boxes[i, j, 0] * Screen.height;
                            float xmin = boxes[i, j, 1] * ((float) Screen.height) * x_stretch ;
                            float ymax = boxes[i, j, 2] * Screen.height;
                            float xmax = boxes[i, j, 3] * ((float) Screen.height) * x_stretch;
                            catalogItem.Box = Rect.MinMaxRect(xmin, Screen.height - ymax, xmax, Screen.height - ymin);
                            items.Add(catalogItem);
                          //  Debug.Log(catalogItem.DisplayName+" "+i+" "+j+" "+num[i]);
                        }
                    }
                }

                //right frame
                shape = new TFShape(1, INPUT_SIZE, INPUT_SIZE, 3);
                tensor = TFTensor.FromBuffer(shape, pixels_R, 0, pixels_R.Length);
                runner = session.GetRunner();
                runner.AddInput(graph["image_tensor"][0], tensor).Fetch(
                    graph["detection_boxes"][0],
                    graph["detection_scores"][0],
                    graph["num_detections"][0],
                    graph["detection_classes"][0]);
                output = runner.Run();

                boxes = (float[,,])output[0].GetValue(jagged: false);
                scores = (float[,])output[1].GetValue(jagged: false);
                num = (float[])output[2].GetValue(jagged: false);
                classes = (float[,])output[3].GetValue(jagged: false);
              //  Debug.Log("[ARMath] object detected #:" + num.Length);
                //loop through all detected objects
                for (int i = 0; i < num.Length; i++)
                {
               //     Debug.Log("[ARMath] num of object class:" + num[i]);
                    //  for (int j = 0; j < scores.GetLength(i); j++) {
                    for (int j = 0; j < num[i]; j++)
                    {
                        float score = scores[i, j];
                        if (score > MIN_SCORE)
                        {
                            CatalogItem catalogItem = _catalog.FirstOrDefault(item => item.Id == Convert.ToInt32(classes[i, j]));
                            catalogItem.Score = score;
                         
                            float x_shift = Screen.width - ((float) Screen.height)*x_stretch;

                            float ymin = boxes[i, j, 0] * Screen.height;
                            float xmin = boxes[i, j, 1] * (float)Screen.height * x_stretch + x_shift;
                            float ymax = boxes[i, j, 2] * Screen.height;
                            float xmax = boxes[i, j, 3] * (float) Screen.height * x_stretch + x_shift;
                            catalogItem.Box = Rect.MinMaxRect(xmin, Screen.height - ymax, xmax, Screen.height - ymin);
                            items.Add(catalogItem);
                         //   Debug.Log(catalogItem.DisplayName+" "+i+" "+j+" "+num[i]);
                        }
                    }
                }
                pixelsUpdated = false;
            }
        }
    }

    void ThreadedWork_singleFrame() {
        while (true) {
            if (pixelsUpdated) {
                TFShape shape = new TFShape(1, INPUT_SIZE, INPUT_SIZE, 3);
                var tensor = TFTensor.FromBuffer(shape, pixels, 0, pixels.Length);
                var runner = session.GetRunner();
                runner.AddInput(graph["image_tensor"][0], tensor).Fetch(
                    graph["detection_boxes"][0],
                    graph["detection_scores"][0],
                    graph["num_detections"][0],
                    graph["detection_classes"][0]);
                output = runner.Run();

                var boxes = (float[,,])output[0].GetValue(jagged: false);
                var scores = (float[,])output[1].GetValue(jagged: false);
                var num = (float[])output[2].GetValue(jagged: false);
                var classes = (float[,])output[3].GetValue(jagged: false);
                items.Clear();
                //loop through all detected objects
                for (int i = 0; i < num.Length; i++) {
                    //  for (int j = 0; j < scores.GetLength(i); j++) {
                    for (int j = 0; j < num[i]; j++)
                    {
                        float score = scores[i, j];
                        if (score > MIN_SCORE) {
                            CatalogItem catalogItem = _catalog.FirstOrDefault(item => item.Id == Convert.ToInt32(classes[i, j]));
                            catalogItem.Score = score;
                     
                            float ymin = boxes[i, j, 0] * Screen.height;
                            float xmin = boxes[i, j, 1] * Screen.height+200;
                            float ymax = boxes[i, j, 2] * Screen.height;
                            float xmax = boxes[i, j, 3] * Screen.height+200;
                            catalogItem.Box = Rect.MinMaxRect(xmin, Screen.height - ymax, xmax, Screen.height - ymin);
                            items.Add(catalogItem);
                         //   Debug.Log(catalogItem.DisplayName+" "+i+" "+j+" "+num[i]);
                        }
                    }
                }
                pixelsUpdated = false;
            }
        }
    }

    IEnumerator ProcessImage_twinFrame()
    {
        
        Texture2D texture2d_l = cameraImage.ProcessImage_twinFrame_texture2d(0);
        Texture2D texture2d_r = cameraImage.ProcessImage_twinFrame_texture2d(1);
        Color32[] colorPixels_LL = texture2d_l.GetPixels32();
        Color32[] colorPixels_RR = texture2d_r.GetPixels32();

        // colorPixels_L = cameraImage.ProcessImage_twinFrame(0);
        // colorPixels_R = cameraImage.ProcessImage_twinFrame(1);
       // scaled.GetPixels32();
        //Debug.Log("[ARMath] debug point #1");
        //update pixels (Cant use Color32[] on non monobehavior thread
        if (colorPixels_LL.Length != colorPixels_RR.Length)
        {
            Debug.Log("[ARMath] the sizes of twin frames do not match");
            yield return null;
        }
        for (int i = 0; i < colorPixels_LL.Length; ++i)
        {
            pixel_L = colorPixels_LL[i];
            pixels_L[i * 3 + 0] = (byte)((pixel_L.r - IMAGE_MEAN) / IMAGE_STD);
            pixels_L[i * 3 + 1] = (byte)((pixel_L.g - IMAGE_MEAN) / IMAGE_STD);
            pixels_L[i * 3 + 2] = (byte)((pixel_L.b - IMAGE_MEAN) / IMAGE_STD);

            pixel_R = colorPixels_RR[i];
            pixels_R[i * 3 + 0] = (byte)((pixel_R.r - IMAGE_MEAN) / IMAGE_STD);
            pixels_R[i * 3 + 1] = (byte)((pixel_R.g - IMAGE_MEAN) / IMAGE_STD);
            pixels_R[i * 3 + 2] = (byte)((pixel_R.b - IMAGE_MEAN) / IMAGE_STD);
        }
        

        //flip bool so other thread will execute
        pixelsUpdated = true;
        //Resources.UnloadUnusedAssets();
        processingImage = false;
        colorPixels_LL = null;
        colorPixels_RR = null;
        UnityEngine.Object.Destroy(texture2d_l);
        UnityEngine.Object.Destroy(texture2d_r);
        yield return null;
    }
    IEnumerator ProcessImage(){
        colorPixels = cameraImage.ProcessImage();
        //update pixels (Cant use Color32[] on non monobehavior thread
        for (int i = 0; i < colorPixels.Length; ++i) {
            pixel = colorPixels[i];
            pixels[i * 3 + 0] = (byte)((pixel.r - IMAGE_MEAN) / IMAGE_STD);
            pixels[i * 3 + 1] = (byte)((pixel.g - IMAGE_MEAN) / IMAGE_STD);
            pixels[i * 3 + 2] = (byte)((pixel.b - IMAGE_MEAN) / IMAGE_STD);
        }
        //flip bool so other thread will execute
        pixelsUpdated = true;
        //Resources.UnloadUnusedAssets();
        processingImage = false;
        yield return null;
    }

	private void Update() {
        if (!pixelsUpdated && !processingImage){
            processingImage = true;
            StartCoroutine(ProcessImage_twinFrame());
        }
	}

	void OnGUI() {
        try {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + 1.5f;
                SceneObjectManager.mSOManager.add_new_object(items);
            }
            /*
            List<CatalogItem> ret = new List<CatalogItem>();
            foreach (CatalogItem item in items) {
                GUI.backgroundColor = objectColor;
                //display score and label
                //GUI.Box(item.Box, item.DisplayName + '\n' + Mathf.RoundToInt(item.Score*100) + "%", style);
                //display only score
                // GUI.Box(item.Box, item.DisplayName + " "+items.Count, style);

                if (item.DisplayName.Equals("apple")
                    || item.DisplayName.Equals("bottle")
                    || item.DisplayName.Equals("cup"))
                {
                    //onApple(item.Box, item.DisplayName);
                    //GUI.Label(item.Box, item.DisplayName);
                    ret.Add(item);
                }
                }
            CVResult cv = new CVResult();
            if (ret.Count > 0) {
                cv.mObjects = ret;
                
            }
            ContentRoot.GetComponent<ContentRoot>().updateScenedata(cv);
            */



        } catch (InvalidOperationException e) {
            Debug.Log("Collection modified during Execution " + e);
        }
    }

    private void onApple(Rect box,string name)
    {
       // AppCountUI.GetComponent<AppCounting>().objectFound(box, name);
    }
}

