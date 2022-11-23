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
		public float? DefaultTabInterval {
			get {
				return GetFloatValue (NSAttributedStringDocumentAttributeKey.NSDefaultTabIntervalDocumentAttribute);
			}
			set {
				if (value is null)
					RemoveValue (NSAttributedStringDocumentAttributeKey.NSDefaultTabIntervalDocumentAttribute);
				else {
					if (value < 0 || value > 1.0f)
						throw new ArgumentException ("value must be between 0 and 1");
					SetNumberValue (NSAttributedStringDocumentAttributeKey.NSDefaultTabIntervalDocumentAttribute, value);
				}
			}
		}
#endif
	}
}
