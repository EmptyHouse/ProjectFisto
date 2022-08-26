using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct FWorldSettings
{
    public EHGameMode GameMode;
    public EHGameState GameState;
    public EHPlayerController PlayerController;
    public EHPlayerState PlayerState;
    public EHCharacter PlayerCharacter;
    public EHBaseGameHUD GameHUD;
    public SceneField BackgroundWorld;
}

public class EHGameInstance : MonoBehaviour
{
    #region static members

    private static EHGameInstance instance;

    public static EHGameInstance Instance
    {
        get
        {
            if (instance) return instance;
            instance = GameObject.FindObjectOfType<EHGameInstance>();
            return instance;
        }
    }

    #endregion static members
    
    [SerializeField]
    private FWorldSettings WorldSettings;

    public EHGameMode GameMode { get; private set; }
    public EHGameState GameState { get; private set; }
    public EHBaseGameHUD GameHUD { get; private set; }
    public EHPlayerState PlayerState { get; private set; }
    public EHCharacter PlayerCharacter { get; private set; }
    public EHPlayerController PlayerController { get; private set; }
    private SceneField CurrentScene;
    
    #region monobehaviour methods
    protected virtual void Awake()
    {
        if (Application.isEditor)
        {
            if (SceneManager.sceneCount > 1)
            {
                CurrentScene = WorldSettings.BackgroundWorld;
            }
        }
        if (instance)
        {
            instance.InitializeGameManagers();
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        InitializeGameManagers();
        
        //NOTE: may want to remove this in the future
        Application.targetFrameRate = 60;
    }
    #endregion monobehaviour methods

    private void InitializeGameManagers()
    {
        LoadBackgroundScene(WorldSettings.BackgroundWorld);
    }

    public void LoadBackgroundScene(SceneField BackgroundWorld, bool LoadAsync = false)
    {
        StartCoroutine(LoadBackgroundSceneCoroutine(BackgroundWorld, LoadAsync));
    }

    private IEnumerator LoadBackgroundSceneCoroutine(SceneField BackgroundWorld, bool LoadAsync = false)
    {
        yield return null;
        if (CurrentScene != null)
        {
            AsyncOperation UnloadSceneOperation = SceneManager.UnloadSceneAsync(CurrentScene.SceneName);
            while (!UnloadSceneOperation.isDone)
            {
                yield return null;
            }
        }
        InitializeMainSceneObjects();
        CurrentScene = BackgroundWorld;
        if (LoadAsync)
        {
            AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(CurrentScene.SceneName, LoadSceneMode.Additive);
            while (!LoadOperation.isDone)
            {
                yield return null;
            }
        }
        else
        {
            SceneManager.LoadScene(CurrentScene.SceneName, LoadSceneMode.Additive);
        }

        yield return null;
        // Set Player at specific spawn position. This will need to be updated in the future to account for saves
        EHSpawnPoint SpawnPoint = GameObject.FindObjectOfType<EHSpawnPoint>();
        if (SpawnPoint != null && PlayerCharacter)
        {
            PlayerCharacter.SetPositionNoSweep(SpawnPoint.transform.position);
        }
    }

    private void InitializeMainSceneObjects()
    {
        if (GameMode) Destroy(GameMode.gameObject);
        if (GameState) Destroy(GameState.gameObject);
        if (GameHUD) Destroy(GameHUD.gameObject);
        if (PlayerController) Destroy(PlayerController.gameObject);
        if (PlayerState) Destroy(PlayerState.gameObject);
        if (PlayerCharacter) Destroy(PlayerCharacter.gameObject);

        if (WorldSettings.GameMode)
        {
            GameMode = Instantiate(WorldSettings.GameMode, Vector3.zero, Quaternion.identity);
        }

        if (WorldSettings.GameState)
        {
            GameState = Instantiate(WorldSettings.GameState, Vector3.zero, Quaternion.identity);
        }

        if (WorldSettings.PlayerState)
        {
            PlayerState = Instantiate(WorldSettings.PlayerState, Vector3.zero, Quaternion.identity);
        }

        if (WorldSettings.PlayerCharacter)
        {
            PlayerCharacter = Instantiate(WorldSettings.PlayerCharacter, Vector2.zero, Quaternion.identity);
        }

        if (WorldSettings.PlayerController)
        {
            PlayerController = Instantiate(WorldSettings.PlayerController, Vector3.zero, Quaternion.identity);
        }

        if (PlayerState)
        {
            PlayerState.PossessPlayerCharacter(PlayerCharacter);
            PlayerState.PossessPlayerController(PlayerController);
        }

        if (WorldSettings.GameHUD)
        {
            GameHUD = Instantiate(WorldSettings.GameHUD);
        }
    }
    #region debug functions

    public void DebugResetGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    #endregion debug functions
}
