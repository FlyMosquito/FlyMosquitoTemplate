#region using
using FlyMosquito.Common;
#endregion

namespace FlyMosquito.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //测试接口
            Console.WriteLine("Hello, World!");
            RequestHelper RequestHelper = new RequestHelper();
            string StringBaseUrl = "http://120.79.185.223:8090";
            string ApiUrl = "/Login/LoginUserAsync?Uid=UID613512&PassWord=123456";
            var StringResponse = await RequestHelper.PostAsync($"{StringBaseUrl}{ApiUrl}");
            Console.WriteLine($"{StringResponse}\n");

            var Dictionary = new Dictionary<string, string>();
            var StringValue = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJVSUQ2MTM1MTIiLCJuYW1lIjoiQWRtaW4iLCJqdGkiOiI0OWIyNmIwOS0yMmVjLTRjYzYtOGZkZi0wYzcwNGU3NmRlODAiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiLnrqHnkIblkZgiLCJleHAiOjE3MzUxNTA3MzgsImlzcyI6IldlYkFwcElzc3VlciIsImF1ZCI6IldlYkFwcEF1ZGllbmNlIn0.acxk8NWwbTBGonNcmjweBv-_QL9ICZunrZ9-RGR0wU8";
            Dictionary.Add("Authorization", StringValue);
            string ApiUrl1 = "/ApiAuth/GetApiAuthAsync";
            var StringResponse1 = await RequestHelper.GetAsync($"{StringBaseUrl}{ApiUrl1}", Dictionary);
            Console.WriteLine($"{StringResponse1}\n");
            Console.ReadKey();
        }
    }
}
