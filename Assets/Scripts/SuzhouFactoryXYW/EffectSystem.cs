using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attorney
{
    public class EffectSystem : MonoBehaviour
    {
        public static EffectSystem Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void DoTween(IEnumerator tween)
        {
            StartCoroutine(tween);
        }
    }
}
