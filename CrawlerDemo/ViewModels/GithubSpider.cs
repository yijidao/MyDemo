using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotnetSpider;
using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.Http;
using DotnetSpider.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CrawlerDemo.ViewModels
{
    class GithubSpider : Spider
    {
        public GithubSpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
        {
        }

        protected override async Task InitializeAsync(CancellationToken stoppingToken = new CancellationToken())
        {
            AddDataFlow(new Parser());
            AddDataFlow(new ConsoleStorage());
            await AddRequestsAsync(new Request("https://github.com/yijidao")
            {
                Timeout = 10000
            });
        }

        protected override SpiderId GenerateSpiderId() => new(ObjectId.CreateId().ToString(), "Github");
    }

    class Parser : DataParser
    {
        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {
            var selectable = context.Selectable;
            var author = selectable.XPath("//span[@class='p-name vcard-fullname d-block overflow-hidden']")?.Value;
            var name = selectable.XPath("//span[@class='p-nickname vcard-username d-block']");
            context.AddData("author", author);
            context.AddData("name", name);
            return Task.CompletedTask;
        }
    }

    class DebugConsoleStorage : DataFlowBase
    {
        public override Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public override Task HandleAsync(DataFlowContext context)
        {
            throw new NotImplementedException();
        }
    }
}
