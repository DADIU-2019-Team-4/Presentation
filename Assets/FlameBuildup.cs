using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlameBuildup : MonoBehaviour
{
    public GameObject Flame;
    private Material material;
    public float BurnDuration = 1f;
    public bool LoadNextScene = true;

    // Start is called before the first frame update
    void Start()
    {
        material = Flame.GetComponent<SpriteRenderer>().material;
        StartCoroutine(EverBlazin());
    }

    IEnumerator EverBlazin()
    {
        for (float timer = 0; timer < BurnDuration; timer += Time.unscaledDeltaTime)
        {
            material.SetFloat("_Charge", Mathf.Lerp(1f, 0, timer / BurnDuration));
            yield return null;
        }
        if (LoadNextScene)
            SceneManager.LoadScene("Presentation");
    }
}
