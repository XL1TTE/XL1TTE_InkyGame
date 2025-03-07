using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

public class ST_WithPicturesAndTooltips : ABS_SceneTransition
{
    [Range(2, 10)]
    public int TipMessageChangeDelay = 5;
    [Range(2, 10)]
    public int PictureChangeDelay = 5;

    public LoadingScreenData LoadingScreenData;

    public TextMeshProUGUI TipMessageHolder;
    public Image PictureHolder;
    public Image LoadingProgressHolder;

    [Range(1, 10)]
    public int TransitionDelay;

    private void Update()
    {
        if (sceneLoadingState != null && !sceneLoadingState.isDone)
        {


            // ProccentsHolder.text = $"????????: {Mathf.RoundToInt(Current * 100)}%";

        }
        else
        {
            StopLoadingScreenVisuals();
            this.gameObject.SetActive(false);
        }
    }

    public override async void StartTransition()
    {

        this.gameObject.SetActive(true);

        StartLoadingScreenVisuals();
        
        await Task.Delay(TransitionDelay * 1000);
      
    }

    private void StartLoadingScreenVisuals()
    {
        StartCoroutine(ShowPicturesCoroutine());
        StartCoroutine(ShowTipMessageCoroutine());
        StartCoroutine(ProgressBarCoroutine());
    }

    private void StopLoadingScreenVisuals()
    {
        StopCoroutine(ShowTipMessageCoroutine());
        StopCoroutine(ShowPicturesCoroutine());
    }

    #region Coroutines


    private IEnumerator ProgressBarCoroutine()
    {

        float time = 0f;
        while (time < 1.0f)
        {
            time += Time.deltaTime / TransitionDelay;

            var progress = Mathf.Clamp01(sceneLoadingState.progress / 0.9f);
            float Current = Mathf.Lerp(0, progress, time);
            LoadingProgressHolder.fillAmount = Current;

            yield return null;
        }
        LoadingProgressHolder.fillAmount = 1;
        sceneLoadingState.allowSceneActivation = true;

    }
    private IEnumerator ShowPicturesCoroutine()
    {
        while (sceneLoadingState == null || !sceneLoadingState.allowSceneActivation)
        {
            var Pictures = LoadingScreenData.Pictures;
            PictureHolder.sprite = Pictures[Random.Range(0, Pictures.Count)];
            yield return new WaitForSeconds(PictureChangeDelay);
        }

    }
    private IEnumerator ShowTipMessageCoroutine()
    {
        while (sceneLoadingState == null || !sceneLoadingState.allowSceneActivation)
        {
            var Messages = LoadingScreenData.TipMessages;
            TipMessageHolder.text = Messages[Random.Range(0, Messages.Count)];
            yield return new WaitForSeconds(TipMessageChangeDelay);
        }

    }
    #endregion

}
