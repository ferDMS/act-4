using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;


public class SelectCover : MonoBehaviour
{
    Texture2D image;
    Sprite newSprite;
    public Image newImage;

    public Books bookNumber = Books.book1;
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

    IEnumerator Start()
    {
        string JSONurl = "https://10.22.227.151:7166/api/books";
        UnityWebRequest request = UnityWebRequest.Get(JSONurl);
        request.useHttpContinue = true;
        var cert = new ForceAceptAll();
        request.certificateHandler = cert;
        cert?.Dispose();

        yield return request.SendWebRequest();
        if(request.result != UnityWebRequest.Result.Success) 
        {
            Debug.Log("Error Downloading: " + request.error);
        }
        else
        {
            List<Book> bookList = new List<Book>();
            bookList = JsonConvert.DeserializeObject<List<Book>>(request.downloadHandler.text);
            string cover = bookList[(int)bookNumber].Cover;
            StartCoroutine(DownloadImage(cover));
        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error Downloading: " + request.error);
        }
        else
        {
            image = DownloadHandlerTexture.GetContent(request);
            newSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
            newImage.sprite = newSprite;
        }
        
    }
}