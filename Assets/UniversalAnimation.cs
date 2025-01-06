using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

public class UniversalDOTweenAnimator : MonoBehaviour
{
    public enum AnimationType
    {
        Move,
        Rotate,
        Scale,
        Color,
        Fade
    }

    [Header("General Settings")]
    [SerializeField] public AnimationType animationType; // ��� ��������
    [SerializeField] private Transform targetTransform; // ������� ������ (���� ���������)
    [SerializeField] private float duration = 1f; // ������������ ��������
    [SerializeField] private Ease easeType = Ease.Linear; // ��� easing
    [SerializeField] public bool loopAnimation = false; // ����������� ��������
    [SerializeField] private LoopType loopType = LoopType.Restart; // ��� �����
    [SerializeField] private bool autoPlay = true; // ������ �������� ��� ������

    [Header("Move Settings")]
    [SerializeField] private Vector3 moveTargetPosition;

    [Header("Rotate Settings")]
    [SerializeField] private Vector3 rotateTargetEulerAngles;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 scaleTarget;

    [Header("Color Settings")]
    [SerializeField] private Color targetColor;
    [SerializeField] private Renderer targetRenderer; // ��� ������ � ������
    [SerializeField] private Image targetImage; // ��� UI Image

    [Header("Fade Settings")]
    [SerializeField] private float fadeTargetValue; // ������� �������� ������������ (0-1)
    [SerializeField] private CanvasGroup targetCanvasGroup; // ��� CanvasGroup

    private Tween currentTween;

    private void Start()
    {
        if (autoPlay)
        {
            PlayAnimation();
        }
    }

    public void PlayAnimation()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        switch (animationType)
        {
            case AnimationType.Move:
                if (targetTransform != null)
                {
                    currentTween = targetTransform.DOMove(moveTargetPosition, duration).SetEase(easeType);
                }
                break;

            case AnimationType.Rotate:
                if (targetTransform != null)
                {
                    currentTween = targetTransform.DORotate(rotateTargetEulerAngles, duration).SetEase(easeType);
                }
                break;

            case AnimationType.Scale:
                if (targetTransform != null)
                {
                    currentTween = targetTransform.DOScale(scaleTarget, duration).SetEase(easeType);
                }
                break;

            case AnimationType.Color:
                if (targetRenderer != null)
                {
                    currentTween = targetRenderer.material.DOColor(targetColor, duration).SetEase(easeType);
                }
                else if (targetImage != null)
                {
                    currentTween = targetImage.DOColor(targetColor, duration).SetEase(easeType);
                }
                break;

            case AnimationType.Fade:
                if (targetCanvasGroup != null)
                {
                    currentTween = targetCanvasGroup.DOFade(fadeTargetValue, duration).SetEase(easeType);
                }
                break;

            default:
                Debug.LogWarning("Invalid animation type selected.");
                break;
        }

        if (loopAnimation && currentTween != null)
        {
            currentTween.SetLoops(-1, loopType);
        }
    }

    public void StopAnimation()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(UniversalDOTweenAnimator))]
public class UniversalDOTweenAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UniversalDOTweenAnimator animator = (UniversalDOTweenAnimator)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("animationType"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetTransform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("easeType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("loopAnimation"));

        if (animator.loopAnimation)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loopType"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("autoPlay"));

        switch (animator.animationType)
        {
            case UniversalDOTweenAnimator.AnimationType.Move:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveTargetPosition"));
                break;

            case UniversalDOTweenAnimator.AnimationType.Rotate:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateTargetEulerAngles"));
                break;

            case UniversalDOTweenAnimator.AnimationType.Scale:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleTarget"));
                break;

            case UniversalDOTweenAnimator.AnimationType.Color:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetRenderer"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetImage"));
                break;

            case UniversalDOTweenAnimator.AnimationType.Fade:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeTargetValue"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetCanvasGroup"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif