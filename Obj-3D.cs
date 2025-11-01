using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TemaEGC_04
{
    internal class Obj_3D
    {
        private bool visibility;
        private bool isGravityBound;
        private Color culoare;
        private List<Vector3> coordsL;

        private const int GRAVITY_OFFSET = 1;



        public Obj_3D() {
            RND randm = new RND();
            visibility = true;
            culoare = Color.Pink;
            isGravityBound = true;


            coordsL = new List<Vector3>();

            int size = randm.RandomInt(2,11);
            int height = randm.RandomInt(22, 31);
            int radius = randm.RandomInt(15, 26);

            coordsL.Add(new Vector3(0*size + radius, 0 * size + height, 1 * size + radius));
            coordsL.Add(new Vector3(0 * size+radius,0 * size + height, 0 * size + radius));
            coordsL.Add(new Vector3(1 * size + radius, 0 * size + height,1 * size + radius));
            coordsL.Add(new Vector3(1 * size + radius, 0 * size + height, 0 * size + radius));
            coordsL.Add(new Vector3(1 * size + radius, 1 * size + height, 1 * size + radius));
            coordsL.Add(new Vector3(1 * size + radius, 1 * size + height,0 * size + radius));
            coordsL.Add(new Vector3(0 * size + radius, 1 * size + height,1 * size + radius));
            coordsL.Add(new Vector3(0 * size + radius, 1 * size + height,0 * size + radius));
            coordsL.Add(new Vector3(0 * size + radius, 0 * size + height, 1 * size + radius));
            coordsL.Add(new Vector3(0 * size + radius, 0 * size + height, 0 * size + radius));
        }

        public void UpdatePossition()
        { if(visibility && isGravityBound && !GNDCollision())
            {
                for(int i = 0; i < coordsL.Count; i++)
                {
                    coordsL[i] = new Vector3(coordsL[i].X, coordsL[i].Y - GRAVITY_OFFSET, coordsL[i].Z);
                }
            }
        }
        public bool GNDCollision()
        {
            foreach (Vector3 v in coordsL)
            {
                if (v.Y <= 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void ToggleGravityBound()
        {
            isGravityBound = !isGravityBound;
        }

        public void SetGravity() {
            isGravityBound = true;
        }

        public void UnsetGravity()
        {
            isGravityBound = false;
        }

        public void Draw()
        {
            if (visibility)
            {
                GL.Color3(culoare);
                GL.Begin(PrimitiveType.QuadStrip);
                foreach (Vector3 v in coordsL)
                {
                    GL.Vertex3(v);
                }
                GL.End();
            }
        }
    }
}
