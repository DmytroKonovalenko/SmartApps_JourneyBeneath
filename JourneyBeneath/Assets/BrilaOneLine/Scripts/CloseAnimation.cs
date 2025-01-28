using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private float animationDuration = 0.5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Додаємо CanvasGroup, якщо його немає
        canvasGroup = panelRectTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panelRectTransform.gameObject.AddComponent<CanvasGroup>();
        }

        
    }
   
    public void AnimateClosePanel()
    {
        Sequence animationSequence = DOTween.Sequence();

        // Анімація прозорості (фейд) та зменшення масштабу
        animationSequence
            .Join(canvasGroup.DOFade(0, animationDuration).SetEase(Ease.Linear))
            .Join(panelRectTransform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack));

        // Додавання колбеку після завершення анімації
        animationSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            canvasGroup.alpha = 1; // Відновлюємо прозорість для подальшого використання
        });
    }
}
