using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Security.Policy;
using System.Text;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class NugetDetailsViewModel : ReactiveObject
    {
        private readonly IPackageSearchMetadata _metadata;
        private readonly Uri _defaultUri;

        public NugetDetailsViewModel(IPackageSearchMetadata metadata)
        {
            _metadata = metadata;
            _defaultUri = new Uri("https://git.io/fAlfh");
            OpenPage = ReactiveCommand.Create(() =>
            {
                Process.Start(new ProcessStartInfo(this.ProjectUrl.ToString())
                {
                    UseShellExecute = true
                });
            });
        }

        public Uri IconUri => _metadata.IconUrl ?? _defaultUri;
        public string Description => _metadata.Description;
        public Uri ProjectUrl => _metadata.ProjectUrl;
        public string Title => _metadata.Title;

        public ReactiveCommand<Unit, Unit> OpenPage { get; }
    }
}
