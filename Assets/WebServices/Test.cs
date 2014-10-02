using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MyEuropeana.Client;

public class Test : MonoBehaviour
{
    [SerializeField]
    private RESTClient m_restClient = null;

    [SerializeField]
    private string m_searchQuery = "butterfly";

    private Future< List<string> > m_searchResult;

    private bool m_completed;

    void Start()
    {
        Uri endpoint = new Uri("http://creative.semantika.si/api/MemoryApp");

        //Fire off the request!
        m_searchResult = m_restClient.Get<List<string>>(endpoint, new { tag = m_searchQuery });
    }

    void Update()
    {
        if (!m_completed)
        {
            //Wait for the operation to complete and display the result via Debug.Log
            if (m_searchResult.State == FutureState.Completed)
            {
                List<string> response = m_searchResult.Result;

                //foreach (string result in response)
                //{
                //}
            }
            else if (m_searchResult.State == FutureState.Faulted)
            {
                Debug.LogError("Something went wrong: " + m_searchResult.Exception);
            }

            m_completed = m_searchResult.State != FutureState.Pending;
        }
    }
}
