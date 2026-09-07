using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameStatsUploader : MonoBehaviour
{
    [SerializeField]
    private string apiBaseUrl = "http://127.0.0.1:8080";

    [SerializeField]
    private string playerId = "local-player";

    private bool hasSubmittedCurrentRun;

    [Serializable]
    private class GameRunRequest
    {
        public string player_id;
        public int survival_seconds;
        public int level;
        public int normal_kills;
        public int fast_kills;
        public int tank_kills;
        public string result;
    }

    public void BeginNewRun()
    {
        hasSubmittedCurrentRun = false;
    }

    public void UploadGameRun(
        int survivalSeconds,
        int level,
        int normalKills,
        int fastKills,
        int tankKills,
        bool victory
    )
    {
        if (hasSubmittedCurrentRun)
        {
            Debug.LogWarning("Game run has already been submitted.");
            return;
        }

        hasSubmittedCurrentRun = true;

        var gameRun = new GameRunRequest
        {
            player_id = playerId,
            survival_seconds = survivalSeconds,
            level = level,
            normal_kills = normalKills,
            fast_kills = fastKills,
            tank_kills = tankKills,
            result = victory ? "completed" : "defeated"
        };

        StartCoroutine(PostGameRun(gameRun));
    }

    private IEnumerator PostGameRun(GameRunRequest gameRun)
    {
        string json = JsonUtility.ToJson(gameRun);
        byte[] requestBody = Encoding.UTF8.GetBytes(json);

        string url = apiBaseUrl + "/api/v1/game-runs";

        using var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);

        request.uploadHandler = new UploadHandlerRaw(requestBody);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(
                "Game run uploaded successfully: " +
                request.downloadHandler.text
            );
        }
        else
        {
            hasSubmittedCurrentRun = false;

            Debug.LogError(
                $"Failed to upload game run. " +
                $"HTTP status: {request.responseCode}; " +
                $"Error: {request.error}; " +
                $"Response: {request.downloadHandler.text}"
            );
        }
    }
}