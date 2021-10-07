using OptionalUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace squeezeThrough
{
    public class MyOI : OptionInterface
    {
        public MyOI() : base(plugin: UnityEngine.Object.FindObjectOfType<Plugin>()) // Your BaseUnityPlugin instance
        {
            
        }

        public static KeyCode[] controls = new KeyCode[4];
        public override void Initialize()
        {
            base.Initialize(); // This should be called before everything else
            Tabs = new OpTab[1]; // The number of OpTab must be 1 ~ 20
            Tabs[0] = new OpTab("Main") // Each OpTab is 600 x 600 pixel sized canvas
            { color = Color.white }; // You can change its button and canvas colour too


            Tabs[0].AddItems(new OpKeyBinder(new Vector2(33, 600 - 100f), new Vector2(150, 32), "org.sov.phlegmShrink", "squeezeKey1", "U", true, OpKeyBinder.BindController.Controller1) { description = "player 1 squeeze control" });
            Tabs[0].AddItems(new OpKeyBinder(new Vector2(33, 600 - 145f), new Vector2(150, 32), "org.sov.phlegmShrunk", "squeezeKey2", "I", true, OpKeyBinder.BindController.Controller2) { description = "player 2 squeeze control" });
            Tabs[0].AddItems(new OpKeyBinder(new Vector2(33, 600 - 190f), new Vector2(150, 32), "org.sov.phlegmShrank", "squeezeKey3", "O", true, OpKeyBinder.BindController.Controller3) { description = "player 3 squeeze control" });
            Tabs[0].AddItems(new OpKeyBinder(new Vector2(33, 600 - 235f), new Vector2(150, 32), "org.sov.phlegmShronk", "squeezeKey4", "P", true, OpKeyBinder.BindController.Controller4) { description = "player 4 squeeze control" });
        }

        public override void ConfigOnChange()
        {
            base.ConfigOnChange();
            controls[0] = OpKeyBinder.StringToKeyCode(config["squeezeKey1"]);
            controls[1] = OpKeyBinder.StringToKeyCode(config["squeezeKey2"]);
            controls[2] = OpKeyBinder.StringToKeyCode(config["squeezeKey3"]);
            controls[3] = OpKeyBinder.StringToKeyCode(config["squeezeKey4"]);
        }
    }
}
