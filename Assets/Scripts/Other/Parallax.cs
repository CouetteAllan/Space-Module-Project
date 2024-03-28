using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Min(0)] public float speed;
    public bool MovingCam = true;
    Transform cam;
    Material mat;
    Vector2 offset;
    Vector2 lastCamPos;
    Vector2 camDelta;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.transform.position;
        mat = GetComponentInChildren<MeshRenderer>().material;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (MovingCam)
        {
            camDelta = (Vector2)cam.transform.position - lastCamPos;
            offset -= camDelta * speed * Time.deltaTime;
            mat.SetVector("_Offset", offset);
            lastCamPos = cam.transform.position;
        }
        else
        {
            distance += Time.deltaTime * speed;
            mat.SetVector("_Offset", Vector2.right * distance);
            

        }

    }
}
