using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Input;
using TemaEGC_04;
using System.Windows;


namespace TemaEGC_04
{

    class Window3D : GameWindow
    {
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;
        private MouseState previousMouseState;
        private MouseState currentMouseState;

        private Axes axes;
        private Grid grid;
        private Camera3DIsometric camera;

        private List<Obj_3D> objects3D;

        public Window3D() : base(800, 600, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;  // Activează sincronizarea verticală pentru o animație mai fluidă

            // Inițializarea obiectelor 3D
            axes = new Axes();     // Creează axele de coordonate (X, Y, Z)
            grid = new Grid();     // Creează grila 3D
            camera = new Camera3DIsometric();  // Creează camera cu vedere izometrică

            objects3D = new List<Obj_3D>(); // Lista pentru obiectele 3D
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Setează culoarea de fundal a ferestrei 3D (negru)
            GL.ClearColor(Color.Black);

            // Activează testul de adâncime pentru corecta suprapunere a obiectelor 3D
            GL.Enable(EnableCap.DepthTest);

            // Setează calitatea de redare la maxim
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            // Inițializează setările 3D
            SetUp3D();
        }

        // Metodă apelată la redimensionarea ferestrei
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
            SetUp3D();
        }

        // Configurează mediul 3D și proiecția
        private void SetUp3D()
        {
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Width / Height,
                1,
                1000);
            // Încarcă matricea de proiecție în OpenGL
            GL.LoadMatrix(ref perspective);

            // Setează poziția camerei
            camera.SetCamera();
        }

        // Metodă apelată la actualizarea cadrului (logică și input)
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // Salvează starea anterioară a tastaturii pentru detectarea apăsărilor
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();  // Obține starea curentă a tastaturii

            // Salvează starea anterioară a mouse-ului
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();  // Obține starea curentă a mouse-ului

            // Procesează input-ul de la utilizator
            HandleInput();
        }

        private void HandleInput()
        {
            // Miscarile camerei
            if (currentKeyboardState.IsKeyDown(Key.W))
                camera.MoveForward();
            if (currentKeyboardState.IsKeyDown(Key.S))
                camera.MoveBackward();
            if (currentKeyboardState.IsKeyDown(Key.A))
                camera.MoveLeft();
            if (currentKeyboardState.IsKeyDown(Key.D))
                camera.MoveRight();
            if (currentKeyboardState.IsKeyDown(Key.Q))
                camera.MoveUp();
            if (currentKeyboardState.IsKeyDown(Key.E))
                camera.MoveDown();

            // F1 pentru a vedea axele
            if (IsKeyPressed(Key.F1))
                axes.ToggleVisibility();

            // F2 pentru a vedea grid
            if (IsKeyPressed(Key.F2))
                grid.ToggleVisibility();
            //spawneaza obiecte
            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released)
            {
                objects3D.Add(new Obj_3D());
            }
            //curata obiectele spawnate
            if (Mouse.GetState().RightButton == ButtonState.Pressed &&
               previousMouseState.LeftButton == ButtonState.Released)
            {
                objects3D.Clear();
            }


            //schimba gravitatia
            if (currentKeyboardState.IsKeyDown(Key.G))
            {
                foreach(Obj_3D obj in objects3D)
                {
                    obj.UnsetGravity();
                }
            }


            foreach (Obj_3D obj in objects3D)
            {
                obj.UpdatePossition();
            }

            // Exit with Escape
            if (currentKeyboardState.IsKeyDown(Key.Escape))
                Exit();

        }

        private bool IsKeyPressed(Key key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            grid.Draw(); //Deseneaza grid

            axes.Draw(); //Deseneaza Axele


            
            foreach (Obj_3D obj in objects3D)
            {
                obj.Draw();
            }



            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F11)
            {
                if (WindowState == WindowState.Fullscreen)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Fullscreen;
            }
        }
    }


    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("       SPATIU 3D CU GRILA - CONTROALE");
            Console.WriteLine("=========================================");
            Console.WriteLine();
            Console.WriteLine("MIȘCAREA CAMEREI:");
            Console.WriteLine("  W          - Înainte");
            Console.WriteLine("  S          - Înapoi");
            Console.WriteLine("  A          - Stânga");
            Console.WriteLine("  D          - Dreapta");
            Console.WriteLine("  Q          - Sus");
            Console.WriteLine("  E          - Jos");
            Console.WriteLine();
            Console.WriteLine("VIZIBILITATE:");
            Console.WriteLine("  F1         - Arată/ascunde axele");
            Console.WriteLine("  F2         - Arată/ascunde grila");
            Console.WriteLine();
            Console.WriteLine("CONTROALE FEREASTRA:");
            Console.WriteLine("  F11        - Comutare ecran complet");
            Console.WriteLine("  ESC        - Ieșire aplicație");
            Console.WriteLine();
            Console.WriteLine("=========================================");
            Console.WriteLine();

            // Pornește direct aplicația 3D
            using (Window3D window = new Window3D())
            {
                window.Title = "Spațiu 3D cu Grile - EGC Tema 04";
                window.WindowBorder = WindowBorder.Fixed;
                window.Size = new Size(1024, 768);

                window.Run(60.0);
            }
        }
    }
}

