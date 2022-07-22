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
        if (instance)
        {
            instance.InitializeGameManagers(ref WorldSettings);
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        InitializeGameManagers(ref WorldSettings);
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
    }
    #endregion monobehaviour methods

    private void InitializeGameManagers(ref FWorldSettings WorldSettings)
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
            PlayerCharacter = Instantiate(WorldSettings.PlayerCharacter);
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
        LoadBackgroundScene(WorldSettings.BackgroundWorld);
    }

    public void LoadBackgroundScene(SceneField BackgroundWorld, bool LoadAsync = false)
    {
        if (CurrentScene != null)
        {
            SceneManager.UnloadSceneAsync(CurrentScene.SceneName);
        }
        CurrentScene = BackgroundWorld;
        if (LoadAsync)
        {
            SceneManager.LoadSceneAsync(CurrentScene.SceneName, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene(CurrentScene.SceneName, LoadSceneMode.Additive);
        }
    }
}
