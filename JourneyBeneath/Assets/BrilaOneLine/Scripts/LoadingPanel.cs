using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private GameObject logoObject;
    [SerializeField] private List<GameObject> loadingDots;
    [SerializeField] private GameObject patricleBubles;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

        PlayLogoAnimation();
        StartCoroutine(StartLoadingDotsSequence(0.3f)); 
    }

    private void PlayLogoAnimation()
    {
        logoObject.transform.localScale = Vector3.zero;

        Sequence logoSequence = DOTween.Sequence();

        logoSequence.Append(logoObject.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack));
        logoSequence.Join(logoObject.transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360));
    }

    private IEnumerator StartLoadingDotsSequence(float delayBetweenDots)
    {
        for (int i = 0; i < loadingDots.Count; i++)
        {
            
            loadingDots[i].SetActive(true);

            
            yield return new WaitForSeconds(delayBetweenDots);
        }

       
        HidePanelWithShrinkEffect();
    }

    private void HidePanelWithShrinkEffect()
    {
        // Анімація масштабу до нуля
        transform.DOScale(Vector3.zero, 0.8f).SetEase(Ease.Linear).OnComplete(() =>
        {
            // Видалення об'єкта і частинок після завершення анімації
            Destroy(gameObject);
            patricleBubles.SetActive(false);
        });

        // Паралельно зменшується прозорість
        canvasGroup.DOFade(0, 0.8f).SetEase(Ease.Linear);
    }
}
