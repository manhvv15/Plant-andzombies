using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Constants;
using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script.Zombies.Accessories
{
    public class NewspaperAccessory : ShieldAccessory
    {
        [SerializeField] private AudioClip newspaperRipClip;
        [SerializeField] private AudioClip angryZombieClip;
        protected override void OnAccessoryRemoved(Zombie zombie)
        {
            zombie.TriggerAnger();
            SoundManager.Instance.PlaySound(newspaperRipClip);
            Invoke(nameof(Anger),1.5f);
        }

        protected virtual void Anger()
        {
            SoundManager.Instance.PlaySound(angryZombieClip);
        }
    }
}
