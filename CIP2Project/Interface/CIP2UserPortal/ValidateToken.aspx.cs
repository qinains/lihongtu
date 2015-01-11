using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text; 


using Linkage.BestTone.Interface.Utility;

public partial class UI_ValidateToken : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CAO PSA OUR");
        GenerateBitmap();
    }


    public static string RandNum(int n)
    {
        char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        StringBuilder num = new StringBuilder();
        Random rnd = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < n; i++)
        {
            num.Append(arrChar[rnd.Next(0, 9)].ToString());
        }
        return num.ToString();
    }


    private void GenerateBitmap()
    {
        //生成图片
        const int width = 70, height = 18;
        Bitmap aBmp = new Bitmap(width, height);
        Graphics aGrph = Graphics.FromImage(aBmp);
        aGrph.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), 0, 0, width, height);//255,153,0

        //给生成的图片添加随即噪点
        int randNum1, randNum2;

        for (int i = 0; i < 200; i++)
        {
            randNum1 = new Random(i * DateTime.Now.Millisecond * 1000).Next(width);
            randNum2 = new Random(i * DateTime.Now.Millisecond * 100).Next(height);

            aBmp.SetPixel(randNum1, randNum2, Color.Gray);
        }
        string adPitch = RandNum(4);

        #region 隐藏

        ////根据客户端随机数对验证码进行处理，同一个随机数对于不同的访问，所对应的验证码不同
        //Random rd = new Random();
        //double par = double.Parse(rd.NextDouble().ToString()) * DateTime.Now.Millisecond;
        //string adPitch = CryptographyUtil.Encrypt(par.ToString());

        //adPitch = adPitch.Replace("/", "");
        //adPitch = adPitch.Replace("+", "");
        //adPitch = adPitch.Replace("=", "");

        //adPitch = adPitch.Substring(0, 4);

        //adPitch = adPitch.ToUpper();

        ////过虑0为O,I和1,J、Q、U、V
        //adPitch = adPitch.Replace("0", "3");
        //adPitch = adPitch.Replace("1", "4");
        //adPitch = adPitch.Replace("G", "B");
        //adPitch = adPitch.Replace("I", "K");
        //adPitch = adPitch.Replace("O", "P");
        //adPitch = adPitch.Replace("Q", "M");
        //adPitch = adPitch.Replace("J", "N");
        //adPitch = adPitch.Replace("U", "X");
        //adPitch = adPitch.Replace("V", "Y");

        #endregion

        //输出验证码到图片
        Font fontBanner = new Font("Arial", 14, FontStyle.Bold);
        StringFormat stringFormat = new StringFormat();
        stringFormat.Alignment = StringAlignment.Center;
        stringFormat.LineAlignment = StringAlignment.Center;

        aGrph.DrawString(adPitch, fontBanner, new SolidBrush(Color.FromArgb(0, 0, 0)),
            new Rectangle(0, 0, width, height), stringFormat);
        aBmp.Save(Response.OutputStream, ImageFormat.Jpeg);
        aGrph.Dispose();
        aBmp.Dispose();

        HttpCookie pitchCookie = new HttpCookie("PASSPORT_USER_VALIDATOR");

        pitchCookie.Values["ValidatorStr"] = CryptographyUtil.Encrypt(adPitch);

        pitchCookie.Values["ExpireTime"] = CryptographyUtil.Encrypt(DateTime.Now.AddMinutes(CommonUtility.ValidatorAvailableMinute).ToString());

        Response.Cookies.Add(pitchCookie);
    }
}
