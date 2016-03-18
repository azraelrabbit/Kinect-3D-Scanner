using Microsoft.Kinect;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _3D_Scanner_v2
{
    [Serializable]
    class MyDepthFrameData
    {
        public MyDepthFrameData(DepthFrame frame)
        {
            Data = new ushort[frame.FrameDescription.LengthInPixels];
            frame.CopyFrameDataToArray(Data);
            MinDepth = frame.DepthMinReliableDistance;
            RelativeTime = frame.RelativeTime;
        }

        public static MyDepthFrameData FromStream(Stream source)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            return (MyDepthFrameData)serializer.Deserialize(source);
        }

        public void ToStream(Stream destination)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(destination, this);
        }

        public ushort[] Data { get; private set; }
        public ushort MinDepth { get; private set; }

        public TimeSpan RelativeTime { get; private set; }

    }
}
