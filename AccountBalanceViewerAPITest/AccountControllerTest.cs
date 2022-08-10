using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Controllers;
using WebApplication1.Data;

namespace AccountBalanceViewerAPITest
{
    [TestClass]
    public class AccountControllerTest
    {
        readonly Dictionary<string, string> config = new Dictionary<string, string>();
        readonly IConfiguration configuration;
        readonly ApplicationContext context;
        public AccountControllerTest()
        {
            config = new Dictionary<string, string> { };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "AccountBalanceViewer").Options;
            context = new ApplicationContext(options);
        }

        [TestMethod]
        public async Task UploadData_ShouldReturnOkAsync()
        {
            var controller = new AccountController(context, configuration);

            List<UploadDataDto> request = new List<UploadDataDto>();
            UploadDataDto data = new UploadDataDto();
            data.DataColumns = new List<string>();
            data.DataColumns.Add("R&D");
            data.DataColumns.Add("5.63");
            request.Add(data);

            var result = await controller.UploadData(request);

            Assert.AreEqual(result.ToString(), "Microsoft.AspNetCore.Mvc.OkResult");
        }

        [TestMethod]
        public async Task GetAccountBalances_ShouldReturnMonthBalanceAsync()
        {
            var controller = new AccountController(context, configuration);

            List<UploadDataDto> request = new List<UploadDataDto>();
            UploadDataDto data1 = new UploadDataDto();
            data1.DataColumns = new List<string>();
            data1.DataColumns.Add("R&D");
            data1.DataColumns.Add("500");
            request.Add(data1);

            UploadDataDto data2 = new UploadDataDto();
            data2.DataColumns = new List<string>();
            data2.DataColumns.Add("Canteen");
            data2.DataColumns.Add("600");
            request.Add(data2);

            UploadDataDto data3 = new UploadDataDto();
            data3.DataColumns = new List<string>();
            data3.DataColumns.Add("CEO’s car");
            data3.DataColumns.Add("1000");
            request.Add(data3);

            UploadDataDto data4 = new UploadDataDto();
            data4.DataColumns = new List<string>();
            data4.DataColumns.Add("Marketing");
            data4.DataColumns.Add("400");
            request.Add(data4);

            UploadDataDto data5 = new UploadDataDto();
            data5.DataColumns = new List<string>();
            data5.DataColumns.Add("Parking fines");
            data5.DataColumns.Add("900");
            request.Add(data5);

            var upload = await controller.UploadData(request);
            var result = await controller.GetAccountBalances();

            Assert.AreEqual(result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }

        [TestMethod]
        public async Task GetReport_ShouldReturnAnnualBalanceAsync()
        {
            var controller = new AccountController(context, configuration);

            List<UploadDataDto> request = new List<UploadDataDto>();
            UploadDataDto data1 = new UploadDataDto();
            data1.DataColumns = new List<string>();
            data1.DataColumns.Add("R&D");
            data1.DataColumns.Add("500");
            request.Add(data1);

            UploadDataDto data2 = new UploadDataDto();
            data2.DataColumns = new List<string>();
            data2.DataColumns.Add("Canteen");
            data2.DataColumns.Add("600");
            request.Add(data2);

            UploadDataDto data3 = new UploadDataDto();
            data3.DataColumns = new List<string>();
            data3.DataColumns.Add("CEO’s car");
            data3.DataColumns.Add("1000");
            request.Add(data3);

            UploadDataDto data4 = new UploadDataDto();
            data4.DataColumns = new List<string>();
            data4.DataColumns.Add("Marketing");
            data4.DataColumns.Add("400");
            request.Add(data4);

            UploadDataDto data5 = new UploadDataDto();
            data5.DataColumns = new List<string>();
            data5.DataColumns.Add("Parking fines");
            data5.DataColumns.Add("900");
            request.Add(data5);

            var upload = await controller.UploadData(request);
            var result = await controller.GetReport();

            Assert.AreEqual(result.ToString(), "Microsoft.AspNetCore.Mvc.OkObjectResult");
        }
    }
}