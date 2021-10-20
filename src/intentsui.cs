//
// IntentsUI bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using System;
using CoreGraphics;
using Foundation;
using Intents;
using ObjCRuntime;
#if !MONOMAC
using UIKit;
#else
using AppKit;
#endif

namespace IntentsUI {

	[NoMac]
	[iOS (10, 0)]
	[MacCatalyst (13, 0)]
	[Native]
	public enum INUIHostedViewContext : ulong {
		SiriSnippet,
		MapsCard
	}

	[NoMac]
	[iOS (11,0)]
	[MacCatalyst (13, 0)]
	[Native]
	public enum INUIInteractiveBehavior : ulong {
		None,
		NextView,
		Launch,
		GenericAction,
	}

	[NoWatch, NoTV, Mac (12,0), iOS (12,0)]
	[Native]
	public enum INUIAddVoiceShortcutButtonStyle : ulong {
		White = 0,
		WhiteOutline,
		Black,
		BlackOutline,
		[iOS (13,0)]
		Automatic,
		[iOS (13,0)]
		AutomaticOutLine,
	}

	[NoMac]
	[iOS (11,0)]
	delegate void INUIHostedViewControllingConfigureViewHandler (bool success, NSSet<INParameter> configuredParameters, CGSize desiredSize);

	[NoMac]
	[iOS (10, 0)]
	[MacCatalyst (13, 0)]
	[Protocol]
	interface INUIHostedViewControlling {

#if !XAMCORE_4_0 && !__MACCATALYST__ // Apple made this member optional in iOS 11
		[Abstract]
#endif
		[Export ("configureWithInteraction:context:completion:")]
		void Configure (INInteraction interaction, INUIHostedViewContext context, Action<CGSize> completion);

		[iOS (11,0)]
		[Export ("configureViewForParameters:ofInteraction:interactiveBehavior:context:completion:")]
		void ConfigureView (NSSet<INParameter> parameters, INInteraction interaction, INUIInteractiveBehavior interactiveBehavior, INUIHostedViewContext context, INUIHostedViewControllingConfigureViewHandler completionHandler);
	}

	[NoMac]
	[iOS (10, 0)]
	[MacCatalyst (13, 0)]
	[Category]
	[BaseType (typeof (NSExtensionContext))]
	interface NSExtensionContext_INUIHostedViewControlling {

		[Export ("hostedViewMinimumAllowedSize")]
		CGSize GetHostedViewMinimumAllowedSize ();

		[Export ("hostedViewMaximumAllowedSize")]
		CGSize GetHostedViewMaximumAllowedSize ();

		[iOS (11,0)]
		[Export ("interfaceParametersDescription")]
		string GetInterfaceParametersDescription ();
	}

	[NoMac]
	[iOS (10, 0)]
	[MacCatalyst (13, 0)]
	[Protocol]
	interface INUIHostedViewSiriProviding {

		[Export ("displaysMap")]
		bool DisplaysMap { get; }

		[Export ("displaysMessage")]
		bool DisplaysMessage { get; }

		[Export ("displaysPaymentTransaction")]
		bool DisplaysPaymentTransaction { get; }
	}

	[Mac (12,0)]
	[iOS (12,0)]
#if !MONOMAC
	[BaseType (typeof (UIViewController))]
#else
	[BaseType (typeof (NSViewController))]
#endif
	[DisableDefaultCtor]
	interface INUIAddVoiceShortcutViewController {

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IINUIAddVoiceShortcutViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[MacCatalyst (13,4)]
		[Export ("initWithShortcut:")]
		IntPtr Constructor (INShortcut shortcut);
	}

	interface IINUIAddVoiceShortcutViewControllerDelegate { }

	[Mac (12,0)]
	[iOS (12,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface INUIAddVoiceShortcutViewControllerDelegate {

		[Abstract]
		[Export ("addVoiceShortcutViewController:didFinishWithVoiceShortcut:error:")]
		void DidFinish (INUIAddVoiceShortcutViewController controller, [NullAllowed] INVoiceShortcut voiceShortcut, [NullAllowed] NSError error);

		[Abstract]
		[Export ("addVoiceShortcutViewControllerDidCancel:")]
		void DidCancel (INUIAddVoiceShortcutViewController controller);
	}

	[Mac (12,0)]
	[iOS (12,0)]
#if !MONOMAC
	[BaseType (typeof (UIViewController))]
#else
	[BaseType (typeof (NSViewController))]
#endif
	[DisableDefaultCtor]
	interface INUIEditVoiceShortcutViewController {

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IINUIEditVoiceShortcutViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[MacCatalyst (13,4)]
		[Export ("initWithVoiceShortcut:")]
		IntPtr Constructor (INVoiceShortcut voiceShortcut);
	}

	interface IINUIEditVoiceShortcutViewControllerDelegate { }

	[Mac (12,0)]
	[iOS (12,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface INUIEditVoiceShortcutViewControllerDelegate {

		[Abstract]
		[Export ("editVoiceShortcutViewController:didUpdateVoiceShortcut:error:")]
		void DidUpdate (INUIEditVoiceShortcutViewController controller, [NullAllowed] INVoiceShortcut voiceShortcut, [NullAllowed] NSError error);

		[Abstract]
		[Export ("editVoiceShortcutViewController:didDeleteVoiceShortcutWithIdentifier:")]
		void DidDelete (INUIEditVoiceShortcutViewController controller, NSUuid deletedVoiceShortcutIdentifier);

		[Abstract]
		[Export ("editVoiceShortcutViewControllerDidCancel:")]
		void DidCancel (INUIEditVoiceShortcutViewController controller);
	}

	[NoWatch, NoTV, Mac (12,0), iOS (12,0)]
#if !MONOMAC
	[BaseType (typeof (UIButton))]
#else
	[BaseType (typeof (NSButton))]
#endif
	[DisableDefaultCtor]
	interface INUIAddVoiceShortcutButton {

		[MacCatalyst (13, 4)]
		[Export ("initWithStyle:")]
		[DesignatedInitializer]
		IntPtr Constructor (INUIAddVoiceShortcutButtonStyle style);

		[Export ("style")]
		INUIAddVoiceShortcutButtonStyle Style { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IINUIAddVoiceShortcutButtonDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("shortcut", ArgumentSemantic.Strong)]
		INShortcut Shortcut { get; set; }

		[iOS (12,2)]
		[Export ("cornerRadius", ArgumentSemantic.Assign)]
		nfloat CornerRadius { get; set; }
		
		[iOS (13,0)]
		[Export ("setStyle:")]
		void SetStyle (INUIAddVoiceShortcutButtonStyle style);
	}

	interface IINUIAddVoiceShortcutButtonDelegate {}

	[NoWatch, NoTV, Mac (12,0), iOS (12,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof(NSObject))]
	interface INUIAddVoiceShortcutButtonDelegate {

		[Abstract]
		[Export ("presentAddVoiceShortcutViewController:forAddVoiceShortcutButton:")]
		void PresentAddVoiceShortcut (INUIAddVoiceShortcutViewController addVoiceShortcutViewController, INUIAddVoiceShortcutButton addVoiceShortcutButton);

		[Abstract]
		[Export ("presentEditVoiceShortcutViewController:forAddVoiceShortcutButton:")]
		void PresentEditVoiceShortcut (INUIEditVoiceShortcutViewController editVoiceShortcutViewController, INUIAddVoiceShortcutButton addVoiceShortcutButton);
	}
}
