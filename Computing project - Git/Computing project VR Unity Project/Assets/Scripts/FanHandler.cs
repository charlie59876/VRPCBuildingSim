using UnityEngine;

public class FanHandler : MonoBehaviour
{
    [SerializeField] private float speed = .5f;
    [SerializeField] private Transform[] fans;
    [SerializeField] private Material fanMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fanMat.mainTextureOffset = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform t in fans)
        {
            t.Rotate(Vector3.up, speed);
        }
    }
}
