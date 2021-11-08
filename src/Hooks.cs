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
            On.Creature.Update += Creature_Update;
        }

        private void Creature_Update(On.Creature.orig_Update orig, Creature self, bool eu)
        {
            orig(self, eu);
            if (!self.dead)
            {
                if (self is BigNeedleWorm noot)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = noot.AI.focusCreature;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player && noot.AI.behavior == BigNeedleWormAI.Behavior.Attack)
                            {
                                isPursued[player] = true;
                                Debug.Log("noot pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
                else if (self is Scavenger scav)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = scav.AI.focusCreature;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player && scav.AI.behavior == ScavengerAI.Behavior.Attack)
                            {
                                isPursued[player] = true;
                                Debug.Log("scav pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
                else if (self is BigSpider spider)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = spider.AI.preyTracker.MostAttractivePrey;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player && spider.AI.behavior == BigSpiderAI.Behavior.Hunt)
                            {
                                isPursued[player] = true;
                                Debug.Log("spider pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
                else if (self is Centipede centi)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = centi.AI.preyTracker.MostAttractivePrey;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player && centi.AI.behavior == CentipedeAI.Behavior.Hunt)
                            {
                                isPursued[player] = true;
                                Debug.Log("spider pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
                else if (self is TentaclePlant plant)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = plant.AI.preyTracker.MostAttractivePrey;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player)
                            {
                                isPursued[player] = true;
                                Debug.Log("spider pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
                else if (self is DropBug wig)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = wig.AI.focusCreature;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player && wig.AI.behavior == DropBugAI.Behavior.Hunt)
                            {
                                isPursued[player] = true;
                                Debug.Log("liz pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
                else if (self is Lizard liz)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = liz.AI.focusCreature;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player && liz.AI.behavior == LizardAI.Behavior.Hunt)
                            {
                                isPursued[player] = true;
                                Debug.Log("liz pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
                else if (self is Vulture vulch)
                {
                    try
                    {
                        Tracker.CreatureRepresentation focus = vulch.AI.focusCreature;
                        if (focus.representedCreature.realizedCreature != null)
                        {
                            if (focus.representedCreature.realizedCreature is Player player && vulch.AI.behavior == VultureAI.Behavior.Hunt)
                            {
                                isPursued[player] = true;
                                Debug.Log("something pursuing player " + player.playerState.playerNumber);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private void Player_ctor(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            previousRollCounter.Add(self, 0);
            canSqueeze.Add(self, false);
            isPursued.Add(self, false);
            attemptSqueeze.Add(self, false);
            attemptEnterSqueeze.Add(self, false);
            //slipWasHeld.Add(self, false);
        }

        //bool corridorjump = false;
        static Dictionary<Player, int> previousRollCounter = new Dictionary<Player, int>();

        //static Dictionary<Player, bool> slipWasHeld = new Dictionary<Player, bool>();

        static Dictionary<Player, bool> canSqueeze = new Dictionary<Player, bool>();

        static Dictionary<Player, bool> attemptSqueeze = new Dictionary<Player, bool>();

        static Dictionary<Player, bool> isPursued = new Dictionary<Player, bool>();

        static Dictionary<Player, bool> attemptEnterSqueeze = new Dictionary<Player, bool>();

        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            previousRollCounter[self] = self.rollCounter;
            orig(self, eu);
            if (self != null)
            {//===============================================================================================================//
                int wasCollisionLayer = self.collisionLayer;

                bool crouching = self.bodyMode == Player.BodyModeIndex.Crawl;

                bool rolling = previousRollCounter[self] != self.rollCounter;

                bool inCorridor = self.bodyMode == Player.BodyModeIndex.CorridorClimb;

                canSqueeze[self] = (crouching || rolling || inCorridor) && !(self.exhausted || isPursued[self]); //Debug.Log("cansqueeze: " + (bool)canSqueeze[self]);

                Debug.Log(attemptEnterSqueeze[self] = Input.GetKey(MyOI.controls[self.playerState.playerNumber]) && !attemptSqueeze[self]);

                Debug.Log(attemptSqueeze[self] = Input.GetKey(MyOI.controls[self.playerState.playerNumber]));

                //===============================================================================================================//   all code within finds conditions that allow the slug should squeeze

                if (attemptSqueeze[self] && canSqueeze[self])
                {
                    self.ChangeCollisionLayer(2); //within a squeeze
                }
                else
                {
                    self.ChangeCollisionLayer(1); //not squeeze
                }

                if(wasCollisionLayer != self.collisionLayer && !attemptSqueeze[self]) //exit squeeze
                {
                    self.room.PlaySound(SoundID.Snail_Pop, self.mainBodyChunk.pos, 0.4f, 0.8f);
                    Debug.Log("exit squeeze");
                }

                if (attemptEnterSqueeze[self] && !canSqueeze[self]) //fails to enter a squeeze
                {
                    self.room.PlaySound(SoundID.Snail_Pop, self.mainBodyChunk.pos, 0.3f, 0.6f);
                    Debug.Log("FAIL");
                }
                else if (attemptEnterSqueeze[self] && canSqueeze[self]) //wins at entering squeeze
                {
                    self.room.PlaySound(SoundID.Snail_Pop, self.mainBodyChunk.pos, 0.3f, 1f);
                    Debug.Log("WIN");
                }

                isPursued[self] = false;
            }
        }
    }
}