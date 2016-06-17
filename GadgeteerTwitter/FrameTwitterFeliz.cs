using System;
using Microsoft.SPOT;
using GHI.Glide.UI;
using System.Threading;

namespace GadgeteerTwitter
{
    class FrameTwitterFeliz : FrameGHI
    {
        private Button btn_dormir;
        private Image imgFeliz;

        public FrameTwitterFeliz(Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolor)
            : base(multicolor, Resources.StringResources.FrameTwitterFeliz)
        {
            imgFeliz = (Image)window.GetChildByName("imgTwitterFeliz");
            imgFeliz.Bitmap = new Bitmap(Resources.GetBytes(Resources.BinaryResources.twitterFeliz), Bitmap.BitmapImageType.Jpeg);
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
            this.multicolorLed.BlinkRepeatedly(Gadgeteer.Color.Blue, new TimeSpan(0, 0, 1), Gadgeteer.Color.Green, new TimeSpan(0, 0, 1));
        }
    }
}
