﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BrightstarDB.Tests.EntityFramework
{
    public class InversePropertyTests
    {
        private readonly string _connectionString = "Type=embedded;StoresDirectory=c:\\brightstar;StoreName=InversePropertyTests_" + DateTime.Now.Ticks;

        [Fact]
        public void TestAddOne()
        {
            string productionId, performanceId;
            using (var context = new MyEntityContext(_connectionString))
            {
                var production = context.Productions.Create();
                var performance = context.Performances.Create();
                Assert.Equal(production.Performances.Count, 0);
                Assert.Equal(production.Photos.Count, 0);
                Assert.Equal(production.ProductionTeam.Count, 0);

                // Set the Production property on the performance
                performance.Production = production;

                Assert.Equal(production.Performances.Count, 1);
                Assert.Equal(production.Photos.Count, 0);
                Assert.Equal(production.ProductionTeam.Count, 0);
                context.SaveChanges();
                productionId = production.Id;
                performanceId = performance.Id;
            }

            using (var context = new MyEntityContext(_connectionString))
            {
                var production = context.Productions.FirstOrDefault(x => x.Id.Equals(productionId));
                Assert.NotNull(production);
                Assert.Equal(production.Performances.Count, 1);
                Assert.Equal(production.Performances.First().Id, performanceId);
                Assert.Equal(production.Photos.Count, 0);
                Assert.Equal(production.ProductionTeam.Count, 0);
            }
        }

        [Fact]
        public void TestAddToInverse()
        {
            string productionId, performanceId;
            using (var context = new MyEntityContext(_connectionString))
            {
                var production = context.Productions.Create();
                var performance = context.Performances.Create();
                Assert.Equal(production.Performances.Count, 0);
                Assert.Equal(production.Photos.Count, 0);
                Assert.Equal(production.ProductionTeam.Count, 0);

                // Add the performance to the production's perfomances collection
                production.Performances.Add(performance);

                Assert.Equal(production.Performances.Count, 1);
                Assert.Equal(production.Photos.Count, 0);
                Assert.Equal(production.ProductionTeam.Count, 0);
                context.SaveChanges();
                productionId = production.Id;
                performanceId = performance.Id;
            }

            using (var context = new MyEntityContext(_connectionString))
            {
                var production = context.Productions.FirstOrDefault(x => x.Id.Equals(productionId));
                Assert.NotNull(production);
                Assert.Equal(production.Performances.Count, 1);
                Assert.Equal(production.Performances.First().Id, performanceId);
                Assert.Equal(production.Photos.Count, 0);
                Assert.Equal(production.ProductionTeam.Count, 0);
            }
        }
    }
}