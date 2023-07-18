using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader {
    
    private static Scene targetScene;
    private static AsyncOperation targetSceneLoadingOperation = null;
    
    
        
    public static void Load(Scene scene) {
        targetScene = scene;
        
        SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString(), LoadSceneMode.Single).completed += OnLoadingSceneLoaded;
    }

    private static void OnLoadingSceneLoaded(AsyncOperation asyncOperation) {
        targetSceneLoadingOperation = SceneManager.LoadSceneAsync(targetScene.ToString());
    }
    
    public static float GetLoadingProgress() {
        if (targetSceneLoadingOperation == null)
            return 1f;
        return targetSceneLoadingOperation.progress;
    }


    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene
    }
    
}
