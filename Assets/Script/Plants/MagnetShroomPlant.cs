using Assets.Script.Constants;
using Assets.Script.Sound;
using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Zombies.Accessories;
using UnityEngine;

public class MagnetShroomPlant : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;
    private SpriteRenderer spriteRenderer;
    private bool isActive = true;

    private Accessory capturedAccessory;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite cooldownSprite;

    [SerializeField] private float cooldown;
    [SerializeField] private float speed;
    [SerializeField] private float timeUntilKillAccessory;
    [SerializeField] private AudioClip magnetClip;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
        var vectorA = new Vector3(6f, 4f, 0);
        var vectorB = new Vector3(0, -4f, 0);
        var position = gameObject.transform.position;
        pointA = vectorA + position;
        pointB = vectorB + position;
        InvokeRepeating(nameof(TryMagnet),speed,speed);
    }

    // Update is called once per frame
    void Update()
    {
     
        
    }

    void TryMagnet()
    {
        if (!isActive)
        {
            return;
        }
        var overlapArea = Physics2D.OverlapAreaAll(pointA,pointB);
        foreach (var x in overlapArea)
        {
            if (x.TryGetComponent<Zombie>(out var zombie))
            {
                if (zombie.accessory && zombie.accessory.isMetal)
                {
                    //Remove Accessory
                    capturedAccessory = zombie.accessory;
                    Debug.Log($"{this.name} spotted a metal {capturedAccessory.name}.");
                    capturedAccessory.RemoveAccessory(zombie, true);
                    capturedAccessory.transform.SetParent(null);
                    capturedAccessory.MoveTowards(this.transform.position);
                    Invoke(nameof(KillAccessory),timeUntilKillAccessory);
                    StartCooldown();
                    break;
                }
            }
        }
    }

    void KillAccessory()
    {
        if (capturedAccessory)
        {
            Destroy(capturedAccessory.gameObject);
            capturedAccessory = null;
        }
        
    }

    void StartCooldown()
    {
        SoundManager.Instance.PlaySound(magnetClip);
        isActive = false;
        spriteRenderer.sprite = cooldownSprite;
        Invoke(nameof(EndCooldown),cooldown);
    }

    void EndCooldown()
    {
        isActive = true;
        spriteRenderer.sprite = normalSprite;
    }
}
