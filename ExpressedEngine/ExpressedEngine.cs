using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace ExpressedEngine.ExpressedEngine
{

    class Canvas : Form
    {
        public Canvas()
        {
            this.DoubleBuffered = true;
        }
    }
    public abstract class ExpressedEngine
    {
        private Vector2 ScreenSize = new Vector2 (512,512);
        private string Title = "New Game";
        private Canvas Window = null;
        private Thread GameLoopThread = null;




        public static List<Shape2D> AllShapes = new List<Shape2D> ();
        public static List<Sprite2D> AllSprites = new List<Sprite2D> ();


        public System.Drawing.Color BackGroundColor = System.Drawing.Color.Beige;
        
        public Vector2 CameraZoom = new Vector2(1,1);
        public Vector2 CameraPosition = Vector2.Zero();
        public float CameraAngle = 0f;

        // Define the size of the world. Simulation will still work
        // if bodies reach the end of the world, but it will be slower.
        AABB worldAABB = new AABB
        {
            UpperBound = new Vec2(2000,2000),
            LowerBound = new Vec2(-2000, -2000)
        };

        // Define the gravity vector.
        
        Vec2 gravity = new Vec2(0.0f, 2000.0f);
        public static World world = null;
        // Construct a world object, which will hold and simulate the rigid bodies.

        public ExpressedEngine(Vector2 SceenSize,string Title)
        {
            Log.Info("Game is Starting");
            this.ScreenSize = SceenSize;
            this.Title = Title;

            Window = new Canvas();
            Window.Size = new Size((int)SceenSize.X, (int)SceenSize.Y);
            Window.Text = this.Title;
            Window.Paint += Renderer;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp += Window_KeyUp;
            Window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Window.FormClosing += Window_FormClosing;
            GameLoopThread = new Thread(GameLoop);
            GameLoopThread.Start();

            world = new World(worldAABB, gravity, false);


            Application.Run(Window);
            
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameLoopThread.Abort();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            GetKeyUp(e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            GetKeyDown(e);
        }

        public static void RegisterShape(Shape2D shape)
        {
            AllShapes.Add(shape);
        }
        public static void UnRegisterShape(Shape2D shape)
        {
            AllShapes.Remove(shape);
        }
        public static void RegisterSprite(Sprite2D sprite)
        {
            AllSprites.Add(sprite);
        }
        public static void UnRegisterSprite(Sprite2D sprite)
        {
            AllSprites.Remove(sprite);
        }
        // Prepare for simulation. Typically we use a time step of 1/60 of a
        // second (60Hz) and 10 iterations. This provides a high quality simulation
        // in most game scenarios.
        float timeStep = 1.0f / 60.0f;
        int velocityIterations = 10;
        int positionIterations = 3;
        void GameLoop()
        {

            OnLoad();
            while (GameLoopThread.IsAlive)
            {
                try
                {
                    OnDraw();
                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                    world.Step(timeStep, velocityIterations, positionIterations);
                    OnUpdate();
                    Thread.Sleep(1);
                }
                catch
                {
                    Log.Error("Window has not been found... Waiting...");
                }
            }
        }

        private void Renderer(object sender, PaintEventArgs e)
        {

            // Instruct the world to perform a single step of simulation. It is
            // generally best to keep the time step and iterations fixed.
            

            Graphics g = e.Graphics;

            g.Clear(BackGroundColor);

            g.TranslateTransform(CameraPosition.X, CameraPosition.Y);
            g.RotateTransform(CameraAngle);
            g.ScaleTransform(CameraZoom.X,CameraZoom.Y);
            try
            {
                foreach (Shape2D shape in AllShapes)
                {
                    g.FillRectangle(new SolidBrush(System.Drawing.Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);
                }
                foreach (Sprite2D sprite in AllSprites)
                {
                    if (!sprite.IsReference)
                    {
                        g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);
                    }
                }
            }
            catch
            {

            }
        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void GetKeyDown(KeyEventArgs e);
        public abstract void GetKeyUp(KeyEventArgs e);
    }
}
