using System;
using Microsoft.SPOT;
using GHI.Glide.UI;

namespace GadgeteerTwitter
{
    class FrameTwitterDormido : FrameGHI
    {
        private Button btn_activar;
        private Button btn_resetear;
        private TextBlock txtCantidad;
        private Image imgDormido;

        public FrameTwitterDormido(Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolor)
            : base(multicolor, Resources.StringResources.FrameTwitterDormido)
        {
            this.imgDormido = (Image)window.GetChildByName("imgTwitterDormido");
            this.imgDormido.Bitmap = new Bitmap(Resources.GetBytes(Resources.BinaryResources.twitterDormido), Bitmap.BitmapImageType.Jpeg);
            this.txtCantidad = (TextBlock)window.GetChildByName("txtCantidad");
            this.btn_activar = (Button)window.GetChildByName("btnActivar");
            this.btn_activar.Enabled = true;
            this.btn_resetear = (Button)window.GetChildByName("btnResetear");
            this.btn_resetear.Enabled = true;
            this.ContadorTwit = 0;
        }

        public event GHI.Glide.OnTap BtnTapEvent
        {
            add
            {
                this.btn_activar.TapEvent += value;
            }
            remove
            {
                this.btn_activar.TapEvent -= value;
            }
        }

        public event GHI.Glide.OnTap BtnTapReset
        {
            add
            {
                this.btn_resetear.TapEvent += value;
            }
            remove
            {
                this.btn_resetear.TapEvent -= value;
            }
        }

        protected override void Parpadeo()
        {
            this.txtCantidad.Text = "Usted tiene " + ContadorTwit + " menciones en twits";
            this.multicolorLed.BlinkRepeatedly(Gadgeteer.Color.Blue);
        }

        public int ContadorTwit { get; set; }
    }
}
