namespace Assets.Scripts.CreateSet
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Assets.GameEngine.Data;
    using Assets.GameEngine.Providers;
    using Assets.Scripts.Controls;
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class CreateSetForm : MonoBehaviour
    {
        public GameObject SearchItem;
        public GameObject SpawnStartLocation;
        public GameObject SpawnEndLocation;
        public GameObject SpawnMyDeckStartLocation;
        public GameObject SpawnMyDeckEndLocation;
        public GameObject NoResults;
        public GameObject InstructionsGameObject;
        public GameObject SearchingGameObject;
        public GameObject SearchTextArea;
        public GameObject TextArea;
        public GameObject SaveSetObject;
        public GameObject StartSetInputObject;
        public GameObject EndSetInputObject;
        public Texture FrameTexture;
        public Font DefaultFont;
        public GameObject[] SaveSetPositions;
        public Texture SaveSetTexture;
        public GameObject[] SaveButtonPositions;
        public Texture SaveTexture;
        public GameObject[] CancelButtonPositions;
        public Texture CancelTexture;
        public GameObject StartSaveButton;
        public Texture MiniCancelTexture;
        public Texture MiniInfoTexture;

        private bool showSaveSet;
        private ItemsControl searchItems = new ItemsControl();
        private float searchScrollContentHeight;
        private Vector2 searchScrollViewVector;
        private List<CardData> selectedCards = new List<CardData>();
        private List<IUnityControl> selectedCardsImages = new List<IUnityControl>();
        private float selectedCardsScrollContentHeight;
        private Vector2 selectedCardsScrollViewVector;
        private bool isMousePressed;
        private int mouseTimeout = 30;
        private int x;
        private int breakIn = 3;
        private string nameSet = string.Empty;
        private int minCard = 6;
        private int maxCards = 42;

        // Use this for initialization
        private void Start()
        {
            //DataMessenger.LastSetsQuery = "butterfly";
            //GetStuffFromEuropeana.Current.StartSearch(DataMessenger.LastSetsQuery);
        }

        private void OnGUI()
        {
            Vector3 startPosition = Camera.main.WorldToScreenPoint(SpawnStartLocation.transform.position);
            startPosition.y = Screen.height - startPosition.y;
            startPosition.x -= Scale(14);

            Vector3 endPosition = Camera.main.WorldToScreenPoint(SpawnEndLocation.transform.position);
            endPosition.y = Screen.height - endPosition.y;
            endPosition.x += Scale(14);

            float scrollWidth = endPosition.x - startPosition.x;
            float scrollHeight = endPosition.y - startPosition.y;

            searchScrollViewVector =
                GUI.BeginScrollView(
                    new Rect(startPosition.x, startPosition.y, scrollWidth, scrollHeight),
                    searchScrollViewVector,
                    new Rect(0, 0, scrollWidth - Scale(30), searchScrollContentHeight));

            searchItems.Render();

            GUI.EndScrollView(true);

            startPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckStartLocation.transform.position);
            startPosition.y = Screen.height - startPosition.y;

            endPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckEndLocation.transform.position);
            endPosition.y = Screen.height - endPosition.y;

            scrollWidth = endPosition.x - startPosition.x;
            scrollHeight = endPosition.y - startPosition.y;

            selectedCardsScrollViewVector =
                GUI.BeginScrollView(
                    new Rect(startPosition.x, startPosition.y, scrollWidth, scrollHeight),
                    selectedCardsScrollViewVector,
                    new Rect(0, 0, scrollWidth - Scale(30), selectedCardsScrollContentHeight));

            Rect position = new Rect(0, 0, scrollWidth, selectedCardsScrollContentHeight);
            GUI.BeginGroup(position);

            for (int i = 0; i < selectedCardsImages.Count; i++)
            {
                GUI.BeginGroup(selectedCardsImages[i].Rect);
                selectedCardsImages[i].Render();
                GUI.EndGroup();
            }

            GUI.EndGroup();

            GUI.EndScrollView(true);

            Color backup = GUI.contentColor;
            GUI.skin.settings.cursorColor = Color.black;

            GUI.contentColor = new Color(1, 1, 0);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(SearchTextArea.transform.position);
            float y = Screen.height - screenPosition.y;
            Rect rect = new Rect(screenPosition.x + Scale(15), y + Scale(-8f), Scale(100), Scale(20));

            GUIStyle style = new GUIStyle();
            style.fontSize = Scale(14);
            style.clipping = TextClipping.Clip;
            style.richText = true;

            // *****************************************************************************************************************************************************************************/
            // ******************************************************************************** READ THIS! *********************************************************************************/
            // *****************************************************************************************************************************************************************************/

            // HACK: This is a bug fix introduced in Unity 4.5.x! Remove this once cursor is higher than text!
            Rect fixForUnity4_5CursorBug = new Rect(rect.x, rect.y - Scale(6), rect.width, rect.height);

            // To fix this, I display text field with transparent font color moved up.
            Color backupColor2 = GUI.contentColor;
            GUI.contentColor = new Color(0, 0, 0, 0);
            DataMessenger.LastSetsQuery = GUI.TextField(fixForUnity4_5CursorBug, DataMessenger.LastSetsQuery, style);

            // And than display text on correct position with correct color.
            GUI.contentColor = backupColor2;
            GUI.Label(rect, DataMessenger.LastSetsQuery, style);
            // End of HACK.

            // NOTE: After Unity 4.6 update uncomment this line!
            //DataMessenger.LastSetsQuery = GUI.TextField(rect, DataMessenger.LastSetsQuery, style);

            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
            {
                Debug.Log("Start searching...");
                GetStuffFromEuropeana.Current.StartSearch(DataMessenger.LastSetsQuery);
            }

            GUI.contentColor = backup;

            if (showSaveSet)
            {
                RenderSavePopup();
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && !isMousePressed && mouseTimeout <= 0)
                {
                    mouseTimeout = 30;
                    isMousePressed = true;

                    MouseDown();
                }
                else
                {
                    isMousePressed = false;

                    if (mouseTimeout > 0)
                    {
                        --mouseTimeout;
                    }
                }
            }
        }

        private void RenderSavePopup()
        {
            GUI.DrawTexture(WorldToRect.GetRect(SaveSetPositions), SaveSetTexture);
            GUI.DrawTexture(WorldToRect.GetRect(SaveButtonPositions), SaveTexture);
            GUI.DrawTexture(WorldToRect.GetRect(CancelButtonPositions), CancelTexture);

            GUI.skin.settings.cursorColor = Color.white;

            var style = new GUIStyle();
            style.fontSize = Scale(18);
            style.clipping = TextClipping.Clip;
            style.normal.background = null;
            style.normal.textColor = Color.white;

            nameSet = GUI.TextField(WorldToRect.GetRect(StartSetInputObject, EndSetInputObject), nameSet, style);
        }

        private void SaveCurrentSet()
        {
            print("Save current set");
            CardFactory.Add(nameSet, selectedCards);
            showSaveSet = false;

            Application.LoadLevel("DeckSelector");
        }

        private void ShowSavePopup()
        {
            showSaveSet = true;

            if (SaveSetObject != null)
            {
                SaveSetObject.SetActive(true);
            }
        }

        private void CancelSet()
        {
            nameSet = string.Empty;
            showSaveSet = false;

            // Uncomment if removing cards after cancel is necessary.
            //selectedCards.Clear();
            //selectedCardsImages.Clear();
            //selectedCardsScrollContentHeight = 0;
            //x = 0;

            if (SaveSetObject != null)
            {
                SaveSetObject.SetActive(false);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            var client = GetStuffFromEuropeana.Current;
            client.Update();

            if (client.HasStarted)
            {
                searchItems.Items.Clear();
                NoResults.SetActive(false);
            }

            if (client.IsCompleted)
            {
                Debug.Log("Search completed!");

                searchItems.Items.Clear();
                searchItems.MeasureSize();

                var results = client.Results.Result;

                if (results == null || results.Count == 0)
                {
                    NoResults.SetActive(true);
                }
                else
                {
                    StartCoroutine(LoadImages(results));
                }
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.LoadLevel("Home");
            }

            UpdateUI();

            // This must execute after UpdateUI.
            if (client.IsCompleted)
            {
                client.Reset();
            }
        }

        private IEnumerator LoadImages(IEnumerable<string> results)
        {
            Vector3 startPosition = Camera.main.WorldToScreenPoint(SpawnStartLocation.transform.position);
            startPosition.y = Screen.height - startPosition.y;

            Vector3 endPosition = Camera.main.WorldToScreenPoint(SpawnEndLocation.transform.position);
            endPosition.y = Screen.height - endPosition.y;

            float width = endPosition.x - startPosition.x;
            int boxWidth = (int)(width * 0.8f);
            float leftRightMargin = width * 0.2f;
            float topBottomMargin = width * 0.05f;

            searchScrollContentHeight = 0;

            int i = DataMessenger.LastSetsQuery.GetHashCode();
            foreach (var result in results)
            {
                CardData data = new CardData();
                data.Image = result;
                data.Id = i;
                data.Title = DataMessenger.LastSetsQuery;

                yield return StartCoroutine(WebCache.Cache(data));
                string path = WebCache.GetCachedPath(data);

                WWW www = new WWW(path);
                yield return www;

                var image = AddToSearchResults(www.texture, boxWidth, leftRightMargin, topBottomMargin);
                image.Tag = data;

                searchItems.Items.Add(image);

                searchItems.MeasureSize();
                searchScrollContentHeight = searchItems.Rect.height;
                ++i;
            }
        }

        private int Scale(int number)
        {
            return (int)(number * Initialize.ScaleHeight);
        }

        private float Scale(float number)
        {
            return number * Initialize.ScaleHeight;
        }

        private void MouseDown()
        {
            if (searchItems.Items.Count <= 0 || selectedCards.Count >= maxCards)
            {
                return;
            }

            Vector2 position = Input.mousePosition;
            position.y = Screen.height - position.y;

            bool isTapped = false;
            Rect rect = WorldToRect.GetRect(SpawnStartLocation, SpawnEndLocation);
            if (rect.Contains(position))
            {
                position.x += searchScrollViewVector.x;
                position.y += searchScrollViewVector.y;
                position.x -= rect.x - Scale(14);
                position.y -= rect.y;

                Debug.Log(position);

                var item = searchItems.GetHitTestItem(position) as ImageViewWithFrame;
                if (item != null)
                {
                    var card = item.Tag as CardData;
                    if (card != null && selectedCards.All(c => c.Id != card.Id))
                    {
                        selectedCards.Add(card);
                        AddToSet(item);

                        if (selectedCards.Count >= maxCards && SaveSetObject != null)
                        {
                            ShowSavePopup();
                        }
                        else if (selectedCards.Count >= minCard)
                        {
                            StartSaveButton.SetActive(true);
                        }

                        isTapped = true;
                    }
                }
            }

            if (!isTapped)
            {
                var startPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckStartLocation.transform.position);
                startPosition.y = Screen.height - startPosition.y;

                var endPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckEndLocation.transform.position);
                endPosition.y = Screen.height - endPosition.y;

                position = Input.mousePosition;
                position.y = Screen.height - position.y;

                rect = WorldToRect.GetRect(SpawnMyDeckStartLocation, SpawnMyDeckEndLocation);

                position.x += selectedCardsScrollViewVector.x;
                position.y += selectedCardsScrollViewVector.y;
                position.x -= rect.x - Scale(14);
                position.y -= rect.y;

                for (int i = 0; i < selectedCardsImages.Count; ++i)
                {
                    if (selectedCardsImages[i].Rect.Contains(position))
                    {
                        selectedCards.RemoveAt(i);
                        selectedCardsImages.RemoveAt(i);

                        if (selectedCards.Count < minCard)
                        {
                            StartSaveButton.SetActive(false);
                        }

                        RebuildSelectedCardsList();

                        isTapped = true;
                        break;
                    }
                }
            }
        }

        private void AddToSet(ImageViewWithFrame imageView)
        {
            Vector3 p = new Vector3(Scale(10), Scale(10));

            var startPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckStartLocation.transform.position);
            startPosition.y = Screen.height - startPosition.y;

            var endPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckEndLocation.transform.position);
            endPosition.y = Screen.height - endPosition.y;

            float controlWidth = endPosition.x - startPosition.x;
            float fullSize = controlWidth / 3.6f;

            GroupControl item = new GroupControl();

            var framedImage = new ImageViewWithFrame();
            framedImage.BigSize = (int)(fullSize * 0.9f);
            framedImage.ImageTexture = imageView.ImageTexture;
            framedImage.FrameTexture = imageView.FrameTexture;
            framedImage.LeftMargin = 0;
            framedImage.RightMargin = (int)(fullSize * 0.05f);
            framedImage.TopMargin = 0;
            framedImage.MeasureSize();

            item.Items.Add(framedImage);
            
            Rect rect = new Rect();
            rect.x = (int)(p.x + (int)(fullSize * 0.05f) + (fullSize * (x % breakIn)));
            rect.y = (int)(p.y + (((p.y * 2) + framedImage.BigSize) * (int)(x / breakIn)));
            rect.width = framedImage.Rect.width;
            rect.height = framedImage.Rect.height;
            item.Rect = rect;

            var cancelImage = new ImageView();
            cancelImage.Width = cancelImage.Height = (int)(fullSize * 0.22);
            cancelImage.ImageScaleMode = ScaleMode.ScaleToFit;
            cancelImage.Offset.x = (int)(fullSize * 0.74);
            cancelImage.Offset.y = (int)(fullSize * 0.11);
            cancelImage.ImageTexture = MiniCancelTexture;
            cancelImage.MeasureSize();

            // Revert auto scalling stuff in measure size.
            rect = cancelImage.ImageRect;
            rect.x = cancelImage.Offset.x;
            rect.y = cancelImage.Offset.y;
            rect.width = rect.height = cancelImage.Width;
            cancelImage.ImageRect = rect;

            item.Tag = imageView.Tag;
            item.Items.Add(cancelImage);
            selectedCardsImages.Add(item);

            item.MeasureSize();

            selectedCardsScrollContentHeight = item.Rect.y + item.Rect.height;
            ++x;
        }

        private void RebuildSelectedCardsList()
        {
            x = 0;

            selectedCardsImages.Clear();
            foreach (var cardData in selectedCards)
            {
                IUnityControl card = searchItems.Items.FirstOrDefault(c => ((CardData)c.Tag).Id == cardData.Id);
                var view = card as ImageViewWithFrame;

                if (view != null)
                {
                    AddToSet(view);
                }
            }
        }

        private void AddToSet2(ImageViewWithFrame imageView)
        {
            Vector3 p = new Vector3(Scale(10), Scale(10));

            var startPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckStartLocation.transform.position);
            startPosition.y = Screen.height - startPosition.y;

            var endPosition = Camera.main.WorldToScreenPoint(SpawnMyDeckEndLocation.transform.position);
            endPosition.y = Screen.height - endPosition.y;

            float controlWidth = endPosition.x - startPosition.x;
            float fullSize = controlWidth / 3.6f;

            var framedImage = new ImageViewWithFrame();
            framedImage.BigSize = (int)(fullSize * 0.9f);
            framedImage.ImageTexture = imageView.ImageTexture;
            framedImage.FrameTexture = imageView.FrameTexture;
            framedImage.LeftMargin = p.x + (int)(fullSize * 0.05f) + (fullSize * (x % breakIn));
            framedImage.RightMargin = (int)(fullSize * 0.05f);
            framedImage.TopMargin = p.y + (((p.y * 2) + framedImage.BigSize) * (int)(x / breakIn));
            framedImage.MeasureSize();

            var cancelImage = new ImageView();
            cancelImage.Width = cancelImage.Height = (int)(framedImage.BigSize * 0.12);
            cancelImage.ImageScaleMode = ScaleMode.ScaleToFit;
            cancelImage.LeftMargin = (int)framedImage.LeftMargin + (int)(framedImage.BigSize * 0.85);
            cancelImage.TopMargin = (int)framedImage.TopMargin + Scale(5);
            cancelImage.ImageTexture = MiniCancelTexture;
            cancelImage.MeasureSize();

            selectedCardsImages.Add(framedImage);

            selectedCardsScrollContentHeight = framedImage.TopMargin + framedImage.Rect.height;
            ++x;
        }

        private ImageViewWithFrame AddToSearchResults(
            Texture texture,
            int imageSize,
            float leftRightMargin,
            float topBottomMargin)
        {
            var image = new ImageViewWithFrame();
            image.BigSize = imageSize;
            image.ImageTexture = texture;
            image.FrameTexture = FrameTexture;
            image.LeftMargin = leftRightMargin / 2.0f;
            image.RightMargin = leftRightMargin / 2.0f;
            image.TopMargin = topBottomMargin / 2.0f;
            image.BottomMargin = topBottomMargin / 2.0f;

            image.MeasureSize();

            return image;
        }

        private void SaveSet()
        {
            SaveCurrentSet();
        }

        private void UpdateUI()
        {
            var client = GetStuffFromEuropeana.Current;

            SetGameObjectActiveState(InstructionsGameObject, selectedCards.Count == 0);

            if (client.HasStarted)
            {
                SetGameObjectActiveState(SearchingGameObject, true);
                SetGameObjectActiveState(NoResults, false);
            }

            if (client.IsCompleted)
            {
                var results = client.Results.Result;

                SetGameObjectActiveState(SearchingGameObject, false);
                SetGameObjectActiveState(NoResults, results == null || results.Count == 0);
            }
        }

        private void SetGameObjectActiveState(GameObject gameObject, bool isActive)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(isActive);
            }
        }
    }
}
