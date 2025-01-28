using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimation : MonoBehaviour
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

        canvasGroup.alpha = 0; // Початкова прозорість
    }

    private void OnEnable()
    {
        AnimateOpenPanel();
    }

    private void OnDisable()
    {
        AnimateClosePanel();
    }

    public void AnimateOpenPanel()
    {
        panelRectTransform.localScale = Vector3.zero;

        Sequence animationSequence = DOTween.Sequence();
        // Анімація прозорості та масштабу
        animationSequence
            .Join(canvasGroup.DOFade(1, animationDuration).SetEase(Ease.Linear))
            .Join(panelRectTransform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack));
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
