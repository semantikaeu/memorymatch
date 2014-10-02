namespace Assets.Scripts.Utils
{
    using UnityEngine;

    public class WorldToRect
    {
        public GameObject StartPoint;
        public GameObject EndPoint;

        public Rect GetRect()
        {
            Vector3 startPosition = Camera.main.WorldToScreenPoint(StartPoint.transform.position);
            startPosition.y = Screen.height - startPosition.y;

            Vector3 endPosition = Camera.main.WorldToScreenPoint(EndPoint.transform.position);
            endPosition.y = Screen.height - endPosition.y;

            float width = endPosition.x - startPosition.x;
            float height = endPosition.y - startPosition.y;

            return new Rect(startPosition.x, startPosition.y, width, height);
        }

        public static Rect GetRect(GameObject start, GameObject end)
        {
            Vector3 startPosition = Camera.main.WorldToScreenPoint(start.transform.position);
            startPosition.y = Screen.height - startPosition.y;

            Vector3 endPosition = Camera.main.WorldToScreenPoint(end.transform.position);
            endPosition.y = Screen.height - endPosition.y;

            float width = endPosition.x - startPosition.x;
            float height = endPosition.y - startPosition.y;

            return new Rect(startPosition.x, startPosition.y, width, height);
        }

        public static Rect GetRect(params GameObject[] points)
        {
            if (points == null || points.Length < 2)
            {
                return new Rect();
            }

            Vector3 startPosition = Camera.main.WorldToScreenPoint(points[0].transform.position);
            startPosition.y = Screen.height - startPosition.y;

            Vector3 endPosition = Camera.main.WorldToScreenPoint(points[1].transform.position);
            endPosition.y = Screen.height - endPosition.y;

            float width = endPosition.x - startPosition.x;
            float height = endPosition.y - startPosition.y;

            return new Rect(startPosition.x, startPosition.y, width, height);
        }
    }
}
