using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeControl : MonoBehaviour {

    public CanvasGroup canvas;
    //public int levelIndex;
    public bool isFading;
    public float fadeDuration = 1f;


    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 1f;
        StartCoroutine(Fade(0f));
    }
/*private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }*/

    //private void Update()
    //{

    //}

    public IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        canvas.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(canvas.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(canvas.alpha, finalAlpha))
        {
            canvas.alpha = Mathf.MoveTowards(canvas.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        isFading = false;
        canvas.blocksRaycasts = false;
    }

    //public IEnumerator Fade(float finalAlpha)
    //{
    //    //isFading = true;
    //    //canvas.blocksRaycasts = true;

    //    for (float fade = 1f; fade >= 0; fade-=0.2f)
    //    {            
    //        canvas.alpha = fade;
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}


    //public void Test()
    //{
    //    SceneManager.LoadScene(levelIndex);
    //}

    /*public void DestroySelf()
    {
        Destroy(this.gameObject);
    }*/
}
