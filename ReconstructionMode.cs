using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _3D_Scanner_v2
{
    class ReconstructionMode : Mode
    {
        private bool isrunning = false;

        public event ImageChangeEventHandler ImageChanged;


        public ReconstructionMode()
        {

        }

        public void Start()
        {
            isrunning = true;
        }

        public void Stop()
        {
            isrunning = false;
        }

        public String ButtonRename()
        {
            if (isrunning)
            {
                return "Stop Reconstruction";
            }
            else return "Start Reconstruction";
        }

        public bool isRunning()
        {
            return isrunning;
        }

        public void Initialize()
        {

        }

        protected virtual void OnImageChanged(WriteableBitmap depthBitmap)
        {
            if (ImageChanged != null)
            {
                ImageChanged(this, depthBitmap);
            }
        }

        public void Finish()
        {
        }
    }
}
