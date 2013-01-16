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
    public class CharacterSelectScreen : GameScreen
    {
        SpriteFont font;
        MenuManager menu;
        FileManager fileManager;
        List<Texture2D> images;
        AudioManager audio;
        Song song;

        protected Rectangle sourceRect;

        int imageNumber;

        protected float scale;

        Vector2 position;

        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            images = new List<Texture2D>();
            base.LoadContent(content, inputManager);
            font = this.content.Load<SpriteFont>("TitleScreen/Coolvetica Rg");

            menu = new MenuManager();
            menu.LoadContent(content, "CharacterSelect");

            fileManager = new FileManager();
            fileManager.LoadContent("Load/CharacterSelect.txt", attributes, contents);
            audio = new AudioManager();

            position = Vector2.Zero;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Images":
                            images.Add(this.content.Load<Texture2D>(contents[i][j]));
                            break;
                        case "Position":
                            string[] temp = contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(temp[0]),
                                float.Parse(temp[1]));
                            break;
                        case "Scale":
                            scale = float.Parse(contents[i][j]);
                            break;
                        case "Songs":
                            temp = contents[i][j].Split(',');
                            song = this.content.Load<Song>(temp[1]);
                            audio.songs.Add(song);
                            break;
                    }
                }
                sourceRect = new Rectangle(0, 0, images[imageNumber].Width, images[imageNumber].Height);
            }
            audio.PlaySong(1, true);
            //audio.FadeSong(0.0f, new TimeSpan(0, 0, 5));
        }

        public override void UnloadContent()
        {
            menu.UnloadContent();
            inputManager = null;
            attributes.Clear();
            contents.Clear();
            attributes.Clear();
            contents.Clear();
            content.Unload();
            MediaPlayer.Stop();
            this.content.Unload();
            audio.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            menu.Update(gameTime, inputManager);
            audio.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
            spriteBatch.Draw(images[imageNumber], position, sourceRect, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
        }
    }
}
