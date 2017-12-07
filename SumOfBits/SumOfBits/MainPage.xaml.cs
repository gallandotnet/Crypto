
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
namespace SumOfBits
{
    public partial class MainPage : ContentPage
    {

        private const string EtherUrl = "https://api.independentreserve.com/Public/GetMarketSummary?primaryCurrencyCode=eth&secondaryCurrencyCode=aud";
        public MainPage()
        {
            InitializeComponent();


            Refresh.Clicked += (sender, args) =>
            {
                try
                {
                    GetBitCoinPrices();
                    GetEtheruemPrices();
                }
                catch (Exception ex)
                {

                }

             
            };
            try
            {
                GetBitCoinPrices();
                GetEtheruemPrices();
            }
            catch (Exception ex)
            {

            }
          

        }
        private decimal CalcPercentage(decimal startPrice, decimal endPrice)
        {
            var amt = endPrice - startPrice;
            return (amt / startPrice) * 100 ;

        }
        private void GetEtheruemPrices()
        {
            string url = "https://api.independentreserve.com/Public/GetMarketSummary?primaryCurrencyCode=eth&secondaryCurrencyCode=aud";
            IsBusy = true;
            PriceInfo prices = new PriceInfo();
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    EthereumLabel.Text = string.Format("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        //  Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {

                        prices = JsonConvert.DeserializeObject<PriceInfo>(content);
                        // Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }

                }

                EthereumLabel.Text = string.Format("{0:c}",prices.LastPrice);
                EthereumLowestDayLabel.Text = string.Format("{0:c}", prices.DayLowestPrice);
                UpOrDownEtheruem.Text = "%" + CalcPercentage(prices.DayLowestPrice,prices.LastPrice).ToString("0.##");

            }
        }
        private void GetBitCoinPrices()
        {
            string url = "https://api.independentreserve.com/Public/GetMarketSummary?primaryCurrencyCode=xbt&secondaryCurrencyCode=aud";
            IsBusy = true;
            PriceInfo prices = new PriceInfo();
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            
            // Send the request to the server and wait for the response:
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    BitcoinLabel.Text = string.Format("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        //  Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {

                         prices = JsonConvert.DeserializeObject<PriceInfo>(content);
                        // Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }
                    
                }

               
                BitcoinLabel.Text = string.Format("{0:c}", prices.LastPrice);
                BitcoinLowestDayLabel.Text = string.Format("{0:c}", prices.DayLowestPrice);
                UpOrDownBitCoin.Text = "%" + CalcPercentage(prices.DayLowestPrice, prices.LastPrice).ToString("0.##");
                IsBusy = false;
            }
        }

        public class PriceInfo
        {
            public decimal DayHighestPrice { get; set; }
            public decimal DayLowestPrice { get; set; }
            public decimal DayAvgPrice { get; set; }
            public decimal DayVolumeXbt { get; set; }
            public decimal DayVolumeXbtInSecondaryCurrrency { get; set; }
            public decimal CurrentLowestOfferPrice { get; set; }
            public decimal CurrentHighestBidPrice { get; set; }
            public decimal LastPrice { get; set; }
            public string PrimaryCurrencyCode { get; set; }
            public string SecondaryCurrencyCode { get; set; }
            public DateTime CreatedTimestampUtc { get; set; }
        }

    }
}

