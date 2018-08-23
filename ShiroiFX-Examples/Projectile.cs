using Shiroi.FX.Colors;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace Shiroi.FX.Examples {
    public class Projectile : Ownable<ProjectileShooter> {
        public Effect ShotEffect;
        public Effect HitEffect;
        public Color ProjetileColor;
        public Rigidbody Rigidbody;

        public void Shoot(Vector3 direction) {
            if (!HasOwner) {
                return;
            }

            Rigidbody.velocity = direction;
            ShotEffect.PlayIfPresent(
                new EffectContext(
                    this,
                    new PositionFeature(transform.position),
                    new ColorFeature(ProjetileColor),
                    new VelocityFeature(direction),
                    new FloatFeature(Random.value, "Volume"),
                    new FloatFeature(Random.value, "Pitch")
                )
            );
        }


        private void OnCollisionEnter(Collision other) {
            HitEffect.Play(
                new EffectContext(
                    this,
                    new PositionFeature(transform.position),
                    new ColorFeature(ProjetileColor)
                )
            );
        }
    }
}