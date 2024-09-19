using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    [SerializeField] Renderer Model;

    Color OriginColor;

    // Start is called before the first frame update
    void Start()
    {
        OriginColor = Model.material.color; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && transform.position != gameManager.instance.playerSpawnPos.transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(ColorFlash());
        }
    }

    IEnumerator ColorFlash()
    {
        Model.material.color = Color.blue;
        gameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameManager.instance.checkpointPopup.SetActive(false);
        Model.material.color = OriginColor;
    }
}
