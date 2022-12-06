// 
// UIStringAttributes.cs: Implements strongly typed access to UIKit specific part of UIStringAttributeKey
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012-2013, Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using System;

using ObjCRuntime;
using Foundation;
using CoreGraphics;

#if !MONOMAC
using UIKit;
#endif

namespace Foundation {

	public partial class NSAttributedStringDocumentAttributes : DictionaryContainer {
#if !MONOMAC && !COREBUILD
		public NSAttributedStringDocumentAttributes () : base (new NSMutableDictionary ()) { }
		public NSAttributedStringDocumentAttributes (NSDictionary? dictionary) : base (dictionary) { }

		public NSStringEncoding? StringEncoding {
			get {
				var value = GetInt32Value (UIStringAttributeKey.NSCharacterEncodingDocumentAttribute);
				if (value is null)
					return null;
				else
					return (NSStringEncoding) value.Value;
			}
			set {
				SetNumberValue (UIStringAttributeKey.NSCharacterEncodingDocumentAttribute, (int?) value);
			}
		}

		public NSString? WeakDocumentType {
			get {
				return GetNSStringValue (UIStringAttributeKey.NSDocumentTypeDocumentAttribute);
			}
			set {
				SetStringValue (UIStringAttributeKey.NSDocumentTypeDocumentAttribute, value);
			}
		}

		public NSDocumentType DocumentType {
			get {
				var s = GetNSStringValue (UIStringAttributeKey.NSDocumentTypeDocumentAttribute);
				if (s == NSAttributedStringDocumentType.NSPlainTextDocumentType)
					return NSDocumentType.PlainText;
				if (s == NSAttributedStringDocumentType.NSRtfdTextDocumentType)
					return NSDocumentType.RTFD;
				if (s == NSAttributedStringDocumentType.NSRtfTextDocumentType)
					return NSDocumentType.RTF;
				if (s == NSAttributedStringDocumentType.NSHtmlTextDocumentType)
					return NSDocumentType.HTML;
				return NSDocumentType.Unknown;
			}

			set {
				switch (value) {
				case NSDocumentType.PlainText:
					SetStringValue (UIStringAttributeKey.NSDocumentTypeDocumentAttribute, NSAttributedStringDocumentType.NSPlainTextDocumentType);
					break;
				case NSDocumentType.RTFD:
					SetStringValue (UIStringAttributeKey.NSDocumentTypeDocumentAttribute, NSAttributedStringDocumentType.NSRtfdTextDocumentType);
					break;
				case NSDocumentType.RTF:
					SetStringValue (UIStringAttributeKey.NSDocumentTypeDocumentAttribute, NSAttributedStringDocumentType.NSRtfTextDocumentType);
					break;
				case NSDocumentType.HTML:
					SetStringValue (UIStringAttributeKey.NSDocumentTypeDocumentAttribute, NSAttributedStringDocumentType.NSHtmlTextDocumentType);
					break;
				}
			}
		}

		public CGSize? PaperSize {
			get {
				NSObject value;
				Dictionary.TryGetValue (UIStringAttributeKey.NSPaperSizeDocumentAttribute, out value);
				var size = value as NSValue;
				if (size != null)
					return size.CGSizeValue;
				return null;
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSPaperSizeDocumentAttribute);
				else
					Dictionary [UIStringAttributeKey.NSPaperSizeDocumentAttribute] = NSValue.FromCGSize (value.Value);
			}
		}

		public UIEdgeInsets? PaperMargin {
			get {
				NSObject value;
				Dictionary.TryGetValue (UIStringAttributeKey.NSPaperMarginDocumentAttribute, out value);
				var size = value as NSValue;
				if (size != null)
					return size.UIEdgeInsetsValue;
				return null;
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSPaperMarginDocumentAttribute);
				else
					Dictionary [UIStringAttributeKey.NSPaperMarginDocumentAttribute] = NSValue.FromUIEdgeInsets (value.Value);
			}
		}

		public CGSize? ViewSize {
			get {
				NSObject value;
				Dictionary.TryGetValue (UIStringAttributeKey.NSViewSizeDocumentAttribute, out value);
				var size = value as NSValue;
				if (size != null)
					return size.CGSizeValue;
				return null;
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSViewSizeDocumentAttribute);
				else
					Dictionary [UIStringAttributeKey.NSViewSizeDocumentAttribute] = NSValue.FromCGSize (value.Value);
			}
		}

		public float? ViewZoom {
			get {
				return GetFloatValue (UIStringAttributeKey.NSViewZoomDocumentAttribute);
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSViewZoomDocumentAttribute);
				else
					SetNumberValue (UIStringAttributeKey.NSViewZoomDocumentAttribute, value);
			}
		}

		public NSDocumentViewMode? ViewMode {
			get {
				var value = GetInt32Value (UIStringAttributeKey.NSViewModeDocumentAttribute);
				if (value is null)
					return null;
				else
					return (NSDocumentViewMode) value.Value;
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSViewModeDocumentAttribute);
				else
					SetNumberValue (UIStringAttributeKey.NSViewModeDocumentAttribute, (int) value.Value);
			}
		}

		public bool ReadOnly {
			get {
				var value = GetInt32Value (UIStringAttributeKey.NSReadOnlyDocumentAttribute);
				if (value is null || value.Value <= 0)
					return false;
				return true;
			}
			set {
				SetNumberValue (UIStringAttributeKey.NSReadOnlyDocumentAttribute, value ? 1 : 0);
			}
		}

		public UIColor? BackgroundColor {
			get {
				NSObject? value;
				Dictionary.TryGetValue (UIStringAttributeKey.NSBackgroundColorDocumentAttribute, out value);
				return value as UIColor;
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSBackgroundColorDocumentAttribute);
				else
					Dictionary [UIStringAttributeKey.NSBackgroundColorDocumentAttribute] = value;
			}
		}

		public float? HyphenationFactor {
			get {
				return GetFloatValue (UIStringAttributeKey.NSHyphenationFactorDocumentAttribute);
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSHyphenationFactorDocumentAttribute);
				else {
					if (value < 0 || value > 1.0f)
						throw new ArgumentException ("value must be between 0 and 1");
					SetNumberValue (UIStringAttributeKey.NSHyphenationFactorDocumentAttribute, value);
				}
			}
		}

		public float? DefaultTabInterval {
			get {
				return GetFloatValue (UIStringAttributeKey.NSDefaultTabIntervalDocumentAttribute);
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSDefaultTabIntervalDocumentAttribute);
				else {
					if (value < 0 || value > 1.0f)
						throw new ArgumentException ("value must be between 0 and 1");
					SetNumberValue (UIStringAttributeKey.NSDefaultTabIntervalDocumentAttribute, value);
				}
			}
		}

		public NSDictionary? WeakDefaultAttributes {
			get {
				NSObject? value;
				Dictionary.TryGetValue (UIStringAttributeKey.NSDefaultAttributesDocumentAttribute, out value);
				return value as NSDictionary;
			}
			set {
				if (value is null)
					RemoveValue (UIStringAttributeKey.NSDefaultAttributesDocumentAttribute);
				else
					Dictionary [UIStringAttributeKey.NSDefaultAttributesDocumentAttribute] = value;
			}
		}
#endif
#if !COREBUILD && !TVOS && !WATCH
		// documentation is unclear if an NSString or an NSUrl should be used...
		// but providing an `NSString` throws a `NSInvalidArgumentException Reason: (null) is not a file URL`
#if NET
		[SupportedOSPlatform ("macos10.15")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[Mac (10, 15)]
		[iOS (13, 0)]
#endif
		public NSUrl? ReadAccessUrl {
			get {
				Dictionary.TryGetValue (NSAttributedStringDocumentReadingOptionKeys.ReadAccessUrlKey, out var value);
				return value as NSUrl;
			}
			set {
				if (value is null)
					RemoveValue (NSAttributedStringDocumentReadingOptionKeys.ReadAccessUrlKey);
				else
					Dictionary [NSAttributedStringDocumentReadingOptionKeys.ReadAccessUrlKey] = value;
			}
		}
#endif
	}
}
