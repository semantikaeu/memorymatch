namespace Assets.GameEngine.Providers
{
    using UnityEngine;

    public class DeckLayoutProvider
    {
        public static Vector2 GetGridSize(int cards)
        {
            int columns = 0;
            int rows = 0;

            switch (cards)
            {
                case 6:
                    columns = 3;
                    rows = 2;
                    break;

                case 8:
                    columns = 4;
                    rows = 2;
                    break;

                case 12:
                    columns = 4;
                    rows = 3;
                    break;

                case 18:
                    columns = 6;
                    rows = 3;
                    break;

                case 20:
                    columns = 5;
                    rows = 4;
                    break;

                case 24:
                    columns = 6;
                    rows = 4;
                    break;

                case 26:
                    columns = 8;
                    rows = 5;
                    break;

                case 28:
                    columns = 7;
                    rows = 4;
                    break;

                case 30:
                    columns = 6;
                    rows = 5;
                    break;

                case 32:
                    columns = 8;
                    rows = 4;
                    break;

                case 36:
                    columns = 6;
                    rows = 6;
                    break;

                case 40:
                    columns = 8;
                    rows = 5;
                    break;

                case 48:
                    columns = 8;
                    rows = 6;
                    break;

                case 50:
                    columns = 10;
                    rows = 5;
                    break;

                case 54:
                    columns = 9;
                    rows = 6;
                    break;

                case 56:
                    columns = 8;
                    rows = 7;
                    break;

                case 60:
                    columns = 10;
                    rows = 6;
                    break;

                case 64:
                    columns = 8;
                    rows = 8;
                    break;

                case 66:
                    columns = 11;
                    rows = 6;
                    break;

                case 70:
                    columns = 10;
                    rows = 7;
                    break;

                case 72:
                    columns = 12;
                    rows = 6;
                    break;

                case 78:
                    columns = 13;
                    rows = 6;
                    break;

                case 80:
                    columns = 10;
                    rows = 8;
                    break;

                case 84:
                    columns = 12;
                    rows = 7;
                    break;
            }

            return new Vector2(columns, rows);
        }
    }
}
