
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;


namespace Game1
{

    public class Game1 : Game
    {

        ParticleEngine particleEngine;
        private Texture2D shuttle;
        private Texture2D background;
        private Texture2D earth;
        private Texture2D fuel;
        private Texture2D satellite;
        private Texture2D t;
        protected Song song;
        SoundEffect noise1;
        SoundEffect noise2;
        SoundEffectInstance instance;
        SoundEffectInstance instance2;

        public bool noise_played;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        private SpriteFont font2;
        obj ship;
        object_handler t_obj;
        public bool game_active;


        public class objct
        {
            public Point pos { get; set; }
            public Color colour { get; set; }
            public int text_type { get; set; }
            public objct(Point p, Color clr, int type)
            {
                pos = p;
                colour = clr;
                text_type = type;
            }

        }

        public class object_handler
        {
           

            int count;
            int textures_num;
            private List<Texture2D> textures;
            private List<objct> objects;
            public object_handler(int num, List<Texture2D> texture_list)
            {
                //Console.Write
                objects = new List<objct>();
                textures = texture_list;
                textures_num = texture_list.Count;
                randomize(num);
                
            }
            public void randomize(int num)
            {
                objects.Clear();
                count = 0;
                Random rand = new Random();
                
                for (int i = 0; i < num; i++)
                {
                    Color color = new Color(
                       rand.Next(0, 255),
                       rand.Next(0, 255),
                      rand.Next(0, 255));
                    objects.Add(new objct(new Point(rand.Next(20, 500), rand.Next(20, 300)), color, 0));
                    count++;
                }

            }
            public void delete(objct q)
            {
                objects.Remove(q);
                count--;
            }

            public Point center(objct a)
            {
                return new Point(a.pos.X + 16, a.pos.Y + 16);
            }
            public bool ifIntersect(objct f, Point pos)
            {
                Console.WriteLine("pos {0}", pos);
                int k = 25 ;
                if ((pos.X+16 >= center(f).X - k &&
                    pos.X+16 <= center(f).X + k) && 
                    (pos.Y+16 >= center(f).Y - k && pos.Y+16 <= center(f).Y + k))
                {
                    Console.WriteLine("checkintersect");
                    return true;
                }
                    
                return false;
            }
            public void checkIntersect(Point player)
            {
                for (int i = 0; i < count; i++)
                {

                    if (ifIntersect(objects[i], player))
                    {
                        delete(objects[i]);
                    }

                }
            }


            public void Draw(SpriteBatch spriteBatch)
            {
                foreach (var a in objects)
                {
                    spriteBatch.Draw(textures[a.text_type], a.pos.ToVector2(), null, a.colour);
                }
   
            }

        }

        class obj
        {

            public bool game_win;
            public float angle_range = 0.08f;
            public float velocity_range = 0.3f;
            public int fuel;
            private const int dec_num = 10000;
            public Rectangle pos;
            public Vector2 float_position;
            public int maxfuel = 600;
            const float default_angle = 0.1f;
            const float acceleration = 0.0006f;
            const float deceleration = 0.005f;
            const float angular_acceleration = 0.0005f;
            const float angular_deceleration = 0.001f;
            public Vector2 dir;
            public bool hold { get; set; }
            public float vector_velocity;

            const float max_speed = 0.01f;
            public float angular_velocity;
            public float angle;

            public float velocity;
            public Vector2 vector;
            public bool k_up { get; set; }
            public bool k_down { get; set; }
            public bool k_left { get; set; }
            public bool k_right { get; set; }

            public obj()
            {
                refresh();
            }
           
            public void SatCollision()
            {
              if ((pos.Location.Y < 556) && (pos.Location.X > 550) && ((pos.Location.Y < 215) && (pos.Location.Y > 180)))
                {
                    if ((Math.Abs(angle) < (float)Math.PI + angle_range) && (Math.Abs(angle) > (float)Math.PI - angle_range))
                        if (vector_velocity < velocity_range)
                            game_win = true;
                        else
                         Console.WriteLine("Lost");
                    else
                        Console.WriteLine("Lost");
     
                    hold = true;
                // Console.WriteLine("dasdasdas");
                }  
            }
            public void refresh()
            {
                
                hold = false;
                fuel = maxfuel;

                pos.Width = 30;
                pos.Height = 40;
                angle = 0;
                velocity = 0;
                vector.X = 0.5f;
                vector.Y = 0;
                float_position.X = 100; // level 1 = 100
                float_position.Y = 300; // level 1 = 200
                dir.X = 0;
                dir.Y = 0;
                angular_velocity = 0;
                vector_velocity = (float)Math.Round(Math.Sqrt(Math.Pow(vector.X, 2)) + Math.Pow(vector.Y, 2), 2);
                game_win = false;
            }


            public void Update()
            {
                
                if (angle > Math.PI*2)
                    angle = 0;
                if (angle < -Math.PI * 2)
                    angle = 0;
                vector_velocity = (float)Math.Round(Math.Sqrt(Math.Pow(vector.X, 2)) + Math.Pow(vector.Y, 2), 2);

                if (!hold)
                {
                    if (fuel > 0)
                        if ( k_left || k_right || k_up)
                            fuel--;

                    if (k_right)
                    {
                        if (angular_velocity < default_angle)
                        {
                            angular_velocity += angular_acceleration;
                            if (angular_velocity > default_angle)
                                angular_velocity = default_angle;
                        }

                    }

                    if (k_left)
                    {
                        if (angular_velocity > -1f * default_angle)
                        {
                            angular_velocity -= angular_acceleration;
                            if (angular_velocity < -1f * default_angle)
                                angular_velocity = -1f * default_angle;
                        }
                    }


                    angular_velocity = (float)Math.Round(angular_velocity, 5);
                    angle += angular_velocity;
                    angle = (float)Math.Round(angle, 4);
                    velocity = (float)Math.Round(velocity, 3);
                    dir.X = (float)Math.Cos(angle);
                    dir.Y = (float)Math.Sin(angle);

           
     
                    if (k_up)
                    {
                        vector.X += dir.X * velocity;
                        vector.Y += dir.Y * velocity;

                        if (velocity < max_speed)
                            velocity += acceleration;
                        else
                            velocity = max_speed;
                    }
                    else
                    {

                        if (velocity > 0)
                            velocity -= deceleration;
                        else velocity = 0;

                    }

                    vector.X = (float)Math.Round(vector.X, 3);
                    vector.Y = (float)Math.Round(vector.Y, 3);
            
                    float_position.X += vector.X;
                    float_position.Y += vector.Y;
                    float_position.X = (float)Math.Round(float_position.X, 3);
                    float_position.Y = (float)Math.Round(float_position.Y, 3);
                    pos.X = (int)float_position.X;
                    pos.Y = (int)float_position.Y;

                }
             }

        }

        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color, int thickness)
        {

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    thickness), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            game_active = true;
            
            ship = new obj();
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.


            song = Content.Load<Song>("sound/poque");  // Put the name of your song here instead of "song_title"
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("images/fire2"));

            t = new Texture2D(GraphicsDevice, 1, 1);
            t_obj = new object_handler(10, textures);

            t.SetData<Color>(
                new Color[] { Color.White });// fill the texture with white

            particleEngine = new ParticleEngine(textures, new Vector2(400, 240), 1);
            satellite = Content.Load<Texture2D>("images/satellite");
            background = Content.Load<Texture2D>("images/stars"); // change these names to the names of your images
            shuttle = Content.Load<Texture2D>("images/shuttle");  // if you are using your own images.
            earth = Content.Load<Texture2D>("images/earth");
            fuel = Content.Load<Texture2D>("images/fuel");
            font = Content.Load<SpriteFont>("fonts/font1");
            font2 = Content.Load<SpriteFont>("fonts/font2");
            noise1 = Content.Load<SoundEffect>("sound/noise1");
            noise2 = Content.Load<SoundEffect>("sound/noise2"); 
            instance2 = noise2.CreateInstance();
            instance = noise1.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
            instance.Volume = 0;
            instance2.Volume = 0.2f;     
            noise_played = false;



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            this.IsMouseVisible = true;
            MouseState mouseState = Mouse.GetState();
         //   Console.WriteLine("Mouse X: {0} Mouse Y: {1}", mouseState.X, mouseState.Y);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState state = Keyboard.GetState();
            bool leftArrowKeyDown = state.IsKeyDown(Keys.Left);
            ship.SatCollision();

            particleEngine.EmitterLocation = ship.float_position;
            particleEngine.dir_vector = -1 * ship.dir;
            particleEngine.Update();

            t_obj.checkIntersect(ship.pos.Location);

            if (ship.pos.X < 0 || ship.pos.X > 800)
                game_active = false;
            if (ship.pos.Y < 0 || ship.pos.Y > 480)
                game_active = false;

            if (game_active)
            {
                if (state.IsKeyDown(Keys.CapsLock))
                    ship.hold = true;
                if(ship.hold == true)
                if (state.IsKeyDown(Keys.Space))
                {
                    ship.refresh();
                        t_obj.randomize(5);
                    game_active = true;
                }

                if (ship.fuel > 0)
                {
                    if (state.IsKeyDown(Keys.Left))
                    {
                        ship.k_left = true;
                    }
                    if (state.IsKeyDown(Keys.Up))
                    {
                     
                         noise_played = true;
                        ship.k_up = true;
                        particleEngine.Activity = 15;
                   //     instance.Volume = 0.13f;
                    }
                    if (state.IsKeyDown(Keys.Down))
                        ship.k_down = true;

                    if (state.IsKeyDown(Keys.Right))
                    {
                        ship.k_right = true;
                    }
                }
                else
                {
                    instance.Volume = 0;
                    particleEngine.Activity = 0;
                    if (noise_played)
                    {
                        instance2.Play();
                        noise_played = false;
                    }
                      
                    if (state.IsKeyDown(Keys.Space))
                    {
                        ship.refresh();
                        t_obj.randomize(10);
                        game_active = true;
                    }
                }

                if (state.IsKeyUp(Keys.Left))
                {
                    ship.k_left = false;
                }
                if (state.IsKeyUp(Keys.Up))
                {
                    instance.Volume = 0f;
                    Console.WriteLine("dsadasdas0");
                    if (noise_played)
                    {
                        instance2.Stop();
                        instance2.Play();
                        
                        noise_played = false;
                    }
                    ship.k_up = false;
                    if (ship.fuel > 0)
                    {
                        particleEngine.Activity = 1;                 
                    }
                }
                if (state.IsKeyUp(Keys.Down))
                    ship.k_down = false;

                if (state.IsKeyUp(Keys.Right))
                {
                    ship.k_right = false;
                }
            }
            else if(state.IsKeyDown(Keys.Space))
            {
                ship.refresh();
                game_active = true;
            }

            ship.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
           
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            // spriteBatch.Draw(earth, new Vector2(400, 240), Color.White);
            if (game_active)
            {
                spriteBatch.Draw(satellite, new Vector2(600, 200), null, Color.White, 3 * (float)Math.PI / 2f, satellite.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 1);
                particleEngine.Draw(spriteBatch);
                if (ship.fuel > 0)
                    DrawLine(spriteBatch, //Green line
                        new Vector2(ship.pos.X, ship.pos.Y), //start of lineship
                        new Vector2(ship.pos.X + ship.dir.X * 100, ship.pos.Y + ship.dir.Y * 100), Color.Green, 2 //end of line
                    );

                spriteBatch.Draw(shuttle, ship.pos, null, Color.AliceBlue, ship.angle + (float)Math.PI / 2f, shuttle.Bounds.Center.ToVector2(), SpriteEffects.None, 1);
                if (ship.fuel > 0)
                    DrawLine(spriteBatch, //Blue line
                       new Vector2(ship.pos.X, ship.pos.Y), //start of lineship
                       new Vector2(ship.pos.X + ship.vector.X * 100, ship.pos.Y + ship.vector.Y * 100), Color.DodgerBlue, 1 //end of line
                   );

                Color clr1 = Color.Red;
                Color clr = Color.Green;

                if ((Math.Abs(ship.angle) < (float)Math.PI + ship.angle_range) && (Math.Abs(ship.angle) > (float)Math.PI - ship.angle_range))
                    clr1 = Color.Green;
                spriteBatch.DrawString(font, "Angle: " + 1f * ship.angle, new Vector2(20, 20), clr1);
                //    spriteBatch.DrawString(font, "Acceleration: " + 1f * ship.velocity, new Vector2(20, 40), Color.White);

                t_obj.Draw(spriteBatch);
                if (ship.vector_velocity > ship.velocity_range)
                    clr = Color.Red;
                spriteBatch.DrawString(font, "Velocity: " + ship.vector_velocity, new Vector2(20, 60), clr);
               // spriteBatch.DrawString(font, "Vector: " + 1f * ship.vector, new Vector2(20, 80), Color.White);
                spriteBatch.DrawString(font, "Angular Velocity: " + 1f * Math.Abs(ship.angular_velocity), new Vector2(20, 100), Color.White);
                spriteBatch.DrawString(font, "Fuel: ", new Vector2(20, 120), Color.White);
                if (ship.fuel == 0)
                    spriteBatch.DrawString(font, "OUT OF FUEL", new Vector2(60, 120), Color.Red);
                if (ship.hold)
                    if (ship.game_win == true)
                    {
                        spriteBatch.DrawString(font2, "WIN WTIN ", new Vector2(300, 120), Color.Red);
                    }
                    else
                    {
                        spriteBatch.DrawString(font2, "LOST ", new Vector2(300, 120), Color.Red);
                    }
                spriteBatch.Draw(fuel, new Rectangle(new Point(60, 120), new Point((int)((ship.fuel / (float)ship.maxfuel) * 100), 10)), new Rectangle(20, 500, 2, 2), Color.AliceBlue);
            }
            else
            {
                spriteBatch.DrawString(font2, "daleko sjebalsya", new Vector2(200, 200), Color.Red);
                game_active = false; 
            }
            

            spriteBatch.End();
            
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}