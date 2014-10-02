namespace Assets.Scripts.Utils
{
    using System;
    using System.Collections.Generic;

    using MyEuropeana.Client;

    using UnityEngine;

    public class GetStuffFromEuropeana
    {
        private readonly Uri endpoint = new Uri("http://testing.kokol.it/api/MemoryApp");

        private static GetStuffFromEuropeana client;

        [SerializeField]
        private RESTClient restClient;

        public static GetStuffFromEuropeana Current
        {
            get { return client ?? (client = new GetStuffFromEuropeana()); }
        }

        public bool HasStarted { get; private set; }

        public bool IsCompleted { get; private set; }

        public string SearchQuery { get; private set; }

        public Future<List<string>> Results { get; private set; }

        public void StartSearch(string query)
        {
            Reset();

            SearchQuery = query;
            HasStarted = true;

            restClient = new RESTClient();
            Results = restClient.Get<List<string>>(endpoint, new { tag = SearchQuery });
        }

        public void Update()
        {
            if (!HasStarted)
            {
                return;
            }

            restClient.Update();

            if (!IsCompleted)
            {
                // Wait for the operation to complete and display the result via Debug.Log
                if (Results.State == FutureState.Completed)
                {
                    List<string> response = Results.Result;

                    foreach (string result in response)
                    {
                    }
                }
                else if (Results.State == FutureState.Faulted)
                {
                    Debug.LogError("Something went wrong: " + Results.Exception);
                }

                IsCompleted = Results.State != FutureState.Pending;
            }
        }

        public void Reset()
        {
            Results = null;
            SearchQuery = string.Empty;
            IsCompleted = false;
            HasStarted = false;
        }
    }
}
