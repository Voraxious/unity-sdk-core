﻿#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections;
using System.Threading;

namespace FunPlus
{
	public class FunPlusAndroid : IWorkerMethodDispacther
	{
		private AndroidJavaClass jc;
		private AndroidJavaClass sdkBridgeClass;
		private AndroidJavaObject application;
		private AndroidJavaObject currentActivity;

		public FunPlusAndroid ()
		{
			this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			this.currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			this.application = currentActivity.Call<AndroidJavaObject>("getApplication");
			this.sdkBridgeClass = new AndroidJavaClass ("com.funplus.sdk.UnityBridge");
			FunPlusWorker.GetInstance ().RegisterClient ("sdk", this);
		}

		public void ResolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
			sdkBridgeClass.CallStatic (api, args);
		}

		void SdkApiCall(string api, params object[] args) {
			AddSdkApiCallToQueue ("SdkApiCallWithArgs", api, args);
		}

		void SdkApiCall(string api) {
			AddSdkApiCallToQueue ("SdkApiCall", api, null);
		}

		void AddSdkApiCallToQueue(String methodIdentifier, String api, object[] args) {
			FunPlusWorker.GetInstance ().EnqueueApiCall ("sdk", methodIdentifier, api, args);
		}

		public void Install(string appId, string appKey, string rumTag, string rumKey, string environment)
		{
			SdkApiCall ("install", new object[] { application, appId, appKey, rumTag, rumKey, environment });
		}

		public string GetSessionId()
		{
			return "todo";
		}

		public void GetFPID(string externalID, string externalIDType)
		{
			SdkApiCall("getFPID", new object[] { externalID, externalIDType });
		}

		public void BindFPID(string fpid, string externalID, string externalIDType)
		{
			SdkApiCall("bindFPID", new object[] { fpid, externalID, externalIDType });
		}

		public void TraceRUMServiceMonitoring(string serviceName,
			                                  string httpUrl,
			                                  string httpStatus,
			                                  int requestSize,
			                                  int responseSize,
			                                  long httpLatency,
			                                  long requestTs,
			                                  long responseTs,
			                                  string requestId,
			                                  string targetUserId,
			                                  string gameServerId)
		{
			SdkApiCall ("traceRUMServiceMonitoring", new object[] {
				serviceName, httpUrl, httpStatus, requestSize, responseSize, httpLatency,
				requestTs, responseTs, requestId, targetUserId, gameServerId
			});
		}

		public void SetRUMExtraProperty (string key, string value)
		{
			SdkApiCall ("setRUMExtraProperty", new object[] { key, value });
		}

		public void EraseRUMExtraProperty (string key)
		{
			SdkApiCall ("eraseRUMExtraProperty", new object[] { key });
		}

		public void TraceDataCustom(Dictionary<string, object> dataEvent)
		{
			SdkApiCall ("traceDataCustom", new object[] { Json.Serialize(dataEvent) });
		}

		public void TraceCustomEventWithNameAndProperties(string eventName, Dictionary<string, object> properties)
		{
			SdkApiCall ("traceCustomEventWithNameAndProperties", new object[] { eventName, Json.Serialize(properties) });
		}

		public void TraceDataPayment(double amount,
									 string currency,
									 string productId,
			                         string productName,
			                         string productType,
			                         string transactionId,
									 string paymentProcessor,
								     string itemsReceived,
			                         string currencyReceived)
		{
			SdkApiCall ("traceDataPayment", new object[] {
				amount, currency, productId, productName, productType, transactionId,
				paymentProcessor, itemsReceived, currencyReceived
			});
		}

		public void SetDataExtraProperty (string key, string value)
		{
			SdkApiCall ("setDataExtraProperty", new object[] { key, value });
		}

		public void EraseDataExtraProperty (string key)
		{
			SdkApiCall ("eraseDataExtraProperty", new object[] { key });
		}
	}
}
#endif