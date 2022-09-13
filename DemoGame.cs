using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ExpressedEngine.ExpressedEngine;
using System.Windows.Forms;

namespace ExpressedEngine
{
    class DemoGame : ExpressedEngine.ExpressedEngine
    {
        //FLOATY BOY GAME

        //Shape2D player;
        Sprite2D player;
        Sprite2D enemy;
        int coinsCollected = 0;

        bool left;
        bool right;
        bool up;
        bool down;

        //Vector2 lastPos = Vector2.Zero();
        //Current mappable Variables are: c coin, p player, g ground
        private readonly string[,] Map =
        {
             {"g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g",},
             {"g",".",".",".","g",".","c",".",".",".",".","c","g","g","g",".",".","c","g","c","c","c","g",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".","g",},
             {"g",".",".",".","g",".","g","g","g","g","g",".","g","g",".",".","g",".","g","g","g","c","g",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","g",},
             {"g",".","d",".","g",".","g","c","c","g","g",".","g",".",".","g","g",".",".",".","g","c","g",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".","g",},
             {"g",".",".",".",".",".","g","c","c","g",".",".","g",".","g",".","g","g","g",".","g",".","g",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".","g",},
             {"g",".",".",".",".",".","g","c","c","g",".","g",".",".","g",".",".",".",".",".","g",".",".",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".","g",},
             {"g",".",".",".","g",".","g","c","g",".",".","g",".","g","g",".","g","g","g","g","g",".","g",".","g",".",".",".",".",".",".",".",".","c","c","c",".",".","g",},
             {"g",".",".",".","g","c","c","c","g",".","g","g",".","g",".",".",".",".","g",".",".",".","g",".","g",".",".","e","e",".",".",".",".","c","d","c",".",".","g",},
             {"g",".","p",".","c","c","c","c","c","c",".",".",".","c",".",".","c",".",".",".","c",".","c",".","c",".",".",".",".",".",".",".",".","c","d","c",".",".","g",},
             {"g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g",},
        };
        public DemoGame() : base(new Vector2(615, 515), "Expressed Engine Demo") { }

        public override void OnDraw()
        {

        }
        //anything to by loaded into the game before the renderer starts
        public override void OnLoad()
        {
            BackGroundColor = Color.Black;//this changes the background color
            CameraZoom = new Vector2(.95f, .95f);
            Sprite2D groundRef = new Sprite2D("Tiles/Blue tiles/tileBlue_03");                        
            Sprite2D coinRef = new Sprite2D("Items/yellowCrystal");
            Sprite2D deathRef = new Sprite2D("Other/blockRed");
            Sprite2D enemyRef = new Sprite2D("Enemies/enemyWalking_1");

            //player = new Shape2D(new Vector2(10, 10), new Vector2(10, 10), "Test");//this is for shape
            //player = new Sprite2D(new Vector2(10, 10), new Vector2(36, 45), "Players/Player Grey/playerGrey_walk1","Player");
            for (int i = 0; i < Map.GetLength(1); ++i)
            {
                for (int j = 0; j < Map.GetLength(0); ++j)
                {
                    if (Map[j, i] == "g")
                    {
                        new Sprite2D(new Vector2(i * 50, j * 50), new Vector2(50, 50), "Tiles/Blue tiles/tileBlue_03", "Ground").CreateStatic();
                    }
                    if (Map[j, i] == "c")
                    {
                        new Sprite2D(new Vector2(i * 50 + 15, j * 50 + 15), new Vector2(25, 25), coinRef, "Coin");
                    }
                    if (Map[j, i] == "p")
                    {
                        player = new Sprite2D(new Vector2(i * 50, j * 50), new Vector2(30, 40), "Players/Player Green/playerGreen_walk1", "Player");
                        player.CreateDynamic();
                    }
                    if (Map[j, i] == "e")
                    {
                        enemy = new Sprite2D(new Vector2(i * 50, j * 50), new Vector2(30, 40), enemyRef, "Enemy");
                        //new Sprite2D(new Vector2(i * 50 + 15, j * 50 + 15), new Vector2(25, 25), enemyRef, "Enemy");
                        enemy.CreateDynamic();
                        Log.Warning("Player should of been killed!!");
                        //System.Windows.Forms.Application.Exit();
                        //System.Windows.Forms.Application.ExitThread();
                    }
                    if (Map[j, i] == "d")
                    {
                        new Sprite2D(new Vector2(i * 50 + 15, j * 50 + 15), new Vector2(25, 25), deathRef ,"Death Block");                        
                        Log.Warning("Player has touched a death block and should be killed!!");



                        //System.Windows.Forms.Application.ExitThread();
                    }
                }
            }
            //player infront of tiles
            //player = new Sprite2D(new Vector2(100, 100), new Vector2(30, 40), "Players/Player Green/playerGreen_walk1", "Player");
            //player2 = new Sprite2D(new Vector2(100, 30), new Vector2(30, 40), "Players/Player Green/playerGreen_walk1", "Player2");
        }
        int times = 0;
        public override void OnUpdate()
        {
            --CameraPosition.X;
            if (player == null)
                return;            
            ++times;
            player.UpdatePosition();
            if(up)
            {
                //player.Position.Y -= 3f;
                if (up is true)
                {
                    player.SetVelocity(new Vector2(0, -150));
                    //player.AddImpulse(new Vector2(0,-2000), new Vector2(0,-2000));//new Vector2(0,-55));
                    //player.AddForce(new Vector2(0, -9000), Vector2.Zero());// new Vector2(0,-55));
                    //player.Position.Y -= 100f;
                    Log.Warning($"Camera X up | {CameraPosition.X}");
                    Log.Warning($"Player X up | {player.Position.X}");
                }
                else
                {
                    //player.SetVelocity(new Vector2(0, 0));
                }
            }
            if(down)
            {
                //player.Position.Y += 3f;
                //player.AddImpulse(new Vector2(0, -200), Vector2.Zero());
                //player.AddForce(new Vector2(0, -150000), Vector2.Zero());

                if (down is true)
                {
                    player.SetVelocity(new Vector2(0, 150));
                    Log.Warning($"Camera X down | {CameraPosition.X}");
                    Log.Warning($"Player X down | {player.Position.X}");
                }
                else
                {
                    player.SetVelocity(new Vector2(0, 0));
                }                    
            }
            if (left)
            {
                //player.Position.X -= 3f;
                //player.AddImpulse(new Vector2(-20000, 0), Vector2.Zero());
                //player.AddForce(new Vector2(-200000, 0), Vector2.Zero());
                if (left is true)
                {
                    player.AddForce(new Vector2(0, -9000), Vector2.Zero());// new Vector2(0,-55));
                    player.SetVelocity(new Vector2(-150,0));


                    Log.Warning($"Camera X left | {CameraPosition.X}");
                    Log.Warning($"Player X left | {player.Position.X}");
                }
                else
                {
                    player.SetVelocity(new Vector2(0, 0));
                }
            }
            if (right)
            {
                if (right is true)
                {
                    player.AddForce(new Vector2(0, -9000), Vector2.Zero());// new Vector2(0,-55));

                    //player.AddImpulse(new Vector2(20000, 0), Vector2.Zero());
                    player.SetVelocity(new Vector2(150, 0));
                     
                    if (player.Position.X > CameraPosition.X)
                    {
                        
                    }
                    else
                    {
                        player.Position.X = CameraPosition.X;
                    }


                    Log.Warning($"Camera X right | {CameraPosition.X}");
                        Log.Warning($"Player X right | {player.Position.X}");                    
                }
                else
                {
                    player.SetVelocity(new Vector2(0, 0));
                }
            }
            if (up is true && right is true)
            {
                //player.Position.Y -= 3f;
                if (up is true && right is true)
                {
                    player.SetVelocity(new Vector2(150, 0));
                    player.SetVelocity(new Vector2(0, -150));
                    //player.AddImpulse(new Vector2(0,-2000), new Vector2(0,-2000));//new Vector2(0,-55));
                    //player.AddForce(new Vector2(0, -9000), Vector2.Zero());// new Vector2(0,-55));
                    //player.Position.Y -= 100f;
                    Log.Warning($"Camera X up | {CameraPosition.X}");
                    Log.Warning($"Player X up | {player.Position.X}");
                }
                else
                {
                    //player.SetVelocity(new Vector2(0, 0));
                }
            }

            Sprite2D coin = player.IsColliding("Coin");
            if (coin != null)
            {
                //destroys coin
                coin.DestroySelf();
                ++coinsCollected;
                Log.Warning($"Coin collected! | {coinsCollected}");
            }
            //death block
            Sprite2D deathBlock = player.IsColliding("Death Block");
            if (deathBlock != null)
            {
                //kills player
                player.DestroySelf();
                //logs player death
                if (true)
                Log.Warning("Player has died!");
            }
            //if (player.IsColliding("Ground") != null)
            //{
            //    //Log.Info($"COLLIDING! {times}");
            //    //++times;
            //    //player.Position.X = lastPos.X;
            //    //player.Position.Y = lastPos.Y;
            //}
            //else

        }
        public override void GetKeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.W) { up = true; }
            if (e.KeyCode == Keys.S) { down = true; }
            if (e.KeyCode == Keys.A) { left = true; }
            if (e.KeyCode == Keys.D) { right = true; }
            if (true)            
            Log.Info("Pressing keys!");
        }
        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = false; }
            if (e.KeyCode == Keys.S) { down = false; }
            if (e.KeyCode == Keys.A) { left = false; }
            if (e.KeyCode == Keys.D) { right = false; }
            if(true)
            Log.Info("Key was released!");
        }
    }
}
