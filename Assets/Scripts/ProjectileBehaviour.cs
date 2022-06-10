using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileBehaviour : MonoBehaviour
{
    public float TravelSpeed;
    public GameObject Owner;
    public SoundProjectileManager SoundProjectile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        this.transform.position += TravelSpeed * this.transform.up * Time.deltaTime;
        this.transform.Rotate(0.25f, 0.0f, 0.0f, Space.Self);

        if(this.transform.position.y <= 0.0f){
            Owner.GetComponent<Tank>().shellIsLive = false;
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[0], this.transform.position, 1f);
            /*if(Owner.GetComponent<EnemyTank>()){
                Owner.GetComponent<EnemyTank>().overseer.enemyHasFired = false;
            }*/
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("other is "+other.name+" and the tag is "+other.tag);
        if(other.gameObject.tag == "Vulnerability"){
            Debug.Log("got 'em!");
            if(other.gameObject.transform.parent.parent.parent.parent.parent.gameObject.GetComponent<PlayerManager>()) 
                other.gameObject.transform.parent.parent.parent.parent.parent.gameObject.GetComponent<PlayerManager>().takeHit();
            if(Owner) Owner.GetComponent<Tank>().shellIsLive = false;
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[1], other.gameObject.transform.position, 1f);
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[2], other.gameObject.transform.position, 1f);
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[4], other.gameObject.transform.position, 1f);
            Destroy(this.gameObject);
            Owner.GetComponent<Tank>().shellIsLive = false;
        } else if (other.gameObject.tag == "Invulnerability" || other.gameObject.tag == "Enemy"){
            Debug.Log("darn!");
            if(Owner) Owner.GetComponent<Tank>().shellIsLive = false;
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[1], other.gameObject.transform.position, 1f);
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[3], other.gameObject.transform.position, 1f);
            Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Start"){
            Debug.Log("Good luck");
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[1], other.gameObject.transform.position, 1f);
            SceneManager.LoadScene("Game");
        } else if (other.gameObject.tag == "Quit"){
            Debug.Log("See you space cowboy");
            AudioSource.PlayClipAtPoint(SoundProjectile._impacts[1], other.gameObject.transform.position, 1f);
            Application.Quit();
        }
    }

}
