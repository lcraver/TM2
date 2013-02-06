﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TM2
{
    public class CreditsScreen : GameScreen
    {
        SpriteFont font;
        Song song;
        FileManager fileManager;
        GUIManager gui;
        AudioManager audio;

        Video video;
        VideoPlayer videoPlayer;
        Texture2D videoTexture;

        bool dul;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            font = this.content.Load<SpriteFont>("CreditsScreen/Coolvetica Rg");

            fileManager = new FileManager();
            fileManager.LoadContent("Load/Credits.txt", attributes, contents);

            gui = new GUIManager();
            gui.LoadContent(content, "Credits");

            audio = new AudioManager();
            audio.LoadContent(content, "Credits");

            dul = false;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Videos" :
                            video = this.content.Load<Video>(contents[i][j]);
                            videoPlayer = new VideoPlayer();
                            break;
                    }
                }
            }

            audio.PlaySong(0, true);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
            videoPlayer.Stop();
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            if (inputManager.KeyDown(Keys.Space, Keys.Enter))
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);

            if (inputManager.KeyPressed(Keys.D))
            {
                if (videoPlayer.State == MediaState.Paused | videoPlayer.State == MediaState.Stopped)
                {
                    videoPlayer.IsLooped = true;
                    videoPlayer.Resume();
                    videoPlayer.Play(video);
                    MediaPlayer.Pause();
                }
                dul = !dul;
            }
            else if (dul == false)
            {
                if (videoPlayer.State == MediaState.Playing)
                {
                    videoPlayer.Pause();
                    videoPlayer.IsMuted = true;
                    MediaPlayer.Resume();
                }
            }
            else
            {
                videoPlayer.IsMuted = false;
            }
                
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Only call GetTexture if a video is playing or paused
            if (videoPlayer.State == MediaState.Playing)
                videoTexture = videoPlayer.GetTexture();
            else
                videoTexture = null;

            // Drawing to the rectangle will stretch the 
            // video to fill the screen
            Rectangle screen = new Rectangle(0, 0, 1280, 720);

            // Draw the video, if we have a texture to draw.
            if (videoTexture != null)
            {
                spriteBatch.Draw(videoTexture, screen, Color.White);
            }
            spriteBatch.DrawString(font, "Made by Liam Craver (Lime Studios)", new Vector2(100, 200), Color.Black);
            spriteBatch.DrawString(font, "Based off the members of Team Mongoose", new Vector2(100, 300), Color.Black);
        }
    }
}
