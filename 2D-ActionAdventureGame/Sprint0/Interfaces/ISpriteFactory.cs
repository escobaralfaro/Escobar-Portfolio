using Microsoft.Xna.Framework;
using System;

namespace Sprint0.Classes
{

    public interface ISpriteFactory
    {
        void LoadTexture(String sheetName);

       Rectangle[] CreateFrames();
    }
}
