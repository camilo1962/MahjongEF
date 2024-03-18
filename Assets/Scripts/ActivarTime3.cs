using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivarTime3 : MonoBehaviour
{

    public Image ModeloCircular;
    public float totalTiempo = 60f;
    private float ContadorTiempo;
    public TMP_Text porcentajeTienpo;

    public BlockManager3 gameover;

    void Start()
    {
        gameover = FindObjectOfType<BlockManager3>();
        ContadorTiempo = 0;
    }

   

    public void Update()
    {

        
        if (ContadorTiempo <= totalTiempo)
        {
            ContadorTiempo = ContadorTiempo + Time.deltaTime;
            ModeloCircular.fillAmount =  ContadorTiempo / totalTiempo;
            float v = (100 * ModeloCircular.fillAmount);
            porcentajeTienpo.text = v.ToString("F0") + "%";

            if(v == 100f)
            {
                gameover.GameOver();
            }
        }
    }
     
    
}
