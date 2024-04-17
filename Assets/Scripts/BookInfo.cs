using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BookInfo : MonoBehaviour
{

    private string title;
    public Text titleText;

    private string author;
    public Text authorText;

    // Start is called before the first frame update
    void Start()
    {
        title = PlayerPrefs.GetString("book_name");
        titleText.text = title;
        author = PlayerPrefs.GetString("author");
        authorText.text = author;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
