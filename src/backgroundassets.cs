//
// BackgroundAssets C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2022 Microsoft Corporation All rights reserved.
//

using System;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace BackgroundAssets {
	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
	[Native]
	public enum BADownloadState : long {
		Failed = -1,
		Created = 0,
		Waiting,
		Downloading,
		Finished,
	}

	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
	[Native]
	public enum BAContentRequest : long {
		Install = 1,
		Update,
		Periodic,
	}

	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BADownload : NSCoding, NSSecureCoding, NSCopying
	{
		[Export ("state")]
		BADownloadState State { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("uniqueIdentifier")]
		string UniqueIdentifier { get; }

		[Export ("priority")]
		nint Priority { get; }

	}

	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BAAppExtensionInfo : NSSecureCoding { 

		[Mac (13,0), iOS (16,1), MacCatalyst (16,1)]
		[NullAllowed]
		[Export ("restrictedDownloadSizeRemaining", ArgumentSemantic.Strong)]
		NSNumber RestrictedDownloadSizeRemaining { get; }
	}

	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
	[Protocol]
	interface BADownloaderExtension {

		[Export ("extensionWillTerminate")]
		void WillTerminate ();

		[Export ("backgroundDownload:didReceiveChallenge:completionHandler:")]
		void DidReceiveChallenge (BADownload download, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler);

		[Export ("backgroundDownload:failedWithError:")]
		void Failed (BADownload download, NSError error);

		[Export ("backgroundDownload:finishedWithFileURL:")]
		void Finished (BADownload download, NSUrl fileUrl);

		[Export ("downloadsForRequest:manifestURL:extensionInfo:")]
		NSSet<BADownload> GetDownloads (BAContentRequest contentRequest, NSUrl manifestUrl, BAAppExtensionInfo extensionInfo);
	}

	interface IBADownloadManagerDelegate {}

	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
#if NET
	[Protocol][Model]
#else
	[Protocol][Model (AutoGeneratedName = true)]
#endif
	[BaseType (typeof (NSObject))]
	interface BADownloadManagerDelegate
	{
		[Export ("downloadDidBegin:")]
		void DidBegin (BADownload download);

		[Export ("downloadDidPause:")]
		void DidPause (BADownload download);

		[Export ("download:didWriteBytes:totalBytesWritten:totalBytesExpectedToWrite:")]
		void DidWriteBytes (BADownload download, long bytesWritten, long totalBytesWritten, long totalExpectedBytes);

		[Export ("download:didReceiveChallenge:completionHandler:")]
		void DidReceiveChallenge (BADownload download, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler);

		[Export ("download:failedWithError:")]
		void Failed (BADownload download, NSError error);

		[Export ("download:finishedWithFileURL:")]
		void Finished (BADownload download, NSUrl fileUrl);
	}

	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface BADownloadManager
	{
		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		BADownloadManager SharedManager { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IBADownloadManagerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Async]
		[Export ("fetchCurrentDownloadsWithCompletionHandler:")]
		void FetchCurrentDownloads (Action<NSArray<BADownload>, NSError> completionHandler);

		[Export ("scheduleDownload:error:")]
		bool ScheduleDownload (BADownload download, [NullAllowed] out NSError outError);

		[Export ("performWithExclusiveControl:")]
		void PerformWithExclusiveControl (Action<NSError> performHandler);

		[Export ("startForegroundDownload:error:")]
		bool StartForegroundDownload (BADownload download, [NullAllowed] out NSError outError);

		[Export ("cancelDownload:error:")]
		bool CancelDownload (BADownload download, [NullAllowed] out NSError error);

		[MacCatalyst (16,1), iOS (16,1)]
		[Export ("performWithExclusiveControlBeforeDate:performHandler:")]
		void PerformWithExclusiveControlBeforeDate (NSDate date, Action<bool, NSError> performHandler);
	}

	[NoWatch, NoTV, Mac (13,0), iOS (16,0), MacCatalyst (16,0)]
	[BaseType (typeof (BADownload), Name = "BAURLDownload")]
	[DisableDefaultCtor]
	interface BAUrlDownload
	{

		[Field ("BADownloaderPriorityMin")]
		nint MinPriority { get; }

		[Field ("BADownloaderPriorityDefault")]
		nint DefaultPriority { get; }

		[Field ("BADownloaderPriorityMax")]
		nint MaxPriority { get; }

		[Export ("initWithIdentifier:request:applicationGroupIdentifier:")]
		NativeHandle Constructor (string identifier, NSUrlRequest request, string applicationGroupIdentifier);

		[Export ("initWithIdentifier:request:applicationGroupIdentifier:priority:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string identifier, NSUrlRequest request, string applicationGroupIdentifier, nint priority);
	}

}