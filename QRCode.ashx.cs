using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Service
{
    /// <summary>
    /// QRCode 的摘要说明
    /// </summary>
    public class QRCode : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                string text = context.Request["text"];				//二维码的内容
                string sizeStr = context.Request["size"];			//二维码尺寸
                string contentImg = context.Request["contentImg"];	//是否包含背景图

                if (string.IsNullOrEmpty(text))
                {
                    context.Response.Write("参数不正确");
                }
                else
                {
                    bool isContentImg = !string.IsNullOrEmpty(contentImg);
                    int size = int.TryParse(sizeStr, out int s) ? s : 160;
                    byte[] bts = QRCodeHasBackImg(text, size, isContentImg);

                    context.Response.ContentType = "image/jpeg";
                    context.Response.BinaryWrite(bts);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("图片生成失败." + ex.Message);
            }
        }

        //生成带背景图的二维码图片
        public static byte[] QRCodeHasBackImg(string text, int size, bool contentImg, string fg = "#000000", string bg = "#FFFFFF")
        {
            byte[] result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qr;
                if (encoder.TryEncode(text, out qr))
                {
                    //背景图存在 则生成带背景图的; 
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Images/poster2.jpg")) && contentImg)
                    {
                        Image img = Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/poster2.jpg"));
                        Graphics g = Graphics.FromImage(img);
                        g.DrawImage(img, 0, 0, 400, 600);
                        Bitmap image = null;
                        using (MemoryStream msQR = new MemoryStream())
                        {
                            GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), new SolidBrush(ColorTranslator.FromHtml(fg)), new SolidBrush(ColorTranslator.FromHtml(bg)));
                            renderer.WriteToStream(qr.Matrix, ImageFormat.Png, msQR);
                            var imageTemp = new Bitmap(msQR);
                            image = new Bitmap(imageTemp, new Size(new Point(size, size)));
                        }
                        g.DrawImage(image, 245, 74, image.Width, image.Height);
                        GC.Collect();
                        img.Save(ms, ImageFormat.Jpeg);
                        result = ms.GetBuffer();
                    }
                    else
                    {
						//若不存在 则直接生成二维码
                        GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), new SolidBrush(ColorTranslator.FromHtml(fg)), new SolidBrush(ColorTranslator.FromHtml(bg)));
                        renderer.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
                        var imageTemp = new Bitmap(ms);
                        var image = new Bitmap(imageTemp, new Size(new Point(size, size)));
                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            image.Save(ms2, ImageFormat.Jpeg);
                            result = ms2.GetBuffer();
                        }
                    }
                }
            }
            return result;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}