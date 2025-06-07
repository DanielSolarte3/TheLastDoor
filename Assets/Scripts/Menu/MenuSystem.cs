using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Jugar(){
        // esto es para que cargue la del menu primero: SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Salir(){
        Debug.Log("Saliendo del juego...");
        // Si estamos en el editor de Unity
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Si estamos en una build (juego real)
            Application.Quit();
        #endif
    }
}
