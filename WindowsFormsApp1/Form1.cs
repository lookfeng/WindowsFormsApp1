using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //save a gif

            // PropertyItem for the frame delay (apparently, no other way to create a fresh instance).
            var frameDelay = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            frameDelay.Id = 0x5100; // PropertyTagFrameDelay;
            frameDelay.Type = 4; // PropertyTagTypeLong;
            // Length of the value in bytes.
            frameDelay.Len = 6 * 4;
            // The value is an array of 4-byte entries: one per frame.
            // Every entry is the frame delay in 1/100-s of a second, in little endian.
            frameDelay.Value = new byte[6 * 4];
            // E.g., here, we're setting the delay of every frame to 1 second.
            var frameDelayBytes = BitConverter.GetBytes((uint)100);
            for (int j = 0; j < 6; ++j)
                Array.Copy(frameDelayBytes, 0, frameDelay.Value, j * 4, 4);

            // PropertyItem for the number of animation loops.
            var loopPropertyItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            loopPropertyItem.Id = 0x5101; // PropertyTagLoopCount;
            loopPropertyItem.Type = 3; // PropertyTagTypeShort;
            loopPropertyItem.Len = 1;
            // 0 means to animate forever.
            loopPropertyItem.Value = BitConverter.GetBytes((ushort)0);

            Image img = Image.FromFile(@"E:\Pictures\图片\PNG\AC Cesena.png");
            img.SetPropertyItem(frameDelay);
            img.SetPropertyItem(loopPropertyItem);
            EncoderParameters p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            img.Save(@"C:\Users\lkf\Desktop\test.gif", FindEncoder(ImageFormat.Gif), p);
            p.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionTime);
            img.SaveAdd(Image.FromFile(@"E:\Pictures\图片\PNG\AC Milan.png"), p);
            img.SaveAdd(Image.FromFile(@"E:\Pictures\图片\PNG\AC Parma.png"), p);
            img.SaveAdd(Image.FromFile(@"E:\Pictures\图片\PNG\AS Roma.png"), p);
            img.SaveAdd(Image.FromFile(@"E:\Pictures\图片\PNG\Atalanta.png"), p);
            img.SaveAdd(Image.FromFile(@"E:\Pictures\图片\PNG\Bologna.png"), p);
            p.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush);
            img.SaveAdd(p);

            //img = Image.FromFile(@"C:\Users\lkf\Desktop\test.gif"); //must re read it
            pictureBox1.Image = img;
        }

        static ImageCodecInfo FindEncoder(ImageFormat format)
        {
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
            {
                if (imageCodecInfo.FormatID.Equals(format.Guid))
                {
                    return imageCodecInfo;
                }
            }
            return null;
        }
    }    
}
