using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Attorney
{
    public static class TweenExtensions
    {
        public static void DoVolume(this AudioSource target, float endValue, float duration, Ease ease = Ease.Linear, TwennCallback callback = null)
        {
            FloatTween tween = new FloatTween(() => target.volume, (x) => target.volume = x, endValue, duration);
            tween.ease = ease;
            tween.onComplete = callback;
            EffectSystem.Instance.DoTween(tween.StartTween());
        }

        public static void DoAlpha(this SpriteRenderer target, float endValue, float duration, Ease ease = Ease.Linear, TwennCallback callback = null)
        {
            FloatTween tween = new FloatTween(() => target.color.a, (x) => target.color = new Color(target.color.r, target.color.g, target.color.b, x), endValue, duration);
            tween.ease = ease;
            tween.onComplete = callback;
            EffectSystem.Instance.DoTween(tween.StartTween());
        }

        public static void DoAlpha(this Image target, float endValue, float duration, Ease ease = Ease.Linear, TwennCallback callback = null)
        {
            FloatTween tween = new FloatTween(() => target.color.a, (x) => target.color = new Color(target.color.r, target.color.g, target.color.b, x), endValue, duration);
            tween.ease = ease;
            tween.onComplete = callback;
            EffectSystem.Instance.DoTween(tween.StartTween());
        }

        public static void DoMove(this Transform target, Vector3 endValue, float duration, Ease ease = Ease.Linear, TwennCallback callback = null)
        {
            Vector3Tween tween = new Vector3Tween(() => target.position, (x) => target.position = x, endValue, duration);
            tween.ease = ease;
            tween.onComplete = callback;
            EffectSystem.Instance.DoTween(tween.StartTween());
        }

        public static void DoText(this TMP_Text target, string endValue, float duration, Ease ease = Ease.Linear, TwennCallback callback = null)
        {
            StringTween tween = new StringTween(() => target.text, (x) => target.text = x, endValue, duration);
            tween.ease = ease;
            tween.onComplete = callback;
            EffectSystem.Instance.DoTween(tween.StartTween());
        }
    }
}

