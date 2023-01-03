using System.Collections;
using UnityEngine;

public class DeathRay : MonoBehaviour
{
    public GameObject ray;
    private Vector3 scaleChange, positionChange;
    private void Start() 
    {
        
        scaleChange = new Vector3(+1.0f, .0f, .0f);
        StartCoroutine(ExampleCoroutine());
    }
    void FixedUpdate()
    {
        ray.transform.localScale += scaleChange;
    }
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
