using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100;
    public float Timer = 1.0f;
    public int AttackPoint = 10;

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterHealthUp();       
        CheckDeath();
    }
    void CharacterHealthUp()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Timer = 1.0f;
            Health += 20;
        }
    }
    public void CharacterHit(int Damage)
    {
        Health -= Damage;
    }

    void CheckDeath()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }
}  
