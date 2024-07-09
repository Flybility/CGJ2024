using UnityEngine;

namespace Attorney
{
    public delegate float FloatToFloat(float t);

    public static class Interpolation
    {
        private static FloatToFloat[] interpolations = new FloatToFloat[] {
        (float t) => t,
        (float t) => 1f - Mathf.Cos((t * Mathf.PI) / 2f),
        (float t) => Mathf.Sin((t * Mathf.PI) / 2f),
        (float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f,
        (float t) => t * t,
        (float t) => 1f - (1f - t) * (1f - t),
        (float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f,
        (float t) => t * t * t,
        (float t) => 1f - Mathf.Pow(1f - t, 3f),
        (float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f,
        (float t) => t * t * t * t,
        (float t) => 1f - Mathf.Pow(1f - t, 4f),
        (float t) => t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) / 2f,
        (float t) => t * t * t * t * t,
        (float t) => 1f - Mathf.Pow(1f - t, 5f),
        (float t) => t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5f) / 2f,
        (float t) => t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f),
        (float t) => t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t),
        (float t) => t == 0f ? 0f : t == 1f ? 1f: t < 0.5f ? Mathf.Pow(2f, 20f * t - 10f) / 2f: (2f - Mathf.Pow(2f, -20f * t + 10f)) / 2f,
        (float t) => 1 - Mathf.Sqrt(1f - Mathf.Pow(t, 2f)),
        (float t) => Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f)),
        (float t) => t < 0.5f ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * t, 2f))) / 2f : (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) / 2f,
        (float t) => 2.70158f * t * t * t - 1.70158f * t * t,
        (float t) => 1f + 2.70158f * Mathf.Pow(t - 1.70158f, 3f) + 1.70158f * Mathf.Pow(t - 1f, 2f),
        (float t) => t < 0.5 ? (Mathf.Pow(2f * t, 2f) * (3.59491f * 2f * t - 2.59491f)) / 2f : (Mathf.Pow(2f * t - 2f, 2f) * (3.59491f * (t * 2f - 2f) + 2.59491f) + 2f) / 2f,
        (float t) => t == 0f ? 0f : t == 1f ? 1f : -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * 2.0944f),
        (float t) => t == 0f ? 0f : t == 1f ? 1f : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * 2.0944f) + 1f,
        (float t) => t == 0f ? 0f : t == 1f ? 1f : t < 0.5f ? -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * 1.39626f)) / 2f : (Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * 1.39626f)) / 2f + 1f,
        (float t) => 1f - interpolations[(int)Ease.OutBounce](1f - t),
        (float t) => {
            if (t < 1f / 2.75f) {
                return 7.5625f * t * t;
            } else if (t < 2f / 2.75f) {
                return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
            } else if (t < 2.5f / 2.75f) {
                return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
            } else {
                return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
            }
        },
        (float t) => t < 0.5f ? (1f - interpolations[(int)Ease.OutBounce](1f - 2f * t)) / 2f : (1f + interpolations[(int)Ease.OutBounce](2f * t - 1f)) / 2f
    };

        public static float Calc(float t, Ease ease)
        {
            return interpolations[(int)ease](t);
        }
    }

}
