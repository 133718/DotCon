using SgoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SgoApi.Clients
{
    public class DiaryClient : BaseClient
    {
        internal DiaryClient(User user) : base(user) { }

        public Task GetDays(DateTime startDate, DateTime endDate) 
        {
            using var request = new HttpRequestMessage(HttpMethod.Post,
                "webapi/student/diary?" +
                $"studentId={user.LogInfo.AccountInfo.User.Id}&" +
                $"weekEnd={endDate.ToString("yyyy-MM-dd")}&" +
                $"weekStart={startDate.ToString("yyyy-MM-dd")}&" +
                "yearId=197819");
            request.Headers.Add("at", user.LogInfo.AuthToken);

            return Task.CompletedTask;
        }
    }
}
