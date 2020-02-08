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

  //  [SerializeField]
    private List<Button> btnNonPremuti;

    [SerializeField]
    private Button btnResetta;

    private int turno = 1; //1 --> X | 1 --> O_
    private bool finePartita = false; //true = la partita è finita
    int sequenzaVincita = 0;

    void Start()
    {

        // btnNonPremuti = new List<Button>(btnGioco);
        campo = new Button[3,3];

        //aggiungiListener();
        RiempiMatriceCampo();
        resettaPartita();

        btnResetta.onClick.AddListener(() => resettaPartita());

    }

    private void premuto(Button btnPremuto) //btnPremuto è l'ultimo pulsante che è stato premuto
    {
        if(turno == 1 && !finePartita) //giocatore
        {
            settaPulsantiPremuti(btnPremuto, turno);
            turno = 2;
            Button bloccoBtn = vincita(btnPremuto);
            if (!finePartita)
            {
                StartCoroutine(mossaComputer(bloccoBtn));
            }
        }
    }

    #region mossaComputer

    IEnumerator mossaComputer(Button bloccoBtn)
    {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);    //Aspetta 'waitTime' secondi
        scegliMossa(bloccoBtn);
        turno = 1;
    }

    private void scegliMossa(Button bloccoBtn)
    {
        int nRiga;
        int nColonna;
        if (bloccoBtn == null  /*btnNonPremuti.BinarySearch(bloccoBtn) == -1*/)
        {
            int nMossaComputer = (int)Random.Range(0f, btnNonPremuti.Count);
            nRiga = btnNonPremuti[nMossaComputer].GetComponent<ButtonScript>().numBtn % 3;
            nColonna = btnNonPremuti[nMossaComputer].GetComponent<ButtonScript>().numBtn / 3;
        }
        else if(btnNonPremuti.FindIndex(x => x.GetComponent<ButtonScript>().numBtn == bloccoBtn.GetComponent<ButtonScript>().numBtn) != -1)
        {
            nRiga = bloccoBtn.GetComponent<ButtonScript>().numBtn % 3;
            nColonna = bloccoBtn.GetComponent<ButtonScript>().numBtn / 3;
        }
        else
        {
            int nMossaComputer = (int)Random.Range(0f, btnNonPremuti.Count);
            nRiga = btnNonPremuti[nMossaComputer].GetComponent<ButtonScript>().numBtn % 3;
            nColonna = btnNonPremuti[nMossaComputer].GetComponent<ButtonScript>().numBtn / 3;
        }

        Button pulsanteCampo = campo[nRiga, nColonna];

        settaPulsantiPremuti(pulsanteCampo, turno);
        vincita(pulsanteCampo);
    }

   // private bool controlloButt
    #endregion

    #region GestionePulsanti
    private void resettaPartita()
    {
        btnNonPremuti = new List<Button>(btnGioco);
        foreach(Button elemento in btnGioco)
        {
            //campo[0, 0].GetComponentInChildren<Text>().color = Color.black;
            elemento.GetComponentInChildren<Text>().text = "";
            elemento.GetComponentInChildren<Text>().color = Color.black;
            elemento.onClick.AddListener(() => premuto(elemento));
            elemento.GetComponent<ButtonScript>().premuto = 0;
        }
         finePartita = false;
        turno = 1;
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

    private void coloraSequenza(int numPulsante)
    {

        //int numPulsante = pulsante.GetComponent<ButtonScript>().numBtn;
        int riga = numPulsante / 3;
        int colonna = numPulsante % 3;
        int k;
        switch (sequenzaVincita)
        {
            case 1:
                for(int i = 0; i<3; i++)
                {
                    campo[i, riga].GetComponentInChildren<Text>().color = Color.red;
                }
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    campo[colonna, i].GetComponentInChildren<Text>().color = Color.red;
                }
                break;
            case 3:
                k = 0;
                for (int i = 0; i < 3; i++)
                {
                    campo[i, k].GetComponentInChildren<Text>().color = Color.red;
                    k++;
                }
                break;
            case 4:
                k = 2;
                for (int i = 0; i < 3; i++)
                {
                    campo[i, k].GetComponentInChildren<Text>().color = Color.red;
                    k--;
                }
                break;
        }
    }

    #endregion

    #region Vincita
    private Button vincita(Button ultimoPremuto)
    {
        int numBtn = ultimoPremuto.GetComponent<ButtonScript>().numBtn;
        sequenzaVincita = 0; //0 --> nessuno | 1 --> orizzontale | 2 --> verticale | 3 --> obliquo destro | 4 --> obliuquo sinistro

        Button orizz = vincitaOrizz(ultimoPremuto);
        Button vert = vincitaVert(ultimoPremuto);
        Button obDestra = vincitaObliquaDestra(ultimoPremuto);
        Button obSinistra = vincitaObliquaSinistra(ultimoPremuto);

        if (orizz == ultimoPremuto || vert == ultimoPremuto || obDestra == ultimoPremuto || obSinistra == ultimoPremuto)
        {
            Debug.Log("Vittoria");
            coloraSequenza(ultimoPremuto.GetComponent<ButtonScript>().numBtn);
            finePartita = true;
            return null;
        }
        else if (btnNonPremuti.Count == 0) //Campo Pieno
        {
            finePartita = true;
            Debug.Log("Campo Pieno");
            return null;
        }
        else if (orizz != null)
        {
            return orizz;
        }
        else if (vert != null)
        {
            return vert;
        }
        else if (obDestra != null)
        {
            return obDestra;
        }
        else
        {
            return obSinistra;
        }
    }

    private Button vincitaOrizz(Button btnPremuto) //Controllo vincita Orizzontale
    {
        int numBtn = btnPremuto.GetComponent<ButtonScript>().numBtn;
        int i = 0;
        int numRiga = numBtn / 3;
        int count = 0; //Contatore pulsanti premuti stesso giocatore
        Button ultimoBtn = btnPremuto;

        for (i = 0; i < 3; i++)
        {
            if (campo[i, numRiga].GetComponent<ButtonScript>().premuto == btnPremuto.GetComponent<ButtonScript>().premuto)
            {
                count++;
            }
            else
            {
                ultimoBtn = campo[i, numRiga];
            }
        }
        if (count == 3)
        {
            sequenzaVincita = 1;
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else if(count == 2)
        {
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else
        {
            return null;
        }
    }

    private Button vincitaVert(Button btnPremuto) //Controllo Vincita Verticale
    {
        int numBtn = btnPremuto.GetComponent<ButtonScript>().numBtn;
        int i = 0;
        int numColonna = numBtn % 3;
        int count = 0; //Contatore pulsanti premuti stesso giocatore
        Button ultimoBtn = btnPremuto;

        for (i = 0; i < 3; i++)
        {
            if (campo[numColonna, i].GetComponent<ButtonScript>().premuto == btnPremuto.GetComponent<ButtonScript>().premuto)
            {
                count++;
            }
            else
            {
                ultimoBtn = campo[numColonna, i];
            }
        }

        if (count == 3)
        {
            sequenzaVincita = 2;
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else if (count == 2)
        {
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else
        {
            return null;
        }
    }

    private Button vincitaObliquaDestra(Button btnPremuto)
    {
        int numBtn = btnPremuto.GetComponent<ButtonScript>().numBtn;
        int i = 0, k = 0;
        int numColonna = numBtn % 3;
        int count = 0; //Contatore pulsanti premuti stesso giocatore
        Button ultimoBtn = btnPremuto;

        for (i = 0; i < 3; i++)
        {
            if (campo[i, k].GetComponent<ButtonScript>().premuto == btnPremuto.GetComponent<ButtonScript>().premuto)
            {
                count++;
            }
            else
            {
                ultimoBtn = campo[i, k];
            }
            k++;
        }

        if (count == 3)
        {
            sequenzaVincita = 3;
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else if (count == 2)
        {
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else
        {
            return null;
        }
    }

    private Button vincitaObliquaSinistra(Button btnPremuto)
    {
        int numBtn = btnPremuto.GetComponent<ButtonScript>().numBtn;
        int i = 0, k = 2;
        int numColonna = numBtn % 3;
        int count = 0; //Contatore pulsanti premuti stesso giocatore
        Button ultimoBtn = btnPremuto;

        for (i = 0; i < 3; i++)
        {
            if (campo[i, k].GetComponent<ButtonScript>().premuto == btnPremuto.GetComponent<ButtonScript>().premuto)
            {
                count++;
            }
            else
            {
                ultimoBtn = campo[i, k];
            }
            k--;
        }

        if (count == 3)
        {
            sequenzaVincita = 4;
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else if (count == 2)
        {
            return ultimoBtn; // se ritorna btnPremuto allora Vincita
        }
        else
        {
            return null;
        }
    }

    #endregion
}
