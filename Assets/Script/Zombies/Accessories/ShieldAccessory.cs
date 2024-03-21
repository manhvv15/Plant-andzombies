using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Constants;

namespace Assets.Script.Zombies.Accessories
{
    public class ShieldAccessory : Accessory
    {
        public override int Hit(int damage, int damageType)
        {
            if (damageType == DamageType.DIRECT)
            {
                return base.Hit(damage, damageType);
            } else if (damageType == DamageType.LASER)
            {
                base.Hit(damage, damageType);
            }
            return damage;
        }
    }
}
