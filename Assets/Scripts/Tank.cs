using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Tank : MonoBehaviour
{
    public float fireSpeed;

    public int health;

    public int remainingHealth;

    public GameObject bulletPrefab;

    public float bulletSpeed;

    public Transform fireTransform;

    public float lastShootTime = 0f;

    public void Start()
    {
        remainingHealth = health;
    }

    public IEnumerator FireTo(Vector3 position, float duration)
    {
        //Eðer attýðýn anda efekt istiyorsan fireTransform.position'a duman Instantiate yap.
        var bullet = Instantiate(bulletPrefab, fireTransform.position, Quaternion.identity);
        bullet.transform.LookAt(position);
        bullet.transform.DOMove(position,duration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(duration);

        //Eðer patladýðý yerde efekt istiyorsan position'da duman Instantiate yap.
        Destroy(bullet);
    }

    public void Hit()
    {
        if (--remainingHealth <= 0)
        {
            //Patlama efekti yapabilirsin.
            Destroy(gameObject);
        }
    }
}
