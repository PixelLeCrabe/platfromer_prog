using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cinemachine_cameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float ShakeTimer;

    public static Cinemachine_cameraShake instance;

    // Start is called before the first frame update
   
    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        instance = this;
    }

    public void MCAttackShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        ShakeTimer = time;
    }

    public void MCSpecialAttackShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        ShakeTimer = time;
    }

    private void Update()
    {
        if (ShakeTimer > 0)
        {
            ShakeTimer -= Time.deltaTime;
        }
            if (ShakeTimer <= 0f)
            {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}
