using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Esp32BleAndroidSampleCode : MonoBehaviour
{
    private string TAG = "ESP32";
    private AndroidJavaObject obj;

    public float accelx;
    public float accely;
    public float accelz;

    public int counter = 0;
    public string text;

    public Text starttext;
    public Text updatetext;
    public Text exceptiontext;
    //delete this \/
    public Text errortext;

    void Start()
    {
        this.transform.localScale = new Vector3(7, 7, 7);

        starttext = GameObject.Find("StartText").GetComponent<Text>();
        starttext.text = "";
        updatetext = GameObject.Find("UpdateText").GetComponent<Text>();
        updatetext.text = "";
        exceptiontext = GameObject.Find("ExceptionText").GetComponent<Text>();
        exceptiontext.text = "";

        UnityEngine.Debug.LogWarning(TAG + " start");
        obj = new AndroidJavaObject("com.tomosoft.esp32bleandroid.Esp32BleAndroid");

        string str = obj.Call<string>("onScanner");
        UnityEngine.Debug.LogWarning(TAG + " " + str);

        starttext.text = str;
    }

    // Update is called once per frame
    void Update()
    {
        float[] acceldata = new float[3];
        byte[] data;

        string str = obj.Call<string>("UpdateRead");
        //delete this \/
        errortext.text = "Text: " + obj;
        UnityEngine.Debug.LogWarning(TAG + " Update: " + str);

        if (str == null)
        {
            return;
        }

        updatetext.text = str;

        //try
        // {
        data = str.Split(',').Select(byte.Parse).ToArray();
            string text = System.Text.Encoding.UTF8.GetString(data);
            //delete this \/
            //errortext.text = "Text: " + text;

            string[] arr = text.Split(',');
            acceldata[0] = float.Parse(arr[0]);
            acceldata[1] = float.Parse(arr[1]);
            acceldata[2] = float.Parse(arr[2]);
            UnityEngine.Debug.LogWarning(TAG + " x: " + acceldata[0] + " y: " + acceldata[1] + " z: " + acceldata[2]);

            accelx = acceldata[0] * 100 + (-90);
            accely = acceldata[1] * 100;
            accelz = acceldata[2] * 100;

            transform.rotation = Quaternion.AngleAxis(accelx, Vector3.up) * Quaternion.AngleAxis(accely, Vector3.right);
            //delete this \/
            //errortext.text = "Ok:" + TAG + " x: " + acceldata[0] + " y: " + acceldata[1] + " z: " + acceldata[2];
      //  }
       // catch (Exception e)
       /* {
            exceptiontext.text = e.ToString();
            UnityEngine.Debug.LogError(TAG + e.ToString());

            //delete this \/
            errortext.text = "Error: " + e.ToString();
      */ // }
       // finally
       // { }
    }


    private void OnApplicationQuit()
    {
        UnityEngine.Debug.LogWarning(TAG + " OnApplicationQuit: ");
        string str = obj.Call<string>("Quit");
        UnityEngine.Debug.LogWarning(TAG + " OnApplicationQuit: " + str);
    }
}
