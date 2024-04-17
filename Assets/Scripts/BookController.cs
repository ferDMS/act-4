using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Librería para manejar textos de UI
using UnityEngine.UI;

// Librería para conectarse a un servidor por HTTP
using UnityEngine.Networking;

// Librería para hacer parsing the JSON
using Newtonsoft.Json;

public class BookController : MonoBehaviour
{
    // Variables para guardar las instancias de cada script para ejecutarlos
    static public BookController Instance;
    public SelectCover SelectCover;
    // MySelectCover MySelectCover;
    public SelectBook SelectBook;

    // Variables para guardar la selección del libro
    public Text nameText;
    public Text authorText;
    public int BookSelection;

    // Primera función llamada al inicio de la escena
    public void Awake()
    {
        // Arreglar las instancias seleccionadas
        Instance = this;
        Instance.SetReferences();
        DontDestroyOnLoad(this.gameObject);
    }

    // Función para arreglar instancias a usar
    void SetReferences()
    {
        if (SelectCover == null)
        {
            SelectCover = FindObjectOfType<SelectCover>();
        }

        if (SelectBook == null)
        {
            SelectBook = FindObjectOfType<SelectBook>();
        }
    }

    // Función para llamar las demás funciones que seleccionan un libro
    public void Select(int _selection)
    {
        BookSelection = _selection;
        StartCoroutine(GetData());
    }

    // Mi propia implementación de GetData, para obtener información de un solo libro
    // en vez de todos a través de una llamada API que regresa un único libro especificado
    IEnumerator GetData()
    {
        // Preparar llamada a API con URL, ignorando certificado SSL
        string JSONurl = "https://10.22.227.151:7166/api/book/" + (BookSelection).ToString();
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
        // Si es exitosa, actualizar información con el libro seleccíonado
        else
        {
            // Convertir el JSON de la resupuesta a la estructura de libro y guardar la selección en variables PlayerPrefs
            Book book = JsonConvert.DeserializeObject<Book>(request.downloadHandler.text);
            PlayerPrefs.SetInt("book_no", BookSelection);
            // Actualizar información con libro seleccionado
            LoadBookInfo(book);
        }
    }

    // Mi propia implementación de LoadBookInfo, que obtiene en vez de una lista de libros
    // únicamente el libro para el cual queremos cargar la información
    public void LoadBookInfo(Book book)
    {
        // Aqui depende de como hayamos definido el nombre de la variable
        // que guarda el nombre del libro dentro de la estructura de Book
        string title = book.Title;
        PlayerPrefs.SetString("book_name", title);
        nameText.text = title;

        // Al igual que arriba "Author" se usa porque así se llama la propiedad
        // dentro de la estructura de Book
        string author = book.Author;
        PlayerPrefs.SetString("author", author);
        authorText.text = author;
    }


    /*
    IEnumerator GetData()
    {
        string JSONurl = "https://localhost:7166/api/books";
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
            LoadBookInfo(BookSelection, bookList);
            PlayerPrefs.SetInt("book_no", BookSelection);
        }
    }


    public void LoadBookInfo(int idBook, List<Book> bookList)
    {
        // Aqui depende de como hayamos definido el nombre de la variable
        // que guarda el nombre del libro dentro de la estructura de Book
        string book = bookList[idBook - 1].Title;
        PlayerPrefs.SetString("book_name", book);
        nameText.text = book;

        // Al igual que arriba "Author" se usa porque así se llama la propiedad
        // dentro de la estructura de Book
        string author = bookList[idBook - 1].Author;
        PlayerPrefs.SetString("author", author);
        authorText.text = author;
    }
    */
}