using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint2.GameStates
{
    public class AchievementManager
    {
        private List<Achievement> achievements;
        private List<Achievement> unlockedAchievements;
        private List<Achievement> alreadyPrintedAchievements;
        public int EnemyDefeatedCount { get; private set; }
        private float achievementVisibleTime = 8f; // achievements are visible for 8 seconds   
        private float achievementTimer = 0f;
        private Link _link;
        public Vector2 _scale;
        private Dictionary<Achievement, float> achievementTimers;

        public AchievementManager(Link link, Vector2 scale)
        {
            achievements = new List<Achievement>();
            unlockedAchievements = new List<Achievement>();
            alreadyPrintedAchievements = new List<Achievement>();
            _link = link;
            _scale = scale;
            achievementTimers = new Dictionary<Achievement, float>();
        }

        public void AddAchievement(Achievement achievement)
        {
            achievements.Add(achievement);
        }

        public void Update(GameTime gameTime)
        {

            foreach (var achievement in achievements)
            {
                achievement.Update();
                if (achievement.IsUnlocked && !unlockedAchievements.Contains(achievement) && !alreadyPrintedAchievements.Contains(achievement))
                {
                    unlockedAchievements.Add(achievement);
                    achievementTimers[achievement] = 0f; // Start the display timer
                }
            }

            // Update timers for unlocked achievements
            foreach (var achievement in unlockedAchievements.ToList()) 
            {
                achievementTimers[achievement] += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (achievementTimers[achievement] >= achievementVisibleTime)
                {
                    //Debug.WriteLine($"Hiding achievement: {achievement.Name}");
                    unlockedAchievements.Remove(achievement);
                    achievementTimers.Remove(achievement); // Clean up timer
                    alreadyPrintedAchievements.Add(achievement); // to make sure achievement doesnt print again
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice graphicsDevice)
        {
            //spriteBatch.Begin();
            if (achievements.Count > 0)
            {
                //Debug.WriteLine($"Link's position: {_link._position.X}, {_link._position.Y}");
                //Vector2 basePosition = new Vector2(graphicsDevice.Viewport.Width - 300, 50);
                Vector2 basePosition = new Vector2(500, 750);
                //Vector2 basePosition = new Vector2(_link._position.X * _scale.X, _link._position.Y * _scale.Y);
                int yOffset = 0;

                foreach (var achievement in unlockedAchievements)
                {
                    //Debug.WriteLine($"Drawing unlocked achievement: {achievement.Name}");
                    spriteBatch.DrawString(font,
                        $"Achievement: {achievement.Name}",
                        basePosition + new Vector2(2, yOffset + 2),
                        Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 0); // Shadow
                    spriteBatch.DrawString(font,
                        $"Achievement: {achievement.Name}",
                        basePosition + new Vector2(0, yOffset),
                        Color.Yellow, 0, Vector2.Zero, 2f, SpriteEffects.None, 0); // Text
                    yOffset += 30;
                }
            }
        }
    }
}