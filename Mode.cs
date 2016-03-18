using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _3D_Scanner_v2
{
    public delegate void ImageChangeEventHandler(object source, WriteableBitmap depthBitmap);

    interface Mode
    {
        void Start();
        void Stop();
        void Finish();
        String ButtonRename();
        bool isRunning();
        void Initialize();

        //void Reader_FrameReady(object sender, FrameArrivedEventArgs e);

        event ImageChangeEventHandler ImageChanged;
    }
}
