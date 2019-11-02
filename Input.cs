using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GordonWare
{
    public static class Input
    {
        public static KeyboardState keyboard { get; private set; }
        public static MouseState mouse { get; private set; }

        public static void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
        }

        public static bool AnyKeyPressed()
        {
            return !(keyboard.GetPressedKeys().Length == 0);
        }
    }
}
