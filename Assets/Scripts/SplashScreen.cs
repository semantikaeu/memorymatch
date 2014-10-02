namespace Assets.Scripts
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    /// This component will navigate to home screen after few seconds.
    /// </summary>
    public class SplashScreen : MonoBehaviour
    {
        /// <summary>
        /// Use this for initialization.
        /// </summary>
        /// ReSharper disable once UnusedMember.Local
        private void Start()
        {
            StartCoroutine(NavigateToHome());
        }

        /// <summary>
        /// Navigates to home after 3 seconds.
        /// </summary>
        /// <returns>Returns enumerator for Unity stuff.</returns>
        private IEnumerator NavigateToHome()
        {
            yield return new WaitForSeconds(3);

            Application.LoadLevel("Home");
        }
    }
}
