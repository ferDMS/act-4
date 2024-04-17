using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Ir a la escena de vista de un libro
    public void PreviewBook()
    {
        SceneManager.LoadScene("BookPreview");
    }

    // Ir a la escena de vista de la librería completa
    public void MyLibrary()
    {
        SceneManager.LoadScene("My Library");
    }
}
