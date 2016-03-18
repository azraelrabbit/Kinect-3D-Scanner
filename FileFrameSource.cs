using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

namespace _3D_Scanner_v2
{
    class FileFrameSource
    {
        public event EventHandler<FrameArrivedEventArgs> FrameArrived = delegate { };

        private readonly Stream sourceStream;
        private bool running;

        public FileFrameSource(Stream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public void Start()
        {
            if (running)
                return;

            running = true;
            new Thread(() =>
            {
                while (running)
                {
                    try
                    {
                        var frame = MyDepthFrameData.FromStream(sourceStream);
                        FrameArrived(null, new FrameArrivedEventArgs(frame));
                    }
                    catch (SerializationException)
                    {
                        break;
                    }
                }
            }) {IsBackground = true}.Start();
        }

        public void Stop()
        {
            running = false;
        }

    }
}
