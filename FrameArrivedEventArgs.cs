using System;

namespace _3D_Scanner_v2
{
    class FrameArrivedEventArgs : EventArgs
    {
        public MyDepthFrameData DepthFrame { get; private set; }

        public FrameArrivedEventArgs(MyDepthFrameData depthFrame)
        {
            DepthFrame = depthFrame;
        }
    }
}