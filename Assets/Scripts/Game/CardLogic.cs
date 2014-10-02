namespace Assets.Scripts.Game
{
#if UNITY_WINRT && !UNITY_EDITOR
    using Path = System.IO.Path;
    using File = UnityEngine.Windows.File;
#endif

    using System;
    using System.Collections;

    using Assets.GameEngine;
    using Assets.GameEngine.Data;
    using Assets.GameEngine.Utils;
    using Assets.Scripts.Utils;

    using UnityEngine;

    /// <summary>
    /// Defines logic and behavior for single card with minimum game logic.
    /// Loads front image and uncover card on tap if game logic allows it.
    /// </summary>
    public class CardLogic : MonoBehaviour
    {
        private Animator animator;

        /// <summary>
        /// Gets or sets card data.
        /// </summary>
        public CardData CardData { get; set; }

        /// <summary>
        /// Sets card data and card image. (called as message)
        /// </summary>
        /// <param name="card">Card data.</param>
        public void SetCard(CardData card)
        {
            CardData = new CardData(card) { Instance = gameObject };
            StartCoroutine(SetCardAsync(card));
        }

        /// <summary>
        /// Use this for initialization. (Unity built-in method)
        /// </summary>
        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Called when mouse is down. (Unity built-in method)
        /// </summary>
        public void OnMouseDown()
        {
            ChangeCardState();
        }

        /// <summary>
        /// Sets card image asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Returns enumerator Unity async pattern.</returns>
        private IEnumerator SetCardAsync(CardData data)
        {
            string path; // = "Decks/" + data.Title + "/" + Path.GetFileNameWithoutExtension(data.Image);
            Texture2D texture;
            if (data.Image.StartsWith("res://"))
            {
                path = data.Image.Substring(6);
                texture = Resources.Load<Texture2D>(path);
            }
            else
            {
                path = WebCache.GetCachedPath(data);
                WWW www = new WWW(path);
                yield return www;

                texture = www.texture;
            }

            CardData.Texture = data.Texture = texture;

            // First child must be front image.
            var cardObject = gameObject.transform.GetChild(0);
            var spriteRenderer = cardObject.renderer as SpriteRenderer;
            if (spriteRenderer != null)
            {
                // This math will crop image to target aspect ratio and scale game object rather than texture.
                float scale;
                Rect rect = MathHelper.ScaleAndCrop(texture.width, texture.height, 277, 274, out scale);
                scale *= .84f;

                cardObject.transform.localScale = new Vector3(scale, scale);
                var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            
                spriteRenderer.sprite = sprite;
            }

            // NOTE: This method uses Unity async pattern because image can be on the Internet in future versions.
            // We're forced to use yield break because there is no yield return.
            yield break;
        }

        /// <summary>
        /// Changes the state of the card.
        /// </summary>
        private void ChangeCardState()
        {
            if (!MemoryGame.Current.CanFlipCard(CardData))
            {
                return;
            }

            bool flip = true;
            try
            {
                var currentBaseState = animator.GetCurrentAnimatorStateInfo(0);

                if (currentBaseState.IsName("Cover") || currentBaseState.IsName("Uncover"))
                {
                    flip = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error message:" + ex.Message);
            }

            if (flip)
            {
                animator.SetBool("IsUncovering", true);
                animator.SetBool("IsCovering", false);

                var sound = GetComponent<AudioSource>();
                if (sound != null)
                {
                    sound.Play();
                }

                // Notifies UI to execute game logic for flipping this card.
                var gameLogic = GameObject.Find("Game");
                gameLogic.SendMessage("FlipCard", CardData);
            }
        }
    }
}
