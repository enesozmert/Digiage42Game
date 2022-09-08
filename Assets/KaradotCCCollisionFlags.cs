using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KaradotCCCollisionFlags : MonoBehaviour
{

    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.collisionFlags == CollisionFlags.None)
            print("Havada süzülüyor, temas yok!");
        if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
            print("Etrafýndan bir þeylere temas ediyor");
        if (controller.collisionFlags == CollisionFlags.Sides)
            print("Yalnýzca yanlardan temas var,baþka bir temas yok");
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            print("Tavana deðiyor");
        if (controller.collisionFlags == CollisionFlags.Above)
            print("Sadece yukardýan temas,baþka bir temas yok");
        if ((controller.collisionFlags & CollisionFlags.Below) != 0)
            print("Yere deðiyor");
        if (controller.collisionFlags == CollisionFlags.Below)
            print("Yalnýzca yere deðiyor, baþka bir temas yok");
    }
}