using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Librer�a para manejar textos de UI
using UnityEngine.UI;

// Librer�a para conectarse a un servidor por HTTP
using UnityEngine.Networking;

// Librer�a para hacer parsing the JSON
using Newtonsoft.Json;

using System.Linq;

public class DialogueController : MonoBehaviour
{
    public Text pageText;
    private int pageNo;

    public Text dialogueText;
    string[] Sentences = { "", "", "", "", "" };

    private int Index = 0;

    // Tiempo a esperar entre caracteres al escribir una p�gina, en segundos
    // Se usa para poder generar una animaci�n de que se escribe el libro
    public float DialogueSpeed = 0.05f;

    // Tiempo a esperar antes de empezar a escribir una nueva p�gina, en segundos
    public float DelayToWrite = 0.5f;

    private int book;


    // Start is called before the first frame update
    void Start()
    {
        book = PlayerPrefs.GetInt("book_no");

        // Llamar la funci�n de obtener datos de las p�ginas del libro
        StartCoroutine(GetData());

        // Llamar la funci�n para escribir el contenido de la p�gina
        // Utiliza la variable de index, que salva la p�gina en la que nos encontramos
        StartCoroutine(WriteSentence());
    }

    // Mi propia implementaci�n de GetData, para obtener informaci�n de un solo libro
    // en vez de todos a trav�s de una llamada API que regresa un �nico libro especificado
    IEnumerator GetData()
    {
        // Preparar llamada a API con URL, ignorando certificado SSL
        string JSONurl = "https://10.22.227.151:7166/api/book/" + book + "/pages";
        UnityWebRequest request = UnityWebRequest.Get(JSONurl);
        request.useHttpContinue = true;
        var cert = new ForceAceptAll();
        request.certificateHandler = cert;
        cert?.Dispose();

        // Hacer llamada a la API.
        yield return request.SendWebRequest();

        // Si falla, desplegar el error
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error Downloading: " + request.error);
        }
        // Si es exitosa, actualizar informaci�n con el libro selecc�onado
        else
        {
            // Convertir el JSON de la resupuesta a la estructura de libro y guardar la selecci�n en variables PlayerPrefs
            List<Page> pageList = JsonConvert.DeserializeObject<List<Page>>(request.downloadHandler.text);
            
            // Dependiendo de cuantas p�ginas tenemos, estamos reestructurando el arreglo de dialogues
            if(pageList.Count > 0)
            {
                Sentences = Enumerable.Repeat<string>("", pageList.Count).ToArray<string>();

                // Actualizar lista de dialogues local con la lista de contenidos encontrados en la llamada a la API
                for (var i = 0; i < pageList.Count; i++)
                {
                    Sentences[i] = pageList[i].Content;
                }
            }
        }
    }

    IEnumerator WriteSentence()
    {
        pageNo = Index + 1;
        pageText.text = pageNo.ToString();
        yield return new WaitForSeconds(DelayToWrite);

        foreach(char Character in Sentences[Index].ToCharArray()) {
            dialogueText.text += Character;
            yield return new WaitForSeconds(DialogueSpeed);
        }
    }

    // Funci�n que se llamar� por los botones para cambiar de p�gina
    void NextSentence()
    {
        // Si existe una siguiente p�gina posible de desplegar en
        // el �ndice (nuevo) al que buscamos ir
        if (Index < Sentences.Length && Index >= 0)
        {
            // Borrar
            dialogueText.text = Sentences[Index];
            dialogueText.text = "";
            StartCoroutine(WriteSentence());
        }
    }

    // Funci�n llamada por el bot�n de ir a siguiente p�gina
    public void Next()
    {
        if (Index < Sentences.Length - 1)
        {
            StopAllCoroutines();
            Index++;
            NextSentence();
        }
    }

    public void Past()
    {
        if (Index >= 0 + 1)
        {
            StopAllCoroutines();
            Index--;
            NextSentence();
        }
    }
}
