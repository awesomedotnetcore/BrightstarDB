﻿using System;
using Xunit;

namespace BrightstarDB.Tests.EntityFramework
{
    
    public class EmbeddedOptimisticLockingTests : OptimisticLockingTestsBase
    {
        private const string TestStoreLocation = "c:\\brightstar";
        private readonly string _storeName = "EmbeddedOptimisticLockingTests_" + DateTime.Now.Ticks;

        protected override MyEntityContext NewContext()
        {
            return new MyEntityContext(
                String.Format("type=embedded;storesDirectory={0};storeName={1};optimisticLocking=true", TestStoreLocation, _storeName));
        }

        #region SingleObjectRefres
        [Fact]
        public new void TestSimplePropertyRefreshWithClientWins()
        {
            base.TestSimplePropertyRefreshWithClientWins();
        }

        [Fact]
        public new void TestSimplePropertyRefreshWithStoreWins()
        {
            base.TestSimplePropertyRefreshWithStoreWins();

        }

        [Fact]
        public new void TestRelatedObjectRefreshWithClientWins()
        {
            base.TestRelatedObjectRefreshWithClientWins();
        }

        [Fact]
        public new void TestRelatedObjectRefreshWithStoreWins()
        {
            base.TestRelatedObjectRefreshWithStoreWins();
        }

        [Fact]
        public new void TestLiteralCollectionRefreshWithClientWins()
        {
            base.TestLiteralCollectionRefreshWithClientWins();
        }

        [Fact]
        public new void TestLiteralCollectionRefreshWithStoreWins()
        {
            base.TestLiteralCollectionRefreshWithStoreWins();
        }

        [Fact]
        public new void TestObjectCollectionRefreshWithClientWins()
        {
            base.TestObjectCollectionRefreshWithClientWins();
        }

        [Fact]
        public new void TestObjectCollectionRefreshWithStoreWins()
        {
            base.TestObjectCollectionRefreshWithStoreWins();
        }
        #endregion

        #region Multiple Object Updates

        [Fact]
        public new void MultiLiteralPropertyRefreshClientWins()
        {
            base.MultiLiteralPropertyRefreshClientWins();
        }

        [Fact]
        public new void MultiLiteralPropertyRefreshStoreWins()
        {
            base.MultiLiteralPropertyRefreshStoreWins();
        }

        [Fact]
        public new void MultiLiteralPropertyRefreshMixedModes()
        {
            base.MultiLiteralPropertyRefreshMixedModes();
        }

        #endregion

        #region CRUD
        [Fact]
        public new void TestCreateAndDeleteInSameContext()
        {
            base.TestCreateAndDeleteInSameContext();
        }
        #endregion
    }
}