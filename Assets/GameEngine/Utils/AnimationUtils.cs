namespace Assets.GameEngine.Utils
{
    using System.Collections;

    using UnityEngine;

    public class AnimationUtils
    {
        public static IEnumerator AnimateScale(GameObject gObject, float scale, float transitionInSeconds = 0.25f)
        {
            if (transitionInSeconds < 0)
            {
                Debug.LogError("Transition time must be greather or equal 0!");
            }

            float frameTime = 0.033f;
            float delta = scale - gObject.transform.localScale.x;
            float timeStep = transitionInSeconds / frameTime;
            float scaleStep = delta / timeStep;

            Vector3 localScale;

            float timeLeft = transitionInSeconds;

            timeLeft -= frameTime;
            while (timeLeft > 0)
            {
                localScale = gObject.transform.localScale;
                localScale.x += scaleStep;
                localScale.y += scaleStep;

                gObject.transform.localScale = localScale;

                yield return new WaitForSeconds(frameTime);

                timeLeft -= frameTime;
            }

            localScale = gObject.transform.localScale;
            localScale.x = scale;
            localScale.y = scale;

            gObject.transform.localScale = localScale;
        }
    }
}
