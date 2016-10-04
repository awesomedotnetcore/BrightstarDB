﻿using System.Security.Claims;
using System.Xml;
using BrightstarDB.Server.Modules.Configuration;
using BrightstarDB.Server.Modules.Permissions;
using Moq;
using Nancy.Security;
using NUnit.Framework;

namespace BrightstarDB.Server.Modules.Tests
{
    [TestFixture]
    public class ConfigurationSectionHandlerSpec
    {
        private const string SimpleConfiguration = @"
<brightstarService connectionString='type=embedded;storesDirectory=c:\brightstar\'> 
    <storePermissions>
        <fallback authenticated='All' anonymous='Read'/>
    </storePermissions>
    <systemPermissions>
        <fallback authenticated='All' anonymous='ListStores'/>
    </systemPermissions>
</brightstarService>";

        private const string FallbackDefaultsConfiguration = @"
<brightstarService connectionString='type=embedded;storesDirectory=c:\brightstar\'> 
    <storePermissions>
        <fallback authenticated='Read,Export'/>
    </storePermissions>
    <systemPermissions>
        <fallback authenticated='ListStores'/>
    </systemPermissions>
</brightstarService>";

        [Test]
        public void TestSimpleConfigurationSection()
        {
            var xml = new XmlDocument();
            xml.LoadXml(SimpleConfiguration);
            var handler = new BrightstarServiceConfigurationSectionHandler();
            var config = handler.Create(null, null, xml.DocumentElement) as BrightstarServiceConfiguration;
            var mockUser =
                new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "alice")}, "Mock Authenticated"));

            Assert.That(config, Is.Not.Null);
            Assert.That(config.ConnectionString, Is.Not.Null);
            Assert.That(config.ConnectionString, Is.EqualTo("type=embedded;storesDirectory=c:\\brightstar\\"));
            Assert.That(config.StorePermissionsProvider, Is.Not.Null);
            Assert.That(config.StorePermissionsProvider, Is.InstanceOf<FallbackStorePermissionsProvider>());
            Assert.That(config.StorePermissionsProvider.GetStorePermissions(null, "foo"), Is.EqualTo(StorePermissions.Read));
            Assert.That(config.StorePermissionsProvider.GetStorePermissions(mockUser, "foo"), Is.EqualTo(StorePermissions.All));
            Assert.That(config.SystemPermissionsProvider, Is.Not.Null);
            Assert.That(config.SystemPermissionsProvider, Is.InstanceOf<FallbackSystemPermissionsProvider>());
            Assert.That(config.SystemPermissionsProvider.GetPermissionsForUser(null), Is.EqualTo(SystemPermissions.ListStores));
            Assert.That(config.SystemPermissionsProvider.GetPermissionsForUser(mockUser), Is.EqualTo(SystemPermissions.All));
        }

        [Test]
        public void TestFallbackDefaults()
        {
            var xml = new XmlDocument();
            xml.LoadXml(FallbackDefaultsConfiguration);
            var handler = new BrightstarServiceConfigurationSectionHandler();
            var config = handler.Create(null, null, xml.DocumentElement) as BrightstarServiceConfiguration;
            var mockUser =
                new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "alice") }, "Mock Authenticated"));

            Assert.That(config, Is.Not.Null);
            Assert.That(config.ConnectionString, Is.Not.Null);
            Assert.That(config.ConnectionString, Is.EqualTo("type=embedded;storesDirectory=c:\\brightstar\\"));
            Assert.That(config.StorePermissionsProvider, Is.Not.Null);
            Assert.That(config.StorePermissionsProvider, Is.InstanceOf<FallbackStorePermissionsProvider>());
            Assert.That(config.StorePermissionsProvider.GetStorePermissions(null, "foo"), Is.EqualTo(StorePermissions.None));
            Assert.That(config.StorePermissionsProvider.GetStorePermissions(mockUser, "foo"), Is.EqualTo(StorePermissions.Read|StorePermissions.Export));
            Assert.That(config.SystemPermissionsProvider, Is.Not.Null);
            Assert.That(config.SystemPermissionsProvider, Is.InstanceOf<FallbackSystemPermissionsProvider>());
            Assert.That(config.SystemPermissionsProvider.GetPermissionsForUser(null), Is.EqualTo(SystemPermissions.None));
            Assert.That(config.SystemPermissionsProvider.GetPermissionsForUser(mockUser), Is.EqualTo(SystemPermissions.ListStores));
        }
    }
}
