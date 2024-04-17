using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class MySelectCover : MonoBehaviour
{
    Texture2D image;
    Sprite newSprite;
    public Image newImage;

    // Inicializar una enumeraci�n con default value book1
    public Books bookNumber = Books.book1;
    // Cada valor de la estructura representa un int del 0 al 6
    public enum Books
    {
        book1,
        book2,
        book3,
        book4,
        book5,
        book6,
        book7
    }

    // Llamar a la API para obtener la informaci�n del libro seleccionado por `bookNumber`
    IEnumerator Start()
    {
        // Preparar llamada a la API, ignorando el certificado de SSL
        // En la llamada se adjunta el n�mero de libro a obtener como par�metro
        string JSONurl = "https://10.22.227.151:7166/api/book/" + ((int)bookNumber+1).ToString();
        UnityWebRequest request = UnityWebRequest.Get(JSONurl);
        request.useHttpContinue = true;
        var cert = new ForceAceptAll();
        request.certificateHandler = cert;
        cert?.Dispose();

        // Hacer la llamada a la API
        yield return request.SendWebRequest();

        // Si la llamada falla, se regresa el error
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error Downloading: " + request.error);
        }
        // Si la llamada es exitosa, se descarga la portada del libro
        else
        {
            Book book;
            book = JsonConvert.DeserializeObject<Book>(request.downloadHandler.text);
            string cover = book.Cover;
            StartCoroutine(DownloadImage(cover));
        }
    }

    // Funci�n para descargar la portada del libro seg�n la URL obtenida de la llamada a la API
    IEnumerator DownloadImage(string MediaUrl)
    {
        // Obtener textura desde una URL
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error Downloading: " + request.error);
        }

        // Si la textura se obtiene con �xito, se despliega la im�gen como un sprite
        else
        {
            image = DownloadHandlerTexture.GetContent(request);
            newSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
            newImage.sprite = newSprite;
        }

    }
}