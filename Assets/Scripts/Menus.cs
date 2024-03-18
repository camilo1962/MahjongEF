using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menus : MonoBehaviour
{
    int Record1;
    public TMP_Text record1;
    int Record2;
    public TMP_Text record2;
    int Record3;
    public TMP_Text record3;
    int Record4;
    public TMP_Text record4;
    int Record5;
    public TMP_Text record5;
    int Record6;
    public TMP_Text record6;






    public void CambiodeEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);

    }
    public void Salir()
    {
        Application.Quit();
    }

    public void BorrarRecords()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Start()
    {
        Record1 = PlayerPrefs.GetInt("HightScore");
        record1.text = Record1.ToString();
        Record2 = PlayerPrefs.GetInt("HightScore2");
        record2.text = Record2.ToString();
        Record3 = PlayerPrefs.GetInt("HightScore3");
        record3.text = Record3.ToString();
        Record4 = PlayerPrefs.GetInt("HightScore4");
        record4.text = Record4.ToString();
        Record5 = PlayerPrefs.GetInt("HightScore5");
        record5.text = Record5.ToString();
        Record6 = PlayerPrefs.GetInt("HightScore6");
        record6.text = Record6.ToString();
    }
}
