﻿// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information of entitlements owned by the user
    /// </summary>
    public class Entitlement
    {
        private readonly string @namespace;
        private readonly EntitlementApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal Entitlement(EntitlementApi api, ISession session, string ns, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(ns), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = ns;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Query user entitlements.
        /// </summary>
        /// <param name="entitlementName">The name of the entitlement (optional)</param>
        /// <param name="itemId">Item's id (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="callback">Returns a Result that contains EntitlementPagingSlicedResult via callback when completed</param>
        /// <param name="entitlementClazz">Class of the entitlement (optional)</param>
        /// <param name="entitlementAppType">This is the type of application that entitled (optional)</param>
        public void QueryUserEntitlements(string entitlementName, string itemId, int offset, int limit, ResultCallback<EntitlementPagingSlicedResult> callback,
            EntitlementClazz entitlementClazz = EntitlementClazz.NONE, EntitlementAppType entitlementAppType = EntitlementAppType.NONE)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(entitlementName, "Can't query user entitlements! EntitlementName parameter is null!");
            Assert.IsNotNull(itemId, "Can't query user entitlements! ItemId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.QueryUserEntitlements(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    entitlementName,
                    itemId,
                    offset,
                    limit,
                    entitlementClazz,
                    entitlementAppType,
                    callback));
        }

        /// <summary>
        /// Get user's entitlement by the entitlementId.
        /// </summary>
        /// <param name="entitlementId">The id of the entitlement</param>
        /// <param name="callback">Returns a Result that contains EntitlementInfo via callback when completed</param>
        public void GetUserEntitlementById(string entitlementId, ResultCallback<EntitlementInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(entitlementId, "Can't get user entitlement by id! entitlementId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementById(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    entitlementId,
                    callback));
        }

        /// <summary>
        /// Consume a user entitlement.
        /// </summary>
        /// <param name="entitlementId">The id of the user entitlement.</param>
        /// <param name="useCount">Number of consumed entitlement</param>
        /// <param name="callback">Returns a Result that contains EntitlementInfo via callback when completed</param>
        public void ConsumeUserEntitlement(string entitlementId, int useCount, ResultCallback<EntitlementInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(entitlementId, "Can't consume user entitlement! entitlementId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ConsumeUserEntitlement(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    entitlementId,
                    useCount,
                    callback));
        }
    }
}
