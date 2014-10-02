namespace Assets.GameEngine.Utils
{
    using System.Collections;

    using UnityEngine;

    public class MusicUtils
    {
        public static IEnumerator ToggleBackgroundMusic(GameObject gObject, bool mute, float transitionInSeconds = 0.25f)
        {
            return SoundVolumeTransition(gObject, mute ? 0 : 1, transitionInSeconds);
        }

        private static IEnumerator SoundVolumeTransition(GameObject gObject, int toVolume, float transitionInSeconds = 0.25f, int steps = 10)
        {
            if (steps <= 0)
            {
                Debug.LogError("Steps for sound volume transition must be a positive value!");
            }

            // Calculates pause between each volume change.
            float delay = transitionInSeconds / steps;

            if (gObject != null)
            {
                var sound = gObject.GetComponent<AudioSource>();
                if (sound != null)
                {
                    // Calculates change in volume per step.
                    float delta = (toVolume - sound.volume) / steps;

                    // Music or sound should play while transitioning.
                    if (!sound.isPlaying)
                    {
                        sound.Play();
                    }

                    // Actual transitioning.
                    while (--steps >= 0)
                    {
                        sound.volume += delta;

                        yield return new WaitForSeconds(delay);
                    }

                    if (sound.volume <= 0.001f)
                    {
                        // Pausing music for optimization purposes.
                        sound.volume = 0;
                        sound.Pause();
                    }
                }
            }
        }
    }
}
