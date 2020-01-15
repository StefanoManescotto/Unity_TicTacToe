using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IATicTacToe : MonoBehaviour
{
    [SerializeField]
    private Button[] btnGioco;

    [SerializeField]
    private Button[,] campo;


    private List<Button> btnNonPremuti;

    private int turno = 1; //1 --> X | 1 --> O_

    void Start()
    {

        btnNonPremuti = new List<Button>(btnGioco);
        campo = new Button[3,3];

        aggiungiListener();
        resettaPartita();
        RiempiMatriceCampo();
    }

    private void premuto(Button btnPremuto) //btnPremuto è l'ultimo pulsante che è stato premuto
    {
        if(turno == 1) //giocatore
        {
            settaPulsantiPremuti(btnPremuto, turno);
            turno = 2;
            if (!vincita(btnPremuto))
            {
                StartCoroutine(mossaComputer());
            }
        }
    }

    IEnumerator mossaComputer()
    {
        float waitTime = 1;
        yield return new WaitForSeconds(waitTime);    //Aspetta 'waitTime' secondi
        scegliMossa();
        turno = 1;
    }

    private void scegliMossa()
    {
        int nMossaComputer = (int) Random.Range(0f, btnNonPremuti.Count);
        settaPulsantiPremuti(btnNonPremuti[nMossaComputer], 2);
    }

    #region GestionePulsanti
    private void resettaPartita()
    {
        foreach(Button elemento in btnGioco)
        {
            elemento.GetComponentInChildren<Text>().text = "";
        }
    }

    private void aggiungiListener()
    {
        foreach (Button elemento in btnGioco)
        {
            elemento.onClick.AddListener(() => premuto(elemento));
        }
    }

    private void RiempiMatriceCampo()
    {
        int cont = 0;

        foreach (Button elemento in btnGioco)
        {
            int x = cont % 3;
            int y = cont / 3;
            campo[x,y] = elemento;
            cont++;
        }
    }

    private void settaPulsantiPremuti(Button button, int turno)
    {
        btnNonPremuti.Remove(button); //rimozione pulsante premuto dalla lista
        button.GetComponent<ButtonScript>().premuto = turno; //setto 'premuto' del pulsante ad 1 --> il giocatore ha premuto il pulsante
        button.onClick.RemoveAllListeners(); //rimuovo il listener del pulsante
        if(turno == 1)
        {
            button.GetComponentInChildren<Text>().text = "X"; //imposto testo del pulsante premuto
        }
        else
        {
            button.GetComponentInChildren<Text>().text = "O"; //imposto testo del pulsante premuto
        }
    }

    #endregion

    #region Vincita
    private bool vincita(Button ultimoPremuto)
    {
        int numBtn = ultimoPremuto.GetComponent<ButtonScript>().numBtn;

        if(vincitaOrizz(ultimoPremuto) || vincitaVert(ultimoPremuto))
        {
            Debug.Log("Vittoria");
            return true;
        }

        return false;
    }

    private bool vincitaOrizz(Button btnPremuto) //Controllo vincita Orizzontale
    {
        int numBtn = btnPremuto.GetComponent<ButtonScript>().numBtn;
        int i = 0;
        int numRiga = numBtn / 3;
        int count = 0; //Contatore pulsanti premuti stesso giocatore

        for (i = 0; i < 3; i++)
        {
            if (campo[i, numRiga].GetComponent<ButtonScript>().premuto == btnPremuto.GetComponent<ButtonScript>().premuto)
            {
                count++;
            }
        }

        if(count == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool vincitaVert(Button btnPremuto) //Controllo Vincita Verticale
    {
        int numBtn = btnPremuto.GetComponent<ButtonScript>().numBtn;
        int i = 0;
        int numColonna = numBtn % 3;
        int count = 0; //Contatore pulsanti premuti stesso giocatore

        for (i = 0; i < 3; i++)
        {
            if (campo[numColonna, i].GetComponent<ButtonScript>().premuto == btnPremuto.GetComponent<ButtonScript>().premuto)
            {
                count++;
            }
        }

        if (count == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
