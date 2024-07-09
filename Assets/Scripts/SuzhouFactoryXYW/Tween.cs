using System.Collections;
using UnityEngine;

namespace Attorney
{
    public delegate T TweenGetter<T>();

    public delegate void TweenSetter<T>(T t);

    public delegate void TwennCallback();

    public abstract class Tween<T>
    {
        protected TweenGetter<T> getter;

        protected TweenSetter<T> setter;

        protected T startValue;

        protected T endValue;

        protected float timer;

        protected float duration;

        public Ease ease = Ease.Linear;

        public TwennCallback onComplete;

        public Tween(TweenGetter<T> _getter, TweenSetter<T> _setter, T _endValue, float _duration)
        {
            getter = _getter;
            setter = _setter;
            startValue = getter();
            endValue = _endValue;
            timer = 0f;
            duration = _duration;
        }

        public abstract void ChangeValue(float t);

        public IEnumerator StartTween()
        {
            WaitForEndOfFrame frame = new WaitForEndOfFrame();
            while (timer < duration)
            {
                float t = Interpolation.Calc(timer / duration, ease);
                ChangeValue(t);
                timer += Time.deltaTime;
                yield return frame;
            }
            ChangeValue(1f);
            onComplete?.Invoke();
        }
    }

    public class FloatTween : Tween<float>
    {
        public FloatTween(TweenGetter<float> _getter, TweenSetter<float> _setter, float _endValue, float _duration) : base(_getter, _setter, _endValue, _duration) { }

        public override void ChangeValue(float t)
        {
            float v = Mathf.Lerp(startValue, endValue, t);
            setter(v);
        }
    }

    public class Vector2Tween : Tween<Vector2>
    {
        public Vector2Tween(TweenGetter<Vector2> _getter, TweenSetter<Vector2> _setter, Vector2 _endValue, float _duration) : base(_getter, _setter, _endValue, _duration) { }

        public override void ChangeValue(float t)
        {
            Vector2 v = Vector2.Lerp(startValue, endValue, t);
            setter(v);
        }
    }

    public class Vector3Tween : Tween<Vector3>
    {
        public Vector3Tween(TweenGetter<Vector3> _getter, TweenSetter<Vector3> _setter, Vector3 _endValue, float _duration) : base(_getter, _setter, _endValue, _duration) { }

        public override void ChangeValue(float t)
        {
            Vector3 v = Vector3.Lerp(startValue, endValue, t);
            setter(v);
        }
    }

    public class ColorTween : Tween<Color>
    {
        public ColorTween(TweenGetter<Color> _getter, TweenSetter<Color> _setter, Color _endValue, float _duration) : base(_getter, _setter, _endValue, _duration) { }

        public override void ChangeValue(float t)
        {
            Color v = Color.Lerp(startValue, endValue, t);
            setter(v);
        }
    }

    public class StringTween : Tween<string>
    {
        private const string COLORTAGHEAD = "<color=";

        private const string COLORTAGTAIL = "</color>";

        private const char RIGHTARROW = '>';

        public StringTween(TweenGetter<string> _getter, TweenSetter<string> _setter, string _endValue, float _duration) : base(_getter, _setter, _endValue, _duration) { }

        public override void ChangeValue(float t)
        {
            string result = startValue + endValue;
            bool isLabel = false;
            int index = Mathf.FloorToInt(startValue.Length + t * endValue.Length);
            string subStr = endValue.Substring(index, endValue.Length - index);
            if (subStr.StartsWith(COLORTAGHEAD))
            {
                isLabel = true;
                while (endValue[index] != RIGHTARROW) index++;
                index++;
            }
            else if (subStr.StartsWith(COLORTAGTAIL))
            {
                isLabel = false;
                while (endValue[index] != RIGHTARROW) index++;
                index++;
            }
            setter(result.Substring(0, Mathf.Min(index + 1, endValue.Length)) + (isLabel ? COLORTAGTAIL : string.Empty));
        }
    }

}
