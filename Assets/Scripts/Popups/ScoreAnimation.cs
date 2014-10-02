namespace Assets.Scripts.Popups
{
    using Assets.GameEngine;

    using UnityEngine;

    public class ScoreAnimation : MonoBehaviour
    {
        public GameObject LuckyScore;
        public GameObject ConcludeScore;
        public GameObject NormalScore;
        public GameObject BadScore;
        public GameObject WorseScore;

        public float FromScale = 0.0f;
        public float ToScale = 2.0f;
        public float FadeOutScale = 2.3f;

        public float ScaleTimeStep = 0.05f;
        public float WaitTimeStep = 0.07f;
        public float FadeOutTimeStep = 0.05f;

        private GameObject lastScore;
        private SpriteRenderer lastRenderer;
        private bool showScore;

        private int state;
        private float progress;

        private void Start()
        {
            MemoryGame.Current.PropertyChanged += Current_PropertyChanged;
        }

        private void OnDestroy()
        {
            MemoryGame.Current.PropertyChanged -= Current_PropertyChanged;
        }

        private void ShowScore()
        {
            GameObject animatedObject = GetObjectByScore(MemoryGame.Current.ScoreChanged);

            if (animatedObject != null)
            {
                if (lastScore != null)
                {
                    lastScore.SetActive(false);
                }

                lastRenderer = null;

                lastRenderer = animatedObject.GetComponent<SpriteRenderer>();
                if (lastRenderer != null)
                {
                    var color = lastRenderer.color;
                    color.a = 1;
                    lastRenderer.color = color;
                }

                var scale = transform.localScale;
                scale.x = 0;
                scale.y = 0;
                transform.localScale = scale;

                lastScore = animatedObject;
                animatedObject.SetActive(true);

                state = 0;
                progress = 0;
                showScore = true;
            }
        }

        private void Update()
        {
            if (!showScore)
            {
                return;
            }

            if (state == 0)
            {
                float step = Mathf.SmoothStep(FromScale, ToScale, progress);
                progress += ScaleTimeStep;

                var scale = transform.localScale;
                scale.x = step;
                scale.y = step;
                transform.localScale = scale;

                if (progress > 1)
                {
                    ++state;
                    progress = 0;
                }
            }
            else if (state == 1)
            {
                progress += WaitTimeStep;
                if (progress > 1)
                {
                    ++state;
                    progress = 0;
                }
            }
            else if (state == 2)
            {
                float step = Mathf.SmoothStep(ToScale, FadeOutScale, progress);

                var scale = transform.localScale;
                scale.x = step;
                scale.y = step;
                transform.localScale = scale;

                if (lastRenderer != null)
                {
                    float opacity = Mathf.SmoothStep(1, 0, progress);

                    var color = lastRenderer.color;
                    color.a = opacity;
                    lastRenderer.color = color;
                }

                progress += FadeOutTimeStep;

                if (progress > 1)
                {
                    showScore = false;

                    state = 0;
                    progress = 0;
                    lastScore.SetActive(false);
                    lastRenderer = null;
                }
            }
        }

        private GameObject GetObjectByScore(int score)
        {
            var rules = MemoryGame.Current.Rules;
            GameObject animatedObject = null;
            if (score == rules.LucyPoints)
            {
                animatedObject = LuckyScore;
            }
            else if (score == rules.ConcludePoints)
            {
                animatedObject = ConcludeScore;
            }
            else if (score == rules.PairFoundPoints)
            {
                animatedObject = NormalScore;
            }
            else if (score == rules.MissPoints)
            {
                animatedObject = BadScore;
            }
            else if (score == rules.MissPoints * 2)
            {
                animatedObject = WorseScore;
            }

            return animatedObject;
        }

        void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ScoreChanged")
            {
                ShowScore();
            }
        }
    }
}
