using UnityEngine;

public class SkyManager : MonoBehaviour
{
    [SerializeField] float _skySpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * _skySpeed);
    }
}
