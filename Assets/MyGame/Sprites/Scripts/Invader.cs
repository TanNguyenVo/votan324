using UnityEngine;

public class Invader : MonoBehaviour
{
    public System.Action killed;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            
            this.killed?.Invoke();Debug.Log("Invader hit by projectile!");
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
