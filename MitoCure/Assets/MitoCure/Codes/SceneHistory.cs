using UnityEngine;

// Bu bir "static" class. Yani sahneler arasında kaybolmaz
// ve bir GameObject'e eklenmesine gerek yoktur.
public static class SceneHistory
{
    // Bir önceki sahnenin adını burada saklayacağız
    public static string previousSceneName;
}