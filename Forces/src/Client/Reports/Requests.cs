using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Forces.Client.Reports
{
    public partial class Requests : DevExpress.XtraReports.UI.XtraReport
    {
        public Requests()
        {
            InitializeComponent();
        }
        public string ConvertToEasternArabicNumerals(string input)
        {
            System.Text.UTF8Encoding utf8Encoder = new UTF8Encoding();
            System.Text.Decoder utf8Decoder = utf8Encoder.GetDecoder();
            System.Text.StringBuilder convertedChars = new System.Text.StringBuilder();
            char[] convertedChar = new char[1];
            byte[] bytes = new byte[] { 217, 160 };
            char[] inputCharArray = input.ToCharArray();
            foreach (char c in inputCharArray)
            {
                if (char.IsDigit(c))
                {
                    bytes[1] = Convert.ToByte(160 + char.GetNumericValue(c));
                    utf8Decoder.GetChars(bytes, 0, 2, convertedChar, 0);
                    convertedChars.Append(convertedChar[0]);
                }
                else
                {
                    convertedChars.Append(c);
                }
            }
            return convertedChars.ToString();
        }
        public string ConvertToArabic(string input) 
        {
            var source = input;

            var westernLatin = Encoding.GetEncoding(1252);
            var arabic = Encoding.GetEncoding("utf-8");

            var bytes = arabic.GetBytes(source);
            return arabic.GetString(bytes);
        }

        private void Requests_BeforePrint(object sender, CancelEventArgs e)
        {
           
        }

        private void topMarginBand1_BeforePrint(object sender, CancelEventArgs e)
        {
            
         //  xrLabel1.Text = ConvertToArabic("<h1>MOHAMMED</h1>");
        }
    }
}
