using SgoApi.Diary;
using SgoApi.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SgoApi.Clients
{
    public class DiaryClient : BaseClient
    {
        internal DiaryClient(User user) : base(user) { }

        public async Task<DayCollection> GetDays(DateTime startDate, DateTime endDate) 
        {
            using var request = new HttpRequestMessage(HttpMethod.Get,
                "webapi/student/diary?" +
                $"studentId={user.LogInfo.AccountInfo.User.Id}&" +
                $"weekEnd={endDate:yyyy-MM-dd}&" +
                $"weekStart={startDate:yyyy-MM-dd}&" +
                "yearId=197819");
            request.Headers.Add("at", user.LogInfo.AuthToken);
            return JsonSerializer.Deserialize<DayCollection>(await SendHttpRequestAsync(request));
        }

        public async Task<DayCollection> GetDay(DateTime date) => await GetDays(date, date);
    }
}
