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
            On.Weapon.HitSomethingWithoutStopping += Weapon_HitSomethingWithoutStopping;
        }

        private void Weapon_HitSomethingWithoutStopping(On.Weapon.orig_HitSomethingWithoutStopping orig, Weapon self, PhysicalObject obj, BodyChunk chunk, PhysicalObject.Appendage appendage)
        {
            if(!(self.thrownBy is Player player && player == obj))
            {
                orig(self, obj, chunk, appendage);
            }
            else
            {
                Debug.Log("#winning");
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