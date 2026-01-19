using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightColor = Color.blue;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float lerpSpeed = 1f;
    private Material mat;
    private float time;
    void Start()
    {
        transform.position = new Vector3(3, 4, 1);
        transform.localScale = Vector3.one * 1.3f;
        rotationSpeed = Random.Range(0.0f, 40.0f);
        mat = meshRenderer.material;
    }


    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0.0f, 0.0f);
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, moveSpeed * Time.deltaTime);
        time += Time.deltaTime * lerpSpeed;
        float lerpValue = Mathf.PingPong(time, 1f);
        mat.color = Color.Lerp(defaultColor, highlightColor, lerpValue);
    }

}
