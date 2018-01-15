﻿// Range.cs
//
// Author:
//       Ricky Curtice <ricky@rwcproductions.com>
//
// Copyright (c) 2018 Richard Curtice
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibF_Stop {
	public struct Range : IEquatable<Range> {
		// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Range
		// https://tools.ietf.org/html/rfc7233#appendix-D
		// http://rextester.com/YSKLH42433
		// Aperture incorrectly limits the spec to
		// ^bytes=[0-9]+-([0-9]+)?$
		private static readonly Regex _rangeHeaderValuePattern = new Regex(@"^bytes=(,[\s]*)*(([0-9]+-([0-9]+)?)|(-[0-9]+))([\s]*,([\s]*(([0-9]+-([0-9]+)?)|(-[0-9]+)))?)*$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		private static readonly Regex _rangeHeaderValueParsePattern = new Regex(@"([0-9]+-(?:[0-9]+)?)|(-[0-9]+)", RegexOptions.Compiled);

		public int? Min;
		public int? Max;

		public Range(int? min, int? max) {
			Min = min;
			Max = max;

			if (
				(min == null && max >= 0)
				||
				(min < 0)
				||
				(min > max)
			) {
				throw new ArgumentOutOfRangeException(nameof(max), $"Min ({Min}) was greater than than max ({Max})");
			}
			/*

			null null OK  null    
			null -1   OK  "-1"    
			null 1    BAD "-1"    a == null && b >= 0
			null 0    BAD "-0"    a == null && b >= 0
			-1 null   BAD "-1-"   a < 0
			0 null    OK  "0-"    
			1 null    OK  "1-"    

			-2 -1     BAD "-2--2" a < 0
			-1 -2     BAD "-1--2" a < 0
			-1 -1     BAD "-1--1" a < 0
			-1 0      BAD "-1-0"  a < 0
			-1 1      BAD "-1-1"  a < 0
			0 -1      BAD "0--1"  a > b
			0 0       OK  "0-0"   
			0 1       OK  "0-1"   
			1 -1      BAD "1--1"  a > b
			1 0       BAD "1-0"   a > b
			1 1       OK  "1-1"   
			1 2       OK  "1-2"   
			2 1       BAD "2-1"   a > b

			Of course if B is large enough it could wrap past A given a short enough length, but we can't tell that until we actually have a length.
			*/
		}

		public static IEnumerable<Range> ParseRanges(string range) {
			if (range == null) {
				return null;
			}

			if (!_rangeHeaderValuePattern.IsMatch(range)) {
				// Failed pattern match.
				throw new FormatException();
			}

			var ranges = new List<Range>();

			// Parse and store the ranges.

			var rangeMatches = _rangeHeaderValueParsePattern.Matches(range);

			foreach (Match match in rangeMatches) {
				var first = true; // Can't use Linq with these silly things.
				foreach (Group matchGroup in match.Groups) {
					if (first) { // The 0th item is the whole match, not just the groups.
						first = false;
						continue;
					}

					var rangeStr = matchGroup.Value.Trim();

					var dashPos = rangeStr.IndexOf('-');
					// According to spec it should always have exactly one dash per range.
					// For now that means we've got a noise entry, possibly from the Regex parser...
					if (dashPos < 0) {
						continue;
					}

					if (dashPos == 0) { // Starts with a dash.
						ranges.Add(new Range(null, int.Parse(rangeStr)));
						continue;
					}

					if (dashPos + 1 == rangeStr.Length) { // Ends with a dash
						ranges.Add(new Range(int.Parse(rangeStr.Substring(0, dashPos)), null));
						continue;
					}

					ranges.Add(new Range(int.Parse(rangeStr.Substring(0, dashPos)), int.Parse(rangeStr.Substring(dashPos + 1))));
				}
			}

			return ranges;
		}

		public IEnumerable<T> GetRange<T>(IEnumerable<T> list) {
			if (list == null || (Min == null && Max == null)) {
				return null;
			}

			var min = Min ?? 0;
			var len = list.Count();
			var max = Max ?? len - 1;

			if (max < 0) {
				min = len + max;
				max = len - 1;
			}

			if (min < 0 || max < 0 || min > max) {
				throw new IndexOutOfRangeException($"Range invalid for given list! After normallizing to list length {len}, the range was [{min}, {max}]");
			}

			return list.Skip(min).Take(max - min + 1);
		}

		public override string ToString() {
			if (Min == null && Max == null) {
				return null;
			}

			if (Min == null && Max < 0) {
				return $"{Max}";
			}

			return $"{Min}-{Max}";
		}

		bool IEquatable<Range>.Equals(Range other) {
			return Min == other.Min && Max == other.Max;
		}
	}
}
