using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;

public class PlayerFollow : MonoBehaviour {

    public Transform PlayerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = false;

    public bool RotateAroundPlayer = true;

    public float RotationsSpeed = 5.0f;

    static bool fogEnabled = true;
    bool weatherEnabled = true;
    bool motionBlurEnabled = true;
    GameObject weather;

    // Use this for initialization
    void Start () {
        Camera.main.GetComponent<GlobalFog>().enabled = true;
        _cameraOffset = transform.position - PlayerTransform.position;
        weather = GameObject.Find("WeatherGen");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            EnableFog();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EnableWeather();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            EnableMotionBlur();
        }

        //DontDestroyOnLoad(gameObject);
    }
    // LateUpdate is called after Update methods
    void LateUpdate () {

        if(RotateAroundPlayer)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Quaternion camTurnAngleX =
                    Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
                Quaternion camTurnAngleY =
                    Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationsSpeed, Vector3.right);

                _cameraOffset = camTurnAngleX * camTurnAngleY * _cameraOffset;
            }
        }

        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtPlayer || RotateAroundPlayer)
            transform.LookAt(PlayerTransform);
	}

    public void EnableFog()
    {
        Debug.Log("Fog Disabled");

        //turn off
        if (fogEnabled)
        {
            Camera.main.GetComponent<GlobalFog>().enabled = false;
            fogEnabled = false;
        }
        //on
        else
        {
            Camera.main.GetComponent<GlobalFog>().enabled = true;
            fogEnabled = true;
        }
        
    }

    public void EnableMotionBlur()
    {
        Debug.Log("Motion Blur Disabled");
        PostProcessVolume ppv = Camera.main.GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out MotionBlur mb);

        if (motionBlurEnabled)
        {
            mb.enabled.value = false;
            motionBlurEnabled = false;
        }
        //on
        else
        {
            mb.enabled.value = true;
            motionBlurEnabled = true;
        }

    }
        public void EnableWeather()
    {
        
        Debug.Log("Weather Disabled");

        //turn off
        if (weatherEnabled)
        {
            weather.SetActive(false);
            weatherEnabled = false;
        }
        //on
        else
        {
            weather.SetActive(true);
            weatherEnabled = true;

            
        }

    }

}
