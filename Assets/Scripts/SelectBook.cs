using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Para el botón de cambiar de escena
using UnityEngine.SceneManagement;


public class SelectBook : MonoBehaviour
{

    // Se definen múltiples funciones pues cada una será llamada por
    // un botón diferente correspondiente a cada libro de la librería

   public void ChooseBook1()
   {
        BookController.Instance.Select(1);
   }

   public void ChooseBook2()
   {
        BookController.Instance.Select(2);
   }

   public void ChooseBook3()
   {
        BookController.Instance.Select(3);
   }

   public void ChooseBook4()
   {
        BookController.Instance.Select(4);
   }

   public void ChooseBook5()
   {
        BookController.Instance.Select(5);
   }

   public void ChooseBook6()
   {
        BookController.Instance.Select(6);
   }

   public void ChooseBook7()
   {
        BookController.Instance.Select(7);
   }
}
