using System;
using Microsoft.SPOT;
using GHI.Glide.UI;

namespace GadgeteerTwitter
{
    class FrameTwitterTriste : FrameGHI
    {
        private Button btn_dormir;
        private Image imgTriste;

        public FrameTwitterTriste(Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolor)
            : base(multicolor, Resources.StringResources.FrameTwitterTriste)
        {
            imgTriste = (Image)window.GetChildByName("imgTwitterTriste");
            imgTriste.Bitmap = new Bitmap(Resources.GetBytes(Resources.BinaryResources.twitterTriste), Bitmap.BitmapImageType.Jpeg);
            btn_dormir = (Button)window.GetChildByName("btnDormir");
            btn_dormir.Enabled = true;
        }

        public event GHI.Glide.OnTap BtnEvent
        {
            add
            {
                this.btn_dormir.TapEvent += value;
            }
            remove
            {
                this.btn_dormir.TapEvent -= value;
            }
        }

        protected override void Parpadeo()
        {
            this.multicolorLed.BlinkRepeatedly(Gadgeteer.Color.Purple);
        }
    }
}
