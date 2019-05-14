using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadingHelper
{
    //Stored loadingscreen object
    private static GameObject currentLoadingScreen;

    //Flag
    private static bool isLoading;


    /// <summary>
    /// Creates a loading screen object and returns it.
    /// </summary>
    /// <param name="_parentCanvas"></param>
    /// <returns></returns>
    public static void ShowLoadingScreen(Transform _parentCanvas)
    {
        if (currentLoadingScreen != null)
        {
            RemoveLoadingScreen();
        }
        currentLoadingScreen = GameObject.Instantiate(ParameterManager.Instance.GameParameter.loadingScreenPrefab, _parentCanvas);
    }

    /// <summary>
    /// Destroy the loadingscreen oject.
    /// </summary>
    /// <param name="_loadingScreenObject"></param>
    public static void RemoveLoadingScreen()
    {
        if (currentLoadingScreen != null)
        {
            GameObject.Destroy(currentLoadingScreen);
            currentLoadingScreen = null;
        }       
    }


    /// <summary>
    /// Helps to load a scene and plays a loadingscreen.
    /// </summary>
    /// <param name="_canvasObject"></param>
    /// <param name="_sceneName"></param>
    /// <returns></returns>
    public static IEnumerator LoadSceneAsync(GameObject _canvasObject, string _sceneName)
    {
        if (!isLoading)
        {
            //Flag
            isLoading = true;

            //Creates a loadingscreen object
            LoadingHelper.ShowLoadingScreen(_canvasObject.transform);

            //Loads the game
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
            asyncOperation.allowSceneActivation = false;

            // Wait until the asynchronous scene fully loads
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.90f)
                {
                    //Flag
                    isLoading = false;

                    //Actovate scene
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
