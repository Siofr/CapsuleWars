using System;
using System.Threading.Tasks;
using Unity.Services.Multiplayer;
using UnityEngine;

public class SessionListUI : MonoBehaviour
{
    [SerializeField] private GameObject sessionListItem;

    private void OnEnable()
    {
        CreateList();
    }

    private void OnDisable()
    {
        DestroyList();
    }

    public void CreateList()
    {
        RetrieveActiveSessions();
    }

    public void DestroyList()
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child);
        }
    }

    private async Task RetrieveActiveSessions()
    {
        QuerySessionsResults results;

        try
        {
            results = await MultiplayerService.Instance.QuerySessionsAsync(new QuerySessionsOptions());
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return;
        }

        if (results.Sessions.Count <= 0) { return; }

        // For each existing session display it

        foreach(ISession session in results.Sessions)
        {
            CreateListItem(session);
        }
    }

    private void CreateListItem(ISession session)
    {
        GameObject newSessionListItem = Instantiate(sessionListItem, this.transform);
        SessionInfoUI s = newSessionListItem.GetComponent<SessionInfoUI>();
        s.FillSessionDetails(session);
    }
}
