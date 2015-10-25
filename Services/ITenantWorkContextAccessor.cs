using Orchard;
using Orchard.Environment;
using Orchard.Environment.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Orchard.Environment.ShellBuilders;
using Orchard.MultiTenancy.Extensions;
using System.Web.Mvc;

namespace MainBit.MultiTenancy.Services
{

    // http://orcharddojo.net/blog/advanced-orchard-accessing-other-tenants-services
    // http://dotnest.com/knowledge-base/topics/lombiq-hosting-suite

    public interface ITenantWorkContextAccessor : IDependency
    {
        WorkContext GetContext(string tenantName);
    }

    public static class ITenantWorkContextAccessorEstensions {
        public static WorkContext GetDefaultTenantContext(this ITenantWorkContextAccessor tenantWorkContextAccessor) {
            return tenantWorkContextAccessor.GetContext(ShellSettings.DefaultName);
        }
    }

    public class TenantWorkContextAccessor : ITenantWorkContextAccessor, IWorkContextEvents
    {
        // ShellSettingsManager lets you access the shell settings of all the tenants.
        private readonly IShellSettingsManager _shellSettingsManager;
        // OrchardHost is the very core Orchard service running the environment.
        private readonly IOrchardHost _orchardHost;
        private readonly IShellContextFactory _shellContextFactory;
        private readonly IDictionary<string, IWorkContextScope> TenantWorkContextScopes = new Dictionary<string, IWorkContextScope>();
        private readonly UrlHelper _urlHelper;
        private readonly ShellSettings _shellSettings;
        private readonly IWorkContextAccessor _wca;

        public TenantWorkContextAccessor(
            IShellSettingsManager shellSettingsManager,
            IOrchardHost orchardHost,
            IShellContextFactory shellContextFactory,
            ShellSettings shellSettings,
            IWorkContextAccessor wca
           // UrlHelper urlHelper
            )
        {
            _shellSettingsManager = shellSettingsManager;
            _orchardHost = orchardHost;
            _shellContextFactory = shellContextFactory;
            _shellSettings = shellSettings;
            _wca = wca;
            //_urlHelper = urlHelper;
        }

        public WorkContext GetContext(string tenantName)
        {
            if(_shellSettings.Name == tenantName) {
                return _wca.GetContext();
            }

            if(!TenantWorkContextScopes.ContainsKey(tenantName)) {
                var tenantShellSettings = _shellSettingsManager.LoadSettings().Where(settings => settings.Name == tenantName).Single();
                var shellContext = _orchardHost.GetShellContext(tenantShellSettings);

                //var tenantBaseUrl = _urlHelper.Tenant(tenantShellSettings);
                //var httpContextBase = new Orchard.Mvc.MvcModule.HttpContextPlaceholder(tenantBaseUrl);
                //var httpContext = shellContext.LifetimeScope.Resolve<HttpContextBase>();
                //context.Resolve<IWorkContextAccessor>().CreateWorkContextScope(httpContextBase);
                //return httpContextBase;

                //shellContext.LifetimeScope.Resolve<IHttpContextAcсessor>()
                TenantWorkContextScopes[tenantName] = shellContext.LifetimeScope.Resolve<IWorkContextAccessor>().CreateWorkContextScope();
            }
            return TenantWorkContextScopes[tenantName].Resolve<WorkContext>();
        }

        ~TenantWorkContextAccessor()
        {
            
        }

        public void Started()
        {
            
        }

        public void Finished()
        {
            foreach (var workContextScope in TenantWorkContextScopes)
            {
                workContextScope.Value.Dispose();
            }
            TenantWorkContextScopes.Clear();
        }
    }
}