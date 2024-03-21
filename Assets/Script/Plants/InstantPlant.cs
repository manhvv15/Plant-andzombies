using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Plants
{
    public class InstantPlant : Plant
    {
        public float timeUntilEffectsApply;
        public override void Start()
        {
            base.Start();
            Invoke(nameof(ApplyEffect), timeUntilEffectsApply);
        }

        public override void Hit(int damage)
        {
            return;
        }

        private void ApplyEffect()
        {
            Effect();
            Destroy(gameObject);
        }

        protected virtual void Effect()
        {

        }
    }
}
