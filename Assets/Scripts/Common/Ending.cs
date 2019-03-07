using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField]
    Animator end;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            end.SetTrigger("EndGame");
        }
    }
}
