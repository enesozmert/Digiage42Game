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
            print("Havada s�z�l�yor, temas yok!");
        if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
            print("Etraf�ndan bir �eylere temas ediyor");
        if (controller.collisionFlags == CollisionFlags.Sides)
            print("Yaln�zca yanlardan temas var,ba�ka bir temas yok");
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            print("Tavana de�iyor");
        if (controller.collisionFlags == CollisionFlags.Above)
            print("Sadece yukard�an temas,ba�ka bir temas yok");
        if ((controller.collisionFlags & CollisionFlags.Below) != 0)
            print("Yere de�iyor");
        if (controller.collisionFlags == CollisionFlags.Below)
            print("Yaln�zca yere de�iyor, ba�ka bir temas yok");
    }
}