using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Vector2 parallaxFactor = new Vector2(0.5f, 0.5f);

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

        transform.position += new Vector3(delta.x * parallaxFactor.x, delta.y * parallaxFactor.y, 0f);
        lastCameraPos = camera.position;

    }
}
