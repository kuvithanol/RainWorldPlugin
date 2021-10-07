using squeezeThrough;
using UnityEngine;
namespace squeezeThrough
{

    public sealed class Hooks
    {
        private readonly PluginState state;
        public Hooks(PluginState state)
        {
            this.state = state;

            On.Player.Update += Player_Update;
            On.Player.ctor += Player_ctor;
            On.Creature.Violence += Creature_Violence;
        }

        private void Creature_Violence(On.Creature.orig_Violence orig, Creature self, BodyChunk source, Vector2? directionAndMomentum, BodyChunk hitChunk, PhysicalObject.Appendage.Pos hitAppendage, Creature.DamageType type, float damage, float stunBonus)
        {
            if (self is Player player)
            {
                if(self.collisionLayer != 0 && (source.owner is Spear || source.owner is Rock)) // && (type == Creature.DamageType.Stab || type == Creature.DamageType.Blunt)
                {
                    orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus);
                }
                else
                {
                    //NOT calling orig!!! owned!!!
                }
            }
            else
            {
                orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus);
            }
        }

        private void Player_ctor(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            previousRollCounter.Add(self, 0);
            rolling.Add(self, false);
            crouching.Add(self, false);
            canSqueeze.Add(self, false);
            inCorridor.Add(self, false);
        }

        //bool corridorjump = false;
        static Dictionary<Player, int> previousRollCounter = new Dictionary<Player, int>();

        static Dictionary<Player, bool> rolling = new Dictionary<Player, bool>();

        static Dictionary<Player, bool> crouching = new Dictionary<Player, bool>();

        static Dictionary<Player, bool> inCorridor = new Dictionary<Player, bool>();

        static Dictionary<Player, bool> canSqueeze = new Dictionary<Player, bool>();

        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            previousRollCounter[self] = self.rollCounter;
            orig(self, eu);
            //===============================================================================================================//

            crouching[self] = self.bodyMode == Player.BodyModeIndex.Crawl;

            if (previousRollCounter[self] != self.rollCounter) 
                rolling[self] = true; else rolling[self] = false;

            if (self.bodyMode == Player.BodyModeIndex.CorridorClimb)
                inCorridor[self] = true; else inCorridor[self] = false;

            canSqueeze[self] = (crouching[self] || rolling[self] || inCorridor[self]) && !self.exhausted;

            //===============================================================================================================//   all code within finds conditions that allow the slug to squeeze

            if (Input.GetKey(MyOI.controls[self.playerState.playerNumber]) && canSqueeze[self])
            {
                self.ChangeCollisionLayer(0);
            }
            else
            {
                self.ChangeCollisionLayer(1);
            }
        }
    }
}