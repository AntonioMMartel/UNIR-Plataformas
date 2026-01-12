using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxFactor = 0.5f;

    private Transform camera;
    private Vector3 lastCameraPos;
    private float textureUnitSizeX;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        lastCameraPos = camera.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit; // Ambos background tienen mismas dimensiones asi que con 1 me basta
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 delta = camera.position - lastCameraPos;

        transform.position += new Vector3(delta.x * parallaxFactor, 0f, 0f);
        lastCameraPos = camera.position;

        // Tiling
        if (Mathf.Abs(camera.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offset = (camera.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(camera.position.x + offset, transform.position.y, transform.position.z);
        }
    }
}
